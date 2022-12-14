using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.Serialization;

public class Player : MonoBehaviour
{
    private float _speed = 5f;
    [SerializeField] private GameObject laserPrefab;
    private float _canFire = -1f;
    private const float FireRate = 0.15f;
    [SerializeField] private int lives = 3;
    private SpawnManager _spawnManager;
    private bool _tripleShotActive = false;
    private bool _shieldActive = false;
    [SerializeField] private GameObject tripleShotPrefab;
    [SerializeField] private GameObject shieldEffect;
    [SerializeField] private GameObject _rightEngine, _leftEngine;
    [SerializeField] private int _score;
    private int _bestScore;
    private UIManager _uiManager;
    [SerializeField] private AudioClip _laserSound;
    private AudioSource _audioSource;
    private AudioClip _deathSound;
    private Animator _animator;
    
    private void Start()
    {
        transform.position = new Vector3(0, -3, 0);
       _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
       _audioSource = GetComponent<AudioSource>();
       _animator = GetComponent<Animator>();
       _audioSource.clip = _laserSound;
       _bestScore = PlayerPrefs.GetInt("HighScore", 0);
       if (_spawnManager == null)
       {
           Debug.Log("Spawn is null.");
       }
       _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
    }
    
    private void Update()
    {
        CalculateMovement();
        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire)
        {
            FireLaser();
        }
    }

    private void FireLaser()
    {
        _canFire = Time.time + FireRate;
        var position = transform.position;
        if (_tripleShotActive)
        {
            Instantiate(tripleShotPrefab, new Vector3(position.x, position.y + 1, position.z), Quaternion.identity);
        }
        else{
            Instantiate(laserPrefab, new Vector3(position.x, position.y + 1, position.z),
            Quaternion.identity);
        }
        _audioSource.Play(0);
    }

    private void CalculateMovement()
    {
        var horizontalInput = Input.GetAxis("Horizontal");
        var verticalInput = Input.GetAxis("Vertical");

        if (horizontalInput < 0)
        {
            _animator.SetTrigger("isTurningLeft");
        }
        else if (horizontalInput > 0)
        {
            _animator.SetTrigger("isTurningRight");
        }
        else
        {
            _animator.SetTrigger("none");
        }
        
        transform.Translate(new Vector3(horizontalInput, verticalInput, 0) * (_speed * Time.deltaTime));
        if (transform.position.y >= 0)
        {
            var transform1 = transform;
            var position = transform1.position;
            position = new Vector3(position.x, 0, position.z);
            transform1.position = position;
        }
        else if (transform.position.y <= -4f)
        {
            var transform1 = transform;
            var position = transform1.position;
            position = new Vector3(position.x, -4f, position.z);
            transform1.position = position;
        }

        if (transform.position.x >= 11f)
        {
            var transform1 = transform;
            var position = transform1.position;
            position = new Vector3(-11, position.y, position.z);
            transform1.position = position;
        }
        else if (transform.position.x <= -11f)
        {
            var transform1 = transform;
            var position = transform1.position;
            position = new Vector3(11, position.y, position.z);
            transform1.position = position;
        }
    }

    public void Damage()
    {
        if (_shieldActive)
        {
            _shieldActive = false;
            shieldEffect.SetActive(false);
            return;
        }
        
        lives -= 1;
        _uiManager.UpdateLives(lives);
        if (lives == 2)
        {
            _leftEngine.SetActive(true);
        }

        if (lives == 1)
        {
            _rightEngine.SetActive(true);
        }
        
        if (lives == 0)
        {
            _uiManager.OnPlayerDeath();
            _spawnManager.OnPlayerDeath();
            Destroy(this.gameObject);
        }
        
    }

    public void ActivateTripleShotPowerUp()
    {
        _tripleShotActive = true;
        StartCoroutine(TripleShotPowerRoutine());
    }

    public void ActivateSpeedPowerUp()
    {
        _speed = 8;
        StartCoroutine(SpeedPowerUpRoutine());
    }

    public void ShieldActive()
    {
        _shieldActive = true;
        shieldEffect.SetActive(_shieldActive);
    }

    private IEnumerator SpeedPowerUpRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        _speed = 5;
    }

    private IEnumerator TripleShotPowerRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        _tripleShotActive = false;
    }

    public void AddScore()
    {
        _score += 10;
        BestScore();
        _uiManager.UpdateScore(_score, _bestScore);
    }

    private void BestScore()
    {
        if (_score > _bestScore)
        {
            _bestScore = _score;
            PlayerPrefs.SetInt("HighScore", _bestScore);
        }
    }
}
