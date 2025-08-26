using UnityEngine;
using UnityEngine.U2D;

public abstract class DestructableBase : InteractableBase
{
    [SerializeField] ParticlesController particlesController;
    [SerializeField] int destructionReward = 2;
    [SerializeField] float cameraShakeDuration = 0.1f;
    GameCameraController cameraController;
    protected override void OnStart()
    {
        cameraController = FindFirstObjectByType<GameCameraController>();
    }

    public override void OnInteract(PlayerSpaceship spaceship)
    {
        particlesController.StartParticles();
        FindFirstObjectByType<ObjectSpawner>().SpawnText(transform.position, $"+${destructionReward}");
        cameraController.ShakeCamera(cameraShakeDuration);
        FindFirstObjectByType<SessionManager>().AddDestructionValue(destructionReward);
        OnDestruction();
    }

    public abstract void OnDestruction();
}
