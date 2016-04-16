using UnityEngine;
using System.Collections;

public class PulsingObject : MonoBehaviour {

    public enum EnumPulsingObjectType { SwitchType,JustPulse }
    public EnumPulsingObjectType Type;

    public enum EnumPulsingStates { Kreis,Dreieck,Viereck }
    public EnumPulsingStates[] States;

    public Sprite KreisSprite, DreieckSprite, ViereckSprite;

    private Animator anim;
    private SpriteRenderer rend;
    private int currentStateIndex;
	// Use this for initialization
	void Start () {
        PulseManagerHardCoded.Instance.RegisterPulse(Pulse);
        anim = GetComponent<Animator>();
        rend = GetComponent<SpriteRenderer>();
	}

    void Pulse()
    {
        anim.SetTrigger("Pulse");

        if (Type == EnumPulsingObjectType.JustPulse)
            return;
        
        currentStateIndex++;

        if (currentStateIndex > States.Length - 1)
            currentStateIndex = 0;

        switch (States[currentStateIndex])
        {
            case EnumPulsingStates.Dreieck:
                rend.sprite = DreieckSprite;
                break;
            case EnumPulsingStates.Kreis:
                rend.sprite = KreisSprite;
                break;
            case EnumPulsingStates.Viereck:
                rend.sprite = ViereckSprite;
                break;
        }
    }
}


