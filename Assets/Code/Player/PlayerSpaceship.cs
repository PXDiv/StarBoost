using System;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
public class PlayerSpaceship : MonoBehaviour
{
    #region Serialized Fields
    [Header("Movement")]
    [SerializeField] Vector2 baseAcceleration = new Vector2(1, 1);
    [SerializeField] Vector2 baseVelocity;
    [SerializeField] float fallVelocity;
    [SerializeField] float maxBoost = 10;

    [Header("Attributes")]
    [SerializeField] float baseFuelReductionRate = 0.1f;
    [SerializeField] float baseFuel;

    [Header("Visual")]
    [SerializeField] SpriteRenderer visualSprite;
    [SerializeField] float leanRotationSpeed = 10;
    [Range(0, 180)][SerializeField] float maxVisualRotation;

    [Header("Refs")]
    [SerializeField] Rigidbody2D rb;
    [SerializeField] TMP_Text debugText;
    [SerializeField] GameObject paintSplashGameobj;
    [SerializeField] VariableJoystick joystick;
    //[SerializeField] VehicleUpgradeManager upgradeManager;


    [Header("Debug")]
    public bool gameOver = false;
    #endregion

    #region Events
    //Events
    public Action OnGameOver;
    public Action OnFuelOver;

    #endregion

    #region Internal Fields
    Vector2 sessionMaxVelocity; public Vector2 SessionMaxVelocity { get { return sessionMaxVelocity; } }
    float sessionCurrentFuel; public float CurrentFuel { get { return sessionCurrentFuel; } }
    float currentFuelReductionRate;
    float sessionMaxFuel; public float CurrentMaxFuel { get { return sessionMaxFuel; } }
    float vehicleMaxFuel; public float VehicleMaxFuel { get { return vehicleMaxFuel; } }
    Vector2 vehicleMaxVelocity; public Vector2 VehicleMaxVelocity { get { return vehicleMaxVelocity; } }

    float yInput, xInput;
    public float InputRecivedX { get { return xInput; } }
    public float InputRecivedY { get { return yInput; } }
    Vector3 leanAngle;
    Vector2 currentVelocity; public Vector2 CurrentVelocity { get { return currentVelocity; } }
    float maxHeightReached = 0; public float MaxHeightReached { get { return maxHeightReached; } }

    bool fuelOver = false;
    #endregion

    #region Initialization
    void Awake()
    {
        SetupBaseSessionAttributes();
        SetVehicleMaxAttributes();
    }
    void Start()
    {
        OnGameOver += SessionOver;
        InvokeRepeating("UpdateDebugText", 0, 0.1f);
    }
    #endregion

    #region UpdateChanges
    void Update()
    {
        yInput = Input.GetAxis("Vertical") + (Input.GetMouseButton(0) ? 1f : 0f);
        xInput = joystick.Horizontal;
    }
    void FixedUpdate()
    {
        if (IsFuelAvailable() && !gameOver)
        {
            Movement();
            ReduceFuel(currentFuelReductionRate);
            LeanAnimate();
        }
        else if (IsFuelAvailable() == false && transform.position.y > maxHeightReached)
        {
            maxHeightReached = transform.position.y;
            if (!fuelOver) { OnFuelOver?.Invoke(); fuelOver = true; }
        }
        else if (gameOver == false && transform.position.y < maxHeightReached)
        {
            gameOver = true;
            OnGameOver?.Invoke();
        }

    }

    private void UpdateDebugText()
    {
        debugText.text = sessionCurrentFuel + "<br> Height: " + transform.position.y + "<br> Reward: " + Convert.ToInt16(transform.position.y / 2);
    }
    #endregion

    #region MovementStuff
    private void Movement()
    {
        currentVelocity.y = baseAcceleration.y * yInput;
        currentVelocity.y = Mathf.Clamp(rb.linearVelocity.y + currentVelocity.y, -fallVelocity, sessionMaxVelocity.y);

        currentVelocity.x = baseAcceleration.x * xInput;
        currentVelocity.x = Mathf.Clamp(rb.linearVelocity.x + currentVelocity.x, -sessionMaxVelocity.x, sessionMaxVelocity.x);

        rb.linearVelocity = currentVelocity;
    }

    void LeanAnimate()
    {
        //calculate the target location to lean towards
        float targetZ = xInput * -maxVisualRotation;

        //actually lean towards it
        leanAngle.z = Mathf.LerpAngle(
           visualSprite.transform.localEulerAngles.z,
            targetZ,
            Time.deltaTime * leanRotationSpeed
        );

        // apply the lean
        visualSprite.transform.localEulerAngles = leanAngle;


    }

    #endregion

    #region FuelStuff

    public void AddFuel(int amount)
    {
        sessionCurrentFuel += amount;
    }

    public void RefillFuel()
    {
        sessionCurrentFuel = CurrentMaxFuel;
    }

    void ReduceFuel(float amount)
    {
        if (yInput > Mathf.Epsilon)
        {
            sessionCurrentFuel = Mathf.Max(0, sessionCurrentFuel - amount);
        }
    }

    bool IsFuelAvailable()
    {
        if (sessionCurrentFuel > Mathf.Epsilon)
        { return true; }

        return false;
    }
    #endregion

    #region AttributesSetup

    //Setups the current attributes based on the base attributes
    void SetupBaseSessionAttributes()
    {
        sessionMaxVelocity = baseVelocity;
        sessionCurrentFuel = baseFuel;
        currentFuelReductionRate = baseFuelReductionRate;
    }

    //Setups Upgrade Modifiers Recived from the params
    public void SetUpgrades(float engineModifier = 0, float boostModifier = 0, float turnSpeedModifier = 0, float fuelModifier = 0, float fuelReductionModifier = 0)
    {
        sessionMaxVelocity.y += engineModifier;
        sessionMaxVelocity.x += turnSpeedModifier;
        baseAcceleration.x += turnSpeedModifier / 2;

        sessionMaxFuel = baseFuel + fuelModifier;
        sessionCurrentFuel = sessionMaxFuel;
        currentFuelReductionRate -= fuelReductionModifier;

        maxBoost += boostModifier;

        print($"Recieved Upgrades: Engine+{engineModifier}, Boost+{boostModifier}, TurnSpeed+{turnSpeedModifier}, Fuel+{fuelModifier}, FuelReduction-{fuelReductionModifier}");
    }

    //Setups vehicle's top most possible values recieved from the scriptable object's last upgrade
    public void SetVehicleMaxAttributes(float maxEngineSpeed = 10, float maxFuelCapacity = 10, float maxTurnSpeed = 2)
    {
        vehicleMaxFuel = maxFuelCapacity + sessionMaxFuel;

        vehicleMaxVelocity.y = maxEngineSpeed + baseVelocity.y;
        vehicleMaxVelocity.x = maxTurnSpeed + baseVelocity.x;
    }
    #endregion

    #region CollisionStuff
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<IInteractable>() != null)
        {
            collision.GetComponent<IInteractable>().Interact(this);
        }
    }
    #endregion

    void SessionOver()
    {
        Instantiate(paintSplashGameobj, transform.position, quaternion.identity);
        FindAnyObjectByType<ScorePositionsSaver>().RegisterLocation(transform.position);
        maxHeightReached = transform.position.y;
    }

}
