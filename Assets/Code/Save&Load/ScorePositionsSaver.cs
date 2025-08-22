using System.Collections.Generic;
using Esper.ESave;
using Esper.ESave.SavableObjects;
using UnityEngine;

public class ScorePositionsSaver : MonoBehaviour
{
    private string saveKey = "scorePositions";
    public List<Vector2> scorePositions = new List<Vector2>();
    SaveFileSetup saveFileSetup;
    SaveFile saveFile;

    public void Awake()
    {

        saveFileSetup = GetComponent<SaveFileSetup>();
        saveFile = saveFileSetup.GetSaveFile();

    }
    public void Start()
    {
        //saveKey = "scorePositions" + LevelManager.GetActiveLevelNo();
        //LoadPositions();
    }

    public void RegisterLocation(Vector2 position)
    {
        scorePositions.Add(position);
        SavePositions();
    }

    public void SavePositions()
    {
        var savableList = new List<SavableVector>(scorePositions.Count);
        foreach (var pos in scorePositions)
        {
            savableList.Add(pos.ToSavable());
            print("saved " + pos);
        }

        DeleteAllScorePositions();
        saveFile.AddOrUpdateData(saveKey, savableList);
        saveFile.Save();
    }

    public void LoadPositions()
    {
        if (!saveFile.HasData(saveKey))
        { print("No Save found"); return; }

        scorePositions.Clear();

        var loadedList = saveFile.GetData<List<SavableVector>>(saveKey);

        scorePositions = new List<Vector2>(loadedList.Count);

        foreach (SavableVector savVec in loadedList)
        {
            scorePositions.Add(savVec.vector2Value);
        }
    }

    public List<Vector2> GetPositions()
    {
        LoadPositions();
        return scorePositions;
    }


    public void DeleteAllScorePositions()
    {
        saveFile.DeleteData(saveKey);
        saveFile.Save();
    }

    public void DeleteAllSavedData()
    {
        DeleteAllScorePositions();
        PlayerPrefs.DeleteAll();
    }
}
