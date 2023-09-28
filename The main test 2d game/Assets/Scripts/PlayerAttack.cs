using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public Rigidbody2D rb;
    public Animator animator;
    public PlayerController player;

    public float attackRate = 2f;
    float nextAttackTime = 0f;

    public Transform attackPoint;
    public Transform attackPoint2;


    public float attackRange = 0.8f;
    public Vector2 attackRange2 = Vector2.one;

    public LayerMask enemyLayers;
    public int attackDamage = 20;

    // Update is called once per frame
    void Update()
    {
        if(Time.time >= nextAttackTime)
        {
            if (Input.GetKeyDown(KeyCode.X) && player.CheckingGround())
            {
                Attack(1);
                nextAttackTime = Time.time + 1f / attackRate;
            }
        }


        if (Input.GetKeyDown(KeyCode.X) && !player.CheckingGround())
        {
            Attack(2);
        }

        if (Input.GetKey(KeyCode.UpArrow) && Input.GetKeyDown(KeyCode.C) && Time.time >= nextAttackTime && player.CheckingGround() && !IsAttack_3_start)
        {
            Attack(4);
            nextAttackTime = Time.time + 1f / attackRate;
        }

        if (Input.GetKeyDown(KeyCode.C) && Time.time >= nextAttackTime && player.CheckingGround() && !IsAttack_4_start)
        {
            Attack(3);
            nextAttackTime = Time.time + 1f / attackRate;
        }



        if (player.CheckingGround())
        {
            jumpTime = 0;
        }
    }
    private int jumpTime = 0;
    void Attack(int attack)
    {


        if(attack == 1)
        {
            animator.SetTrigger("Attack_1");
        }
        else if (attack == 2 && jumpTime == 0)
        {
           animator.SetTrigger("Attack_2");
        }
        else if (attack == 3)
        {
            animator.SetTrigger("Attack_3");
        }
        else if (attack == 4)
        {
            animator.SetTrigger("Attack_4");
        }

    }

    private void OnDrawGizmosSelected()
    {
        if(attackPoint == null) { return; }
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
        Gizmos.DrawWireCube(attackPoint2.position, attackRange2);
    }

    /// <AnimationsAndEvent>
    /// ||||||||||||||||||||

    //SimpleAttack
    public void Attack_1_move()
    {
        rb.constraints = RigidbodyConstraints2D.FreezeAll;

    }

    public void Attack_1_inMove()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);
        foreach (Collider2D enemy in hitEnemies)
        {
            enemy.GetComponent<EnemySimple>().TakeDamage(attackDamage);
        }
    }
    public void Attack_1_moved()
    {
        rb.constraints = RigidbodyConstraints2D.None;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    ///AirAttack
    public void inAirAttack()
    {
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        jumpTime += 1;
    }

    public void inAirAttacking()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);
        foreach (Collider2D enemy in hitEnemies)
        {
            enemy.GetComponent<EnemySimple>().TakeDamage(attackDamage);
        }
    }

    public void isntAirAttack()
    {
        rb.constraints = RigidbodyConstraints2D.None;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    ///TheirdAttack
    public float attackForceTheird = 5f;
    public bool IsAttack_3_start = false;
    public void TheirdAttackStart()
    {
        IsAttack_4_start = false;
        IsAttack_3_start = true;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
    }

    public void TheirdAttack()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapBoxAll(attackPoint2.position, attackRange2, enemyLayers);
        foreach (Collider2D enemy in hitEnemies)
        {
            EnemySimple enemySimple = enemy.GetComponent<EnemySimple>();
            if (enemySimple != null)
            {
                enemySimple.TakeDamage(attackDamage);
            }
        }


        rb.constraints = RigidbodyConstraints2D.None;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        if (player.faceright)
        {
            rb.AddForce(Vector2.right * attackForceTheird, ForceMode2D.Impulse);
        }

        else { rb.AddForce(Vector2.left * attackForceTheird, ForceMode2D.Impulse); }


    }


    public void TheirdAttackEnd()
    {

        rb.constraints = RigidbodyConstraints2D.None;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        IsAttack_3_start = false;
    }

    ///FourdAttack
    public bool IsAttack_4_start = false;
    public float attackForceFour = 2f;
    public void FourAttackStart()
    {
        IsAttack_3_start = false;
        IsAttack_4_start = true;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
    }

    public void FourAttack()
    {
        attackRange = 1.4f;
        rb.constraints = RigidbodyConstraints2D.None;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);
        foreach (Collider2D enemy in hitEnemies)
        {
            enemy.GetComponent<EnemySimple>().TakeDamage(attackDamage);
        }
        if (player.faceright)
        {
            rb.AddForce(Vector2.right * attackForceFour, ForceMode2D.Impulse);
        }
        else { rb.AddForce(Vector2.left * attackForceFour, ForceMode2D.Impulse); }
        attackRange = 0.8f;
    }


    public void FourAttackEnd()
    {
        rb.constraints = RigidbodyConstraints2D.None;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        IsAttack_4_start = false;
    }
}
