using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    GameObject playerObject;

    public CharacterController2D controller;
    public Animator animator;

    public float runSpeed = 40f;                //The speed of player horizontal movement
    public int rollEnergyCost = 15;             //The amount of energy required to roll
    public int jumpEnergyCost = 10;             //The amount of energy required to jump
    float horizontalMove = 0f;                  //Float variable for horizontal movement
    bool jump = false;                          //bool variable for jump command
    bool roll = false;                          //bool variable for roll command

    // Start is called before the first frame update
    void Start()
    {
        playerObject = this.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        //If dungeon is completed, disable this script
        if (animator.GetBool("Victory") || animator.GetBool("IsDead"))
            this.enabled = false;

        horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;

        //Set "Speed" parameter to current horizontalMove value to play movement animations
        animator.SetFloat("Speed", Mathf.Abs(horizontalMove));

        //When jump button is pressed...
        if (Input.GetButtonDown("Jump"))
        {
            //and character has enough energy...
            if (this.GetComponent<PlayerCombat>().currentEnergy >= jumpEnergyCost)
            {
                //Jump if possible
                jump = true;
                animator.SetBool("IsJumping", true);
            }
        }

        //When roll button is pressed and character is not already rolling...
        if (Input.GetButtonDown("Roll") && !animator.GetBool("IsRolling"))
        {
            //and character has enough energy...
            if(this.GetComponent<PlayerCombat>().currentEnergy >= rollEnergyCost)
            {
                //Roll if possible
                roll = true;                            //Called in CharacterController
                animator.SetBool("IsRolling", true);    //Called in Animator
                playerObject.layer = LayerMask.NameToLayer("Invulnerable");
            }
        }
    }

    //Occurs when player lands
    public void OnLanding()
    {
        animator.SetBool("IsJumping", false);
    }

    //Stops rolling
    public void StopRolling()
    {
        animator.SetBool("IsRolling", false);
        roll = false;
        playerObject.layer = LayerMask.NameToLayer("Player");

        //Re-enables colliders after roll
        Physics2D.IgnoreLayerCollision(8, 9, false);
    }

    private void FixedUpdate()
    {
        //Move our character
        controller.Move(horizontalMove * Time.fixedDeltaTime, roll, jump);
        jump = false;

    }
}
