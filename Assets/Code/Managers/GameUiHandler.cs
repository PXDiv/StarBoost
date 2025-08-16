using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class GameUiHandler : MonoBehaviour
{
    [SerializeField] GameObject pauseMenu;
    [SerializeField] Slider fuelSlider;
    [SerializeField] SessionOverController sessionOverController;
    SessionManager sessionManager;
    PlayerSpaceship playerSpaceship;

    void Awake()
    {
        sessionManager = FindFirstObjectByType<SessionManager>();
        playerSpaceship = FindFirstObjectByType<PlayerSpaceship>();
        sessionManager.SessionOver += OnSessionOver;
    }
    void Update()
    {
        fuelSlider.value = Mathf.Lerp(fuelSlider.value, playerSpaceship.CurrentFuel, Time.deltaTime * 10f);
        fuelSlider.maxValue = playerSpaceship.CurrentMaxFuel;
    }

    private void OnSessionOver(SessionEndData endData)
    {
        sessionOverController.SetData(endData);
        sessionOverController.gameObject.SetActive(true);
    }


    public void ShowPauseMenu(bool toShow)
    {
        pauseMenu.SetActive(toShow);
    }

    public void PauseGame()
    {
        sessionManager.Pause();
        pauseMenu.SetActive(true);
    }

    public void ResumeGame()
    {
        sessionManager.Resume();
        pauseMenu.SetActive(false);
    }

    public void RestartLevel()
    {
        LevelLoader.RestartLevel();
    }

    public void ExitToMainMenu()
    {
        LevelLoader.LoadMenu();
    }
}