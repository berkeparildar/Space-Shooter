using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private GameObject enemyContainer;
    [SerializeField] private GameObject powerUpPrefab;
    [SerializeField] private GameObject[] powerUps;
    private bool stopSpawning = false;
    private void Start()
    {
    }

    public void StartSpawning()
    {
        StartCoroutine(SpawnEnemyRoutine());
        StartCoroutine(SpawnPowerUpRoutine());
    }

    void Update()
    {
        
    }

    private IEnumerator SpawnEnemyRoutine()
    {
        yield return new WaitForSeconds(3.0f);
        while (stopSpawning == false)
        {
            var spawnPlace = new Vector3(Random.Range(-8f, 8f), 7, 0);
            var newEnemy = Instantiate(enemyPrefab, spawnPlace, Quaternion.identity);
            newEnemy.transform.parent = enemyContainer.transform;
            yield return new WaitForSeconds(2.0f);
        }
    }

    private IEnumerator SpawnPowerUpRoutine()
    {
        yield return new WaitForSeconds(3.0f);
        while (stopSpawning == false)
        {
            var spawnPlace = new Vector3(Random.Range(-8f, 8f), 7, 0);
            var newPower = Instantiate(powerUps[Random.Range(0, powerUps.Length)], spawnPlace, Quaternion.identity);
            yield return new WaitForSeconds(Random.Range(3, 8));
        }
    }

    public void OnPlayerDeath()
    {
        stopSpawning = true;
    }
}
