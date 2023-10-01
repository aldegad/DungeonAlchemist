using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    // .. �����յ��� ������ ����
    public GameObject[] prefabs;

    // .. Ǯ ����� �ϴ� ����Ʈ��
    List<GameObject>[] pools;
    private void Awake()
    {
        // Ǯ�� �ʱ�ȭ �ؾ��Ѵ�.... ��...?
        pools = new List<GameObject>[prefabs.Length];

        for (int i = 0; i < pools.Length; i++)
        { 
            pools[i] = new List<GameObject>();
        }
    }
    public GameObject Get(int index)
    {
        GameObject select = null;

        // ... ������ Ǯ�� ��� �ִ� (��Ȱ��ȭ��) ���ӿ�����Ʈ ����
        // ... �߰��ϸ� select ������ �Ҵ�

        foreach (GameObject item in pools[index])
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
            select = Instantiate(prefabs[index], transform);
            pools[index].Add(select);
        }

        return select;
    }
}