using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "LevelList", menuName = "Scriptable Objects/LevelList")]
public class LevelList : ScriptableObject
{
    [SerializeField] private List<SceneAsset> availableLevels;
    [SerializeField] private List<string> sceneNames; // runtime-safe names

    // Called in editor when values change
    private void OnValidate()
    {
        sceneNames = new List<string>();
        foreach (var scene in availableLevels)
        {
            if (scene != null)
                sceneNames.Add(scene.name);
        }
    }

    public string GetSceneNameByLevelNumber(int levelNo)
    {
        return sceneNames[levelNo - 1];
    }

    public int GetActiveLevelNo()
    {
        string activeSceneName = SceneManager.GetActiveScene().name;
        return sceneNames.IndexOf(activeSceneName) + 1; // +1 since levels start at 1
    }

    public List<string> GetAvailableLevelList()
    {
        return new List<string>(sceneNames);
    }
}
