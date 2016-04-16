using UnityEngine;
using System.Collections;

public class MyCharacterController : MonoBehaviour {
		
	[HideInInspector]
    public Rigidbody2D rb2d { get; private set; }
    
    public CircleCollider2D bc2d { get; private set; }

    public bool grounded { get; private set; }

    public float Speed { get; private set; }
    public float jumpDelay { get; private set; }
    public float jumpForce { get; private set; }
    public const float MaxJumpDelay = 0.3f;

    bool isGrounded = false;
    public Transform GroundCheck1; // Put the prefab of the ground here
    public LayerMask groundLayer; // Insert the layer here.

    public CharacterAttributeController AttributController { get; set; }

    void Start()
    {
        InputManager.Instance.ControllerState = InputManager.EnumControllerState.ControlCharacter;
        rb2d = GetComponent<Rigidbody2D>();
        bc2d = GetComponent<CircleCollider2D>();
        Speed = 700f;
        jumpForce = 15f;
        //AttributController(Energybar)
    }

	// Update is called once per frame
	void Update ()
    {
        HandleInput();
        Delays();
	}

    void HandleInput()
    {
    	// Hier prüfen wir ob der InputManager die Steuerung des Characters zu lässt
        if (InputManager.Instance.ControllerState == InputManager.EnumControllerState.ControlCharacter)
        {
            Move();
            Actions();
        }
        // Wenn die Steuerung nicht zugelassen ist aber wir dennoch Velocity haben setzen wir diese auf Vector2.Zero
        else if(rb2d.velocity.x != 0)
        {
            rb2d.velocity = new Vector2(0, rb2d.velocity.y);
        }
    }


    bool GroundCheck()
    {
        return Physics2D.Raycast(transform.position, -Vector3.up, (bc2d.radius) + 0.1f,groundLayer);
    }
// 
    void Move()
    {
        Vector2 velocity = new Vector2();
		
        velocity.x = InputManager.Instance.Horizontal * Speed * Time.deltaTime;
        velocity.y = rb2d.velocity.y;

        rb2d.velocity = velocity;
    }

    void Delays()
    {
        if (jumpDelay > 0)
            jumpDelay -= Time.deltaTime;
    }

    void Actions()
    {
        if (InputManager.Instance.Trigger_Action_One)
        {
            Jump();
        }
        if (InputManager.Instance.Trigger_Action_Two)
        {
            Debug.Log("Action_Two");
        }

        if (InputManager.Instance.Trigger_Action_Three)
        {
            ChangeSpeed(1.25f);
        }
        else if (InputManager.Instance.Trigger_Action_Four)
        {
            ChangeSpeed(0.75f);
        }
        if (InputManager.Instance.Trigger_Action_Four_Up || InputManager.Instance.Trigger_Action_Three_Up)
        {
            PulseManagerHardCoded.Instance.SetPitch(1);
            SoundManager.Instance.PitchMusik(1);
        }
    }

    void Jump()
    {

        if (jumpDelay <= 0 && GroundCheck())
        {
            rb2d.velocity = new Vector2(rb2d.velocity.x, jumpForce);
            jumpDelay = MaxJumpDelay;
        }
        
    }

    void ChangeSpeed(float pitch)
    {

        if (AttributController.CurrentEnergy > 0)
        {
            SoundManager.Instance.PitchMusik(pitch);
            PulseManagerHardCoded.Instance.SetPitch(pitch);
        }
    }

    void OnCollisionStay2D(Collision2D other)
    {
        if (other.gameObject.tag == "DamageSource")
        {
            LevelController.Instance.PlayerDied();
        }
    }
}
