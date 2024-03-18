using System.Collections;
using System.Collections.Generic;
using StaminaSystem;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonBehaviour : MonoBehaviour
{
    public void retry()
    {
        // Reloads the current scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void settings()
    {
        // Get the name of the current scene
        string currentSceneName = SceneManager.GetActiveScene().name;

        // Conditional checks for the scene name and navigates accordingly
        if (currentSceneName == "Level_1 Long")
        {
            SceneManager.LoadScene("Level_2 Long");
        }
        else if (currentSceneName == "Level_2 Long")
        {
            SceneManager.LoadScene("Level_3 Long");
        }
        else if (currentSceneName == "Level_3 Long")
        {
            SceneManager.LoadScene("MainMenu");
            AudioManager.Instance.PlayMusic("MainMenu");
        }
    }

    public void mainMenu()
    {
        // Loads the MainMenu scene
        SceneManager.LoadScene("MainMenu");
    }
}
