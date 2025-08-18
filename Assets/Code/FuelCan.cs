using DG.Tweening;
using UnityEngine;


public class FuelCan : MonoBehaviour, IInteractable
{
    [SerializeField] float fuelAmount;
    public float animationTime = 0;
    bool interacted;
    public void Interact(PlayerSpaceship spaceship)
    {
        if (!interacted)
        {
            FindFirstObjectByType<Spawner>().SpawnText(transform.position, text: $"Fuel + {fuelAmount}");
            spaceship.RefillFuel(fuelAmount);
            gameObject.transform.DOScale(Vector2.zero, animationTime).SetEase(Ease.OutExpo).OnComplete(() => Destroy(gameObject, 1f));
            interacted = true;
        }
    }
}
