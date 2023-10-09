using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    // .. �����յ��� ������ ����
    public GameObject[] BulletPrefabs;
    public GameObject EnemySpawnPrefab;
    public GameObject[] EnemyPrefabs;
    public GameObject MagicalStonePrefab;
    public GameObject StageEndPortalPrefab;

    // .. Ǯ ����� �ϴ� ����Ʈ��
    List<GameObject>[] BulletPools;
    List<GameObject> EnemySpawnPool;
    List<GameObject>[] EnemyPools;
    List<GameObject> MagicalStonePool;
    GameObject StageClearPortal;

    private void Awake()
    {
        // Ǯ �ʱ�ȭ => ������Ʈ���� ���� ������ ����
        BulletPools = new List<GameObject>[BulletPrefabs.Length];
        for (int i = 0; i < BulletPools.Length; i++)
        {
            BulletPools[i] = new List<GameObject>();
        }
        EnemySpawnPool = new List<GameObject>();
        EnemyPools = new List<GameObject>[EnemyPrefabs.Length];
        for (int i = 0; i < EnemyPools.Length; i++)
        {
            EnemyPools[i] = new List<GameObject>();
        }
        MagicalStonePool = new List<GameObject>();
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

        // ... ������ Ǯ�� ��� �ִ� (��Ȱ��ȭ��) ���ӿ�����Ʈ ����
        // ... �߰��ϸ� select ������ �Ҵ�
        foreach (GameObject spawn in EnemySpawnPool)
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
            EnemySpawnPool.Add(select);
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

    public GameObject GetMagicalStone(GameObject enemy)
    {
        GameObject select = null;

        // ... ������ Ǯ�� ��� �ִ� (��Ȱ��ȭ��) ���ӿ�����Ʈ ����
        // ... �߰��ϸ� select ������ �Ҵ�
        foreach (GameObject magicalStone in MagicalStonePool)
        {
            if (!magicalStone.activeSelf)
            {
                select = magicalStone;
                select.SetActive(true);
                break;
            }
        }

        // ... �� ã������?
        // ... ���Ӱ� �����ϰ� select ������ �Ҵ�
        if (!select)
        {
            select = Instantiate(MagicalStonePrefab, transform);
            MagicalStonePool.Add(select);
        }

        // Debug.Log(select);
        select.transform.position = enemy.transform.position;
        select.GetComponent<Rigidbody2D>().AddForce(Vector2.up, ForceMode2D.Impulse);

        return select;
    }

    public GameObject GetStageClearPortal(Transform portalPosition)
    {
        GameObject select = null;

        if (StageClearPortal)
        {
            select = StageClearPortal;
            select.SetActive(true);
        }

        if (!select)
        {
            select = Instantiate(StageEndPortalPrefab, transform);
            StageClearPortal = select;
        }
        // Debug.Log(select);
        select.transform.position = transform.position;
        select.GetComponent<Rigidbody2D>().AddForce(Vector2.up, ForceMode2D.Impulse);

        return select;
    }
}