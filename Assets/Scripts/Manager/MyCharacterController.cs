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

    public AudioClip takeDamageClip, JumpClip, DieClip;

    public ParticleSystem[] dieParticleSystem;

    public bool dead = false;

    public Animator anim;

    public bool facingRight = true;

    public GameObject Body, Head;

    void Start()
    {
        InputManager.Instance.ControllerState = InputManager.EnumControllerState.ControlCharacter;
        rb2d = GetComponent<Rigidbody2D>();
        bc2d = GetComponent<CircleCollider2D>();
        Speed = 600f;
        jumpForce = 15f;
        AttributeController = new CharacterAttributeController(EnergyBar,HpController);

        transform.position = LevelController.Instance.SpawnPosition;

        for (int i = 0; i <= dieParticleSystem.Length -1; i++)
        {
            dieParticleSystem[i].GetComponent<Renderer>().sortingOrder = 200;
        }

        dead = false;

        PulseManagerHardCoded.Instance.SetPitch(1);
        SoundManager.Instance.PitchMusik(1);
    }

	// Update is called once per frame
	void Update ()
    {
        if (dead) return;
        HandleInput();
        Delays();

        if (facingRight && rb2d.velocity.x < 0)
        {
            Flip();
        }
        else if (!facingRight && rb2d.velocity.x > 0)
        {
            Flip();
        }
        anim.SetFloat("SpeedX" ,Mathf.Abs( rb2d.velocity.x));
        if (GroundCheck())
        {
            anim.SetFloat("SpeedY", 0);
        }
        else
        {
            anim.SetFloat("SpeedY", rb2d.velocity.y);
        }
        
	}

    void Flip()
    {
        Head.transform.Rotate(0, 180, 0);
        Body.transform.Rotate(0, 180, 0);
        facingRight = !facingRight;
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
        else if (PulseManagerHardCoded.Instance.GetPitch() != 1 && ( InputManager.Instance.Action_Three || InputManager.Instance.Action_Four))
        {
            energyConsumption = 15 * Time.deltaTime;
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

            SoundManager.Instance.PlaySoundEffect(JumpClip, 0.9f, 1.1f);
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
        if (other.gameObject.tag == "DeathSource" && dead == false)
        {
            AttributeController.CurrentHP = 0;
            PlayerDies();
        }

        if (other.gameObject.tag == "DamageSource" && dead== false)
        {
            if (currentImmuneTime > 0) return;

            var heading = transform.position - other.transform.position;
            var distance = heading.magnitude;
            var direction = heading / distance;

            

            rb2d.velocity = direction * 15;


            rb2d.AddForce(new Vector2(direction.x * 1000, 0));

            AttributeController.CurrentHP -= 1;
            currentImmuneTime = MAX_IMMUNE_TIME;
            if (AttributeController.CurrentHP <= 0)
            {
                PlayerDies();
            }
            else
            {
                SoundManager.Instance.PlaySoundEffect(takeDamageClip, 0.9f, 1.1f);
            }
        }
    }

    void PlayerDies()
    {
        Invoke("IDied", 1.5f);

        for (int i = 0; i <= dieParticleSystem.Length - 1; i++)
        {
            dieParticleSystem[i].Emit(40);
        }
        dead = true;
        rb2d.velocity = Vector2.zero;
        rb2d.isKinematic = true;
        rb2d.gravityScale = 0;
        GetComponent<SpriteRenderer>().enabled = false;
        Body.GetComponent<SpriteRenderer>().enabled = false;
        Head.GetComponent<SpriteRenderer>().enabled = false;
        SoundManager.Instance.PlaySoundEffect(DieClip, 0.9f, 1.1f);
    }

    void IDied()
    {
        LevelController.Instance.PlayerDied();
    }
}
