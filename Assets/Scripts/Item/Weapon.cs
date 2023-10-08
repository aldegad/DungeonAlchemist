using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public int id; // 이거 어따 쓰는데...?
    public int prefabId = -1;
    public float damage;
    public int count;
    public float attackSpeed;
    public int penetration;

    float timer;

    // Update is called once per frame
    void Update()
    {
        switch (prefabId)
        {
            case 0:
                transform.Rotate(Vector3.forward * 150/attackSpeed * Time.deltaTime);
                break;
            case 1:
                timer += Time.deltaTime;

                if (timer > attackSpeed)
                {
                    FireballFire();
                    for (int i = 0; i < count; i++)
                    {
                        Invoke("FireballFire", i * 0.1f + 1);
                    }
                    timer = 0f;
                }
                break;
            case 2:
                timer += Time.deltaTime;

                if (timer > attackSpeed)
                {
                    BombFire();
                    for (int i = 0; i < count; i++)
                    {
                        Invoke("BombFire", i * 0.1f + 1);
                    }
                    timer = 0f;
                }
                break;
            default:
                break;
        }


    }

    public void Init(ItemData itemData)
    {
        // Basic Set
        name = "Weapon " + itemData.itemId;
        transform.parent = GameManager.Instance.player.transform;
        transform.localPosition = Vector3.zero;

        // Property Set
        id = itemData.itemId;
        damage = itemData.baseDamage;
        count = itemData.baseCount;
        attackSpeed = itemData.baseAttackSpeed;
        penetration = itemData.basePenetration;

        for (int index = 0; index < GameManager.Instance.pool.BulletPrefabs.Length; index++)
        { 
            if(itemData.projectile == GameManager.Instance.pool.BulletPrefabs[index])
            {
                prefabId = index;
                break;
            }
        }
        


        switch (prefabId)
        { 
            case 0:
                Batch();
                break;
            case 1:
                break;
            case 2:
                break;
            default:
                break;
        }

        // 내 모든 아이템의 ApplyGear 호출
        // GameManager.Instance.player.BroadcastMessage("ApplyGear", SendMessageOptions.DontRequireReceiver);
    }

    public void LevelUp(float damage, int count)
    {
        this.damage = damage;
        this.count += count;
        switch (prefabId)
        {
            case 0:
                Batch();
                break;
            case 1:
                break;
            case 2:
                break;
            default:
                break;
        }
        // 내 모든 아이템의 ApplyGear 호출
        // GameManager.Instance.player.BroadcastMessage("ApplyGear", SendMessageOptions.DontRequireReceiver);
    }

    // 0 - Portion
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

            bullet.localPosition = Vector2.zero;
            bullet.localRotation = Quaternion.identity;

            Vector3 rotVec = Vector3.forward * 360 * i / count;
            bullet.Rotate(rotVec);
            bullet.Translate(bullet.up * 1.5f, Space.World);

            bullet.GetComponent<Bullet>().Init(damage, -1); // -1 is Infinity Per.
        }
    }


    // 1 - Bee
    void BombFire()
    {
        if (!GameManager.Instance.player.scanner.nearestTarget)
        {
            return;
        }

        Vector3 targetPos = GameManager.Instance.player.scanner.nearestTarget.position;
        Vector3 dir = targetPos - transform.position;
        float forceX = (dir.x > 10 ? 10 : dir.x) * 0.8f;

        Transform bullet = GameManager.Instance.pool.GetBullet(prefabId).transform;

        bullet.parent = transform;

        bullet.localPosition = Vector3.zero;
        bullet.localRotation = Quaternion.FromToRotation(Vector3.up, dir);

        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();

        rb.AddForce(new Vector2(forceX, 15f), ForceMode2D.Impulse);

        bullet.GetComponent<Bullet>().Init(damage, penetration);
    }

    // 2 - Fireball
    void FireballFire()
    {
        Transform nearestTarget = GameManager.Instance.player.scanner.nearestTarget;
        if (!nearestTarget)
        {
            return;
        }

        Vector3 targetPos = nearestTarget.position;
        Vector3 dir = targetPos - transform.position;
        dir = dir.normalized * 15f;

        Transform bullet = GameManager.Instance.pool.GetBullet(prefabId).transform;

        bullet.parent = transform;

        bullet.localPosition = Vector3.zero;
        bullet.localRotation = Quaternion.FromToRotation(Vector3.up, dir);

        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();

        rb.velocity = dir;

        bullet.GetComponent<Bullet>().Init(damage, penetration);
    }
}