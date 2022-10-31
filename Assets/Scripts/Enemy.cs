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
    private float _speed = 4f;
    private Player _player;
    private Animator _animator;
    // Start is called before the first frame update
    private void Start()
    {
        _animator = GetComponent<Animator>();
        _player = GameObject.Find("Player").GetComponent<Player>();
    }

    // Update is called once per frame
    private void Update()
    {
        transform.Translate(Vector3.down * (Time.deltaTime * _speed));

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
            _animator.SetTrigger("OnEnemyDeath");
            _speed = 0;
            Destroy(this.gameObject, 2.5f);
        }
        else if (other.transform.tag.Equals("Laser"))
        {
            Destroy(other.gameObject);
            if (_player != null)
            {
                _player.AddScore();
            }
            _animator.SetTrigger("OnEnemyDeath");
            _speed = 0;
            Destroy(this.gameObject, 2.5f);
        }
    }
}
