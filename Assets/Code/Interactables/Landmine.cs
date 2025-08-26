using System;
using System.Collections;
using System.Linq;
using DG.Tweening;
using UnityEngine;

[ExecuteAlways] // ensures it can also update while not playing (optional)
public class Landmine : InteractableBase
{
    [Header("Landmine Vars")]
    [SerializeField] float detectionRadius = 1;
    [SerializeField] float colliderRangeFactor = 0.9f;
    [SerializeField] LayerMask playerLayer;
    [SerializeField] float vibrationStrength = 0.05f;
    [SerializeField] float delayInterval = 0.5f;
    [SerializeField] int intervalTimes = 3;
    [SerializeField] int reward = 10;

    [Header("Refs")]
    [SerializeField] GameObject radiusVisualizerObj;
    [SerializeField] AudioClip beepAudioClip, boomAudioClip;
    [SerializeField] ParticleSystem explosionParticleSystem;

    //internal
    PlayerSpaceship _playerSpaceship;
    GameCameraController cameraController;

    protected override void OnStart()
    {
        ApplyRadius(); // make sure it applies at runtime too
        cameraController = FindFirstObjectByType<GameCameraController>();
    }

    // This runs whenever a serialized field changes in the Inspector
    private void OnValidate()
    {
        ApplyRadius();
    }

    private void ApplyRadius()
    {
        if (radiusVisualizerObj != null)
        {
            radiusVisualizerObj.transform.localScale = Vector3.one * detectionRadius;
        }

        var col = GetComponent<CircleCollider2D>();
        if (col != null)
        {
            col.radius = detectionRadius * colliderRangeFactor;
        }
    }

    public override void OnInteract(PlayerSpaceship spaceship)
    {
        _playerSpaceship = spaceship;
        StartCoroutine(ExplosionCo());
    }

    IEnumerator ExplosionCo()
    {
        foreach (var t in Enumerable.Range(0, intervalTimes))
        {
            PlayBeepSound();
            transform.DOShakePosition(strength: vibrationStrength, vibrato: 20, duration: delayInterval, fadeOut: true);
            yield return new WaitForSeconds(delayInterval);
        }

        Explode();
        cameraController.ShakeCamera(0.5f, strength: 2);
        yield return new WaitForSeconds(0.6f);
        transform.DOScale(0, 0.5f);
        GiveReward();
        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);
    }

    private void GiveReward()
    {
        FindFirstObjectByType<SessionManager>().AddDestructionValue(reward);
        FindFirstObjectByType<ObjectSpawner>().SpawnText(transform.position, text: $" +${reward}");
    }

    private void Explode()
    {
        explosionParticleSystem.Play();
        sfxPlayer.PlaySFX(boomAudioClip);

        GetComponent<SpriteRenderer>().enabled = false;
        if (GetComponent<Collider2D>().IsTouchingLayers(playerLayer))
        {
            _playerSpaceship.HitPlayer();
        }
    }

    private void PlayBeepSound()
    {
        sfxPlayer.PlaySFX(beepAudioClip, 0.5f);
    }
}
