using UnityEngine;

public abstract class InteractableBase : MonoBehaviour, IInteractable
{
    [SerializeField] AudioClip interactAudioClip;
    bool didInteract;
    [HideInInspector] protected SFXPlayer sfxPlayer;

    protected void Start()
    {
        FindSFXPlayer();
        OnStart();
    }

    protected virtual void OnStart()
    {

    }

    void FindSFXPlayer()
    {
        sfxPlayer = FindFirstObjectByType<SFXPlayer>();
    }

    public void Interact(PlayerSpaceship spaceship)
    {
        if (!didInteract)
        {
            didInteract = true;

            OnInteract(spaceship);

            if (sfxPlayer == null)
            { FindSFXPlayer(); }
            if (interactAudioClip != null)
            { sfxPlayer.PlaySFX(interactAudioClip); }
        }
    }

    public abstract void OnInteract(PlayerSpaceship spaceship);

}
