using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicalStone : MonoBehaviour
{
    public float speed = 5f;
    Rigidbody2D rigid;
    Collider2D collision;
    // Start is called before the first frame update
    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        collision = GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (GameManager.Instance.stageManager.isStageClear)
        {
            PlayerManager playerManager = GameManager.Instance.player;
            Vector2 distance = playerManager.transform.position - transform.position;
            distance = distance.normalized;
            float moveSpeed = speed * Time.deltaTime * 60f;
            speed += Time.deltaTime * 10f;

            collision.isTrigger = true;
            rigid.velocity = distance * moveSpeed;
            rigid.gravityScale = 0;
        }
    }
}
