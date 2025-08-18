using DG.Tweening;
using TMPro;
using Unity.Mathematics;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] TMP_Text textPrefab;

    public void SpawnText(Vector2 at, string text, float rotation = 0, int size = 4, float destroyDelay = 1)
    {
        var textSpawned = Instantiate(textPrefab.gameObject, at, Quaternion.Euler(0, 0, rotation)).GetComponent<TMP_Text>();
        textSpawned.fontSize = size;
        textSpawned.text = text;
        textSpawned.transform.DOScale(Vector2.one, 0.5f).From(0).SetEase(Ease.OutExpo)
        .OnComplete(() => DeleteSpawnedText(textSpawned.gameObject, destroyDelay));
    }

    public void DeleteSpawnedText(GameObject textSpawned, float delay)
    {
        textSpawned.transform.DOScale(0, 0.5f).SetEase(Ease.OutExpo).SetDelay(delay);
    }
}