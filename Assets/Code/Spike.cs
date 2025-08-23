using UnityEngine;

public class Spike : InteractableBase
{
    public override void OnInteract(PlayerSpaceship spaceship)
    {
        spaceship.HitPlayer();

    }

}
