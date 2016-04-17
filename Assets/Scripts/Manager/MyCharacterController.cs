using UnityEngine;
using System.Collections;
using UnityEngine.UI;

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

    public float energyConsumption = 0;

    public Transform GroundCheck1; // Put the prefab of the ground here
    public LayerMask groundLayer; // Insert the layer here.

    public CharacterAttributeController AttributeController { get; set; }

    public Image EnergyBar;
    public HPController HpController;

    public const float MAX_IMMUNE_TIME = 0.5f;
    public float currentImmuneTime = 0;



    void Start()
    {
        InputManager.Instance.ControllerState = InputManager.EnumControllerState.ControlCharacter;
        rb2d = GetComponent<Rigidbody2D>();
        bc2d = GetComponent<CircleCollider2D>();
        Speed = 600f;
        jumpForce = 15f;
        AttributeController = new CharacterAttributeController(EnergyBar,HpController);

        transform.position = LevelController.Instance.SpawnPosition;
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
            RegEnergy();
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

    void RegEnergy()
    {
        AttributeController.RegEnergy(10 * Time.deltaTime);

        AttributeController.CurrentEnergy -= energyConsumption;
    }

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

        if (currentImmuneTime > 0)
            currentImmuneTime -= Time.deltaTime;
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
            ChangeSpeed(1.5f);
        }
        else if (InputManager.Instance.Trigger_Action_Four)
        {
            ChangeSpeed(0.35f);
        }
        if (InputManager.Instance.Trigger_Action_Four_Up || InputManager.Instance.Trigger_Action_Three_Up  || AttributeController.CurrentEnergy <=0)
        {
            PulseManagerHardCoded.Instance.SetPitch(1);
            SoundManager.Instance.PitchMusik(1);
            energyConsumption = 0;
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
        if (AttributeController.CurrentEnergy > 0)
        {
            SoundManager.Instance.PitchMusik(pitch);
            PulseManagerHardCoded.Instance.SetPitch(pitch);

            energyConsumption = 15 * Time.deltaTime;
        }
    }

    void OnCollisionStay2D(Collision2D other)
    {
        if (other.gameObject.tag == "DamageSource")
        {
            if (currentImmuneTime > 0) return;

            AttributeController.CurrentHP -= 1;
            currentImmuneTime = MAX_IMMUNE_TIME;
            if(AttributeController.CurrentHP <= 0)
                LevelController.Instance.PlayerDied();
        }
    }
}
