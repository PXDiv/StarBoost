using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SessionOverController : MonoBehaviour
{
    [SerializeField] float sliderAnimationDuration = 1f;
    [SerializeField] float animationInbetweenInterval = 0.2f;
    [SerializeField] Slider levelProgressSlider;
    [SerializeField] Ease easeSlider, easeTexts;

    [SerializeField]
    TMP_Text
        dayText,
        rewardDistanceText,
        rewardDestructionText,
        penalityDamageText,
        totalRewardText;

    [SerializeField] TMP_Text distanceTravelledText;

    [SerializeField] Button garageButton, menuButton, restartButton;

    SessionEndData _endData;

    void Awake()
    {
        garageButton.onClick.AddListener(() => LevelLoader.LoadMenu());
    }

    public void SetData(SessionEndData endData)
    {
        _endData = endData;
        StartCoroutine(AnimateAndShowData());
    }

    public IEnumerator AnimateAndShowData()
    {
        dayText.text = $"Day {_endData.dayNumber} Over";
        distanceTravelledText.text = _endData.completedLevelLength.ToString("00.0") + "m";

        Transform[] textFieldsTransform = { rewardDistanceText.transform, rewardDestructionText.transform, penalityDamageText.transform, totalRewardText.transform };
        foreach (Transform transform in textFieldsTransform)
        {
            transform.localScale = Vector2.zero;
        }

        rewardDistanceText.text = $": ${_endData.rewardDistance}";
        rewardDestructionText.text = $": ${_endData.rewardDestruction}";
        penalityDamageText.text = $": ${_endData.noDamageReward}";
        totalRewardText.text = $": ${_endData.totalReward}";

        levelProgressSlider.maxValue = _endData.totalLevelLength;

        levelProgressSlider.DOValue(_endData.completedLevelLength, sliderAnimationDuration).SetEase(easeSlider);

        yield return new WaitForSeconds(sliderAnimationDuration);

        foreach (Transform transform in textFieldsTransform)
        {
            DoAnimateTransform(transform);
            yield return new WaitForSeconds(animationInbetweenInterval);
        }
    }

    public void DoAnimateTransform(Transform transform)
    {
        transform.DOScale(Vector2.one * 1, animationInbetweenInterval * 2).From(0f).SetEase(Ease.OutExpo);
    }
}

public struct SessionEndData
{
    public int dayNumber;
    public int rewardDistance;
    public int rewardDestruction;
    public int noDamageReward;
    public int totalReward;
    public float totalLevelLength;
    public float completedLevelLength;
}
