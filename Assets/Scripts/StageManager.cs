using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    public float maxStageTime = 20f;
    public Vector2 playerInitPosition;
    public SpawnData[] spawnData;

    Transform[] spawnPoint;
    float stageTime;

    void Awake()
    {
        spawnPoint = GetComponentsInChildren<Transform>();
    }

    void Update()
    {

        stageTime += Time.deltaTime;

        // �������� �ð� ǥ��
        GameManager.Instance.setStageTime(maxStageTime - stageTime);

        // �� �� ����
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
        //GameObject SpawnObj = GameManager.Instance.pool.EnemySpawnPrefab;
        GameObject Enemy = GameManager.Instance.pool.GetEnemy(data.spawnEnemyPrefabIndex);
        Enemy.transform.position = spawnPoint[Random.Range(1, spawnPoint.Length)].position;
        Enemy.GetComponent<Enemy>().Init(data);
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