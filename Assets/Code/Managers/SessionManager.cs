using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SessionManager : MonoBehaviour
{
    [SerializeField] PlayerSpaceship spaceship;
    public Action<SessionEndData> SessionOver;
    bool sessionOver;
    bool isLevelFinished;
    [SerializeField] SpriteRenderer flashFxSpriteRenderer;

    string sceneDaySaveKey;

    void Awake()
    {
        Resume();
        spaceship.OnGameOver += DoSessionOver;
        sceneDaySaveKey = $"Level{SceneManager.GetActiveScene().buildIndex}Day";
    }

    public void DoSessionOver()
    {
        if (sessionOver)
        { return; }

        StartCoroutine(SessionOverCo(delay: 1));

    }
    private IEnumerator SessionOverCo(float delay)
    {
        float finishLineHeight = 0;

        if (FindFirstObjectByType<FinishLine>() != null)
        { finishLineHeight = FindFirstObjectByType<FinishLine>().transform.position.y; }

        float maxHeightReached = isLevelFinished ? finishLineHeight : spaceship.MaxHeightReached;

        // Sets the Current Unfinished Level to the next one
        if (isLevelFinished) { PlayerPrefs.SetInt(LevelLoader.CurrentUnfinishedLevelKey, SceneManager.GetActiveScene().buildIndex + 1); }

        int rewardGranted = CalculateReward(maxHeightReached, collectedCoinValue);
        MoneyMan.AddMoney(rewardGranted);

        PlayerPrefs.SetInt(sceneDaySaveKey, PlayerPrefs.GetInt(sceneDaySaveKey, 0) + 1);

        SessionEndData endData = new SessionEndData
        {
            dayNumber = PlayerPrefs.GetInt(sceneDaySaveKey),
            rewardDistance = Mathf.FloorToInt(maxHeightReached) / 2,
            rewardDestruction = 0,
            coinCollectedReward = collectedCoinValue,
            totalReward = rewardGranted,

            completedLevelLength = maxHeightReached,
            totalLevelLength = finishLineHeight,
        };
        sessionOver = true;
        flashFxSpriteRenderer.gameObject.SetActive(true);
        flashFxSpriteRenderer.DOFade(0, 1f).From(1).SetUpdate(true);
        Pause();
        yield return new WaitForSecondsRealtime(delay);
        SessionOver?.Invoke(endData);
    }

    public int CalculateReward(float heightReached, int itemValueDestroyed = 0, int coinsCollected = 0)
    {
        float formula = (heightReached / 2) + itemValueDestroyed + coinsCollected;
        int reward = Mathf.FloorToInt(formula);
        return reward;
    }

    public int collectedCoinValue;
    public void AddCollectCoinValue(int coinValue)
    {
        collectedCoinValue += coinValue;
    }

    public void Pause()
    {
        Time.timeScale = 0;
    }

    public void Resume()
    {
        Time.timeScale = 1;
    }

    public void LevelCleared(FinishLine finishLine)
    {
        isLevelFinished = true;
        DoSessionOver();
    }


}

public static class MoneyMan
{
    const string key = "money";
    public static int CurrentMoneyAmount { get { return PlayerPrefs.GetInt(key, 0); ; } }
    public static void AddMoney(int amount)
    {
        var money = PlayerPrefs.GetInt(key);
        money += amount;
        SaveMoneyAmount(money);
        Debug.Log($"Added Money {amount}");
    }

    static public void ReduceMoney(int amount)
    {
        var money = PlayerPrefs.GetInt(key);
        money -= amount;
        SaveMoneyAmount(money);
        Debug.Log($"Reduced Money {amount}");
    }

    static void SaveMoneyAmount(int money)
    {
        PlayerPrefs.SetInt("money", money);
        Debug.Log(CurrentMoneyAmount);

    }
}

public static class LevelLoader
{
    public static readonly string CurrentUnfinishedLevelKey = "CurrentUnfinishedLevel";

    public static void LoadLevel(int levelIndex)
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(levelIndex);
        SceneManager.LoadSceneAsync("BootStraper", LoadSceneMode.Additive);
    }

    public static void LoadLevel(string levelName)
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(levelName);
        SceneManager.LoadSceneAsync("BootStraper", LoadSceneMode.Additive);
    }

    public static void LoadCurrentUnfinishedLevel()
    {
        int currentUnfinishedLevelIndex = PlayerPrefs.GetInt(CurrentUnfinishedLevelKey, 1);
        LoadLevel(currentUnfinishedLevelIndex);
    }

    public static void RestartLevel()
    {
        LoadLevel(SceneManager.GetActiveScene().buildIndex);
    }

    public static void LoadGarage()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("Garage");
    }
}

public static class MoneyVisualFormatter
{
    public static string Format(int amount)
    {
        if (amount >= 1_000_000_000)
            return (amount / 1_000_000_000f).ToString("0.#") + "B";
        else if (amount >= 1_000_000)
            return (amount / 1_000_000f).ToString("0.#") + "M";
        else if (amount >= 1_000)
            return (amount / 1_000f).ToString("0.#") + "K";
        else
            return amount.ToString();
    }
}
