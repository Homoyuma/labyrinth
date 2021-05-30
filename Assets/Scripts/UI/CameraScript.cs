using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public Transform player;
    public float smoothSpeed = 0.125f;
    public float offset = -10.0f;

    // Update is called once per frame
    void FixedUpdate()
    {
        float charPosX = player.position.x;
        float charPosY = player.position.y;

        Vector3 desiredPosition = new Vector3(charPosX, charPosY, offset);
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;
    }
}
