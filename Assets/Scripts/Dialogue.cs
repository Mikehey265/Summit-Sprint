using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

[System.Serializable]
public class DialogueEvent
{
    public int index; // The index after which the event should be fired
    public UnityEvent dialogueAction; // The event to fire
}

public class Dialogue : MonoBehaviour
{
    public TextMeshProUGUI textComponent;
    public string[] lines;
    public float textSpeed;
    private int index = 0;

    public GameObject onboardingUI;
    public GameObject bodyPhysics; // GameObject that contains the BodyPhysics script

    public GameObject coachCliffRight;

    public GameObject coachCliffLeft;

    public List<DialogueEvent> dialogueEvents;

    private bool isLineFullyDisplayed = false;

    void Start()
    {
        // Deactivate the onboardingUI GameObject at the start
        if (onboardingUI != null)
        {
            onboardingUI.SetActive(false);
        }

        StartDialogue();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (!isLineFullyDisplayed)
            {
                CompleteLineDisplay();
            }
            else
            {
                // Check for events before moving to the next line
                CheckAndInvokeEvent();
                // Then, if there are more lines, proceed to the next one
                if (index < lines.Length - 1)
                {
                    NextLine();
                }
                else if (index == lines.Length - 1)
                {
                    // Handle the end of dialogue scenario
                    EndDialogue();
                }
            }
        }
    }

    void StartDialogue()
    {
        textComponent.text = string.Empty;
        StartCoroutine(TypeLine());
    }

    IEnumerator TypeLine()
    {
        isLineFullyDisplayed = false;
        foreach (char c in lines[index])
        {
            textComponent.text += c;
            yield return new WaitForSeconds(textSpeed);
        }
        isLineFullyDisplayed = true;
    }

    void CompleteLineDisplay()
    {
        StopAllCoroutines();
        textComponent.text = lines[index];
        isLineFullyDisplayed = true;
    }

    void NextLine()
    {
        index++;
        textComponent.text = string.Empty;
        StartCoroutine(TypeLine());
    }

    void CheckAndInvokeEvent()
    {
        foreach (DialogueEvent de in dialogueEvents)
        {
            if (de.index == index)
            {
                de.dialogueAction.Invoke();
                break;
            }
        }
    }

    void EndDialogue()
    {
        gameObject.SetActive(false);
        // Optionally, trigger events or other logic specific to the dialogue ending
    }

    public void LevelZero()
    {
        SceneManager.LoadScene("Level_0");
    }

    public void StartOnboarding()
    {
        // Assuming the BodyPhysics script is attached to the 'bodyPhysics' GameObject
        BodyPhysics bodyPhysicsScript = bodyPhysics.GetComponent<BodyPhysics>();
        if (bodyPhysicsScript != null)
        {
            // Enable the input by setting these booleans to true
            bodyPhysicsScript.InputIsEnabled = true;
            bodyPhysicsScript.InputIsEnabledLeftHand = true;
            bodyPhysicsScript.InputIsEnabledRightHand = true;
            bodyPhysicsScript.InputIsEnabledSlingShot = false;
        }

        coachCliffRight.SetActive(false);
    }

    public void StartJump()
    {
        // Assuming the BodyPhysics script is attached to the 'bodyPhysics' GameObject
        BodyPhysics bodyPhysicsScript = bodyPhysics.GetComponent<BodyPhysics>();
        if (bodyPhysicsScript != null)
        {
            // Enable the input by setting these booleans to true
            bodyPhysicsScript.InputIsEnabled = true;
            bodyPhysicsScript.InputIsEnabledLeftHand = true;
            bodyPhysicsScript.InputIsEnabledRightHand = true;
            bodyPhysicsScript.InputIsEnabledSlingShot = true;
        }

        coachCliffLeft.SetActive(false);
    }
}