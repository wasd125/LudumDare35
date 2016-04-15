using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using UnityEngine.EventSystems;

public class TitleScreenButton : MonoBehaviour, IPointerEnterHandler
{
    [SerializeField]
    public enum EnumButtonState { Idle, Selected }
    
    public enum EnumButtonAction { Start,Load,Options,Quit,Unpause}

    private EnumButtonState buttonState;
    public EnumButtonState ButtonState
    {
        get
        { return buttonState; }
        set
        {
            buttonState = value;

            if (buttonState == EnumButtonState.Idle)
            {
                MySpriteRenderer.sprite = IdleSprite;
            }
            if (buttonState == EnumButtonState.Selected)
            {
                MySpriteRenderer.sprite = SelectedSprite;
            }
        }
    }

    public EnumButtonAction ButtonAction;

    public Sprite IdleSprite, SelectedSprite;

    public Image MySpriteRenderer;
    public Text TextComponent;
	
    public int MyID { get; set; }
    public Action<int> Hover;
	
    public void OnPointerEnter(PointerEventData eventData)
    {
        Hover(MyID);
    }
}
