using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    private Vector3 offset = new Vector3 (0, 0, -10f);
    [SerializeField] private float smoothTime = 0.25f;
    private Vector3 velocity = Vector3.zero;

    [SerializeField] private Transform target;

    


    private float cameraYPos = 0;

    private void Start()
    {
        //playerMovementScript = GetComponent<PlayerMovement>();
    }
    void Update()
    {
        cameraYPos = PlayerMovement.main.cameraYPos;
        Debug.Log(cameraYPos);
        Vector3 targetPosition = new Vector3(target.position.x, cameraYPos, target.position.z) + offset;
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
    }
}
