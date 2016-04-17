using UnityEngine;
using System.Collections;

public class PulsingObject : MonoBehaviour {

    public enum EnumPulsingObjectType { SwitchType,JustPulse,JustSwitch,JustSwitchBG }
    public EnumPulsingObjectType Type;

    public enum EnumPulsingStates { KreisSolid,DreieckSolid,ViereckSolid,KreisUnsoild,DreieckUnsolid,ViereckUnsolid,KreisDamageSource,DreieckDamageSource,ViereckDamageSource }
    public EnumPulsingStates[] States;

    public Sprite KreisSprite, DreieckSprite, ViereckSprite,KreisTransparent,DreieckTransparent,ViereckTransparent,KreisDamage,DreieckDamage,ViereckDamage;

    private Animator anim;
    private SpriteRenderer rend;
    private BoxCollider2D bc2d;
    private int currentStateIndex;
	// Use this for initialization
	void Start () {
        PulseManagerHardCoded.Instance.RegisterPulse(Pulse);
        anim = GetComponent<Animator>();
        rend = GetComponent<SpriteRenderer>();
        bc2d = GetComponent<BoxCollider2D>();
        LevelController.Instance.ResgisterPulsingObject(this);
	}

    public void Unregister()
    {
        PulseManagerHardCoded.Instance.UnregisterPulse(Pulse);
    }

    void Pulse()
    {
        if((Type ==  EnumPulsingObjectType.SwitchType || Type== EnumPulsingObjectType.JustPulse) && anim != null )
            anim.SetTrigger("Pulse");

        if (Type == EnumPulsingObjectType.JustPulse)
            return;
        
        currentStateIndex++;

        if (currentStateIndex > States.Length - 1)
            currentStateIndex = 0;

        gameObject.tag = "Obstacle";
        rend.color = new Color(rend.color.r, rend.color.g, rend.color.b, 1f);

        if(bc2d != null)
        bc2d.enabled = true;

        switch (States[currentStateIndex])
        {
            case EnumPulsingStates.DreieckSolid:
                rend.sprite = DreieckSprite;
                break;
            case EnumPulsingStates.KreisSolid:
                rend.sprite = KreisSprite;                
                break;
            case EnumPulsingStates.ViereckSolid:
                rend.sprite = ViereckSprite;
                break;
            case EnumPulsingStates.DreieckUnsolid:
                rend.sprite = DreieckTransparent;
                rend.color = new Color(rend.color.r, rend.color.g, rend.color.b, 0.5f);
                bc2d.enabled = false;
                break;
            case EnumPulsingStates.ViereckUnsolid:
                rend.sprite = ViereckTransparent;
                rend.color = new Color(rend.color.r, rend.color.g, rend.color.b, 0.5f);
                bc2d.enabled = false;
                break;
            case EnumPulsingStates.KreisUnsoild:
                rend.sprite = KreisTransparent;
                rend.color = new Color(rend.color.r, rend.color.g, rend.color.b, 0.5f);
                bc2d.enabled = false;
                break;
            case EnumPulsingStates.DreieckDamageSource:
                rend.sprite = DreieckDamage;
                tag = "DamageSource";
                break;
            case EnumPulsingStates.KreisDamageSource:
                rend.sprite = KreisDamage;
                tag = "DamageSource";
                break;
            case EnumPulsingStates.ViereckDamageSource:
                rend.sprite = ViereckDamage;
                tag = "DamageSource";
                break;
        }
    }
}


