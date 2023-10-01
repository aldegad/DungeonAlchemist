using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    [SerializeField] float speed = 1f;
    [SerializeField] float health = 10f;
    [SerializeField] float maxHealth = 10f;
    [SerializeField] Rigidbody2D target;

    bool isLive = false;

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
        distance = transform.position - target.transform.position;

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

    private void OnEnable()
    {
        target = GameManager.Instance.player.GetComponent<Rigidbody2D>();
        isLive = true;
        health = maxHealth;
    }

    public void Init(SpawnData data)
    {
        speed = speed * data.speed;
        maxHealth = maxHealth * data.health;
        health = maxHealth;
    }

    public void OnDamaged()
    {
        spriteRenderer.color = new Color(1,1,1,0.4f);

        spriteRenderer.flipY = true;

        collision.enabled = false;

        rigid.velocity = Vector2.up * 5;

        Invoke("DeActive", 3);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Bullet"))
        {
            return;
        }

        health -= collision.GetComponent<Bullet>().damage;

        if (health > 0)
        {
            // ... Live, Hit Action
        }
        else
        {
            // ... Die
            Dead();
        }
    }

    void Dead()
    {
        gameObject.SetActive(false);
    }
}
