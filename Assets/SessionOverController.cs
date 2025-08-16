using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SessionOverController : MonoBehaviour
{
    [SerializeField] Slider levelProgressSlider;

    [SerializeField]
    TMP_Text
        dayText,
        rewardDistanceText,
        rewardDestructionText,
        penalityDamageText,
        totalRewardText;

    [SerializeField] TMP_Text distanceTravelledText;

    [SerializeField] Button garageButton, menuButton, restartButton;

    void Awake()
    {
        garageButton.onClick.AddListener(() => LevelLoader.LoadMenu());
    }

    public void SetData(SessionEndData endData)
    {
        dayText.text = $"Day {endData.dayNumber} Over";
        distanceTravelledText.text = endData.completedLevelLength.ToString("00.0") + "m";

        rewardDistanceText.text = $"Distance Reward <br>$<b>{endData.rewardDistance.ToString()}";
        rewardDestructionText.text = $"Destruction Reward <br>$<b>{endData.rewardDestruction.ToString()}";
        penalityDamageText.text = $"No Damage Bonus<br>$<b>{endData.noDamageReward.ToString()}";
        totalRewardText.text = $"Total Rewards <br>$<b>{endData.totalReward.ToString()}";

        levelProgressSlider.maxValue = endData.totalLevelLength;
        levelProgressSlider.value = endData.completedLevelLength;
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
