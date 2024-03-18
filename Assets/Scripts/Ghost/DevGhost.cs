using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "DevGhost", menuName = "ScriptableObjects/DevGhost", order = 1)]
public class DevGhost : ScriptableObject
{
    public bool isUnlocked;
    public bool clearData;
    public bool isRecording; 
    public bool isReplaying;
    public bool shouldPlayDevGhost;
    public float recordFrequency = 10f;
    [Header("--------------------------------------------------------------")]
    [Header("Body")]
    public List<float> timeStamp;
    public List<Vector3> position;
    public List<Quaternion> rotation;
    
    [Header("--------------------------------------------------------------")]
    [Header("LeftArm")]
    public List<Vector3> leftArmPosition;
    public List<Quaternion> leftArmRotation;
    
   
    [Header("--------------------------------------------------------------")]
    [Header("RightArm")]
    public List<Vector3> rightArmPosition;
    public List<Quaternion> rightArmRotation;
    
    [Header("--------------------------------------------------------------")]
    [Header("LeftFoot")]
    public List<Vector3> leftFootPosition;
    public List<Quaternion> leftFootRotation;
    
    [Header("--------------------------------------------------------------")]
    [Header("RightFoot")]
    public List<Vector3> rightFootPosition;
    public List<Quaternion> rightFootRotation;
    
    [Header("--------------------------------------------------------------")]
    [Header("RigValue")]
    public List<float> rigWeight;
    
    public void ResetData()
    {
        timeStamp.Clear();
        position.Clear();
        rotation.Clear();
        rightArmPosition.Clear();
        rightArmRotation.Clear();
        leftArmPosition.Clear();
        leftArmRotation.Clear();
        rightFootPosition.Clear();
        rightFootRotation.Clear();
        leftFootPosition.Clear();
        leftFootRotation.Clear();
        rigWeight.Clear();
    }
}
