using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float _speed = 4f;
    private Player _player;
    private Animator _animator;
    private AudioSource _audioSource;
    [SerializeField] private AudioClip _laserClip;
    [SerializeField] private AudioClip _deathClip;
    [SerializeField] private GameObject _enemyLaserPrefab;
    private float _canFire = -1f;
    private float _fireRate;
    // Start is called before the first frame update
    private void Start()
    {
        _animator = GetComponent<Animator>();
        _player = GameObject.Find("Player").GetComponent<Player>();
        _audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    private void Update()
    {
        CalculateMovement();
        FireLaser();
    }
    
    private void FireLaser()
    {
        if (Time.time > _canFire)
        {
            _fireRate = Random.Range(3f, 7f);
            _canFire = Time.time + _fireRate;
            var position = transform.position;
            GameObject enemyLaser = Instantiate(_enemyLaserPrefab, new Vector3(position.x, position.y - 1, position.z), 
                Quaternion.identity);
            var lasers = enemyLaser.GetComponentsInChildren<Laser>();
            lasers[0].AssignEnemyLaser();
            lasers[1].AssignEnemyLaser();
            _audioSource.clip = _laserClip;
            _audioSource.Play();
        }
    }
    
    private void CalculateMovement()
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
            Destroy(this.gameObject, 1.5f);
            _audioSource.clip = _deathClip;
            _audioSource.Play();
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
            Destroy(GetComponent<Collider2D>());
            Destroy(this.gameObject, 1.5f);
            _audioSource.clip = _deathClip;
            _audioSource.Play();
        }
    }
}
