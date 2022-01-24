using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] private Transform ObstacleCheck = null;        //position marking where to check if the enemy is touching an obstacle
    [SerializeField] private LayerMask m_WhatIsObstacle;            // A mask determining what is an obstacle to the enemy
    public LayerMask playerLayer;                                   //Layer of player object
    private bool obstacleBlocked;                                   //bool stating whether or not the enemy has an obstacle directly in front
    private bool isAttacking;                                       //bool stating whether or not enemy is currently attacking

    public float speed;                         //Speed at which the enemy moves
    public float stoppingDistance;              //Distance from target where the enemy stops and attacks

    public Transform attackPoint;               //Central point of attack range circle
    public float attackRange = 0.9f;            //Default range for attacks
    public int attackDamage = 40;               //Damage the enemy does on attack hits

    public float stunLockDuration = 0.15f;       //Duration of stun-lock when hit by player attack
    float nextAttackTime = 0f;                  //Next possible time enemy can attack

    private Transform target;       //Reference to target to follow
    bool facingRight = false;       //Variable for which way the enemy is facing

    public Animator animator;       //Reference to enemy animator
    public GameObject canvas;       //Reference to enemy canvas
    public Rigidbody2D rb;          //Reference to enemy RigidBody

    // Start is called before the first frame update
    void Start()
    {
        //Sets the player as a target for the enemy to follow
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //Flip the enemy to face player if not currently attacking
        if (!isAttacking)
        {
            //Makes enemy always face toward player
            if (target.position.x < transform.position.x && !facingRight) //if player is to the right and enemy is facing left
                Flip();
            if (target.position.x > transform.position.x && facingRight) //if player is to the left and enemy is facing right
                Flip();
        }
        //Determines whether or not the enemy is blocked by an obstacle
        obstacleBlocked = false;
        Collider2D[] colliders = Physics2D.OverlapCircleAll(ObstacleCheck.position, 0.1f, m_WhatIsObstacle);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject)
                obstacleBlocked = true;
        }

        //If not blocked by an obstacle and not attacking...
        if (obstacleBlocked == false && !isAttacking)
        {
            //Moves toward player until within stopping distance
            if (Vector2.Distance(transform.position, target.position) > stoppingDistance)
            {
                rb.constraints = RigidbodyConstraints2D.FreezeRotation; //Disables any RigidBody X freezes before movement
                transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
                animator.SetBool("IsWalking", true); //Plays walk animation while moving
            }
            else
            {
                //Freeze movement when not moving (stops sliding)
                rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;

                //Stops walk animation when not moving
                animator.SetBool("IsWalking", false); 

                //Attack if enough time has passed since the enemy was stunlocked
                if(Time.time >= nextAttackTime)
                    StartAttack();
            }
        }

        //if blocked by an obstacle... 
        else
            animator.SetBool("IsWalking", false); //Cancel possible walk animation if wallblocked
    }

    void StartAttack() //Starts attack animation
    {
        animator.SetTrigger("Attack");
        isAttacking = true;
    }

    public void Attack() //Performs on-hit effects (animations, damage, etc.)
    {
        //Detect player in range of attack
        //Returns player collider of player if hit by attack
        Collider2D hitPlayer = Physics2D.OverlapCircle(attackPoint.position, attackRange, playerLayer); //(center, radius, what its checking)

        //Deal damage to player if hit
        if(hitPlayer != null)
            hitPlayer.GetComponent<PlayerCombat>().TakeDamage(attackDamage); //Calls TakeDamage function from enemy
    }

    public void StopAttack() //Indicates the attack has finished or was interrupted
    {
        //Re-enables movement after attack finishe
        isAttacking = false;

        //Sets the next possible attack time depending on stunlock duration
        nextAttackTime = Time.time + stunLockDuration; 
    }

    //draws hit range indicator in editor (for testing)
    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }

    //flips direction the enemy is facing
    void Flip() 
    {
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;

        //flips canvas so UI isn't flipped
        Vector3 uiscale = canvas.transform.localScale;
        uiscale.x *= -1;
        canvas.transform.localScale = uiscale;

        facingRight = !facingRight;
    }

}
