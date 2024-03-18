using System;
using System.Collections;
using System.Collections.Generic;
using StaminaSystem;
using UnityEngine;

public class PlayWinSound : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
			// we do this in gamemanager instead!!!!!!!!!!
           //AudioManager.Instance.PlayFX("WinSound");
        }
    }
}
