using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipComponent : MonoBehaviour
{
    //config
    [SerializeField] float moveSpeed = .1f;
    [SerializeField] float rotationSpeed = .1f;
    [SerializeField] Vector3 offsetFromPlayer = new Vector2();
    [SerializeField] GameObject playerBullet = null;
    [SerializeField] float rateOfFire = 2f;
    [SerializeField] float bulletSpeed = 10f;
    [SerializeField] int componentHealth = 100;
    [SerializeField] GameObject explosion;
    [SerializeField] float explosionTime = 1f;
    [SerializeField] AudioClip explosionSound;
    [SerializeField] [Range(0, 1)] float explosionSoundVolume = .75f;

    //state
    bool isAttached = false;
    GameObject player = null;
    Player playerScript = null;
    bool firing = false;
    Coroutine routine;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerScript = player.GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isAttached && playerScript.IsDead())
            Die();
        Move();
        if(isAttached)
            Shoot();
    }

    private void Move()
    {
        if (isAttached)
        {
            transform.position = player.transform.position + offsetFromPlayer;
        }
        else
        {
            //move down
            transform.position = new Vector2(transform.position.x, transform.position.y + (moveSpeed * Time.deltaTime));

            //rotate
            transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("tag = " + collision.tag);
        if(collision.tag == "Player")
        {
            isAttached = true;
            offsetFromPlayer = transform.position - collision.transform.position;
        }
        if (collision.tag == "Component")
        {
            isAttached = true;
            offsetFromPlayer = transform.position - player.transform.position;
        }
        else
        {
            var damage = collision.gameObject.GetComponent<DamageHandler>();
            ProcessHit(damage);
        }
    }

    private void Shoot()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            if (firing)
                return;

            routine = StartCoroutine(FireContinuously());
            firing = true;
        }

        if (Input.GetButtonUp("Fire1"))
        {
            if(routine != null)
            {
                StopCoroutine(routine);
                firing = false;
            }
        }
    }

    IEnumerator FireContinuously()
    {
        while (true)
        {
            var bullet = Instantiate(playerBullet, transform.position, Quaternion.identity) as GameObject;
            bullet.GetComponent<Rigidbody2D>().velocity = new Vector2(0, bulletSpeed);
            yield return new WaitForSeconds(1 / rateOfFire);
        }
    }

    private void ProcessHit(DamageHandler damage)
    {
        if (damage != null)
        {
            componentHealth -= damage.Damage;
            damage.Hit();

            if (componentHealth <= 0)
            {
                Die();
            }

        }
    }

    private void Die()
    {
        gameObject.GetComponent<SpriteRenderer>().enabled = false;
        var obj = Instantiate(explosion, transform.position, Quaternion.identity);
        AudioSource.PlayClipAtPoint(explosionSound, Camera.main.transform.position, explosionSoundVolume);
        Destroy(obj, explosionTime);
        Destroy(gameObject);
    }


}
