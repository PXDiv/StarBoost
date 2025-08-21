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
        sceneDaySaveKey = $"Level{SceneManager.GetActiveScene().buildIndex}Day";
        print("Level: " + LevelManager.GetActiveLevelNo());
    }

    void OnEnable()
    {
        spaceship.OnGameOver += DoSessionOver;
    }
    void OnDisable()
    {
        spaceship.OnGameOver -= DoSessionOver;
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
        if (isLevelFinished) { PlayerPrefs.SetInt(LevelManager.CurrentUnfinishedLevelKey, LevelManager.GetActiveLevelNo() + 1); }

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

            didCompleteLevel = isLevelFinished,
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
