using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    // .. 프리팹들을 보관할 변수
    public GameObject[] BulletPrefabs;
    public GameObject[] EnemyPrefabs;

    // .. 풀 담당을 하는 리스트들
    List<GameObject>[] BulletPools;
    List<GameObject>[] EnemyPools;
    private void Awake()
    {
        // 풀을 초기화 해야한다.... 왜...?
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

        // ... 선택한 풀의 놀고 있는 (비활성화된) 게임오브젝트 접근
        // ... 발견하면 select 변수에 할당
        foreach (GameObject item in BulletPools[index])
        {
            if (!item.activeSelf)
            { 
                select = item;
                select.SetActive(true);
                break;
            }
        }

        // ... 못 찾았으면?
        // ... 새롭게 생성하고 select 변수에 할당
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

        // ... 선택한 풀의 놀고 있는 (비활성화된) 게임오브젝트 접근
        // ... 발견하면 select 변수에 할당
        foreach (GameObject item in EnemyPools[index])
        {
            if (!item.activeSelf)
            {
                select = item;
                select.SetActive(true);
                break;
            }
        }

        // ... 못 찾았으면?
        // ... 새롭게 생성하고 select 변수에 할당
        if (!select)
        {
            select = Instantiate(EnemyPrefabs[index], transform);
            EnemyPools[index].Add(select);
        }

        return select;
    }
}