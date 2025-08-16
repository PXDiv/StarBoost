using UnityEngine;

public class GameManager : MonoBehaviour
{
    public void StartSession()
    {
        LevelLoader.LoadLevel("Game");
    }

    public void LoadSavedLevels()
    {

    }
}
