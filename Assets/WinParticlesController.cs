using UnityEngine;

public class ParticlesController : MonoBehaviour
{
    [SerializeField] ParticleSystem[] particleSystems;
    public void StartParticles()
    {
        foreach (var item in particleSystems)
        {
            item.Play();
        }
    }

    public void StopParticles()
    {
        foreach (var item in particleSystems)
        {
            item.Stop();
        }
    }
}
