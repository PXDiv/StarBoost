using DG.Tweening;
using UnityEngine;


public class FuelCan : MonoBehaviour, IInteractable
{
    [SerializeField] float fuelAmount;
    public float animationTime = 0;
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
            FindFirstObjectByType<ObjectSpawner>().SpawnText(transform.position, text: $"Fuel + {fuelAmount}");
            spaceship.RefillFuel(fuelAmount);

            if (sfxPlayer == null)
            {
                FindSfxPlayer();
            }
            sfxPlayer.PlaySFX(interactionSound);

            gameObject.transform.DOScale(Vector2.zero, animationTime).SetEase(Ease.OutExpo).OnComplete(() => Destroy(gameObject, 1f));
            interacted = true;
        }
    }
}
