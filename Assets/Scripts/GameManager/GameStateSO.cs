using UnityEngine;

//Script made by Michał Szcząchor
[CreateAssetMenu(fileName = "NewGameStateSO", menuName = "ScriptableObjects/GameState")]
public class GameStateSO : ScriptableObject
{
    public enum State
    {
        CountdownToStart,
        WaitingForPlayerInput,
        Playing,
        Paused,
        Win,
        Lose
    }

    public State currentState;
    
    public CountdownToStartGameState countdownToStartState;
    public WaitingForPlayerInputState waitingForPlayerInputState;
    public PlayingGameState playingState;
    public PausedGameState pausedGameState;
    public WinGameState winGameState;
    public LoseGameState loseGameState;
    
    [System.Serializable]
    public class CountdownToStartGameState
    {
        public float countdownTimer;
    }
    
    [System.Serializable]
    public class WaitingForPlayerInputState
    {
        
    }
    
    [System.Serializable]
    public class PlayingGameState
    {
        public float staminaValue;
    }
    
    [System.Serializable]
    public class PausedGameState
    {
        
    }
    
    [System.Serializable]
    public class WinGameState
    {
        
    }
    
    [System.Serializable]
    public class LoseGameState
    {
        
    }
}
