using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class EnemySimple : MonoBehaviour
{
    public int maxHealth = 100;
    int currentHelth;

    public Vector2 moveVector;
    public bool faceright = true;
    public Animator animator;
    public Collider2D coll;
    public Rigidbody2D rb;
    public Canvas canvas;
    public HealthBehiviar HealthBar;
    public Collider2D PlayerCollider;

    public Transform attackPointEnemy;
    public float attackRange;
    public LayerMask playerMask;
    public float attackRate = 2f;
    float nextAttackTime = 0f;
    public int enemyDamage = 5;

    Transform player;
    public float speed;

    //Patrol
    public int positionOfPatrol;
    public Transform point;
    public bool movingRight;
    public bool chill = false;
    public bool angry = false;
    public bool goBack = false;
    public int stoppingDistance;
    public Transform EnemyEyes;
    void Start()
    {
        currentHelth = maxHealth;
        HealthBar.SetHelth(currentHelth, maxHealth);
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }
    private void Update()
    {
        EnemyPatrol();
        //EnemyRun();
        //Reflect();
    }

    public void TakeDamage(int damage)
    {
        Debug.Log(damage);
        currentHelth = currentHelth - damage;
        HealthBar.SetHelth(currentHelth, maxHealth);
        Debug.Log(currentHelth);
        animator.SetTrigger("Hurt");
        if (currentHelth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        canvas.enabled = false;
        Debug.Log("enemy died");
        animator.SetBool("isDeath", true);

        rb.constraints = RigidbodyConstraints2D.FreezePositionY;
        coll.enabled = false;
    }
    private void OnDrawGizmosSelected()
    {
        if (attackPointEnemy == null) { return; }
        Gizmos.DrawWireSphere(attackPointEnemy.position, attackRange);
    }
/*    public void EnemyRun()
    {
        if (animator.GetBool("isDeath") != true)
        {
            Vector2 target = new Vector2(player.position.x, rb.position.y);
            Vector2 newPos = Vector2.MoveTowards(rb.position, target, speed * Time.fixedDeltaTime);
            rb.MovePosition(newPos);
        }


        if (Vector2.Distance(player.position, rb.position) <= attackRange * 2)
        {
            if (Time.time >= nextAttackTime)
            {
                animator.SetTrigger("EnemyAttack_1");
                nextAttackTime = Time.time + 1f / attackRate;
            }

        }
    }
    void Reflect()
    {
        if ((transform.position.x < player.position.x && !faceright && chill != true && animator.GetBool("isDeath") != true) || (transform.position.x > player.position.x && faceright && chill != true && animator.GetBool("isDeath") != true))
        {
            transform.Rotate(0, 180, 0);
            faceright = !faceright;
        }
    }*/
    public void EnemyAttack_1()
    {

        PlayerCollider  = Physics2D.OverlapCircle(attackPointEnemy.position, attackRange, playerMask);
        if (PlayerCollider != null)
        {
            Debug.Log(enemyDamage);
            PlayerCollider.GetComponent<PlayerController>().TakeDamagePlayer(enemyDamage);
        }
    }
    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && (Vector2.Distance(transform.position, player.position) < stoppingDistance) && goBack == false && animator.GetBool("isDeath") != true) { angry = true; goBack = false; chill = false; speed = 0.7f; }
    }
    public void EnemyPatrol()
    {
        if (animator.GetBool("isDeath")) { angry = false; goBack = false; chill = false; }

        if (Vector2.Distance(transform.position, point.position) < positionOfPatrol && angry == false && animator.GetBool("isDeath") != true) { chill = true; goBack = false; speed = 0.3f; }

        if (Vector2.Distance(transform.position, point.position) > stoppingDistance && animator.GetBool("isDeath") != true) { goBack = true; angry = false; chill = false; speed = 0.3f; }

        if (chill)
        {
            Patrol(point.position + new Vector3(positionOfPatrol, 0, 0), point.position - new Vector3(positionOfPatrol, 0, 0));
        }
        else if (angry)
        {
            MoveTowards(player.position);
            if (Vector2.Distance(player.position, rb.position) <= attackRange * 2)
            {
                if (Time.time >= nextAttackTime)
                {
                    animator.SetTrigger("EnemyAttack_1");
                    nextAttackTime = Time.time + 1f / attackRate;
                }

            }
        }
        else if (goBack)
        {
            
            MoveTowards(point.position);
        }
    }

    private void Patrol(Vector3 patrolPointRight, Vector3 patrolPointLeft)
    {
        if (transform.position.x > patrolPointRight.x)
        {
            movingRight = false;
            Flip();
        }
        else if (transform.position.x < patrolPointLeft.x)
        {
            movingRight = true;
            Flip();
        }

        if (movingRight) { transform.position = new Vector2(transform.position.x + speed * Time.fixedDeltaTime, transform.position.y); }
        else { transform.position = new Vector2(transform.position.x - speed * Time.fixedDeltaTime, transform.position.y); }
    }

    private void MoveTowards(Vector3 target)
    {
        transform.position = Vector2.MoveTowards(transform.position, target, speed * Time.fixedDeltaTime);
        if (transform.position.x > target.x && faceright)
        {
            Flip();
        }
        else if (transform.position.x < target.x && !faceright)
        {
            Flip();
        }
    }

    private void Flip()
    {
        transform.Rotate(0, 180, 0);
        faceright = !faceright;
    }
}
