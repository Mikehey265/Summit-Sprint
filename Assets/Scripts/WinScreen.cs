using System;
using SaveSystem;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WinScreen : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timeText;
    [SerializeField] private TextMeshProUGUI bestTimeText;
    private GameData gameData;
    public string levelName;

    private void Start()
    {
        LoadGameData();
        DisplayTimes();
    }
    
    private void LoadGameData()
    {
        gameData = SaveManager.LoadData<GameData>(levelName) ?? new GameData();
    }

    private void DisplayTimes()
    {
        timeText.text = "Your time: " + FormatTime(gameData.time);
        bestTimeText.text = "Current best time: " + FormatTime(gameData.bestTimes[0]);
    }

    private string FormatTime(float seconds)
    {
        TimeSpan timeSpan = TimeSpan.FromSeconds(seconds);
        return string.Format("{0:D2}:{1:D2}:{2:D2}", timeSpan.Minutes, timeSpan.Seconds, timeSpan.Milliseconds / 10);
    }
}
