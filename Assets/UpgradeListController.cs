using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeListController : MonoBehaviour
{
    [SerializeField] List<PlayerSpaceship> spaceships;
    [SerializeField] PlayerSpaceship currentVehicle;

    [SerializeField] List<Button> upgradeButtons;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    void Awake()
    {
        foreach (Transform child in transform)
        { upgradeButtons.Add(child.GetComponent<Button>()); }
        ResetAnimation();
    }
    void Start()
    {
        //AnimateListIn();
    }

    void OnEnable()
    {
        //AnimateListIn();
    }

    public void AnimateListIn()
    {
        ResetAnimation();
        StartCoroutine(AnimateRevealButtons());
    }

    public void ResetAnimation()
    {
        upgradeButtons.ForEach(b => b.gameObject.SetActive(false));
    }

    public IEnumerator AnimateRevealButtons()
    {
        foreach (Button button in upgradeButtons)
        {
            button.gameObject.SetActive(true);
            yield return new WaitForSeconds(0.1f);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
