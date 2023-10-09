using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    // .. 프리팹들을 보관할 변수
    public GameObject[] BulletPrefabs;
    public GameObject EnemySpawnPrefab;
    public GameObject[] EnemyPrefabs;
    public GameObject MagicalStonePrefab;
    public GameObject StageEndPortalPrefab;

    // .. 풀 담당을 하는 리스트들
    List<GameObject>[] BulletPools;
    List<GameObject> EnemySpawnPool;
    List<GameObject>[] EnemyPools;
    List<GameObject> MagicalStonePool;
    GameObject StageClearPortal;

    private void Awake()
    {
        // 풀 초기화 => 오프젝트들이 들어가고 관리될 공간
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

    public GameObject GetEnemySpawn(GameObject enemy)
    {
        GameObject select = null;

        // ... 선택한 풀의 놀고 있는 (비활성화된) 게임오브젝트 접근
        // ... 발견하면 select 변수에 할당
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

        // enemySpawn object에 enemy의 정보를 넣어준다.
        // enemy는 enemySpawn박스의 크기를 가지고 있어서, enemy마다 box크기가 다르다.
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

        // ... 선택한 풀의 놀고 있는 (비활성화된) 게임오브젝트 접근
        // ... 발견하면 select 변수에 할당
        foreach (GameObject enemy in EnemyPools[index])
        {
            if (!enemy.GetComponent<Enemy>().isActive)
            {
                select = enemy;
                break;
            }
        }

        // ... 못 찾았으면?
        // ... 새롭게 생성하고 select 변수에 할당
        if (!select)
        {
            select = Instantiate(EnemyPrefabs[index], transform);
            // 스폰 애니메이션 진행 도중 enemy가 active 된다.
            select.SetActive(false);
            EnemyPools[index].Add(select);
        }

        return select;
    }

    public GameObject GetMagicalStone(GameObject enemy)
    {
        GameObject select = null;

        // ... 선택한 풀의 놀고 있는 (비활성화된) 게임오브젝트 접근
        // ... 발견하면 select 변수에 할당
        foreach (GameObject magicalStone in MagicalStonePool)
        {
            if (!magicalStone.activeSelf)
            {
                select = magicalStone;
                select.SetActive(true);
                break;
            }
        }

        // ... 못 찾았으면?
        // ... 새롭게 생성하고 select 변수에 할당
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