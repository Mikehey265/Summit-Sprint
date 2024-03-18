using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SaveSystem;
using TMPro;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] private int numberOfSavedTimes = 5;
    [SerializeField] private TextMeshProUGUI currentTimeText;
    [SerializeField] private TextMeshProUGUI bestTimeText;
    [SerializeField] private string levelName;

    [Header("Medal times (in seconds)")] 
    [SerializeField] private float goldMedalTime;
    [SerializeField] private float silverMedalTime;
    [SerializeField] private float bronzeMedalTime;

    [Header("Medal color game object")] 
    [SerializeField] private GameObject goldMedalGameObject;
    [SerializeField] private GameObject silverMedalGameObject;
    [SerializeField] private GameObject bronzeMedalGameObject;
    

    private GameData gameData;
    private float currentGameTime;
    private bool isCurrentTimeSaved;

    private void Start()
    {
        LoadGameData();
        UpdateUIBestTime();
        UpdateMedalGameObject();
        isCurrentTimeSaved = false;
        gameData.levelId = levelName;
    }

    private void Update()
    {
        if (GameManager.Instance.isGameRunning)
        {
            currentGameTime += Time.deltaTime;
            UpdateCurrentUITime(); // Call method to update UI
        }
        DeleteSaveFile();
    }

    private void UpdateCurrentUITime()
    {
        // Update the text of the TMP Text element with the current time
        currentTimeText.text = FormatTime(currentGameTime); // Format to display two decimal places
    }

    private void UpdateUIBestTime()
    {
        if (gameData.bestTimes.Count > 0)
        {
            bestTimeText.text = FormatTime(gameData.bestTimes[0]);
        }
    }

    public void SaveScore()
    {
        gameData.time = Mathf.Floor(currentGameTime * 100) / 100;
    
        if (gameData.bestTimes.Count < numberOfSavedTimes || currentGameTime < gameData.bestTimes[0])
        {
            if (gameData.bestTimes.Count == 0 || currentGameTime < gameData.bestTimes[0])
            {
                gameData.bestTimes.Insert(0, Mathf.Floor(currentGameTime * 100) / 100);
            
                if (gameData.bestTimes.Count > numberOfSavedTimes)
                {
                    gameData.bestTimes.RemoveAt(numberOfSavedTimes);
                }
    
                Debug.Log("New high score!");
            }
        }
        else 
        { 
            Debug.Log("You have not reached a new high score :(");
        }
            
        SaveManager.SaveData(gameData, levelName);
        isCurrentTimeSaved = true;
        MedalSelection();
    }
    
    private void UpdateMedalGameObject()
    {
        if (gameData.bestTimes.Count > 0)
        {
            if (gameData.bestTimes[0] <= goldMedalTime)
            {
                goldMedalGameObject.SetActive(true);
                silverMedalGameObject.SetActive(false);
                bronzeMedalGameObject.SetActive(false);
            }
            else if (gameData.bestTimes[0] > goldMedalTime && gameData.bestTimes[0] <= silverMedalTime)
            {
                goldMedalGameObject.SetActive(false);
                silverMedalGameObject.SetActive(true);
                bronzeMedalGameObject.SetActive(false);
            }
            else if (gameData.bestTimes[0] > silverMedalTime && gameData.bestTimes[0] <= bronzeMedalTime)
            {
                goldMedalGameObject.SetActive(false);
                silverMedalGameObject.SetActive(false);
                bronzeMedalGameObject.SetActive(true);
            }
            else if (gameData.bestTimes[0] > bronzeMedalTime)
            {
                goldMedalGameObject.SetActive(false);
                silverMedalGameObject.SetActive(false);
                bronzeMedalGameObject.SetActive(false);
            }   
        }
        else
        {
            goldMedalGameObject.SetActive(false);
            silverMedalGameObject.SetActive(false);
            bronzeMedalGameObject.SetActive(false);
        }
    }

    private void MedalSelection()
    {
        if (currentGameTime <= goldMedalTime)
        {
            Debug.Log("Player earned gold medal");
        }
        else if (currentGameTime > goldMedalTime && currentGameTime <= silverMedalTime)
        {
            Debug.Log("Player earned silver medal");
        }
        else if (currentGameTime > silverMedalTime && currentGameTime <= bronzeMedalTime)
        {
            Debug.Log("Player earned bronze medal");
        }
        else if (currentGameTime > bronzeMedalTime)
        {
            Debug.Log("No medals earned");
        }
    }
    
    private void LoadGameData()
    {
        gameData = SaveManager.LoadData<GameData>(levelName) ?? new GameData();
    }
    
    private string FormatTime(float seconds)
    {
        TimeSpan timeSpan = TimeSpan.FromSeconds(seconds);
        return string.Format("{0:D2}:{1:D2}:{2:D2}", timeSpan.Minutes, timeSpan.Seconds, timeSpan.Milliseconds / 10);
    }
    
    //for keyboard use only
    private void DeleteSaveFile()
    {
        if(Input.GetKeyDown(KeyCode.D))
        {
            SaveManager.DeleteSaveData(levelName);
        }
        
    }
}