using UnityEngine;


public class CameraMovement : MonoBehaviour
{
    // Smooth speed
    public float smoothSpeed = 0.125f;


    // Player reference
    private GameObject player;

    // Offset
    private Vector3 offset;

    // New transform
    private Vector3 newTrans;


    // Awake method
    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Start method
    void Start()
    {
        // Setting start position
        offset.x = transform.position.x - player.transform.position.x;
        offset.z = transform.position.z - player.transform.position.z;
        newTrans = transform.position;
    }

    private void Update()
    {
        // Zoom down
        if(Input.GetAxis("Mouse ScrollWheel") < 0f)
        {
            Camera.main.orthographicSize++;
            if(Camera.main.orthographicSize > 14f)
            {
                Camera.main.orthographicSize = 14f;
            }
        }

        // Zoom up
        else if(Input.GetAxis("Mouse ScrollWheel") > 0f)
        {
            Camera.main.orthographicSize--;
        }
    }

    // Fixed update method
    void FixedUpdate()
    {
        // Set new transform position
        newTrans.x = player.transform.position.x + offset.x;
        newTrans.z = player.transform.position.z + offset.z;

        // Smoothly change position
        Vector3 smoothPosition = Vector3.Lerp(transform.position, newTrans, smoothSpeed * Time.deltaTime);

        // Change position
        transform.position = smoothPosition;
    }
}
