using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bee : EnemyData
{
    Rigidbody2D rigid;
    SpriteRenderer spriteRenderer;
    Vector2 distance;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        distance = Vector2.zero;
    }
    public override void OnInit()
    {
        rigid = GetComponent<Rigidbody2D>();
        rigid.gravityScale = 0;
    }
    public override void OnMove(Rigidbody2D target, float speed)
    {
        // 유저 방향으로 돌진 - 중력 영향없이 쭈우우욱 이동함
        distance = transform.position - target.transform.position;
        distance = distance.normalized;

        float moveSpeed = speed * Time.fixedDeltaTime * 60;

        if (distance.x < 0)
        {
            rigid.velocity = new Vector2(moveSpeed, rigid.velocity.y);
            spriteRenderer.flipX = true;
        }
        else
        {
            rigid.velocity = new Vector2(moveSpeed * -1, rigid.velocity.y);
            spriteRenderer.flipX = false;
        }
    }
}
