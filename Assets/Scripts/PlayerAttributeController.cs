using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CharacterAttributeController
{
    Image EnegryBar;

    private float currentEnergy;
    public float CurrentEnergy
    {
        get { return currentEnergy; }
        set
        {
            currentEnergy = value;

            //EnegryBar.fillAmount = Max / current;
        }
    }

    public CharacterAttributeController(Image energyBar)
    {
        EnegryBar = energyBar;
    }
}
