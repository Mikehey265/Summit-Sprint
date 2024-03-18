using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacemakerVDScript : MonoBehaviour
{
    public GameObject Victory;
    public GameObject Loss;

    public void Win()
    {
        if (Victory != null)
        {
            Victory.SetActive(true);
        }
        else
        {
            Debug.LogError("Victory GameObject is not assigned!");
        }
    }

    public void Lose()
    {
        if (Loss != null)
        {
            Loss.SetActive(true);
        }
        else
        {
            Debug.LogError("Lose GameObject is not assigned!");
        }
    }
}
