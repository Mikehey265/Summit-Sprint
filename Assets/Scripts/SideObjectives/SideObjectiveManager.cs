using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public enum SideObjectiveEnum
{
    NoJumping,
    JumpedLessThanX,
    JumpedMoreThanX,
    PickUpXChalk,
    HandMovesLessThanX,
    HandMovesMoreThanX,
}

public class SideObjectiveManager : MonoBehaviour
{
    [SerializeField] private List<SideObjectivesSO> sideObjectives = new List<SideObjectivesSO>();
    private List<SideObjectivesSO> selectedObjectives = new List<SideObjectivesSO>();

    [SerializeField] private DevGhost devGhostSO;

    [Header("OnScreenGameObjects")]
    [SerializeField] private GameObject jumpGameObject;
    [SerializeField] private GameObject handGameObject;
    [SerializeField] private GameObject chalkGameObject;
    
    [Header("OnScreenUIText")]
    [SerializeField] private TextMeshProUGUI jumpText;
    [SerializeField] private TextMeshProUGUI chalkText;
    [SerializeField] private TextMeshProUGUI handText;

    [Header("PauseMenuGameObjects")] 
    [SerializeField] private GameObject jumpPause;
    [SerializeField] private GameObject handPause;
    [SerializeField] private GameObject chalkPause;
    
    [Header("PauseMenuUIText")] 
    [SerializeField] private TextMeshProUGUI jumpDescription;
    [SerializeField] private TextMeshProUGUI handDescription;
    [SerializeField] private TextMeshProUGUI chalkDecription;
    
    private BodyPhysics bodyPhysics;
    private SlingShot slingShot;
    private PlayerStamina playerStamina;

    private void Awake()
    {
        slingShot = FindObjectOfType<SlingShot>();
        playerStamina = FindObjectOfType<PlayerStamina>();
        bodyPhysics = FindObjectOfType<BodyPhysics>();
    }

    private void Start()
    {
        selectedObjectives = GetSelectedObjectives();
    }

    private void Update()
    {
        foreach (SideObjectivesSO objectives in selectedObjectives)
        {
            if (objectives != null)
            {
                objectives.jumpsCompleted = slingShot.jumpCount;
                objectives.chalkPickedUp = playerStamina.chalkPickedUpCount;
                objectives.handMovesMade = bodyPhysics.armMoves;
            }
            else
            {
                Debug.LogWarning("Objectives not selected!");
            }
        }

        if (AreAllObjectivesCompleted())
        {
            devGhostSO.isUnlocked = true;
            Debug.LogWarning("All side objectives completed! Developer ghost unlocked!");
        }
        UpdateUIElements();
    }
    
    private void UpdateUIElements()
    {
        if (!selectedObjectives[0].isCompleted)
        {
            jumpText.text = selectedObjectives[0].jumpsCompleted + "/" + selectedObjectives[0].requiredJumps;
            jumpDescription.text = selectedObjectives[0].jumpsDescription;
        }
        else
        {
            jumpGameObject.SetActive(false);
            jumpPause.SetActive(false);
        }

        if (!selectedObjectives[1].isCompleted)
        {
            handText.text = selectedObjectives[1].handMovesMade + "/" + selectedObjectives[1].requiredHandMoves;
            handDescription.text = selectedObjectives[1].handMovesDescription;
        }
        else
        {
            handGameObject.SetActive(false);
            handPause.SetActive(false);
        }

        if (!selectedObjectives[2].isCompleted)
        {
            chalkText.text = selectedObjectives[2].chalkPickedUp + "/" + selectedObjectives[2].amountOfChalkToPickup;
            chalkDecription.text = selectedObjectives[2].pickupChalkDescription;
        }
        else
        {
            chalkGameObject.SetActive(false);
            chalkPause.SetActive(false);
        }
    }

    public void CheckSideObjectiveCompletion()
    {
        foreach (SideObjectivesSO objective in selectedObjectives)
        {
            if (!IsObjectiveCompleted(objective.objectiveType))
            {
                switch (objective.objectiveType)
                {
                    case SideObjectiveEnum.NoJumping:
                        if(!slingShot.hasPlayerJumpedThisRun)
                            CompleteObjective(SideObjectiveEnum.NoJumping);
                        break;
                    case SideObjectiveEnum.JumpedLessThanX:
                        if(slingShot.jumpCount <= objective.requiredJumps)
                            CompleteObjective(SideObjectiveEnum.JumpedLessThanX);
                        break;
                    case SideObjectiveEnum.JumpedMoreThanX:
                        if(slingShot.jumpCount >= objective.requiredJumps)
                            CompleteObjective(SideObjectiveEnum.JumpedMoreThanX);
                        break;
                    case SideObjectiveEnum.PickUpXChalk:
                        if(playerStamina.chalkPickedUpCount >= objective.amountOfChalkToPickup)
                            CompleteObjective(SideObjectiveEnum.PickUpXChalk);
                        break;
                    case SideObjectiveEnum.HandMovesLessThanX:
                        if(bodyPhysics.armMoves < objective.requiredHandMoves)
                            CompleteObjective(SideObjectiveEnum.HandMovesLessThanX);
                        break;
                    case SideObjectiveEnum.HandMovesMoreThanX:
                        if(bodyPhysics.armMoves >= objective.requiredHandMoves)
                            CompleteObjective(SideObjectiveEnum.HandMovesMoreThanX);
                        break;
                }
            }
        }
    }

    private void CompleteObjective(SideObjectiveEnum objectiveType)
    {
        SideObjectivesSO sideObjective = selectedObjectives.Find(obj => obj.objectiveType == objectiveType);

        if (sideObjective != null && !sideObjective.isCompleted)
        {
            sideObjective.isCompleted = true;
        }
    }

    private bool IsObjectiveCompleted(SideObjectiveEnum objectiveType)
    {
        return selectedObjectives.Exists(obj => obj.objectiveType == objectiveType && obj.isCompleted);
    }

    private bool AreAllObjectivesCompleted()
    {
        foreach (SideObjectivesSO objective in sideObjectives)
        {
            if (!objective.isCompleted)
            {
                return false;
            }
        }

        return true;
    }

    private List<SideObjectivesSO> GetSelectedObjectives()
    {
        List<SideObjectivesSO> selected = new List<SideObjectivesSO>();

        for (int i = 0; i < sideObjectives.Count; i++)
        {
            selected.Add(sideObjectives[i]);
        }

        return selected;
    }
}
