using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    [Header("# 스테이지 정보")]
    public float maxStageTime = 20f;
    public float stageTime = 0f;
    public Vector2 playerInitPosition;
    public SpawnData[] spawnData;



    Transform[] spawnPoints;
    bool isSpawnStageClearPortal = false;
    public bool isStageClear = false;

    void Awake()
    {
        spawnPoints = GetComponentsInChildren<Transform>();
    }

    void Update()
    {
        // 스테이지가 클리어되면 모든 리스폰 및 로직이 멈춘다.
        if (isStageClear)
        {
            GameManager.Instance.pool.BroadcastMessage("Dead", SendMessageOptions.DontRequireReceiver);
            return;
        }

        stageTime += Time.deltaTime;

        // 각 몹 리젠
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

        if (stageTime >= maxStageTime && !isSpawnStageClearPortal)
        {
            // 스테이지 시간이 지나면, 클리어 포탈이 생기고,
            isSpawnStageClearPortal = true;
            SpawnStageClearPortal();
            // 패널티 몹이 나온다.
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

    void SpawnStageClearPortal()
    {
        Transform spawnPoint = spawnPoints[Random.Range(1, spawnPoints.Length)];
        GameManager.Instance.pool.GetStageClearPortal(spawnPoint);
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