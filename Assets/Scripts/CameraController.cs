using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {
    
    // Location of object for camera to follow
    [SerializeField]
    Transform objectToFollow;
    [SerializeField]
    float cameraFollowSpeed = 5;
    [SerializeField]
    float xOffset;
    [SerializeField]
    float yOffset;
    [SerializeField]
    float xmax = 1000;
    [SerializeField]
    float ymax = 1000;

    float ymin = 0;
    float xmin = 0;
    float zOffset = -10;
	// Use this for initialization
	void Start ()
    {
	    
	}
	
	// Update is called once per frame
	void Update ()
    {
        Vector3 newPosition = new Vector3(objectToFollow.position.x + xOffset, objectToFollow.position.y + yOffset, zOffset);
        if (newPosition.x < xmin)
            newPosition.x = xmin;
        else if (newPosition.x > xmax)
            newPosition.x = xmax;
        if (newPosition.y < ymin)
            newPosition.y = ymin;
        else if (newPosition.y > ymax)
            newPosition.y = ymax;
        transform.position = Vector3.Lerp(transform.position, newPosition, cameraFollowSpeed * Time.deltaTime);
	}
}
