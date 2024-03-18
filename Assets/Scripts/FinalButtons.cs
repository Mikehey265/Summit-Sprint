using System.Collections;
using System.Collections.Generic;
using StaminaSystem;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinalButtons : MonoBehaviour
{
    public void SwitchScene()
    {
        SceneManager.LoadScene("Level_1 Long");
        AudioManager.Instance.PlayMusic("GameMusic");
    }

    public void SwitchScene2()
    {
        SceneManager.LoadScene("LevelSelector");
    }
}
