using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    //config
    [Header("Enemy Stats")]
    [SerializeField] int health = 100;
    [SerializeField] float shotCounter;
    [SerializeField] float minTimeBetweenShots = .2f;
    [SerializeField] float maxTimeBetweenShots = 2f;
    [SerializeField] GameObject explosion;
    [SerializeField] float explosionTime = 1f;
    [SerializeField] AudioClip explosionSound;
    [SerializeField] [Range(0, 1)] float explosionSoundVolume = .75f;
    [SerializeField] int scoreForKill = 100;
    [SerializeField] GameObject component;
    [SerializeField] int componentSpawnChange = 50;
    [Header("Bullet Stats")]
    [SerializeField] GameObject enemyBullet;
    [SerializeField] float bulletSpeed;
    [SerializeField] int playerLayer;
    [SerializeField] AudioClip[] bulletSounds;
    [SerializeField] [Range(0,1)] float bulletSoundVolume = .75f;
    [SerializeField] bool soundEnabled;

    //state
    GameObject player;
    State state;
    bool isDead;
    //AudioSource sound;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        state = FindObjectOfType<State>();
        shotCounter = UnityEngine.Random.Range(minTimeBetweenShots, maxTimeBetweenShots);
        //sound = GetComponent<AudioSource>();
        //sound.clip = bulletSounds[0];
    }

    // Update is called once per frame
    void Update()
    {
        CountDownAndShoot();
    }

    private void CountDownAndShoot()
    {
        shotCounter -= Time.deltaTime;
        if(shotCounter <= 0)
        {
            Shoot();
            shotCounter = UnityEngine.Random.Range(minTimeBetweenShots, maxTimeBetweenShots);
        }
    }

    private void Shoot()
    {
        if(player != null)
        {
            var bullet = Instantiate(enemyBullet, transform.position, Quaternion.identity);
            var bulletDirection = (player.transform.position - transform.position).normalized;
            AudioSource.PlayClipAtPoint(bulletSounds[0], Camera.main.transform.position, bulletSoundVolume);
            bullet.GetComponent<Rigidbody2D>().velocity = (bulletDirection * bulletSpeed);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var damage = collision.gameObject.GetComponent<DamageHandler>();
        ProcessHit(damage);
    }

    private void ProcessHit(DamageHandler damage)
    {
        if (damage != null)
        {
            health -= damage.Damage;
            damage.Hit();

            if (health <= 0 && !isDead)
            {
                Die();
            }
        }
    }

    private void Die()
    {
        isDead = true;
        state.AddToScore(scoreForKill);
        var obj = Instantiate(explosion, transform.position, Quaternion.identity);
        AudioSource.PlayClipAtPoint(explosionSound, Camera.main.transform.position, explosionSoundVolume);
        Destroy(obj, explosionTime);

        if (UnityEngine.Random.Range(0, 100) < componentSpawnChange)
        {
            GetComponent<DamageHandler>().Damage = 0;
            Instantiate(component, transform.position, Quaternion.identity);
        }
        Destroy(gameObject);
    }
}
