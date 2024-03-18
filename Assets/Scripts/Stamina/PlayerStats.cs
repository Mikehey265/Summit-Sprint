using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "PlayerStats", menuName = "ScriptableObjects/PlayerStats", order = 1)]
public class PlayerStats : ScriptableObject
{
    [Space(10)]
    [SerializeField]internal  float maxChalk;
    [Space(10)]
    [SerializeField] internal float chalkDeplete;
    [Space(10)]
    [SerializeField] internal float chalkDepleteOnSlingShot;
    [Space(10)]
    [SerializeField] internal float maxStamina;
    [FormerlySerializedAs("staminaDeplete")]
    [Space(10)]
    [SerializeField] internal float staminaDepleteRate;
    [Space(10)]
    [Header("Stamina States")]
    [SerializeField] internal float fullStaminaValue = 100f;
    [SerializeField] internal float mediumStaminaValue = 75f;
    [SerializeField] internal float lowStaminaValue = 50f;
    [SerializeField] internal float noStaminaValue = 0f;
    
    
}