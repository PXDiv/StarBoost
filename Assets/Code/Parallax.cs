using UnityEngine;

public class SimpleParallax : MonoBehaviour
{
    [Tooltip("Usually your Main Camera")]
    public Transform cam;

    [Tooltip("How much this layer moves relative to the camera. 0 = static, 1 = same as camera")]
    public float parallaxEffect = 0.5f;

    Vector3 startPos;

    void Start()
    {
        if (cam == null)
            cam = Camera.main.transform;

        startPos = transform.position;
    }

    void Update()
    {
        float distX = cam.position.x * parallaxEffect;
        float distY = cam.position.y * parallaxEffect;

        transform.position = new Vector3(startPos.x + distX, startPos.y + distY, transform.position.z);
    }
}
