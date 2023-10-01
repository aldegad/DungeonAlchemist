using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    [SerializeField] float speed = 1f;

    //int nextMove;

    Rigidbody2D rigid;
    SpriteRenderer spriteRenderer;
    CapsuleCollider2D collision;
    Vector2 distance = new Vector2();

    
    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        collision = GetComponent<CapsuleCollider2D>();
        //Think();
    }

    private void FixedUpdate()
    {
        // 유저 방향으로 돌진
        distance = transform.position - GameManager.Instance.player.transform.position;

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

        // 낭떠러지 만나면 점프
        /* Vector2 frontVec = new Vector2(rigid.position.x + nextMove * 0.2f, rigid.position.y);
        Debug.DrawRay(frontVec, Vector3.down * 1.5f, new Color(0, 1, 0));
        RaycastHit2D raycastHit = Physics2D.Raycast(frontVec, Vector3.down, 1.5f, LayerMask.GetMask("Platform"));

        if (raycastHit.collider == null)
        {
            nextMove = nextMove * -1;
        }

        spriteRenderer.flipX = nextMove == 1;*/
    }

    /*void FixedUpdate()
    {
        rigid.velocity = new Vector2(nextMove, rigid.velocity.y);

        // Platform Check
        Vector2 frontVec = new Vector2(rigid.position.x + nextMove*0.2f, rigid.position.y);
        Debug.DrawRay(frontVec, Vector3.down * 1.5f, new Color(0, 1, 0));
        RaycastHit2D raycastHit = Physics2D.Raycast(frontVec, Vector3.down, 1.5f, LayerMask.GetMask("Platform"));

        if (raycastHit.collider == null)
        {
            nextMove = nextMove * -1;
        }

        spriteRenderer.flipX = nextMove == 1;
    }

    void Think()
    {
        
        // 좌우 순찰 로직
        nextMove = Random.Range(-1, 2);

        if (nextMove == 0)
        {
            Think();
        }
    }*/

    public void OnDamaged()
    {
        spriteRenderer.color = new Color(1,1,1,0.4f);

        spriteRenderer.flipY = true;

        collision.enabled = false;

        rigid.velocity = Vector2.up * 5;

        Invoke("DeActive", 3);
    }

    void DeActive()
    { 
        gameObject.SetActive(false);
    }
}
