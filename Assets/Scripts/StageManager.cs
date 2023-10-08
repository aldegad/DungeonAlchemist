using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    public float maxStageTime = 20f;
    public Vector2 playerInitPosition;
    public SpawnData[] spawnData;

    Transform[] spawnPoints;

    void Awake()
    {
        spawnPoints = GetComponentsInChildren<Transform>();
    }

    void Update()
    {

        float stageTime = GameManager.Instance.gameTime;

        // °Ά Έχ Έ®Α¨
        for (int i = 0; i < spawnData.Length; i++)
        {
            SpawnData data = spawnData[i];

            // Debug.Log(i + ": " + (data.lastSpawnTime + data.spawnDelay + data.spawnCycle).ToString() + " / " + stageTime);
            if (data.spawnDelay > stageTime) 
            {
                data.lastSpawnTime = data.spawnDelay;
            }
            else if (data.spawnCount < data.maxSpawnCount)
            {
                if (data.lastSpawnTime + data.spawnCycle < stageTime)
                {
                    data.lastSpawnTime = stageTime;
                    data.spawnCount++;
                    Spawn(data);
                }
            }
        }
    }

    void Spawn(SpawnData data)
    {
        int prefabIndex = -1;

        for (int index = 0; index < GameManager.Instance.pool.EnemyPrefabs.Length; index++)
        {
            if (data.enemyPrefab == GameManager.Instance.pool.EnemyPrefabs[index])
            {
                prefabIndex = index;
                break;
            }
        }
        GameObject enemy = GameManager.Instance.pool.GetEnemy(prefabIndex);
        GameObject enemySpawn = GameManager.Instance.pool.GetEnemySpawn(enemy);
        Transform spawnPoint = spawnPoints[Random.Range(1, spawnPoints.Length)];

        enemySpawn.transform.position = spawnPoint.position;
        enemy.transform.position = spawnPoint.position;
        enemy.GetComponent<Enemy>().Init(data);
    }
}

[System.Serializable]
public class SpawnData
{
    public GameObject enemyPrefab;
    public int maxSpawnCount = 10;
    public float spawnDelay = 1f;
    public float spawnCycle = 1f;
    public float health = 1f;
    public float speed = 1f;

    public float lastSpawnTime = 0f;
    public int spawnCount = 0;
}