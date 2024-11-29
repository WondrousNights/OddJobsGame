using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class GameManager : NetworkBehaviour
{
    [SerializeField] float timeToNextRound;

    [SerializeField] GameObject boatPrefab;

    GameObject[] spawnPositions;

    float count;

    bool waveSpawned;

    int waveNumber;


    private void Awake()
    {
        count = timeToNextRound;
        spawnPositions = GameObject.FindGameObjectsWithTag("EnemyBoatSpawnPosition");
    }
    private void FixedUpdate()
    {
        if (!IsHost || waveSpawned) return;
        count -= Time.deltaTime;


        if (count <= 0)
        {

            SpawnWave(waveNumber);
            waveSpawned = true;
            count = timeToNextRound;
        }
    }

    void SpawnWave(int count)
    {
        int randNumb = Random.Range(0, spawnPositions.Length);
        GameObject spawnedEnemy = Instantiate(boatPrefab, spawnPositions[randNumb].transform.position, Quaternion.identity);
        spawnedEnemy.GetComponent<NetworkObject>().Spawn(true);
        
    }
}
