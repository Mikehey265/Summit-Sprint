using System.Collections;
using System.Collections.Generic;
using StaminaSystem;
using UnityEngine;

public class ChalkPickUp : MonoBehaviour
{
    private PlayerStamina _stamina;
    private SlingShot _slingShot;
    [SerializeField] private float rotationSpeed = 100f;
    [SerializeField] private float respawnTime = 10f;
    
    private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.CompareTag("Player")) return;
        AudioManager.Instance.PlayFX("ChalkPickup");

        _stamina.currentChalk = _stamina.playerStats.maxChalk;
        _stamina.currentStamina = _stamina.playerStats.maxStamina;
        _stamina.chalkPickedUpCount++;
        _slingShot.hasJumpedWithNoChalk = false;
        gameObject.SetActive(false);
        Invoke(nameof(Reactivate), respawnTime);
    }

    private void Reactivate()
    {
        gameObject.SetActive(true);
    }
    void Start()
    {
        _stamina = FindObjectOfType<PlayerStamina>();
        _slingShot = FindObjectOfType<SlingShot>();
    }
    
    void Update()
    {
        gameObject.transform.Rotate(0, rotationSpeed * Time.deltaTime, 0);
    }
}
