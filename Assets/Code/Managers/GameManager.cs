using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] int startLevel = 1;
    private void Awake()
    {
        Application.targetFrameRate = 60;
    }
    public void StartSession()
    {
        LevelManager.LoadCurrentUnfinishedLevel();
    }

    public void LoadScene(int levelNo)
    {
        SceneManager.LoadScene(levelNo);
    }
    public void LoadScene(string levelName)
    {
        SceneManager.LoadScene(levelName);
    }

    public void LoadSavedLevels()
    {

    }
}
