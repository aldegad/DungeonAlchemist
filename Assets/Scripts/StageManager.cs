using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public float maxStageTime = 20f;
    public SpawnData[] spawnData;

    Transform[] spawnPoint;
    int level;
    float stageTime;
    float timer;

    private void Awake()
    {
        spawnPoint = GetComponentsInChildren<Transform>();
    }
    void Update()
    {

        stageTime += Time.deltaTime;
        level = Mathf.FloorToInt(GameManager.Instance.gameTime / 10f);

        // 스테이지 시간 표시
        GameManager.Instance.setStageTime(maxStageTime - stageTime);

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
    }

    void Spawn(SpawnData data)
    {
        GameObject enemy = GameManager.Instance.pool.GetEnemy(data.spawnEnemyPrefabIndex);
        enemy.transform.position = spawnPoint[Random.Range(1, spawnPoint.Length)].position;
        enemy.GetComponent<EnemyMove>().Init(data);
    }
}

[System.Serializable]
public class SpawnData
{
    public int spawnEnemyPrefabIndex = 0;
    public int maxSpawnCount = 10;
    public float spawnDelay = 1f;
    public float spawnCycle = 1f;
    public float health = 1f;
    public float speed = 1f;

    public float lastSpawnTime = 0f;
    public int spawnCount = 0;
}