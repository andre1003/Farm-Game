using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour {
    public GameObject player;
    public float smoothSpeed = 0.125f;

    private Vector3 offset;
    private Vector3 newtrans;

    void Start() {
        // Setting start position
        offset.x = transform.position.x - player.transform.position.x;
        offset.z = transform.position.z - player.transform.position.z;
        newtrans = transform.position;
    }

    void FixedUpdate() {
        // Set new transform position
        newtrans.x = player.transform.position.x + offset.x;
        newtrans.z = player.transform.position.z + offset.z;

        // Smoothly change position
        Vector3 smoothPosition = Vector3.Lerp(transform.position, newtrans, smoothSpeed * Time.deltaTime);

        // Change position
        transform.position = smoothPosition;
    }
}
