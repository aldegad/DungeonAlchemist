using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    // .. �����յ��� ������ ����
    public GameObject[] BulletPrefabs;
    public GameObject EnemySpawnPrefab;
    public GameObject[] EnemyPrefabs;

    // .. Ǯ ����� �ϴ� ����Ʈ��
    List<GameObject>[] BulletPools;
    List<GameObject> EnemySpawnPools;
    List<GameObject>[] EnemyPools;

    private void Awake()
    {
        // Ǯ �ʱ�ȭ => ������Ʈ���� ���� ������ ����
        BulletPools = new List<GameObject>[BulletPrefabs.Length];
        for (int i = 0; i < BulletPools.Length; i++)
        {
            BulletPools[i] = new List<GameObject>();
        }
        EnemySpawnPools = new List<GameObject>();
        EnemyPools = new List<GameObject>[EnemyPrefabs.Length];
        for (int i = 0; i < EnemyPools.Length; i++)
        {
            EnemyPools[i] = new List<GameObject>();
        }
    }
    public GameObject GetBullet(int index)
    {
        GameObject select = null;

        // ... ������ Ǯ�� ��� �ִ� (��Ȱ��ȭ��) ���ӿ�����Ʈ ����
        // ... �߰��ϸ� select ������ �Ҵ�
        foreach (GameObject item in BulletPools[index])
        {
            if (!item.activeSelf)
            { 
                select = item;
                select.SetActive(true);
                break;
            }
        }

        // ... �� ã������?
        // ... ���Ӱ� �����ϰ� select ������ �Ҵ�
        if (!select)
        {
            select = Instantiate(BulletPrefabs[index], transform);
            BulletPools[index].Add(select);
        }

        return select;
    }

    public GameObject GetEnemySpawn(GameObject enemy)
    {
        GameObject select = null;

        foreach (GameObject spawn in EnemySpawnPools)
        {
            if (!spawn.activeSelf)
            {
                select = spawn;
                select.SetActive(true);
                break;
            }
        }

        if (!select)
        {
            select = Instantiate(EnemySpawnPrefab, transform);
            EnemySpawnPools.Add(select);
        }

        // enemySpawn object�� enemy�� ������ �־��ش�.
        // enemy�� enemySpawn�ڽ��� ũ�⸦ ������ �־, enemy���� boxũ�Ⱑ �ٸ���.
        EnemySpawn enemySpawn = select.GetComponent<EnemySpawn>();
        Enemy enemyScript = enemy.GetComponent<Enemy>();
        enemySpawn.enemy = enemy;
        enemySpawn.transform.localScale = Vector3.one * enemyScript.spawnBoxSize;
        enemySpawn.transform.position = new Vector3(enemyScript.spawnBoxOffsetX, enemyScript.spawnBoxOffsetY, 0);

        return select;
    }

    public GameObject GetEnemy(int index)
    {
        GameObject select = null;

        // ... ������ Ǯ�� ��� �ִ� (��Ȱ��ȭ��) ���ӿ�����Ʈ ����
        // ... �߰��ϸ� select ������ �Ҵ�
        foreach (GameObject enemy in EnemyPools[index])
        {
            if (!enemy.GetComponent<Enemy>().isActive)
            {
                select = enemy;
                break;
            }
        }

        // ... �� ã������?
        // ... ���Ӱ� �����ϰ� select ������ �Ҵ�
        if (!select)
        {
            select = Instantiate(EnemyPrefabs[index], transform);
            // ���� �ִϸ��̼� ���� ���� enemy�� active �ȴ�.
            select.SetActive(false);
            EnemyPools[index].Add(select);
        }

        return select;
    }
}