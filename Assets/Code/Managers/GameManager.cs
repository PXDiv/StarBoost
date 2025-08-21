using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] int startLevel = 1;
    public void StartSession()
    {
        LevelManager.LoadCurrentUnfinishedLevel();
    }

    public void LoadScene(int levelNo)
    {
        SceneManager.LoadScene(levelNo);
    }

    public void LoadSavedLevels()
    {

    }
}
