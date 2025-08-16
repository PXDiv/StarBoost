using UnityEngine;

public class GameManager : MonoBehaviour
{
    public void StartSession()
    {
        LevelLoader.LoadLevel("Level1");
    }

    public void LoadSavedLevels()
    {

    }
}
