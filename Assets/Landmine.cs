using System;
using System.Collections;
using System.Linq;
using DG.Tweening;
using UnityEngine;

public class Landmine : InteractableBase
{
    [Header("Landmine Vars")]
    [SerializeField] float detectionRadius = 1;
    [SerializeField] LayerMask playerLayer;
    [SerializeField] float vibrationStrength = 0.05f;
    [SerializeField] float delayInterval = 0.5f;
    [SerializeField] int intervalTimes = 3;

    [Header("Refs")]
    [SerializeField] GameManager radiusVisualizerObj;
    [SerializeField] AudioClip beepAudioClip, boomAudioClip;
    [SerializeField] ParticleSystem explosionParticleSystem;

    //internal
    PlayerSpaceship _playerSpaceship;

    protected override void OnStart()
    {
        radiusVisualizerObj.transform.DOScale(detectionRadius, 0.5f);
        GetComponent<CircleCollider2D>().radius = detectionRadius;
    }

    public override void OnInteract(PlayerSpaceship spaceship)
    {
        _playerSpaceship = spaceship;
        //transform.DOShakePosition(strength: vibrationStrength, vibrato: 20, duration: delayInterval * intervalTimes, fadeOut: false);
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

        yield return new WaitForSeconds(0.6f);
        transform.DOScale(0, 0.5f);
        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);
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
        //Base contains the sfxPlayer
        sfxPlayer.PlaySFX(beepAudioClip);
    }
}
