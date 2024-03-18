using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewSideObjective", menuName = "ScriptableObjects/SideObjectives")]
public class SideObjectivesSO : ScriptableObject
{
    public SideObjectiveEnum objectiveType;
    public bool isCompleted;

    [Header("JumpedLessThanX/JumpMoreThanX")]
    public int requiredJumps;
    public int jumpsCompleted;
    public string jumpsDescription;

    [Header("PickUpXChalk")] 
    public int amountOfChalkToPickup;
    public int chalkPickedUp;
    public string pickupChalkDescription;

    [Header("HandMovesLessThaX/HandMovesMoreThanX")]
    public int requiredHandMoves;
    public int handMovesMade;
    public string handMovesDescription;
}
