using UnityEngine;

public class SpaceshipFireVisual : MonoBehaviour
{

    [SerializeField] PlayerSpaceship playerSpaceship;
    [SerializeField] float maxScale;

    float calculatedMaxScale;

    void Start()
    {
    }

    void Update()
    {
        // float playerUpwardVelocity = Mathf.Max(0, playerSpaceship.CurrentVelocity.y);
        // var fireSizeCalc = playerUpwardVelocity / playerSpaceship.MaxVelocity;

        calculatedMaxScale = (playerSpaceship.CurrentVelocity.y / playerSpaceship.VehicleMaxVelocity.y) * maxScale;
        float fireSizeCalc = !playerSpaceship.gameOver ? Mathf.Clamp01(playerSpaceship.InputRecivedY) * calculatedMaxScale : 0;
        gameObject.transform.localScale = Vector2.one * fireSizeCalc;

    }
}