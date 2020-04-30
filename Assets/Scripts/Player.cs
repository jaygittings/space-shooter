using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    //config
    [Header("Player Stats")]
    [SerializeField] int playerHealth = 200;
    [SerializeField] float moveSpeed = 10f;
    [SerializeField] float padding = 2f;
    [SerializeField] int enemyLayer;
    [SerializeField] GameObject explosion;
    [SerializeField] float explosionTime = 1f;
    [SerializeField] AudioClip explosionSound;
    [SerializeField] [Range(0, 1)] float explosionSoundVolume = .75f;
    [Header("Bullet Stats")]
    [SerializeField] GameObject playerBullet = null;
    [SerializeField] float rateOfFire = 2f;
    [SerializeField] float bulletSpeed = 10f;
    [SerializeField] AudioClip[] bulletSounds;
    [SerializeField] [Range(0, 1)] float bulletSoundVolume = .75f;
    [SerializeField] bool soundEnabled;

    //state
    float xMin = 0f;
    float xMax = 1f;
    float yMin = 0f;
    float yMax = 1f;
    Coroutine routine = null;
    bool firing = false;
    //AudioSource sound;
    SceneControl controller;
    Animator animator;
    bool isDead;


    // Start is called before the first frame update
    void Start()
    {
        CreateMoveBoundry();
        //sound = GetComponent<AudioSource>();
        if(bulletSounds.Length > 0)
            //sound.clip = bulletSounds[0];
        controller = FindObjectOfType<SceneControl>();
        animator = GetComponent<Animator>();

    }


    // Update is called once per frame
    void Update()
    {
        Move();
        Shoot();
    }

    private void Shoot()
    {
        if(Input.GetButtonDown("Fire1"))
        {
            if (firing)
                return;

            routine = StartCoroutine(FireContinuously());
            firing = true;
        }

        if(Input.GetButtonUp("Fire1"))
        {
            StopCoroutine(routine);
            firing = false;
        }
    }

    IEnumerator FireContinuously()
    {
        while(true)
        {
            var bullet = Instantiate(playerBullet, transform.position, Quaternion.identity) as GameObject;
            bullet.GetComponent<Rigidbody2D>().velocity = new Vector2(0, bulletSpeed);
            if (soundEnabled)
            {
                AudioSource.PlayClipAtPoint(bulletSounds[0], Camera.main.transform.position, bulletSoundVolume);
            }
            yield return new WaitForSeconds(1 / rateOfFire);
        }
    }

    private void CreateMoveBoundry()
    {
        Camera gameCamera = Camera.main;
        xMin = gameCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).x + padding;
        yMin = gameCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).y + padding;

        xMax = gameCamera.ViewportToWorldPoint(new Vector3(1, 0, 0)).x - padding;
        yMax = gameCamera.ViewportToWorldPoint(new Vector3(0, 1, 0)).y - padding;


    }

    private void Move()
    {
        var deltaX = Input.GetAxis("Horizontal") * Time.deltaTime * moveSpeed;
        var newX = transform.position.x + (deltaX);
        newX = Mathf.Clamp(newX, xMin, xMax);

        var deltaY = Input.GetAxis("Vertical") * Time.deltaTime * moveSpeed;
        var newY = transform.position.y + (deltaY);
        newY = Mathf.Clamp(newY, yMin, yMax);

        transform.position = new Vector2(newX, newY);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var damage = collision.gameObject.GetComponent<DamageHandler>();
        ProcessHit(damage);
    }

    private void ProcessHit(DamageHandler damage)
    {
        if(damage != null)
        {
            animator.SetTrigger("PlayerHit");
            playerHealth -= damage.Damage;
            damage.Hit();

            if(playerHealth <= 0)
            {
                StartCoroutine(Die());
            }

        }
    }

    private IEnumerator Die()
    {
        isDead = true;
        gameObject.GetComponent<SpriteRenderer>().enabled = false;
        gameObject.GetComponent<PolygonCollider2D>().enabled = false;
        var obj = Instantiate(explosion, transform.position, Quaternion.identity);
        AudioSource.PlayClipAtPoint(explosionSound, Camera.main.transform.position, explosionSoundVolume);
        Destroy(obj, explosionTime);
        yield return new WaitForSeconds(explosionTime);
        controller.LoadGameOver();
        Destroy(gameObject);
    }

    public int GetHealth()
    {
        return playerHealth;
    }

    public bool IsDead()
    {
        return isDead;
    }
}
