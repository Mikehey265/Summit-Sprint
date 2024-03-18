using System;
using System.Collections;
using System.Collections.Generic;
using StaminaSystem;
using UnityEngine;

public class Ground : MonoBehaviour
{
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player") && GameManager.Instance.isGameRunning)
        {
            Debug.Log(other);
            GameManager.Instance.UpdateGameState(GameStateSO.State.Lose);
        }
    }
}
