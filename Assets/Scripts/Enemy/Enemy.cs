using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("# 기본 능력치")]
    public float baseSpeed = 1f;
    public float baseMaxHealth = 10f;
    public Rigidbody2D target;

    [Header("# 스폰 관련")]
    public bool isActive = true;
    public float spawnBoxSize = 0.66f;
    public float spawnBoxOffsetX = 0f;
    public float spawnBoxOffsetY = 0f;

    [Header("# 현재 능력치 -- 자동 생성")]
    public float speed = 1f;
    public float maxHealth = 10f;
    public float health = 10f;

    EnemyData enemy;

    bool isHit = false;
    bool isLive = false;

    Rigidbody2D rigid;
    SpriteRenderer spriteRenderer;
    Collider2D collision;

    WaitForSeconds waitForDamaged;

    


    
    void Awake()
    {
        enemy = GetComponent<EnemyData>();

        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        collision = GetComponent<Collider2D>();

        waitForDamaged = new WaitForSeconds(0.1f);
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

    public void Init(SpawnData data)
    {
        // 능력치 초기화 및 스테이지 능력치 보정 셋팅
        isActive = true;
        isLive = true;
        speed = baseSpeed * data.speed;
        maxHealth = baseMaxHealth * data.health;
        health = maxHealth;

        // 몹 위치 및 움직임 리셋
        target = GameManager.Instance.player.GetComponent<Rigidbody2D>();
        
        collision.enabled = true;
        rigid.velocity = Vector3.zero;
        spriteRenderer.color = Color.white;
        spriteRenderer.flipY = false;

        // reset by enemy types
        enemy.OnInit();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Debug.Log(collision.CompareTag("Bullet") + " / " + isLive);
        if (!collision.CompareTag("Bullet") || !isLive)
        {
            return;
        }
        
        float damage = collision.GetComponent<Bullet>().damage;

        health -= damage;

        if (health > 0)
        {
            // ... Live, Hit Action
            // Debug.Log("Enemy Damaged: " + damage);
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

    public void Dead()
    {
        if (!isLive)
            return;

        isLive = false;
        collision.enabled = false;
        
        rigid.gravityScale = 1;
        rigid.velocity = Vector2.up * 5;
        spriteRenderer.color = new Color(1, 1, 1, 0.4f);

        spriteRenderer.flipY = true;

        GameManager.Instance.playerStatus.kill++;
        GameManager.Instance.pool.GetMagicalStone(gameObject);

        StartCoroutine(DeActive());
    }
    IEnumerator DeActive()
    {
        yield return new WaitForSeconds(2f);
        isActive = false;
        gameObject.SetActive(false);
    }
}

// enemy 각각 데이터의 기본이 되는 녀석
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