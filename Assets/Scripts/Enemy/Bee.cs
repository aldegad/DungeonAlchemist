using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bee : EnemyData
{
    public override void OnInit()
    {
        OnReset();
        rigid.gravityScale = 0;
    }
    public override void OnMove(Rigidbody2D target, float speed)
    {
        // ���� �������� ���� - �߷� ������� �޿��� �̵���
        distance = target.transform.position - transform.position;
        distance = distance.normalized;

        float moveSpeed = 5 * Time.fixedDeltaTime * 60;

        if (distance.x < 0)
        {
            spriteRenderer.flipX = true;
        }
        else
        {
            spriteRenderer.flipX = false;
        }

        rigid.AddForce(distance * moveSpeed, ForceMode2D.Impulse);

        if (Mathf.Abs(rigid.velocity.magnitude) > speed)
        {
            rigid.velocity = rigid.velocity.normalized * speed;
        }
    }
}
