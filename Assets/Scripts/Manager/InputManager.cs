using UnityEngine;
using System.Collections;

public class InputManager : MonoBehaviour {

    private const float MAX_TRIGGER_DELAY = 0.1f;

    public static InputManager Instance;

    public enum EnumControllerState {ControlCharacter,Menu }

    public EnumControllerState ControllerState { get; set; }

    public InputData Inputs { get; private set; }

    private float horizontalTriggerDelay;
    private float horizontal;
    public float Horizontal    
    {
        get
        {
            return horizontal;
        }
        private set
        {
            horizontal = value;
        }
    }
    public float HorizontalTrigger
    {
        get
        {
            if (horizontalTriggerDelay > 0)
            {
                return 0;
            }
            else
            {
                horizontalTriggerDelay = MAX_TRIGGER_DELAY;
                return horizontal;
            }
        }
    }

    private float verticalTriggerDelay;
    private float vertical;
    public float Vertical
    {
        get
        {
            return vertical;
        }
        private set
        {
            vertical = value;
        }
    }
    public float VerticalTrigger
    {
        get
        {
            if (verticalTriggerDelay > 0)
            {
                return 0;
            }
            else
            {            
                verticalTriggerDelay = MAX_TRIGGER_DELAY;
                return vertical;
            }
        }
    }

    public bool Trigger_Action_One { get; private set; }
    public bool Trigger_Action_Two { get; private set; }
    public bool Trigger_Action_Three { get; private set; }
    public bool Trigger_Action_Four { get; private set; }

    public bool Trigger_Action_Start { get; private set; }

    public bool Action_One { get; private set; }
    public bool Action_Two { get; private set; }
    public bool Action_Three{ get; private set; }
    public bool Action_Four { get; private set; }

    public bool Action_Start { get; private set; }

    // Use this for initialization
    void Awake () {
        // First we check if there are any other instances conflicting
        if (Instance == null)
        {
            // If that is the case, we destroy other instances
            DontDestroyOnLoad(gameObject);
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Inputs = new InputData();
        ControllerState = EnumControllerState.Menu;
    }

    void Update()
    {
        // Hier reduzieren wir die TriggerDelays
        if (verticalTriggerDelay > 0) verticalTriggerDelay -= Time.deltaTime;
        if (horizontalTriggerDelay > 0) horizontalTriggerDelay -= Time.deltaTime;

        // Hier nehmen wir den Horizontalen Input
        if (Mathf.Abs(Input.GetAxisRaw(Inputs.Hori)) > 0.2f || Mathf.Abs(Input.GetAxisRaw(Inputs.Hori_K)) > 0f)
        {
            if (Mathf.Abs(Input.GetAxisRaw(Inputs.Hori)) > 0.2f)
            {
                Horizontal = Input.GetAxis(Inputs.Hori);
            }
            else if (Mathf.Abs(Input.GetAxisRaw(Inputs.Hori_K)) > 0f)
            {
                Horizontal = Input.GetAxisRaw(Inputs.Hori_K);
            }
        }
        else if (Horizontal != 0)
        {
            Horizontal = 0;
        }

        // Hier nehmen wir den Verticalen Input
        if (Mathf.Abs(Input.GetAxis(Inputs.Vert)) > 0.2f || Mathf.Abs(Input.GetAxisRaw(Inputs.Vert_K)) > 0f)
        {
            if (Mathf.Abs(Input.GetAxis(Inputs.Vert)) > 0.2f)
            {
                Vertical = Input.GetAxis(Inputs.Vert);
            }
            else if (Mathf.Abs(Input.GetAxis(Inputs.Vert_K)) > 0f)
            {
                Vertical = -Input.GetAxisRaw(Inputs.Vert_K);
            }
        }
        else if (Vertical != 0)
        {
            Vertical = 0;
        }

        // Hier setzen wir die Action Button Trigger zurück, da diese nur in dem Frame aktiv sein sollen, in dem sie auch gedrückt wurden
        if (Trigger_Action_One) Trigger_Action_One = false;
        if (Trigger_Action_Two) Trigger_Action_Two = false;
        if (Trigger_Action_Three) Trigger_Action_Three = false;
        if (Trigger_Action_Four) Trigger_Action_Four = false;
        if (Trigger_Action_Start) Trigger_Action_Start = false;

        if (Input.GetButtonDown(Inputs.ActionOne) || Input.GetButtonDown(Inputs.ActionOne_K))
        {
            Action_One = true;
            Trigger_Action_One = true;
        }
        else if (Action_One && 
            (Input.GetButton(Inputs.ActionOne) == false || Input.GetButton(Inputs.ActionOne_K) == false))
        {
            Action_One = false;
        }

        if (Input.GetButtonDown(Inputs.ActionTwo) || Input.GetButtonDown(Inputs.ActionTwo_K))
        {
            Action_Two = true;
            Trigger_Action_Two = true;
        }
        else if (Action_Two &&
            (Input.GetButton(Inputs.ActionTwo) == false || Input.GetButton(Inputs.ActionTwo_K) == false))
        {
            Action_Two = false;
        }

        if (Input.GetButtonDown(Inputs.ActionThree) || Input.GetButtonDown(Inputs.ActionThree_K))
        {
            Action_Three = true;
            Trigger_Action_Three = true;
        }
        else if (Action_Three &&
            (Input.GetButton(Inputs.ActionThree) == false || Input.GetButton(Inputs.ActionThree_K) == false))
        {
            Action_Three = false;
        }

        if (Input.GetButtonDown(Inputs.ActionFour) || Input.GetButtonDown(Inputs.ActionFour_K))
        {
            Action_Four = true;
            Trigger_Action_Four = true;
        }
        else if (Action_Four &&
            (Input.GetButton(Inputs.ActionFour) == false || Input.GetButton(Inputs.ActionFour_K) == false))
        {
            Action_Four = false;
        }

        if (Input.GetButtonDown(Inputs.Action_Start) || Input.GetButtonDown(Inputs.Action_Start_K))
        {
            Action_Start = true;
            Trigger_Action_Start = true;
        }
        else if (Action_Start &&
            (Input.GetButton(Inputs.Action_Start) == false || Input.GetButton(Inputs.Action_Start_K) == false))
        {
            Action_Start = false;
        }
    }

}

public class InputData
{
    public string Hori { get; private set; }
    public string Vert { get; private set; }

    public string ActionOne { get; private set; }
    public string ActionTwo { get; private set; }
    public string ActionThree { get; private set; }
    public string ActionFour { get; private set; }

    public string Action_Start { get; private set; }

    public string Hori_K { get; private set; }
    public string Vert_K { get; private set; }

    public string ActionOne_K { get; private set; }
    public string ActionTwo_K { get; private set; }
    public string ActionThree_K { get; private set; }
    public string ActionFour_K { get; private set; }

    public string Action_Start_K { get; private set; }

    public InputData()
    {
        Hori = "Hor_360";
        Vert = "Vert_360";
        ActionOne = "Action1_360";
        ActionTwo = "Action2_360";
        ActionThree = "Action3_360";
        ActionFour = "Action4_360";
        Action_Start = "Start_360";


        Hori_K = "Hor_Keyboard";
        Vert_K = "Vert_Keyboard";
        ActionOne_K = "Action1_Keyboard";
        ActionTwo_K = "Action2_Keyboard";
        ActionThree_K = "Action3_Keyboard";
        ActionFour_K = "Action4_Keyboard";
        Action_Start_K = "Start_Keyboard";
    }
}
