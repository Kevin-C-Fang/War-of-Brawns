using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    GameObject enemy;                       //reference to enemy GameObject

    public Animator animator;               //reference to enemy animator
    public Rigidbody2D enemyRB;             //reference to enemy rigidbody
    public GameObject canvas;               //reference to enemy canvas
    public SimpleHealthBar healthBar;       //reference to enemy health bar
    public Canvas gameCanvas;               //reference to game canvas

    public int maxHealth = 100;             //Max amount of health
    int currentHealth;                      //Current amount of health available

    // Start is called before the first frame update
    void Start()
    {
        //Assign initial variables
        currentHealth = maxHealth;
        enemy = this.gameObject;
    }

    //When the enemy is hit by an attack...
    public void TakeDamage(int damage)
    {
        //Plays sword impact sound
        FindObjectOfType<AudioManager>().Play("SwordImpact");

        currentHealth -= damage;
        healthBar.UpdateBar(currentHealth, maxHealth);

        //Play hurt animation
        animator.SetTrigger("Hurt");

        //Indicate that enemy attack stopped if interrupted by player attack
        enemy.GetComponent<EnemyAI>().StopAttack();

        //If health is 0 or less, die
        if(currentHealth <= 0)
        {
            Die();
        }

    }

    //When the enemy dies...
    void Die()
    {
        //Play skeleton death sound
        FindObjectOfType<AudioManager>().Play("SkeletonDeath");

        //Die animation
        animator.SetBool("IsDead", true);

        //Disable enemy canvas
        canvas.GetComponent<Canvas>().enabled = false;

        //Disable the enemy
        enemyRB.isKinematic = true; //sets enemy to be locked in place, physics disabled

        GetComponent<PolygonCollider2D>().enabled = false; //disables the enemy collider component after death

        this.GetComponent<EnemyAI>().enabled = false; //disables AI

        //Gives enemy "Dead" tag
        this.tag = "Dead";

        //Check if victorious
        GameObject[] allEnemies = GameObject.FindGameObjectsWithTag("Enemy");
        if (allEnemies.Length == 0)
            gameCanvas.GetComponent<EndDungeon>().showVictory();


        this.enabled = false; //disables this script
    }

}
