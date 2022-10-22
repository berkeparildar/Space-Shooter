using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using Random = UnityEngine.Random;

public class Powerup : MonoBehaviour
{
    [SerializeField]
    private float speed = 3f;
    [SerializeField]
    private int powerUpID;
    
    private void Start()
    {
    }

    private void Update()
    {
        transform.Translate(Vector3.down * (speed * Time.deltaTime));
        
        if (transform.position.y < -6.5f)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.transform.name == "Player")
        {
            var _player = col.transform.GetComponent<Player>();
            if (_player != null)
            {
                switch (powerUpID)
                {
                    case 0:
                        _player.ActivateTripleShotPowerUp();
                        break;
                    case 1:
                        _player.ActivateSpeedPowerUp();
                        break;
                    case 2:
                        _player.ShieldActive();
                        break;
                }
            }
            Destroy(this.gameObject);
        }
    }
}