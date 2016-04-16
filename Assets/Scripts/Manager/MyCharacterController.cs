using UnityEngine;
using System.Collections;

public class MyCharacterController : MonoBehaviour {
		
	[HideInInspector]
    public Rigidbody2D rb2d { get; private set; }
    
    public bool grounded { get; private set; }

    public float Speed { get; private set; }
    public float jumpDelay { get; private set; }
    public float jumpForce { get; private set; }
    public const float MaxJumpDelay = 0.3f;

void Start()
    {
        InputManager.Instance.ControllerState = InputManager.EnumControllerState.ControlCharacter;
        rb2d = GetComponent<Rigidbody2D>();
        Speed = 400f;
        jumpForce = 12f;
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
            Debug.Log("Action_Three");
        }
        if (InputManager.Instance.Trigger_Action_Four)
        {
            Debug.Log("Action_Four");
        }
    }

    void Jump()
    {

        if (jumpDelay <= 0)
        {
            rb2d.velocity = new Vector2(rb2d.velocity.x, jumpForce);
            jumpDelay = MaxJumpDelay;
        }
        
    }
}
