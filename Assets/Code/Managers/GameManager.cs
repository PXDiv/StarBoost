using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] int startLevel = 2;
    public void StartSession()
    {
        LevelLoader.LoadLevel(startLevel);
    }

    public void LoadSavedLevels()
    {

    }
}
