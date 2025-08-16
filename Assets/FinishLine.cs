using UnityEngine;

public class FinishLine : MonoBehaviour, IInteractable
{
    bool interactedAlready = false;
    public void Interact(PlayerSpaceship spaceship)
    {
        if (interactedAlready)
            return;

        GetComponent<ParticlesController>().StartParticles();
        FindAnyObjectByType<SessionManager>().LevelCleared(this);

        interactedAlready = true;
    }
}
