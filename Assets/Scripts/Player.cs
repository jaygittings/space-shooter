using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    //config
    [SerializeField] float moveSpeed = 10f;
    [SerializeField] float padding = 2f;
    [SerializeField] GameObject playerBullet = null;
    [SerializeField] float rateOfFire = 2f;
    [SerializeField] float bulletSpeed = 10f;

    //state
    float xMin = 0f;
    float xMax = 1f;
    float yMin = 0f;
    float yMax = 1f;
    Coroutine routine = null;
    bool firing = false;


    // Start is called before the first frame update
    void Start()
    {
        CreateMoveBoundry();
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
}
