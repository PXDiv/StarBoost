using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SessionOverController : MonoBehaviour
{
    [SerializeField] RectTransform container;
    [SerializeField] float sliderAnimationDuration = 1f;
    [SerializeField] float animationInbetweenInterval = 0.2f;
    [SerializeField] Slider levelProgressSlider;
    [SerializeField] Ease easeSlider, easeTexts;

    [SerializeField]
    TMP_Text
        dayText,
        rewardDistanceText,
        rewardDestructionText,
        coinsCollectedText,
        totalRewardText;

    [SerializeField] TMP_Text distanceTravelledText;

    [SerializeField] Button garageButton, menuButton, restartButton;

    SessionEndData _endData;

    void Awake()
    {
        garageButton.onClick.AddListener(() => LevelManager.LoadGarage());
    }

    public void SetData(SessionEndData endData)
    {
        _endData = endData;
        StartCoroutine(AnimateAndShowData());
    }

    public IEnumerator AnimateAndShowData()
    {
        //Reset and set Values for animation
        switch (_endData.gameOverCause)
        {
            case GameOverCause.fuelOver:
                dayText.text = $"Fuel Over";
                break;
            case GameOverCause.enemyHit:
                dayText.text = $"Game Over";
                break;
            case GameOverCause.levelComplete:
                dayText.text = $"Level {LevelManager.GetActiveLevelNo()} Complete";
                break;
        }

        distanceTravelledText.text = _endData.completedLevelLength.ToString("00.0") + "m";

        //Transform[] textFieldsTransform = { distanceTravelledText.transform, rewardDistanceText.transform, rewardDestructionText.transform, coinsCollectedText.transform, totalRewardText.transform };
        // foreach (Transform transform in textFieldsTransform)
        // {
        //     transform.localScale = Vector2.zero;
        // }
        //distanceTravelledText.transform.localScale = Vector2.zero;

        rewardDistanceText.text = $": ${MoneyVisualFormatter.Format(_endData.rewardDistance)}";
        rewardDestructionText.text = $": ${MoneyVisualFormatter.Format(_endData.rewardDestruction)}";
        coinsCollectedText.text = $": ${MoneyVisualFormatter.Format(_endData.coinCollectedReward)}";
        totalRewardText.text = $": ${MoneyVisualFormatter.Format(_endData.totalReward)}";

        levelProgressSlider.maxValue = _endData.totalLevelLength;
        levelProgressSlider.value = 0;
        //Animation Stuff from here

        //Container Animation
        // container.DOScale(Vector2.one, 0.5f).From(0).SetUpdate(true);
        // yield return new WaitForSecondsRealtime(0.5f);

        //Slider Animation

        levelProgressSlider.DOValue(_endData.completedLevelLength, sliderAnimationDuration).SetEase(easeSlider).SetUpdate(true);

        // //Text Animations
        // yield return new WaitForSecondsRealtime(sliderAnimationDuration);

        // foreach (Transform transform in textFieldsTransform)
        // {
        //     DoAnimateTransform(transform);
        //     yield return new WaitForSecondsRealtime(animationInbetweenInterval);
        // }
        yield return null;
    }

    public void DoAnimateTransform(Transform transform)
    {
        transform.DOScale(Vector2.one * 1, animationInbetweenInterval * 2).From(0f).SetEase(Ease.OutExpo).SetUpdate(true);
    }
}

public struct SessionEndData
{
    public int dayNumber;
    public int rewardDistance;
    public int rewardDestruction;
    public int coinCollectedReward;
    public int totalReward;
    public float totalLevelLength;
    public float completedLevelLength;
    public GameOverCause gameOverCause;
}
