using UnityEngine;

public class gameWindZone : MonoBehaviour
{
    [Header("Wind Settings")]
    [SerializeField] private float windStrength = 10f;

    [Header("Particle System")]
    [SerializeField] private ParticleSystem windParticles;
    [SerializeField] private float particleSpeedMultiplier = 0.1f;

    private void OnTriggerStay2D(Collider2D other)
    {
        Rigidbody2D rb = other.attachedRigidbody;
        if (rb != null)
        {
            rb.AddForce(transform.right * windStrength, ForceMode2D.Force);
            print("Add");
        }
    }

    private void Start()
    {
        GetComponent<SpriteRenderer>().enabled = false;
        if (windParticles != null)
        {
            var main = windParticles.main;
            // Adjust particle speed with wind strength
            main.simulationSpeed = 1f + windStrength * particleSpeedMultiplier;


            var shape = windParticles.shape;
            shape.radius = transform.localScale.y / 2; // radius is half the diameter
            main.startLifetime = transform.localScale.x / 4;
        }
    }

    // Optional: call this if you want to change wind dynamically
    public void SetWindStrength(float strength)
    {
        windStrength = strength;
    }
}