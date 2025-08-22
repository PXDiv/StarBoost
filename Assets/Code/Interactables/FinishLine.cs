using Unity.Multiplayer.Center.Common.Analytics;
using UnityEngine;

public class FinishLine : MonoBehaviour, IInteractable
{
    bool interactedAlready = false;
    [SerializeField] AudioClip interactionSound;
    SFXPlayer sfxPlayer;

    public void Awake()
    {
        if (FindFirstObjectByType<FinishLine>() != this)
        {
            Debug.LogWarning("There are two Finish Lines in this level");
        }
    }

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
        FindAnyObjectByType<SessionManager>().LevelCleared();

        interactedAlready = true;
    }
}
