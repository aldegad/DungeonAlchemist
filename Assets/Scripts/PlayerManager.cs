using UnityEngine;

public class PlayerManager : MonoBehaviour
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

    // rays
    bool isRayPlatformHIt = false;
    bool isRayPlatformHitForLanding = false;
    bool isRayEnemyHitForStepJump = false;

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
        // rays
        isRayPlatformHIt = OnDrawPlatformHit_DuringJump();
        isRayPlatformHitForLanding = OnDrawPlatformHit_ForLanding();
        isRayEnemyHitForStepJump = OnDrawEnemyHit_ForStepJump();

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
        if (!isRayPlatformHIt)
        {
            if (rigid.velocity.y < 0)
            {
                Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("PlatformJumpable"), false);
                
                if (isRayPlatformHitForLanding)
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
            if (jumpDown && isRayEnemyHitForStepJump)
            {
                OnStepJump(collision.gameObject);
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
                PlaySound("ITEM");
            }
        }

        else if (collision.gameObject.CompareTag("Finish"))
        {
            // next stage
            GameManager.Instance.StageClear();
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

        Invoke("OffDamaged", 0.2f);
    }

    void OffDamaged()
    {
        // gameObject.layer = 10;
        spriteRenderer.color = new Color(1, 1, 1, 1);
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Enemy"), false);
        isDamage = false;
    }

    void OnStepJump(GameObject enemy)
    {
        // Reaction Force
        rigid.velocity = Vector2.up * 10;
        PlaySound("ATTACK");
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

    RaycastHit2D OnDrawPlayerLaycast(Vector2 start, Vector2 direction, float distance, int layerMask, Color color)
    {
        Debug.DrawRay(rigid.position + start, direction * distance, color);
        RaycastHit2D raycastHit = Physics2D.Raycast(rigid.position + start, direction, distance, layerMask);
        return raycastHit;
    }
    bool OnDrawPlatformHit_DuringJump()
    {
        int PlatformLayerMask = LayerMask.GetMask("Platform", "PlatformJumpable");
        RaycastHit2D raycastHitTop = OnDrawPlayerLaycast(new Vector2(-0.5f, 0.3f), Vector2.right, 1f, PlatformLayerMask, new Color(1, 1, 0));
        RaycastHit2D raycastHitLeft = OnDrawPlayerLaycast(new Vector2(-0.5f, 0.3f), Vector2.down, 1.25f, PlatformLayerMask, new Color(1, 1, 0));
        RaycastHit2D raycastHitRight = OnDrawPlayerLaycast(new Vector2(0.5f, 0.3f), Vector2.down, 1.25f, PlatformLayerMask, new Color(1, 1, 0));
        RaycastHit2D raycastHitBottom = OnDrawPlayerLaycast(new Vector2(-0.5f, -0.95f), Vector2.right, 1f, PlatformLayerMask, new Color(1, 1, 0));

        return raycastHitTop.collider || raycastHitLeft.collider || raycastHitRight.collider || raycastHitBottom.collider;
    }

    bool OnDrawPlatformHit_ForLanding()
    {
        int PlatformLayerMask = LayerMask.GetMask("Platform", "PlatformJumpable");
        RaycastHit2D raycastHitLeft = OnDrawPlayerLaycast(new Vector2(-0.3f, 0), Vector2.down, 1.3f, PlatformLayerMask, new Color(1, 0, 0));
        RaycastHit2D raycastHitCenter = OnDrawPlayerLaycast(Vector2.zero, Vector2.down, 1.3f, PlatformLayerMask, new Color(1, 0, 0));
        RaycastHit2D raycastHitRight = OnDrawPlayerLaycast(new Vector2(0.3f, 0), Vector2.down, 1.3f, PlatformLayerMask, new Color(1, 0, 0));
        return raycastHitLeft.collider || raycastHitCenter.collider || raycastHitRight.collider;
    }

    bool OnDrawEnemyHit_ForStepJump()
    {
        int layerMask = LayerMask.GetMask("Enemy");
        RaycastHit2D raycastHitLeft = OnDrawPlayerLaycast(new Vector2(-0.3f, -0.9f), Vector2.down, 0.3f, layerMask, new Color(0, 1, 0));
        RaycastHit2D raycastHitCenter = OnDrawPlayerLaycast(new Vector2(0f, -0.9f), Vector2.down, 0.3f, layerMask, new Color(0, 1, 0));
        RaycastHit2D raycastHitRight = OnDrawPlayerLaycast(new Vector2(0.3f, -0.9f), Vector2.down, 0.3f, layerMask, new Color(0, 1, 0));
        return raycastHitLeft.collider || raycastHitCenter.collider || raycastHitRight.collider;
    }
}
