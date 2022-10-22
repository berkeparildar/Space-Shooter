using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class Enemy : MonoBehaviour
{
    [SerializeField] 
    private float speed = 4f;

    private Player _player;
    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
    }

    // Update is called once per frame
    private void Update()
    {
        transform.Translate(Vector3.down * (Time.deltaTime * speed));

        if (transform.position.y < -6.5f)
        {
            transform.position = new Vector3(Random.Range(-9f, 9f), 6.5f, 0);
        }

        if (this.gameObject.transform.IsDestroyed())
        {
            Instantiate(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform.name.Equals("Player"))
        {
            var player = other.transform.GetComponent<Player>();
            if (player != null)
            {
                player.Damage();
            }
            
            Destroy(this.gameObject);
        }
        else if (other.transform.tag.Equals("Laser"))
        {
            Destroy(other.gameObject);
            if (_player != null)
            {
                _player.AddScore();
            }
            Destroy(this.gameObject);
        }
    }
}
