using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VineBear : EnemyData
{
    Rigidbody2D rigid;
    SpriteRenderer spriteRenderer;
    Vector2 distance;
    Animator animator;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        distance = Vector2.zero;
        animator = GetComponent<Animator>();
    }
    public override void OnInit()
    {
        rigid = GetComponent<Rigidbody2D>();
        rigid.gravityScale = 1;
    }
    public override void OnMove(Rigidbody2D target, float speed)
    {
        // 유저 방향으로 돌진
        distance = transform.position - target.transform.position;

        float moveSpeed = speed * Time.fixedDeltaTime * 60;

        if (moveSpeed > 0.2)
        {
            animator.SetBool("isRun", true);
        }
        else
        {
            animator.SetBool("isRun", false);
        }

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
