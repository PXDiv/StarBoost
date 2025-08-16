using UnityEngine;

public class FollowScript : MonoBehaviour
{

    [SerializeField] GameObject gameObjectToFollow;
    [SerializeField] bool followMainCamera;
    Vector2 pos;
    public bool followX, followY;

    void Start()
    {
        if (followMainCamera)
            gameObjectToFollow = Camera.main.gameObject;
    }
    void Update()
    {
        if (followX)
            pos.x = gameObjectToFollow.transform.position.x;
        else
            pos.x = transform.position.x;

        if (followY)
            pos.y = gameObjectToFollow.transform.position.y;
        else
            pos.y = transform.position.y;

        transform.position = pos;
    }
}
