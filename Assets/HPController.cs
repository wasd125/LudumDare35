using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HPController : MonoBehaviour {

    public Sprite FullHeartSprite, EmpyHeartSprite;

    public Image HeartOne, HeartTwo, HearthThree;

    public void SetHP(int amount)
    {
        HeartOne.sprite = EmpyHeartSprite;
        HeartTwo.sprite = EmpyHeartSprite;
        HearthThree.sprite = EmpyHeartSprite;

        if (amount >= 1)
            HeartOne.sprite = FullHeartSprite;
        if (amount >= 2)
            HeartTwo.sprite = FullHeartSprite;
        if (amount >= 3)
            HearthThree.sprite = FullHeartSprite;
    }
	

}
