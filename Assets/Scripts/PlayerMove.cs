using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [SerializeField] GameManager gameManager;
    [SerializeField] float maxSpeed = 8f;
    [SerializeField] float acceleration = 1f;
    [SerializeField] float jumpPower = 15f;

    Rigidbody2D rigid;
    CapsuleCollider2D capsuleCollider;
    SpriteRenderer spriteRenderer;
    Animator animator;
    bool jumpUp = false;
    bool jumpDown = false;
    bool isDamage = false;

    // Start is called before the first frame update
    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        //Jump
        if (OnJumpButtonDown() && !animator.GetBool("isJump"))
        {
            rigid.velocity = Vector2.up * jumpPower;
            setJumpUp();
        }

        if (rigid.velocity.y < 0)
        {
            setJumpDown();
        }

        // Landing Platform
        Debug.DrawRay(rigid.position, Vector3.down * 1.5f, new Color(1, 0, 0));
        if (rigid.velocity.y < 0)
        {
            int PlatformLayerMask = LayerMask.GetMask("Platform", "PlatformJumpable");
            RaycastHit2D raycastHit = Physics2D.Raycast(rigid.position, Vector3.down, 1.5f, PlatformLayerMask);

            if (raycastHit.collider != null)
            {
                setJumpEnd();
            }
        }

        // Working
        if (Mathf.Abs(rigid.velocity.x) > 0.1)
        {
            animator.SetBool("isWorking", true);
        }
        else
        {
            animator.SetBool("isWorking", false);
        }

        // Stop speed
        if (Input.GetButtonUp("Horizontal"))
        {
            rigid.velocity = new Vector2(rigid.velocity.normalized.x * 0.1f, rigid.velocity.y);
        }

        // Direction
        if (Input.GetButton("Horizontal"))
        {
            spriteRenderer.flipX = Input.GetAxisRaw("Horizontal") == -1;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // Move Speed
        float h = Input.GetAxisRaw("Horizontal") * acceleration;
        rigid.AddForce(Vector2.right * h, ForceMode2D.Impulse);

        // Max Speed
        if(Mathf.Abs(rigid.velocity.x) > maxSpeed)
        {
            rigid.velocity = new Vector2 (rigid.velocity.normalized.x * maxSpeed, rigid.velocity.y);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            if (jumpDown && transform.position.y > collision.transform.position.y)
            {
                OnAttack(collision.gameObject);
            }
            else if(!isDamage)
            {
                OnDamaged(collision.transform.position);
            }
        }
        
        else if (collision.gameObject.CompareTag("Item"))
        {
            // Deactive Item
            collision.gameObject.SetActive(false);

            // Point
            bool isPoint = collision.gameObject.name.Contains("Point");
            if (isPoint)
            {
                gameManager.stagePoint += 1;
            }
        }

        else if (collision.gameObject.CompareTag("Finish"))
        {
            // next stage
            gameManager.NextStage();
        }
    }

    /*private void OnTriggerEnter2D(Collider2D collision)
    {
        // 이런것도 있드라.
    }*/

    bool OnJumpButtonDown()
    {
        if (Input.GetButtonDown("Jump")) return true;

        float verticalInput = Input.GetAxisRaw("Vertical");
        if (Input.GetButtonDown("Vertical"))
        {
            if (verticalInput > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        return false;
    }

    void setJumpUp()
    {
        jumpUp = true;
        jumpDown = false;
        animator.SetBool("isJump", true);
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("PlatformJumpable"), true);
    }
    void setJumpDown()
    {
        jumpUp = false;
        jumpDown = true;
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("PlatformJumpable"), false);
    }
    void setJumpEnd()
    {
        jumpUp = false;
        jumpDown = false;
        animator.SetBool("isJump", false);
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("PlatformJumpable"), false);
    }

    void OnDamaged(Vector2 targetPos)
    {
        isDamage = true;
        // Health Down
        gameManager.HealthDown(1);

        // Change Layer (Immortal Active)
        // gameObject.layer = 11;
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Enemy"), true);


        // View Alpha
        spriteRenderer.color = new Color(1, 1, 1, 0.4f);

        // Reaction Force
        int dirc = transform.position.x - targetPos.x > 0 ? 1 : -1;
        rigid.velocity = new Vector2(dirc, 1)*10;

        Invoke("OffDamaged", 1);
    }

    void OffDamaged()
    {
        // gameObject.layer = 10;
        spriteRenderer.color = new Color(1, 1, 1, 1);
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Enemy"), false);
        isDamage = false;
    }

    void OnAttack(GameObject enemy)
    {
        // Reaction Force
        rigid.velocity = Vector2.up * 10;

        // Enemy Damaged
        /*EnemyMove enemyMove = enemy.GetComponent<EnemyMove>();
        enemyMove.OnDamaged();*/
    }

    public void OnDie()
    {
        spriteRenderer.color = new Color(1, 1, 1, 0.4f);

        spriteRenderer.flipY = true;

        capsuleCollider.enabled = false;

        rigid.velocity = Vector2.up * 5;
    }

    public void VelocityZero()
    {
        rigid.velocity = Vector2.zero;
    }
}
