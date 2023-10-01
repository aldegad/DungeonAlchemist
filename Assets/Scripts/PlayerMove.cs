using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float maxSpeed = 8f;
    public float acceleration = 1f;
    public float jumpPower = 15f;

    Rigidbody2D rigid;
    SpriteRenderer spriteRenderer;
    Animator animator;
    bool jumpDown = false;

    // Start is called before the first frame update
    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        //Jump
        if (OnJumpButtonDown() && !animator.GetBool("isJump"))
        {
            rigid.velocity = Vector2.up * jumpPower;
            animator.SetBool("isJump", true);
            jumpDown = false;
        }
        if (rigid.velocity.y < 0)
        {
            jumpDown = true;
        }

        // Landing Platform
        Debug.DrawRay(rigid.position, Vector3.down * 1.5f, new Color(1, 0, 0));
        if (rigid.velocity.y < 0)
        {
            RaycastHit2D raycastHit = Physics2D.Raycast(rigid.position, Vector3.down, 1.5f, LayerMask.GetMask("Platform"));

            if (raycastHit.collider != null)
            {
                animator.SetBool("isJump", false);
                jumpDown = false;
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
            else
            {
                OnDamaged(collision.transform.position);
            }
            
        }
    }

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

    void OnDamaged(Vector2 targetPos)
    {
        gameObject.layer = 11;

        // View Alpha
        spriteRenderer.color = new Color(1, 1, 1, 0.4f);

        // Reaction Force
        int dirc = transform.position.x - targetPos.x > 0 ? 1 : -1;
        rigid.velocity = new Vector2(dirc, 1)*10;

        Invoke("OffDamaged", 1);
    }

    void OffDamaged()
    {
        gameObject.layer = 10;
        spriteRenderer.color = new Color(1, 1, 1, 1);
    }

    void OnAttack(GameObject enemy)
    {
        // Reaction Force
        rigid.velocity = Vector2.up * 10;

        // Enemy Damaged
        /*EnemyMove enemyMove = enemy.GetComponent<EnemyMove>();
        enemyMove.OnDamaged();*/
    }
}
