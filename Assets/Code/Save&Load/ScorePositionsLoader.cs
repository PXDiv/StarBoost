using System.Collections.Generic;
using UnityEngine;

public class ScorePositionsSpawnerLoader : MonoBehaviour
{
    ScorePositionsSaver saver;
    [SerializeField] GameObject splashObj;

    void Start()
    {
        saver = FindFirstObjectByType<ScorePositionsSaver>();
        SpawnSpashes();
    }

    private void SpawnSpashes()
    {
        // if (saver.scorePositions == null)
        //     return;

        // if (saver.scorePositions.Count == 0)
        //     return;

        List<Vector2> positions = saver.GetPositions();

        foreach (Vector2 pos in positions)
        {
            Instantiate(splashObj, pos, Quaternion.identity);
            print("spa");
        }

        print("Spawned");
    }
}