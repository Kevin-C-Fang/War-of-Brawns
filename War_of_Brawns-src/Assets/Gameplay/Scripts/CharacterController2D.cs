using UnityEngine;
using UnityEngine.Events;

public class CharacterController2D : MonoBehaviour
{
	[SerializeField] private float m_JumpForce = 780f;							// Amount of force added when the player jumps.
    [SerializeField] private float m_RollForce = 400f;                          // Amount of force added when the player rolls.
    [Range(0, .3f)] [SerializeField] private float m_MovementSmoothing = .05f;	// How much to smooth out the movement
	[SerializeField] private bool m_AirControl = false;							// Whether or not a player can steer while jumping;
	[SerializeField] private LayerMask m_WhatIsGround;					        // A mask determining what is ground to the character
	[SerializeField] private Transform m_GroundCheck = null;					// A position marking where to check if the player is grounded.
	//[SerializeField] private Transform m_CeilingCheck = null;                 // A position marking where to check for ceilings

    const float k_GroundedRadius = .2f;     // Radius of the overlap circle to determine if grounded
	private bool m_Grounded;                // Whether or not the player is grounded.
	const float k_CeilingRadius = .2f;      // Radius of the overlap circle to determine if the player can stand up
	private Rigidbody2D m_Rigidbody2D;
	private bool m_FacingRight = true;      // For determining which way the player is currently facing.
	private Vector3 m_Velocity = Vector3.zero;

	[Header("Events")]
	[Space]

	public UnityEvent OnLandEvent;

	[System.Serializable]
	public class BoolEvent : UnityEvent<bool> { }


	private void Awake()
	{
		m_Rigidbody2D = GetComponent<Rigidbody2D>();

		if (OnLandEvent == null)
			OnLandEvent = new UnityEvent();
	}

	private void FixedUpdate()
	{
		bool wasGrounded = m_Grounded;
		m_Grounded = false;

		// The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
		// This can be done using layers instead but Sample Assets will not overwrite your project settings.
		Collider2D[] colliders = Physics2D.OverlapCircleAll(m_GroundCheck.position, k_GroundedRadius, m_WhatIsGround);
		for (int i = 0; i < colliders.Length; i++)
		{
			if (colliders[i].gameObject != gameObject)
			{
				m_Grounded = true;
                if (!wasGrounded)
                    OnLandEvent.Invoke();
            }
		}
	}


	public void Move(float move, bool roll, bool jump)
	{
		//only control the player if not rolling AND if either grounded or airControl is turned on
		if ((m_Grounded || m_AirControl) && !roll)
		{
			// Move the character by finding the target velocity
			Vector3 targetVelocity = new Vector2(move * 10f, m_Rigidbody2D.velocity.y);
			// And then smoothing it out and applying it to the character
			m_Rigidbody2D.velocity = Vector3.SmoothDamp(m_Rigidbody2D.velocity, targetVelocity, ref m_Velocity, m_MovementSmoothing);

			// If the input is moving the player right and the player is facing left...
			if (move > 0 && !m_FacingRight)
			{
				// ... flip the player.
				Flip();
			}
			// Otherwise if the input is moving the player left and the player is facing right...
			else if (move < 0 && m_FacingRight)
			{
				// ... flip the player.
				Flip();
			}
		}

		// If the player should jump...
		if (m_Grounded && jump && !roll)
		{
			// Add a vertical force to the player
			m_Rigidbody2D.AddForce(new Vector2(0f, m_JumpForce));
		}

        // If the player should roll...
        if (m_Grounded && roll && !jump)
        {
            //Disables collision to roll through enemies (between "Invulnerable" and "Enemies")
            Physics2D.IgnoreLayerCollision(11, 9);

            //Cancels all standard movement
            m_Rigidbody2D.velocity = new Vector2(0,0); 

            //Sets up roll to direction character is facing
            float facing;
            if (m_FacingRight)
                facing = 1f;
            else
                facing = -1f;

            //Roll in direction character is facing
            m_Rigidbody2D.AddForce(new Vector2( facing * m_RollForce, 0f ));
        }
	}

	private void Flip()
	{
		// Switch the way the player is labelled as facing.
		m_FacingRight = !m_FacingRight;

		// Multiply the player's x local scale by -1.
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}
}
