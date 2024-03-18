using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelsButton : MonoBehaviour
{
    public void LevelOneSwitch()
    {
        SceneManager.LoadScene("Level_1 Long");
    }

    public void LevelTwoSwitch()
    {
        SceneManager.LoadScene("Level_2 Long");
    }

    public void LevelThreeSwitch()
    {
        SceneManager.LoadScene("Level_3 Long");
    }

    public void BackButton()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
