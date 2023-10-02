using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] float maxSpeed = 8f;
    [SerializeField] float acceleration = 1f;
    [SerializeField] float jumpPower = 15f;

    // audio
    [SerializeField] AudioClip audioJump;
    [SerializeField] AudioClip audioAttack;
    [SerializeField] AudioClip audioDamaged;
    [SerializeField] AudioClip audioItem;
    [SerializeField] AudioClip audioDie;
    [SerializeField] AudioClip audioFinish;

    public Scanner scanner;

    Rigidbody2D rigid;
    CapsuleCollider2D capsuleCollider;
    SpriteRenderer spriteRenderer;
    Animator animator;
    AudioSource audioSource;
    //bool jumpUp = false;
    bool jumpDown = false;
    bool isDamage = false;

    // Start is called before the first frame update
    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        //Jump
        if (OnJumpButtonDown() && !animator.GetBool("isJump"))
        {
            rigid.velocity = Vector2.up * jumpPower;
            PlaySound("JUMP");
            setJumpUp();
        }

        if (rigid.velocity.y < 0)
        {
            setJumpDown();
        }

        // Landing Platform
        Debug.DrawRay(rigid.position + new Vector2(-0.5f, 0.3f), Vector2.right * 1f, new Color(1, 1, 0));
        Debug.DrawRay(rigid.position + new Vector2(-0.5f, 0.3f), Vector2.down * 1.25f, new Color(1, 1, 0));
        Debug.DrawRay(rigid.position + new Vector2(0.5f, 0.3f), Vector2.down * 1.25f, new Color(1, 1, 0));
        Debug.DrawRay(rigid.position + new Vector2(-0.5f, -0.95f), Vector2.right * 1f, new Color(1, 1, 0));
        Debug.DrawRay(rigid.position, Vector2.down * 1.2f, new Color(1, 0, 0));

        int PlatformLayerMask = LayerMask.GetMask("Platform", "PlatformJumpable");

        RaycastHit2D raycastHitTop = Physics2D.Raycast(rigid.position + new Vector2(-0.5f, 0.3f), Vector2.right, 1f, PlatformLayerMask);
        RaycastHit2D raycastHitLeft = Physics2D.Raycast(rigid.position + new Vector2(-0.5f, 0.3f), Vector2.down, 1.25f, PlatformLayerMask);
        RaycastHit2D raycastHitRight = Physics2D.Raycast(rigid.position + new Vector2(0.5f, 0.3f), Vector2.down, 1.25f, PlatformLayerMask);
        RaycastHit2D raycastHitBottom = Physics2D.Raycast(rigid.position + new Vector2(-0.5f, -0.95f), Vector2.right, 1f, PlatformLayerMask);
        if (!raycastHitTop.collider && !raycastHitLeft.collider && !raycastHitRight.collider && !raycastHitBottom.collider)
        {
            if (rigid.velocity.y < 0)
            {
                Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("PlatformJumpable"), false);
                RaycastHit2D raycastHit = Physics2D.Raycast(rigid.position, Vector2.down, 1.2f, PlatformLayerMask);

                if (raycastHit.collider != null)
                {
                    setJumpEnd();
                }
            }
        }


        // Working
        if (Mathf.Abs(rigid.velocity.x) > 0.2)
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
            rigid.velocity = new Vector2(0, rigid.velocity.y);
        }

        // Direction
        if (Input.GetButton("Horizontal"))
        {
            spriteRenderer.flipX = Input.GetAxisRaw("Horizontal") == -1;
        }
    }
    private void FixedUpdate()
    {
        // Move Speed
        float h = Input.GetAxisRaw("Horizontal") * acceleration * Time.fixedDeltaTime * 60;
        //Debug.Log(Input.GetAxisRaw("Horizontal"));
        rigid.AddForce(Vector2.right * h, ForceMode2D.Impulse);

        // Max Speed
        if (Mathf.Abs(rigid.velocity.x) > maxSpeed)
        {
            rigid.velocity = new Vector2(rigid.velocity.normalized.x * maxSpeed, rigid.velocity.y);
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
                GameManager.Instance.stagePoint += 1;
                PlaySound("ITEM");
            }
        }

        else if (collision.gameObject.CompareTag("Finish"))
        {
            // next stage
            GameManager.Instance.NextStage();
            PlaySound("FINISH");
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
        //jumpUp = true;
        jumpDown = false;
        animator.SetBool("isJump", true);
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("PlatformJumpable"), true);
    }
    void setJumpDown()
    {
        //jumpUp = false;
        jumpDown = true;
    }
    void setJumpEnd()
    {
        //jumpUp = false;
        jumpDown = false;
        animator.SetBool("isJump", false);
    }

    void OnDamaged(Vector2 targetPos)
    {
        isDamage = true;
        // Health Down
        GameManager.Instance.HealthDown(1);

        // Change Layer (Immortal Active)
        // gameObject.layer = 11;
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Enemy"), true);


        // View Alpha
        spriteRenderer.color = new Color(1, 1, 1, 0.4f);

        // Reaction Force
        int dirc = transform.position.x - targetPos.x > 0 ? 1 : -1;
        rigid.velocity = new Vector2(dirc, 1)*10;

        PlaySound("DAMAGED");

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
        PlaySound("ATTACK");

        // Enemy Damaged
        /*EnemyMove enemyMove = enemy.GetComponent<EnemyMove>();
        enemyMove.OnDamaged();*/
    }

    void PlaySound(string action)
    {
        switch (action) {
            case "JUMP":
                audioSource.clip = audioJump;
                break;
            case "ATTACK":
                audioSource.clip = audioAttack;
                break;
            case "DAMAGED":
                audioSource.clip = audioDamaged;
                break;
            case "ITEM":
                audioSource.clip = audioItem;
                break;
            case "DIE":
                audioSource.clip = audioDie;
                break;
            case "FINISH":
                audioSource.clip = audioFinish;
                break;
        }
        audioSource.Play();
    }

    public void OnDie()
    {
        spriteRenderer.color = new Color(1, 1, 1, 0.4f);

        spriteRenderer.flipY = true;

        capsuleCollider.enabled = false;

        rigid.velocity = Vector2.up * 5;

        PlaySound("DIE");
    }

    public void VelocityZero()
    {
        rigid.velocity = Vector2.zero;
    }
}
