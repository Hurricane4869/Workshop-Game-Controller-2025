using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private float spawnInterval;
    private float timer = 0f;

    void Start()
    {
        GameObject spawner =  GameObject.FindGameObjectWithTag("Spawner");
        spawnPoint = spawner.transform;       
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
        if(enemyPrefab != null)
        {
            Vector3 spawnPos = new Vector3 
            (
                spawnPoint.position.x,
                spawnPoint.position.y
            );
            GameObject enemy = Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
        }
    }
}