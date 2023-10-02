using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VineBear : EnemyData
{
    public override void OnInit()
    {
        OnReset();
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
