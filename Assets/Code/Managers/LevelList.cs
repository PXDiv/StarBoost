using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu(fileName = "LevelList", menuName = "Scriptable Objects/LevelList")]
public class LevelList : ScriptableObject
{
#if UNITY_EDITOR
    [SerializeField] private List<SceneAsset> availableLevels; // editor-only
#endif

    [SerializeField, HideInInspector] private List<string> sceneNames; // runtime-safe names

#if UNITY_EDITOR
    private void OnValidate()
    {
        sceneNames = new List<string>();

        foreach (var scene in availableLevels)
        {
            if (scene != null)
            {
                sceneNames.Add(scene.name);
                EnsureSceneInBuildSettings(scene);
            }
        }
    }

    private void EnsureSceneInBuildSettings(SceneAsset sceneAsset)
    {
        string scenePath = AssetDatabase.GetAssetPath(sceneAsset);

        // Get current build settings
        var scenes = new List<EditorBuildSettingsScene>(EditorBuildSettings.scenes);

        // Check if already in build settings
        bool alreadyInBuild = scenes.Exists(s => s.path == scenePath);
        if (!alreadyInBuild)
        {
            // Add it (enabled = true by default)
            scenes.Add(new EditorBuildSettingsScene(scenePath, true));
            EditorBuildSettings.scenes = scenes.ToArray();
        }
    }
#endif

    // Runtime-safe API (works in builds)
    public string GetSceneNameByLevelNumber(int levelNo)
    {
        return sceneNames[levelNo - 1];
    }

    public int GetActiveLevelNo()
    {
        string activeSceneName = SceneManager.GetActiveScene().name;
        return sceneNames.IndexOf(activeSceneName) + 1; // +1 since levels start at 1
    }

    public List<string> GetAvailableLevelNamesList()
    {
        return new List<string>(sceneNames);
    }
}
