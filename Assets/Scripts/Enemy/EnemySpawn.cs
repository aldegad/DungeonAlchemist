using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{
    public GameObject enemy;

    public void OnEnemySpawn()
    {
        enemy.SetActive(true);
    }

    public void OnAnimationEnd()
    {
        gameObject.SetActive(false);
    }
}
