using UnityEngine;
using System.Collections;

public class MyCharacterController : MonoBehaviour {
		
	[HideInInspector]
    public Rigidbody2D rb2d { get; private set; }
    
    public float Speed { get; private set; }

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        Speed = 200f;
    }

	// Update is called once per frame
	void Update ()
    {
        HandleInput();   
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
        else if(rb2d.velocity != Vector2.zero)
        {
        	rb2d.velocity = Vector2.zero;
        }
    }

// 
    void Move()
    {
        Vector2 velocity = new Vector2();
		
        velocity.x = InputManager.Instance.Horizontal;
        velocity.y = InputManager.Instance.Vertical;

        rb2d.velocity = velocity.normalized * Speed * Time.deltaTime;
    }

    void Actions()
    {
        if (InputManager.Instance.Trigger_Action_One)
        {
            Debug.Log("Action_One");
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
}
