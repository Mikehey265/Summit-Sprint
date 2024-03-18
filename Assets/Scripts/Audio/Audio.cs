using System.Collections;
using System.Collections.Generic;
using StaminaSystem;
using UnityEngine;
[CreateAssetMenu(fileName = "Audio", menuName = "ScriptableObjects/Audio", order = 1)]
public class Audio : ScriptableObject
{
    [SerializeField] internal Sound[] music,fxSounds;
    [Header("Low Pass Filter Settings")]
    [SerializeField] internal float maxCutoffFrequency = 5000;
    [SerializeField] internal float minCutoffFrequency = 230;
    [Space(10)]
    [SerializeField] internal float maxResonanceQ = 1.75f;
    [SerializeField] internal float minResonanceQ = 1f;
    [SerializeField] internal AudioSource musicSource;
    [SerializeField] internal AudioSource sfxSource;
}
