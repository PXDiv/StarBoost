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
        spaceship.GameOverEvent += DoSessionOver;
    }
    void OnDisable()
    {
        spaceship.GameOverEvent -= DoSessionOver;
    }

    #region Session
    public void DoSessionOver(GameOverCause l_gameOverCause)
    {
        if (sessionOver)
        { return; }


        //Get and set the Finish Line Height
        float finishLineHeight = 0;
        if (FindFirstObjectByType<FinishLine>() != null)
        { finishLineHeight = FindFirstObjectByType<FinishLine>().transform.position.y; }

        // Sets the Current Unfinished Level to the next one
        // Reset the saved positions as we move into the new level
        if (isLevelFinished)
        {
            Debug.Log("Level Finished");
            PlayerPrefs.SetInt(LevelManager.CurrentUnfinishedLevelKey, LevelManager.GetActiveLevelNo() + 1);
            var saver = FindAnyObjectByType<ScorePositionsSaver>();
            if (saver != null)
            { saver.DeleteAllScorePositions(); }
        }

        //Rewards and Calculation
        float maxHeightReached = isLevelFinished ? finishLineHeight : spaceship.MaxHeightReached;
        int rewardGranted = CalculateReward(maxHeightReached, collectedCoinValue);
        MoneyMan.AddMoney(rewardGranted);

        //Current Day Calc
        PlayerPrefs.SetInt(sceneDaySaveKey, PlayerPrefs.GetInt(sceneDaySaveKey, 0) + 1);

        //Sending Session Data
        SessionEndData endData = new SessionEndData
        {
            dayNumber = PlayerPrefs.GetInt(sceneDaySaveKey),
            rewardDistance = Mathf.FloorToInt(maxHeightReached) / 2,
            rewardDestruction = 0,
            coinCollectedReward = collectedCoinValue,
            totalReward = rewardGranted,

            completedLevelLength = maxHeightReached,
            totalLevelLength = finishLineHeight,

            gameOverCause = l_gameOverCause
        };


        sessionOver = true;

        StartCoroutine(SessionOverCo(delay: 1, endData));
    }
    private IEnumerator SessionOverCo(float delay, SessionEndData endData)
    {


        //FX and stop session
        flashFxSpriteRenderer.gameObject.SetActive(true);
        flashFxSpriteRenderer.DOFade(0, 1f).From(1).SetUpdate(true);
        Pause();
        yield return new WaitForSecondsRealtime(delay);
        SessionOver?.Invoke(endData);
    }

    public void LevelCleared()
    {
        isLevelFinished = true;
        DoSessionOver(GameOverCause.levelComplete);
    }

    #endregion

    #region Reward Stuff
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
    #endregion

    #region TimeStuff
    public void Pause()
    {
        Time.timeScale = 0;
    }

    public void Resume()
    {
        Time.timeScale = 1;
    }
    #endregion



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
