using DG.Tweening;
using UnityEngine;


public class FuelCan : MonoBehaviour, IInteractable
{
    public float animationTime = 0;
    public void Interact(PlayerSpaceship spaceship)
    {
        gameObject.transform.DOScale(Vector2.zero, animationTime).SetEase(Ease.OutExpo).OnComplete(() => Destroy(gameObject, 1f));
        spaceship.RefillFuel();
    }
}
