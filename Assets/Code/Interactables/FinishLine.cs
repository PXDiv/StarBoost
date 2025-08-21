using Unity.Multiplayer.Center.Common.Analytics;
using UnityEngine;

public class FinishLine : MonoBehaviour, IInteractable
{
    bool interactedAlready = false;
    [SerializeField] AudioClip interactionSound;
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
        if (interactedAlready)
            return;

        sfxPlayer.PlaySFX(interactionSound);

        GetComponent<ParticlesController>().StartParticles();
        FindAnyObjectByType<SessionManager>().LevelCleared(this);

        interactedAlready = true;
    }
}
