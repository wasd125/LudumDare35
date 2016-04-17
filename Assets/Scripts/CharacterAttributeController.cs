using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CharacterAttributeController
{
    Image EnegryBar;
    HPController HpController;

    private const int MAX_HP = 3;

    private int currentHP;
    public int CurrentHP
    {
        get { return currentHP; }
        set
        {            
            currentHP = value;

            if (currentHP >= MAX_HP)
                currentHP = MAX_HP;

            if(HpController != null)
            HpController.SetHP(currentHP);
        }
    }

    public float engergyRegPerFrame {get; private set;}
    public const float MAX_ENERGY = 40f;

    private float currentEnergy;
    public float CurrentEnergy
    {
        get { return currentEnergy; }
        set
        {
            currentEnergy = value;

            if(EnegryBar!= null)
            EnegryBar.fillAmount = currentEnergy / MAX_ENERGY;
        }
    }

    public void RegEnergy(float amount)
    {
        CurrentEnergy += amount;
        if (CurrentEnergy > MAX_ENERGY)
            CurrentEnergy = MAX_ENERGY;
    }

    public CharacterAttributeController(Image energyBar, HPController hpc)
    {
        EnegryBar = energyBar;
        HpController = hpc;
        CurrentEnergy = MAX_ENERGY;
        CurrentHP = MAX_HP;
    }
}
