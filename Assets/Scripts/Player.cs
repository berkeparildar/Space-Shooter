using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.Serialization;

public class Player : MonoBehaviour
{
    private float _speed = 5f;
    [SerializeField]
    private GameObject laserPrefab;
    private float _canFire = -1f;
    private const float FireRate = 0.15f;
    [SerializeField]
    private int lives = 3;
    private SpawnManager _spawnManager;
    private bool _tripleShotActive = false;
    private bool _shieldActive = false;
    [SerializeField]
    private GameObject tripleShotPrefab;
    [SerializeField]
    private GameObject shieldEffect;
    [SerializeField] private int _score;
    private UIManager _uiManager;

    public int Score
    {
        get => _score;
        set => _score = value;
    }

    private void Start()
    {
       transform.position = new Vector3(0, 0, 0);
       _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
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
    }

    private void CalculateMovement()
    {
        var horizontalInput = Input.GetAxis("Horizontal");
        var verticalInput = Input.GetAxis("Vertical");
        var direction = new Vector3(horizontalInput, verticalInput);
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
            shieldEffect.SetActive(_shieldActive);
            return;
        }
        lives -= 1;
        if (lives == 0)
        {
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
        _uiManager.UpdateScore(_score);
    }
}
