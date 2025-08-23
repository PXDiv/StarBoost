using System.Collections;
using Unity.Cinemachine;
using UnityEngine;

public class GameCameraController : MonoBehaviour
{
    [SerializeField] CinemachineBasicMultiChannelPerlin shakePerlin;
    [SerializeField] CinemachineConfiner2D confiner2D;


    void Start()
    {
        shakePerlin.enabled = false;
        SetupConfiner();
    }

    public void SetupConfiner()
    {
        var bondingShape2D = GameObject.FindGameObjectWithTag("CameraConfiner").GetComponent<Collider2D>();
        if (bondingShape2D != null)
        { confiner2D.BoundingShape2D = bondingShape2D; }
    }
    public void ShakeCamera(float duration = 0.2f, float strength = 1, float frequency = 1)
    {
        StartCoroutine(ShakeCamCo(duration, strength, frequency));
    }

    IEnumerator ShakeCamCo(float duration, float strength, float frequency)
    {
        shakePerlin.enabled = true;
        shakePerlin.AmplitudeGain = strength;
        shakePerlin.FrequencyGain = frequency;
        yield return new WaitForSeconds(duration);
        shakePerlin.enabled = false;
    }
}
