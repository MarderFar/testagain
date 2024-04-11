using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Rigidbody2D rb;
    public Animator anim;
    
    public Vector2 moveVector;
    public float speed = 2f;
    public bool faceright = true;


    public HealthForPlayer PlayerHealth;
    public int maxHealth = 100;
    int currentHelth;

    public float gravity = 2f;

    void Start()
    {
        rb.GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        currentHelth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        walk();
        Reflect();
        CheckingGround();
        PlatwormDown();

    }
    private void FixedUpdate()
    {
        Jump();
    }

    public void TakeDamagePlayer(int playerDamage)
    {
        PlayerHealth.currentHealthPlayer -= playerDamage;
        //animator.SetTrigger("Hurt");

        if (currentHelth <= 0)
        {
            //Die();
        }
    }
    void Reflect()
    {
        if((moveVector.x > 0 && !faceright) || (moveVector.x < 0 && faceright))
        {
            transform.Rotate(0, 180, 0);
            faceright = !faceright;  
        }
    }

    public PlayerAttack player;
    void walk()
    {
        if (!player.IsAttack_3_start & !player.IsAttack_4_start)
        {
            moveVector.x = Input.GetAxisRaw("Horizontal");
            anim.SetFloat("moveX", Mathf.Abs(moveVector.x));
            rb.velocity = new Vector2(moveVector.x * speed, rb.velocity.y);
            rb.gravityScale = gravity;
            Debug.Log("Velocity.x: " + rb.velocity.x);
        }
    }

    public float jumpForce = 800f;
    private bool jumpControl;
    private float jumpTime = 0;
    public float jumpControlTime = 0.7f;

    void Jump()
    {
        if (!Input.GetKey(KeyCode.DownArrow) || (!rb.IsTouchingLayers(PlatformMask) && !Physics2D.OverlapBox(groundCheckTransform.position, groundCheckSize, 0f, PlatformMask)))
            if (Input.GetKey(KeyCode.Space))
            {
                if (onGround)
                {
                    jumpControl = true;
                }

            }
            else { jumpControl = false; }
            if (jumpControl)
            {

                if ((jumpTime += Time.deltaTime) < jumpControlTime)
                {

                    rb.AddForce(Vector2.up * jumpForce / (jumpTime * 10));

                }
            }
            else { jumpTime = 0; }
    }
    void PlatwormDown()
    {
        if (Input.GetKey(KeyCode.DownArrow) && Input.GetKeyDown(KeyCode.Space))
        {
            if (rb.IsTouchingLayers(PlatformMask) && Physics2D.OverlapBox(groundCheckTransform.position, groundCheckSize, 0f, PlatformMask))
            {
                Physics2D.IgnoreLayerCollision(7, 8, true);
                Invoke("IgnoreLayerOff", 0.35f);
            }
        }
    }
    void IgnoreLayerOff()
    {
        Physics2D.IgnoreLayerCollision(7, 8, false);
    }

    [SerializeField] private LayerMask PlatformMask;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private Vector2 groundCheckSize = Vector2.one;
    [SerializeField] private Transform groundCheckTransform;
    public bool onGround = false;
    private Collider2D groundCol;


    public bool CheckingGround()
    {
        if (onGround == false)
        {
            onGround = rb.IsTouchingLayers(groundMask) && Physics2D.OverlapBox(groundCheckTransform.position, groundCheckSize, 0f, groundMask);
        }
        else
        {
            onGround = Physics2D.OverlapBox(groundCheckTransform.position, groundCheckSize, 0f, groundMask);
        }
        anim.SetBool("onGround", onGround);
        return onGround;
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(groundCheckTransform.position, groundCheckSize);
    }
}
