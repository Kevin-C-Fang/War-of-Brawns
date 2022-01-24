using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Unity.Editor;
using static Firebase.Auth.FirebaseAuth;

public class PlayerCombat : MonoBehaviour
{
    public int strength_level = 1;              //Player's strength level
    public int endurance_level = 1;             //Player's endurance level
    public int defense_level = 1;               //Player's defense level

    public Animator animator;               //Reference to player animator
    public Canvas gameCanvas;               //Reference to game canvas
    public Canvas enemyCanvas;              //Reference to enemy canvas

    public int maxHealth;                   //Max amount of health
    int currentHealth;                      //Current amount of health available

    public int maxEnergy = 100;           //Max amount of energy
    public float currentEnergy;           //Current amount of energy available
    private float energyRegen;            //The amount of energy regerated per interval

    public SimpleHealthBar healthBar;       //Health Bar Object reference
    public SimpleHealthBar energyBar;       //Energy Bar Object reference

    public Transform attackPoint;           //Central point of attack range circle
    public float attackRange = 0.5f;        //Default range for attacks
    public int attackDamage;                //Damage the player does on attack hits
    public int attackEnergyCost = 5;        //Amount of energy required to attack

    public float attackRate = 2f;           //# attacks per second
    float nextAttackTime = 0f;

    public LayerMask enemyLayers;           //Layer of all enemy objects
    public bool getHit;                     //bool variable for camerashake use

    void Start()
    {
        //Sets stats
        strength_level = GameObject.Find("Data Storage").GetComponent<dataStorage> ().strength;
        endurance_level = GameObject.Find("Data Storage").GetComponent<dataStorage>().endurance;
        defense_level = GameObject.Find("Data Storage").GetComponent<dataStorage>().defense;
        //maxHealth = GameObject.Find("Data Storage").GetComponent<dataStorage>().health;
        maxHealth = 100;

        //Initializes based on stats
        currentHealth = maxHealth;
        currentEnergy = maxEnergy;
        energyRegen = 5 + endurance_level;
        attackDamage = 5 + (5 * strength_level);
    }

    // Update is called once per frame
    void Update()
    {
        //If dungeon is completed, disable this script
        if (animator.GetBool("Victory") || animator.GetBool("IsDead"))
            this.enabled = false;

        //If enough time has passed from last attack...
        if (Time.time >= nextAttackTime)
        {
            //and player is trying to attack while having enough energy...
            if (Input.GetMouseButtonDown(0) && currentEnergy >= attackEnergyCost)
            {
                //attack
                StartAttack();
                nextAttackTime = Time.time + 1f / attackRate; //Sets next time we are able to attack
            }
            getHit = false; //stops camerashake
        }

        //If not at full energy...
        if(currentEnergy < maxEnergy)
        {
            //Regen energy
            currentEnergy += energyRegen * Time.deltaTime;
            energyBar.UpdateBar(currentEnergy, maxEnergy);
        }
        
    }

    void StartAttack()
    {
        //Play attack animation
        animator.SetTrigger("Attack"); //"Attack" function called during animation

    }

    public void Attack() //Performs on-hit effects (animations, damage, etc.)
    {
        //Play sword swing sound
        FindObjectOfType<AudioManager>().Play("SwordSwing");

        //Detect enemies in range of attack
        //list of all enemies hit by attack
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers); //(center, radius, what its checking)

        //Deal damage to each enemy hit
        foreach(Collider2D enemy in hitEnemies)
        {
            enemy.GetComponent<Enemy>().TakeDamage(attackDamage); //Calls TakeDamage function from enemy
        }
    }

    //Player takes damage from an enemy attack
    public void TakeDamage(int damage)
    {
        currentHealth -= (damage) - (defense_level);    //Higher defense gives damage reduction
        Debug.Log("Defense level: " + defense_level + ", player took " + ((damage) - (defense_level) + "damage"));

        healthBar.UpdateBar(currentHealth, maxHealth);

        //Play hurt animation
        //animator.SetTrigger("Hurt");

        //Camera shake
        getHit = true;

        //If health is 0 or less, die
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    //Drains energy from energy resource
    public void UseEnergy(int e)
    {
        //Energy cannot be negative
        if ((currentEnergy - e) >= 0)
            currentEnergy = currentEnergy - e;

        //Updates energy bar
        energyBar.UpdateBar(currentEnergy, maxEnergy);
    }

    void Die()
    {

        //Play player death sound
        FindObjectOfType<AudioManager>().Play("PlayerDeath");

        //Die animation
        animator.SetBool("IsDead", true);

        //Disable the player
        //enemyRB.isKinematic = true; //sets enemy to be locked in place, physics disabled

        //Disables all enemy ai and canvas
        GameObject[] allEnemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in allEnemies)
        {
            enemy.GetComponent<EnemyAI>().enabled = false;
            enemyCanvas.GetComponent<Canvas>().enabled = false;
        }

        //Gives player "Dead" tag
        this.tag = "Dead";

        //Shows defeat screen
        gameCanvas.GetComponent<EndDungeon>().showDefeat();


        //this.enabled = false; //disables this script
    }

    //draws hit range indicator in editor (for testing)
    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
