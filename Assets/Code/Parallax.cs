using UnityEngine;

public class SimpleParallax : MonoBehaviour
{
    [Tooltip("Usually your Main Camera")]
    [SerializeField] public Transform cam;

    [Tooltip("How much this layer moves relative to the camera. 0 = static, 1 = same as camera")]
    [SerializeField] float parallaxEffect = 0.5f;

    [HideInInspector] public Vector3 startPos; // make accessible for editor

    void Start()
    {
        if (cam == null)
            cam = Camera.main.transform;

        startPos = transform.position;
    }

    void Update()
    {
        if (Application.isPlaying == false) return; // don’t run in editor preview
        ApplyParallax();
    }

    public void ApplyParallax()
    {
        if (cam == null) return;
        float distX = cam.position.x * parallaxEffect;
        float distY = cam.position.y * parallaxEffect;

        transform.position = new Vector3(startPos.x + distX, startPos.y + distY, transform.position.z);
    }

    public void ResetPosition()
    {
        transform.position = startPos;
    }
}
