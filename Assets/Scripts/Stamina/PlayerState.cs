using System;
using System.Collections;
using System.Collections.Generic;
using StaminaSystem;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerState : MonoBehaviour
{
    private PlayerStamina stamina;
    public float ambienceTarget = 2.5f;
    public float lerpSpeed = 1f; // Adjust this speed as needed
    private float currentAmbience = 0f;

    void Start()
    {
        stamina = GetComponent<PlayerStamina>();
    }

    // Update is called once per frame
    void Update()
    {
        MaterialChange();
    }
    
    private void MaterialChange()
    {
       
            switch (stamina.CurrentStaminaState)
            {
                case PlayerStamina.StaminaStates.FullStamina:
                    currentAmbience = Mathf.Lerp(currentAmbience, 0f, Time.deltaTime * lerpSpeed);
                    break;
                case PlayerStamina.StaminaStates.MediumStamina:
                    currentAmbience = Mathf.Lerp(currentAmbience, ambienceTarget, Time.deltaTime * lerpSpeed);
                    AudioManager.Instance.AdjustAmbience("Theme", currentAmbience);
                    break;
                case PlayerStamina.StaminaStates.LowStamina:
                    break;
                case PlayerStamina.StaminaStates.NoStamina:
                    GameManager.Instance.UpdateGameState(GameStateSO.State.Lose);
                    break;
            }
        
    }
}