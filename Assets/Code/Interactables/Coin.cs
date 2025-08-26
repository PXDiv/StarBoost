using DG.Tweening;
using UnityEngine;

public class Coin : MonoBehaviour, IInteractable
{
    [SerializeField] int coinValue; public int CoinValue { get { return coinValue; } }
    [SerializeField] AudioClip interactionSound;

    bool interacted;

    SFXPlayer sfxPlayer;

    public void Start()
    {
        FindSfxPlayer();
    }
    public void FindSfxPlayer()
    {
        sfxPlayer = FindFirstObjectByType<SFXPlayer>();
    }

    public void Interact(PlayerSpaceship spaceship)
    {
        if (!interacted)
        {
            var sessionManager = FindAnyObjectByType<SessionManager>();
            sessionManager.AddCollectCoinValue(coinValue);
            if (sfxPlayer == null)
            {
                FindSfxPlayer();
            }
            sfxPlayer.PlaySFX(interactionSound, volume: 0.5f);

            FindFirstObjectByType<ObjectSpawner>().SpawnText(transform.position, text: $"+${CoinValue}");
            //GetComponent<AudioSource>().PlayOneShot(interactionSound);

            gameObject.transform.DOScale(Vector2.zero, 0.5f).SetEase(Ease.OutExpo).OnComplete(() => Destroy(gameObject, 1f));
            interacted = true;
        }
    }
}
