using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private List<Transform> spawnPoints;
    [SerializeField] private float spawnInterval = 1f;
    private float timer = 0f;

    void Start()
    {
        GameObject[] spawnerObjects =  GameObject.FindGameObjectsWithTag("Spawner");
        
        foreach(GameObject spawner in spawnerObjects)
        {
            spawnPoints.Add(spawner.transform);
        }

        if(spawnPoints.Count == 0)
        {
            Debug.LogWarning("Tidak ditemukan GameObject dengan tag 'Spawner' dalam scene!");
            return;
        }

        SpawnEnemy();
    }

    void Update() 
    {
        timer += Time.deltaTime;
        if(timer > spawnInterval)
        {
            SpawnEnemy();
            timer = 0f;
        }    
    }

    public void SpawnEnemy()
    {
        if(enemyPrefab == null || spawnPoints.Count == 0) return; 
        
        int index = UnityEngine.Random.Range(0, spawnPoints.Count);
        Vector3 spawnPos = spawnPoints[index].position;
        Instantiate(enemyPrefab, spawnPos, Quaternion.identity);        
    }
}