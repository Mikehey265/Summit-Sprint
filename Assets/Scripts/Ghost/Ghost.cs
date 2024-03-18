using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "Ghost", menuName = "ScriptableObjects/Ghost", order = 1)]
public class Ghost : ScriptableObject
{
    public bool clearData;
   // public bool clearLastData;
    public bool isRecording; 
    public bool isReplaying;
    public bool shouldPlayGhost;
    public bool saveLastData;
    public float recordFrequency = 10f;
    [Header("--------------------------------------------------------------")]
    [Header("Body")]
    public List<float> timeStamp;
    public List<Vector3> position;
    public List<Quaternion> rotation;
    
    public List<float> lastTimeStamp;
    public List<Vector3> lastPosition;
    public List<Quaternion> lastRotation;
    [Header("--------------------------------------------------------------")]
    [Header("LeftArm")]
    public List<Vector3> leftArmPosition;
    public List<Quaternion> leftArmRotation;
    
    public List<Vector3> leftArmLastPosition;
    public List<Quaternion> leftArmLastRotation;
    [Header("--------------------------------------------------------------")]
    [Header("RightArm")]
    public List<Vector3> rightArmPosition;
    public List<Quaternion> rightArmRotation;
    
    public List<Vector3> rightArmLastPosition;
    public List<Quaternion> rightArmLastRotation;
    [Header("--------------------------------------------------------------")]
    [Header("LeftFoot")]
    public List<Vector3> leftFootPosition;
    public List<Quaternion> leftFootRotation;
    
    public List<Vector3> leftFootLastPosition;
    public List<Quaternion> leftFootLastRotation;
    [Header("--------------------------------------------------------------")]
    [Header("RightFoot")]
    public List<Vector3> rightFootPosition;
    public List<Quaternion> rightFootRotation;
    
    public List<Vector3> rightFootLastPosition;
    public List<Quaternion> rightFootLastRotation;
    [Header("--------------------------------------------------------------")]
    [Header("RigValue")]
    public List<float> rigWeight;
    public List<float> lastRigWeight;
    
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

    // public void ResetLastData()
    // {
    //     lastTimeStamp.Clear();
    //     lastPosition.Clear();
    //     lastRotation.Clear();
    //     rightArmLastPosition.Clear();
    //     rightArmLastRotation.Clear();
    //     leftArmLastPosition.Clear();
    //     leftArmLastRotation.Clear();
    //     rightFootLastPosition.Clear();
    //     rightFootLastRotation.Clear();
    //     leftFootLastPosition.Clear();
    //     leftFootLastRotation.Clear();
    //     lastRigWeight.Clear();
    // }
    //
    public void SaveLastData()
    {
        lastTimeStamp = new List<float>(timeStamp);
        lastPosition = new List<Vector3>(position);
        lastRotation = new List<Quaternion>(rotation);
        rightArmLastPosition = new List<Vector3>(rightArmPosition);
        rightArmLastRotation = new List<Quaternion>(rightArmRotation);
        leftArmLastPosition = new List<Vector3>(leftArmPosition);
        leftArmLastRotation = new List<Quaternion>(leftArmRotation);
        rightFootLastPosition = new List<Vector3>(rightFootPosition);
        rightFootLastRotation = new List<Quaternion>(rightFootRotation);
        leftFootLastPosition = new List<Vector3>(leftFootPosition);
        leftFootLastRotation = new List<Quaternion>(leftFootRotation);
        lastRigWeight = new List<float>(rigWeight);
    }

}
