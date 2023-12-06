using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySimple : MonoBehaviour
{
    public int maxHealth = 100;
    int currentHelth;
    public Animator animator;
    public Collider2D coll;
    public Rigidbody2D rb;
    public Canvas canvas;
    public HealthBehiviar HealthBar;


    // Start is called before the first frame update
    void Start()
    {
        currentHelth = maxHealth;

        HealthBar.SetHelth(currentHelth, maxHealth);
    }

    public void TakeDamage(int damage)
    {
        currentHelth -= damage;
        HealthBar.SetHelth(currentHelth, maxHealth);
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

}
