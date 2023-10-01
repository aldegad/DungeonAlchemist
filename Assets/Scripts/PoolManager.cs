using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    // .. �����յ��� ������ ����
    public GameObject[] BulletPrefabs;
    public GameObject[] EnemyPrefabs;

    // .. Ǯ ����� �ϴ� ����Ʈ��
    List<GameObject>[] BulletPools;
    List<GameObject>[] EnemyPools;
    private void Awake()
    {
        // Ǯ�� �ʱ�ȭ �ؾ��Ѵ�.... ��...?
        BulletPools = new List<GameObject>[BulletPrefabs.Length];
        for (int i = 0; i < BulletPools.Length; i++)
        {
            BulletPools[i] = new List<GameObject>();
        }

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

    public GameObject GetEnemy(int index)
    {
        GameObject select = null;

        // ... ������ Ǯ�� ��� �ִ� (��Ȱ��ȭ��) ���ӿ�����Ʈ ����
        // ... �߰��ϸ� select ������ �Ҵ�
        foreach (GameObject item in EnemyPools[index])
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
            select = Instantiate(EnemyPrefabs[index], transform);
            EnemyPools[index].Add(select);
        }

        return select;
    }
}