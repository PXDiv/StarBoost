using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SessionManager : MonoBehaviour
{
    [SerializeField] PlayerSpaceship spaceship;
    public Action<SessionEndData> SessionOver;
    bool sessionOver;

    string sceneDaySaveKey;

    void Awake()
    {
        spaceship.OnGameOver += () => Invoke("DoSessionOver", 2);
        sceneDaySaveKey = $"Level{SceneManager.GetActiveScene().buildIndex}Day";
    }

    void Start()
    {
        PlayerPrefs.GetInt(sceneDaySaveKey);
    }

    public void DoSessionOver()
    {
        if (!sessionOver)
        {
            int rewardGranted = CalculateReward(spaceship.MaxHeightReached);
            MoneyMan.AddMoney(rewardGranted);


            PlayerPrefs.SetInt(sceneDaySaveKey, PlayerPrefs.GetInt(sceneDaySaveKey, 0) + 1);

            SessionEndData endData = new SessionEndData
            {
                dayNumber = PlayerPrefs.GetInt(sceneDaySaveKey),
                rewardDistance = Convert.ToInt16(spaceship.MaxHeightReached) / 2,
                rewardDestruction = 0,
                noDamageReward = 0,
                totalReward = rewardGranted,

                completedLevelLength = spaceship.MaxHeightReached,
                totalLevelLength = FindFirstObjectByType<FinishLine>().transform.position.y,
            };
            sessionOver = true;
            SessionOver?.Invoke(endData);
        }
    }

    public int CalculateReward(float heightReached, int itemValueDestroyed = 0, int damagePenality = 0)
    {
        float formula = heightReached / 2 + itemValueDestroyed - damagePenality;
        int reward = Convert.ToInt16(formula);
        return reward;
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
        if (!sessionOver)
        {
            sessionOver = true;
            int rewardGranted = CalculateReward(finishLine.transform.position.y);
            MoneyMan.AddMoney(rewardGranted);

            PlayerPrefs.SetInt(sceneDaySaveKey, PlayerPrefs.GetInt(sceneDaySaveKey, 0) + 1);

            SessionEndData endData = new SessionEndData
            {
                dayNumber = PlayerPrefs.GetInt(sceneDaySaveKey),
                rewardDistance = Convert.ToInt16(finishLine.transform.position.y) / 2,
                rewardDestruction = 0,
                noDamageReward = 0,
                totalReward = rewardGranted,

                totalLevelLength = finishLine.transform.position.y,
                completedLevelLength = finishLine.transform.position.y,
            };
            SessionOver?.Invoke(endData);
        }
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

    public static void RestartLevel()
    {
        LoadLevel(SceneManager.GetActiveScene().buildIndex);
    }

    public static void LoadMenu()
    {
        SceneManager.LoadScene(0);
    }
}