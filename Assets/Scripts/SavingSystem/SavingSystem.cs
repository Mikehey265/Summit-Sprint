using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace SaveSystem
{
    public abstract class SaveManager
    {
        // private static readonly string savePath = Application.persistentDataPath + "/saveData.json";

        public static void SaveData(object data, string levelId)
        {
            string jsonData = JsonUtility.ToJson(data);
            File.WriteAllText(Application.persistentDataPath + $"/saveData_{levelId}.json", jsonData);
            Debug.Log("Progress saved in: " + levelId);
        }

        public static T LoadData<T>(string levelId)
        {
            string savePath = Application.persistentDataPath + $"/saveData_{levelId}.json";
            if (File.Exists(savePath))
            {
                string jsonData = File.ReadAllText(savePath);
                return JsonUtility.FromJson<T>(jsonData);
            }
            else
            {
                Debug.LogWarning("No saved data found for level: " + levelId);
                return default(T);
            }
        }

        public static void DeleteSaveData(string levelId)
        {
            string savePath = Application.persistentDataPath + $"/saveData_{levelId}.json";
            
            if (File.Exists(savePath))
            {
                File.Delete(savePath);
                Debug.Log("Save data deleted for level: " + levelId);
            }
            else
            {
                Debug.LogWarning("No saved data found");
            }
        }
    } 
}