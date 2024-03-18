using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using StaminaSystem;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

//Scipt made by: Daniel Alvarado

public class PlayerStamina : MonoBehaviour
{
    [SerializeField] public PlayerStats playerStats;
    [SerializeField] public float currentChalk;
    public float currentStamina;
    [SerializeField] private Slider chalkBar;
    [SerializeField] private Slider staminaBar;
    [SerializeField] private BodyPhysics bodyPhysics;
    private float _maxStamina;
    internal float chalkDeplete;
    internal float chalkJumpDeplete;
    private bool _isClimbing;
    //added for side objective purposes
    public int chalkPickedUpCount;
    public bool runOutOfChalk;
    private float timeSpendWithoutChalk;
    public int timeWithoutChalk;
    
    public enum StaminaStates
    {
        FullStamina,
        MediumStamina,
        LowStamina,
        NoStamina,
    }
    public StaminaStates CurrentStaminaState { get; private set; }
    

    private void Start()
    {
        currentChalk = playerStats.maxChalk;
        chalkBar.maxValue = playerStats.maxChalk;
        staminaBar.maxValue = playerStats.maxStamina;
        currentStamina = playerStats.maxStamina;
        chalkPickedUpCount = 0;
        timeSpendWithoutChalk = 0;
        runOutOfChalk = false;
    }
    
    private void Update()
    {
        Debug.Log("Chalk picked up count: " + chalkPickedUpCount);
        chalkBar.value = currentChalk;
        if (currentStamina <= 0)
        {
            GameManager.Instance.UpdateGameState(GameStateSO.State.Lose);
        }


        if (GameManager.Instance.isGameWon)
        {
            currentStamina = playerStats.maxStamina;  
            currentChalk = playerStats.maxChalk;
        }
        
        StaminaState();
        StartDepleteStamina();
    }
    
    public void ResetStamina()
    {
        currentStamina = playerStats.maxStamina;
        staminaBar.value = currentStamina;
    }

    
    private void StartDepleteStamina()
    {
        if(currentChalk <= 0 && GameManager.Instance.isGameRunning)
        {
            staminaBar.gameObject.SetActive(true);
            staminaBar.value = currentStamina;
            currentStamina -= playerStats.staminaDepleteRate * Time.deltaTime;
            runOutOfChalk = true;
            timeSpendWithoutChalk += Time.deltaTime;
            timeWithoutChalk = (int)timeSpendWithoutChalk;
        }
        else
        {
            staminaBar.gameObject.SetActive(false);
        }
    }

    
    public void DepleteChalk()
    {
        if(currentChalk > 0)
        {
            currentChalk -= playerStats.chalkDeplete;
        }
    }
    
    public void DepleteChalkOnSlingShot()
    {
        currentChalk -= playerStats.chalkDepleteOnSlingShot;
    }


    public void IncreaseChalk()
    {
        currentChalk = playerStats.maxChalk;
    }
    
    private void StaminaState()
    {
        if (currentStamina <= playerStats.noStaminaValue)
        {
            CurrentStaminaState = StaminaStates.NoStamina;
        }
        else if (currentStamina <= playerStats.lowStaminaValue)
        {
            CurrentStaminaState = StaminaStates.LowStamina;
        }
        else if (currentStamina <= playerStats.mediumStaminaValue)
        {
            CurrentStaminaState = StaminaStates.MediumStamina;
        }
        else if (currentStamina <= playerStats.fullStaminaValue)
        {
            CurrentStaminaState = StaminaStates.FullStamina;
        }
    }
}
