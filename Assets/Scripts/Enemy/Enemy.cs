using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] float speed = 1f;
    [SerializeField] float health = 10f;
    [SerializeField] float maxHealth = 10f;
    [SerializeField] Rigidbody2D target;

    bool isHit = false;
    bool isLive = false;

    Rigidbody2D rigid;
    SpriteRenderer spriteRenderer;
    Collider2D collision;
    WaitForSeconds waitForDamaged;

    EnemyData enemy;


    
    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        collision = GetComponent<Collider2D>();
        waitForDamaged = new WaitForSeconds(0.1f);
        enemy = GetComponent<EnemyData>();
    }

    private void FixedUpdate()
    {
        if (!isLive || isHit)
            return;

        enemy.OnMove(target, speed);

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
        // default reset
        target = GameManager.Instance.player.GetComponent<Rigidbody2D>();

        isLive = true;
        collision.enabled = true;
        health = maxHealth;
        rigid.velocity = Vector3.zero;
        spriteRenderer.color = Color.white;
        spriteRenderer.flipY = false;

        // reset by enemy types
        enemy.OnInit();
    }

    public void Init(SpawnData data)
    {
        speed = speed * data.speed;
        maxHealth = maxHealth * data.health;
        health = maxHealth;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Bullet") || !isLive)
        {
            return;
        }

        health -= collision.GetComponent<Bullet>().damage;

        if (health > 0)
        {
            // ... Live, Hit Action
            StartCoroutine(OnDamage());
        }
        else
        {
            // ... Die
            Dead();
        }
    }

    IEnumerator OnDamage()
    {
        isHit = true;
        spriteRenderer.material = GameManager.Instance.EnemyDamagedMaterial;
        Vector3 playerPos = GameManager.Instance.player.transform.position;
        Vector3 dirVec = transform.position - playerPos;
        rigid.AddForce(dirVec.normalized * 5, ForceMode2D.Impulse);
        yield return waitForDamaged;
        spriteRenderer.material = GameManager.Instance.EnemyDefaultMaterial;
        isHit = false;
    }

    void Dead()
    {
        isLive = false;
        collision.enabled = false;
        
        rigid.gravityScale = 1;
        rigid.velocity = Vector2.up * 5;
        spriteRenderer.color = new Color(1, 1, 1, 0.4f);

        spriteRenderer.flipY = true;

        GameManager.Instance.kill++;
        GameManager.Instance.GetExp();

        Invoke("DeActive", 2);
    }
    void DeActive()
    {
        gameObject.SetActive(false);
    }
}


public class EnemyData : MonoBehaviour
{
    protected Rigidbody2D rigid;
    protected SpriteRenderer spriteRenderer;
    protected Vector2 distance;
    protected Animator animator;

    void Awake()
    {
        OnReset();
    }

    public void OnReset()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        distance = Vector2.zero;
        animator = GetComponent<Animator>();
    }

    public virtual void OnInit() {}
    public virtual void OnMove(Rigidbody2D target, float speed) {}
}