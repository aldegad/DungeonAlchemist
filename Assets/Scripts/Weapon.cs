using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public int prefabId;
    public float damage;
    public int count;
    public float speed;

    private void Start()
    {
        Init();
    }

    // Update is called once per frame
    void Update()
    {
        switch (prefabId)
        {
            case 0:
                
                break;
            case 1:
                transform.Rotate(Vector3.forward * speed * Time.deltaTime);
                break;
            default:
                break;
        }


    }

    public void LevelUp(float damage, int count)
    {
        this.damage = damage;
        this.count = count;

        if (prefabId == 1)
        {
            Batch();
        }
    }

    public void Init()
    {
        switch (prefabId)
        { 
            case 0:
                
                break;
            case 1:
                speed = 150;
                Batch();
                break;
            default:
                break;
        }
    }
    void Batch()
    {
        for (int i = 0; i < count; i++)
        {
            Transform bullet;


            // 기존에 있는건 있는걸 활용하고, 아니면 추가.
            if (i < transform.childCount)
            {
                bullet = transform.GetChild(i);
            }
            else
            { 
                bullet = GameManager.Instance.pool.GetBullet(prefabId).transform;
            }
            bullet.parent = transform;

            bullet.localPosition = Vector3.zero;
            bullet.localRotation = Quaternion.identity;

            Vector3 rotVec = Vector3.forward * 360 * i / count;
            bullet.Rotate(rotVec);
            bullet.Translate(bullet.up * 1.5f, Space.World);

            bullet.GetComponent<Bullet>().Init(damage, -1); // -1 is Infinity Per.
        }
    }
}
