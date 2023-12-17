using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class EnemySimple : MonoBehaviour
{
    public int maxHealth = 100;
    int currentHelth;

    public Animator animator;
    public Collider2D coll;
    public Rigidbody2D rb;
    public Canvas canvas;
    public HealthBehiviar HealthBar;
    public Collider2D PlayerCollider;


    public float attackRate = 2f;
    float nextAttackTime = 0f;
    public int enemyDamage = 5;
    public HealthForPlayer PlayerHealth;
    public bool trig;

    void Start()
    {
        currentHelth = maxHealth;
        HealthBar.SetHelth(currentHelth, maxHealth);
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

    public void OnTriggerStay2D(Collider2D collision)
    {
        PlayerCollider = collision;
        if (collision.CompareTag("Player"))
            {
                if (Time.time >= nextAttackTime)
                {
                    animator.SetTrigger("EnemyAttack_1");
                    nextAttackTime = Time.time + 1f / attackRate;
                }
            }
        rb.WakeUp();
    }

    


    public void EnemyAttack_1()
    {
        if(PlayerCollider != null)
        {
            Debug.Log(enemyDamage);
            PlayerCollider.GetComponent<PlayerController>().TakeDamagePlayer(enemyDamage);
        }
    }
}
