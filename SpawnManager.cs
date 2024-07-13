using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{

    [SerializeField]
    private GameObject _enemyPrefab;
    [SerializeField]
    private GameObject _enemyContainer;

    [SerializeField]
    private GameObject[] powerups;

    private bool _stopSpawning = false;


    void Update()
    {

    }

    public void StartSpawning()
    {
        StartCoroutine(SpawnEnemyRoutine());
        StartCoroutine(SpawnPowerupRoutine());
    }


    IEnumerator SpawnEnemyRoutine()      //needs a yield event, otherwise the computer will crash
    {
        yield return new WaitForSeconds(3.0f);

        while (_stopSpawning == false)
        {
            Vector3 posToSpawn = new Vector3(Random.Range(-8f, 8f), 7, 0);
            GameObject newEnemy = Instantiate(_enemyPrefab, posToSpawn, Quaternion.identity);
            newEnemy.transform.parent = _enemyContainer.transform;
            yield return new WaitForSeconds(5f);
        }

        //WE NEVER GET HERE (because we never exit the while loop) --> if _stopSpawning never becomes true

    }


    IEnumerator SpawnPowerupRoutine()
    {
        yield return new WaitForSeconds(3.0f);

        while (_stopSpawning == false)
        {
            // every 3-7 seconds, spawn a powerup
            Vector3 posToSpawnPowerup = new Vector3(Random.Range(-9f, 9f), 8.5f, 0);
            int randomPowerUp = Random.Range(0, 2);
            Instantiate(powerups[randomPowerUp], posToSpawnPowerup, Quaternion.identity);
            yield return new WaitForSeconds(Random.Range(3.0f, 7.0f));
        }
    }


    public void OnPlayerDeath()
    {
        _stopSpawning = true;
    }

}
