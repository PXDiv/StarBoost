using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class LevelManager
{
    public static readonly string CurrentUnfinishedLevelKey = "CurrentUnfinishedLevel";


    public static void LoadLevel(int levelNo)
    {
        Time.timeScale = 1;

        LevelList levelList = Resources.Load<LevelList>("LevelList");
        if (levelNo <= levelList.GetAvailableLevelNamesList().Count)
        {
            SceneManager.LoadScene(levelList.GetSceneNameByLevelNumber(levelNo));
            SceneManager.LoadSceneAsync("BootStraper", LoadSceneMode.Additive);
        }
        else
        {
            Debug.LogError("Level Does not exist (Value Greater than Specified List)");
        }
    }

    public static void LoadLevel(string levelName)
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(levelName);
        SceneManager.LoadSceneAsync("BootStraper", LoadSceneMode.Additive);
    }

    public static void LoadCurrentUnfinishedLevel()
    {
        int currentUnfinishedLevelNo = PlayerPrefs.GetInt(CurrentUnfinishedLevelKey, 1);
        Debug.Log("Loading Current Unfinished Level No: " + currentUnfinishedLevelNo);
        LoadLevel(currentUnfinishedLevelNo);
    }

    public static int GetCurrentUnfinishedLevelNo()
    {
        return PlayerPrefs.GetInt(CurrentUnfinishedLevelKey, 1);
    }


    public static int GetActiveLevelNo()
    {
        LevelList levelList = Resources.Load<LevelList>("LevelList");
        return levelList.GetActiveLevelNo();
    }

    public static void RestartLevel()
    {
        LoadLevel(SceneManager.GetActiveScene().buildIndex);
    }

    public static void LoadGarage()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("Garage");
    }

    public static int GetLevelsCount()
    {
        LevelList levelList = Resources.Load<LevelList>("LevelList");
        return levelList.GetAvailableLevelNamesList().Count;
    }

    public static List<String> GetAllLevelSceneNameList()
    {
        LevelList levelList = Resources.Load<LevelList>("LevelList");
        return levelList.GetAvailableLevelNamesList();
    }
}
