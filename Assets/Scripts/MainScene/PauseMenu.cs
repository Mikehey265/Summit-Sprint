using System;
using StaminaSystem;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public Ghost ghost;
    public DevGhost devGhost;
    public GameObject ghostToggle;
    public GameObject devGhostToggle;
    public GameObject ghostObject;
    public GameObject devGhostObject;
    [SerializeField] private GameObject pauseMenu; 
    [SerializeField] private GameObject onScreenMenu;

    private void Awake()
    {
        // ghostToggle.isOn = false;
         //devGhostToggle.isOn = false;
        // ghost.shouldPlayGhost = false;
        // devGhost.shouldPlayDevGhost = false;
       
    }

    private void Start()
    {
        InitializeGameObjectsBasedOnPrefs();
        EnableDevGhostButton();
    }
    
    
    void InitializeGameObjectsBasedOnPrefs() {
       // bool shouldPlayGhost = PlayerPrefs.GetInt("shouldPlayGhost", 0) == 1;
        ghostObject.SetActive(ghost.shouldPlayGhost);

       // bool shouldPlayDevGhost = PlayerPrefs.GetInt("shouldPlayDevGhost", 0) == 1;
        devGhostObject.SetActive(devGhost.shouldPlayDevGhost);
    }

    void EnableDevGhostButton()
    {
        if (devGhost != null)
        {
            devGhostToggle.SetActive(devGhost.isUnlocked);
            Debug.Log($"DevGhost isUnlocked: {devGhost.isUnlocked}");

        }
    }
    

    public void Pause()
    {
        pauseMenu.SetActive(true);
        onScreenMenu.SetActive(false);
        AudioManager.Instance.AdjustAmbience("GameMusic", 3f);
        GameManager.Instance.UpdateGameState(GameStateSO.State.Paused);
        Time.timeScale = 0;
    }

    public void Retry()
    {
        //SaveGhostState();
        AudioManager.Instance.AdjustAmbience("GameMusic", 0);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        GameManager.Instance.UpdateGameState(GameStateSO.State.CountdownToStart);
        Time.timeScale = 1;
    }

    private void SaveGhostState()
    {
        if (ghost != null)
        {
            PlayerPrefs.SetInt("shouldPlayGhost", ghost.shouldPlayGhost ? 1 : 0);
            PlayerPrefs.Save();
        }
    }

    public void Resume()
    {
        AudioManager.Instance.PlayFX("Resume");
        AudioManager.Instance.AdjustAmbience("GameMusic", 0);
        pauseMenu.SetActive(false);
        onScreenMenu.SetActive(true);
        GameManager.Instance.UpdateGameState(GameStateSO.State.CountdownToStart);
        Time.timeScale = 1;
    }

    public void MainMenu()
    {
        SceneManager.LoadScene(0);
        Time.timeScale = 1;
        AudioManager.Instance.PlayMusic("MainMenu");
        AudioManager.Instance.AdjustAmbience("GameMusic", 0);
    }

    public void ToggleGhost()
    {
        if (ghost != null)
        {
            ghost.shouldPlayGhost = !ghost.shouldPlayGhost;
            ghostObject.SetActive(ghost.shouldPlayGhost);
           // PlayerPrefs.SetInt("shouldPlayGhost", ghost.shouldPlayGhost ? 1 : 0);
           // PlayerPrefs.SetInt("shouldPlayGhost", ghostObject.activeSelf ? 1 : 0);
           // PlayerPrefs.Save();

        }
    }

    public void ToggleDevGhost()
    {
        if (devGhost != null)
        {
            devGhost.shouldPlayDevGhost = !devGhost.shouldPlayDevGhost;
            devGhostObject.SetActive(devGhost.shouldPlayDevGhost);
            //PlayerPrefs.SetInt("shouldPlayDevGhost", devGhost.shouldPlayDevGhost ? 1 : 0);
           // PlayerPrefs.Save();
        }

    }

}
