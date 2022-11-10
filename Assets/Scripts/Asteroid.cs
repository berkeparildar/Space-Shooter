using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class Asteroid : MonoBehaviour
{
    private float _speed = 20.0f;
    [SerializeField]
    private GameObject Explosion;
    private SpawnManager _spawnManager;
    // Start is called before the first frame update
    void Start()
    {
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
    }

    // Update is called once per frame
    private void Update()
    {
        transform.Rotate(Vector3.forward * (Time.deltaTime * _speed));
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.transform.tag == "Laser")
        {
            var position = transform.position;
            Instantiate(Explosion, new Vector3(position.x, position.y, position.z), Quaternion.identity);
            Destroy(col.gameObject);
            _spawnManager.StartSpawning();
            Destroy(this.gameObject, 0.1f);
        }
    }
}
