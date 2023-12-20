using System.Collections;
using System.Collections.Generic;
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
    void Start()
    {
        currentHelth = maxHealth;
        HealthBar.SetHelth(currentHelth, maxHealth);
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }
    private void Update()
    {
        EnemyRun();
        Reflect();
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
    public void EnemyRun()
    {
        if(animator.GetBool("isDeath") != true)
        {
            Vector2 target = new Vector2(player.position.x, rb.position.y);
            Vector2 newPos = Vector2.MoveTowards(rb.position, target, speed * Time.fixedDeltaTime);
            rb.MovePosition(newPos);
        }


        if(Vector2.Distance(player.position, rb.position) <= attackRange*2)
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
        if ((transform.position.x < player.position.x && !faceright) || (transform.position.x > player.position.x && faceright) && animator.GetBool("isDeath") != true)
        {
            transform.Rotate(0, 180, 0);
            faceright = !faceright;
        }
    }
    public void EnemyAttack_1()
    {

        PlayerCollider  = Physics2D.OverlapCircle(attackPointEnemy.position, attackRange, playerMask);
        if (PlayerCollider != null)
        {
            Debug.Log(enemyDamage);
            PlayerCollider.GetComponent<PlayerController>().TakeDamagePlayer(enemyDamage);
        }
    }
}
