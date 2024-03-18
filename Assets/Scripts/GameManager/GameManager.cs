using StaminaSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

//Script made by Michał Szcząchor
//if there will be a need for more states, you can add them in GameStateSO script
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private GameStateSO gameState;
    [SerializeField] private Transform playerSpawnPosition;
    [SerializeField] private BodyPhysics playerPrefab;

    [Header("UI")] 
    [SerializeField] private GameObject onScreenPanel;
    [SerializeField] private GameObject winPanel;
    [SerializeField] private GameObject losePanel;
    [SerializeField] private TextMeshProUGUI countdownText;
    [SerializeField] private GameObject countdownPanel;
    
    public bool isGameRunning;
    public bool isGameWon;
    public bool isGameLost;

    private BodyPhysics player;
    private ScoreManager scoreManager;
    private SideObjectiveManager objectiveManager;
    private float countdownTimer;
    private bool hasGameStarted;
    
    private void Awake()
    {
        Instance = this;
        // Instantiate(playerPrefab, playerSpawnPosition);
        player = FindObjectOfType<BodyPhysics>();
        scoreManager = FindObjectOfType<ScoreManager>();
        objectiveManager = FindObjectOfType<SideObjectiveManager>();
    }

    private void Start()
    {
        hasGameStarted = false;
        isGameWon = false;
        isGameLost = false;
        UpdateGameState(GameStateSO.State.WaitingForPlayerInput);
        onScreenPanel.SetActive(true);
    }

    private void Update()
    {
        if (Input.touchCount > 0 && !hasGameStarted)
        {
            if (SceneManager.GetActiveScene().name != "Level_0") // Check if the current scene is not "Level_0"
            {
                UpdateGameState(GameStateSO.State.Playing);
                hasGameStarted = true;
            }
            else
            {
            }
        }
    }

    //call this method whenever you want to change game state
    public void UpdateGameState(GameStateSO.State newState)
    {
        gameState.currentState = newState;
        
        switch (newState)
        {
            case GameStateSO.State.WaitingForPlayerInput:
                HandleWaitingForPlayerInputState();
                break;
            case GameStateSO.State.CountdownToStart:
                HandleCountdownState();
                break;
            case GameStateSO.State.Playing:
                HandlePlayingState();
                break;
            case GameStateSO.State.Paused:
                HandlePausedState();
                break;
            case GameStateSO.State.Win:
                HandleWinState();
                break;
            case GameStateSO.State.Lose:
                HandleLoseState();
                break;
            default:
                break;
        }
    }

    private void HandleWaitingForPlayerInputState()
    {
        countdownPanel.SetActive(false);
        isGameRunning = false;
        player.InputIsEnabled = false;
    }

    private void HandleCountdownState()
    {
        countdownPanel.SetActive(true);
        StartCoroutine(StartCountdown(gameState.countdownToStartState.countdownTimer));
        isGameRunning = false;
        player.InputIsEnabled = false;
    }

    private void HandlePlayingState()
    {
        countdownPanel.SetActive(false);
        isGameRunning = true;
        player.InputIsEnabled = true;
        winPanel.SetActive(false);
        losePanel.SetActive(false);
    }
    
    private void HandlePausedState()
    {
        isGameRunning = false;
        player.InputIsEnabled = false;
        countdownPanel.SetActive(false);
    }
    
    private void HandleWinState()
    {
        if(isGameWon) return;
        AudioManager.Instance.PlayFX("WinSound");
        scoreManager.SaveScore();
        isGameWon = true;
        isGameRunning = false;
        winPanel.SetActive(true);
        onScreenPanel.SetActive(false);
        player.InputIsEnabled = false;
        objectiveManager.CheckSideObjectiveCompletion();
    }
    
    private void HandleLoseState()
    {
        if (!isGameLost) 
        {
            AudioManager.Instance.PlayFX("LooseSound");
            isGameLost = true; 
            isGameRunning = false;
            player.InputIsEnabled = false;
            losePanel.SetActive(true);
        }
    }


    private IEnumerator StartCountdown(float countdownDuration)
    {
        countdownText.text = "3";
        yield return new WaitForSeconds(countdownDuration / 3f);
        
        countdownText.text = "2";
        yield return new WaitForSeconds(countdownDuration / 3f);
        
        countdownText.text = "1";
        yield return new WaitForSeconds(countdownDuration / 3f);
        
        countdownText.text = "Go!";
        yield return new WaitForSeconds(countdownDuration / 3f);
        
        UpdateGameState(GameStateSO.State.Playing);
    }
}
