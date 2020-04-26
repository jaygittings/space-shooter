using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void DestoryOffscreen()
    {
        if(transform.position.y > Camera.main.orthographicSize)
        {
            Destroy(gameObject);
        }
    }
}
