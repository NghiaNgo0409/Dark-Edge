using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraController : MonoBehaviour
{
    Vector3 desiredPosition;
    Vector3 targetZoomPosition;

    [SerializeField] float cameraSpeed;
    [SerializeField] float rotationSpeed;
    [SerializeField] float zoomAmount;

    [SerializeField] CinemachineVirtualCamera cinemachineVirtualCamera;
    CinemachineTransposer cinemachineTransposer;

    const float MIN_ZOOM_AMOUNT = 2f;
    const float MAX_ZOOM_AMOUNT = 30f; 
    // Start is called before the first frame update
    void Start()
    {
        cinemachineTransposer = cinemachineVirtualCamera.GetCinemachineComponent<CinemachineTransposer>();
        targetZoomPosition = cinemachineTransposer.m_FollowOffset;
    }

    // Update is called once per frame
    void Update()
    {
        if (PauseSystemUI.Instance.GetIsPause()) return;
        if (GameManager.Instance.GetIsLose()) return;
        if (GameManager.Instance.GetIsWin()) return;
        HandleMovement();
        HandleRotation();
        HandleZoom();
    }

    void HandleMovement()
    {
        desiredPosition = new Vector3(0, 0, 0);
        if(Input.GetKey(KeyCode.W))
        {
            desiredPosition.z = 1f;
        }
        if(Input.GetKey(KeyCode.S))
        {
            desiredPosition.z = -1f;
        }
        if(Input.GetKey(KeyCode.A))
        {
            desiredPosition.x = -1f;
        }
        if(Input.GetKey(KeyCode.D))
        {
            desiredPosition.x = 1f;
        }

        Vector3 moveDirection = transform.forward * desiredPosition.z + transform.right * desiredPosition.x;
        Vector3 newPosition = transform.position + moveDirection * cameraSpeed * Time.deltaTime;

        newPosition.x = Mathf.Clamp(newPosition.x, 0, 65);
        newPosition.z = Mathf.Clamp(newPosition.z, 0, 85);

        transform.position = newPosition;
    }

    void HandleRotation()
    {
        Vector3 rotationVector = new Vector3(0, 0, 0);
        if(Input.GetKey(KeyCode.Q))
        {
            rotationVector.y = 1f;
        }
        if(Input.GetKey(KeyCode.E))
        {
            rotationVector.y = -1f;
        }
        transform.eulerAngles += rotationVector * rotationSpeed * Time.deltaTime;
    }

    void HandleZoom()
    {
        if(Input.mouseScrollDelta.y > 0)
        {
            targetZoomPosition.y -= zoomAmount;
        }
        else if(Input.mouseScrollDelta.y < 0)
        {
            targetZoomPosition.y += zoomAmount;
        }

        targetZoomPosition.y = Mathf.Clamp(targetZoomPosition.y, MIN_ZOOM_AMOUNT, MAX_ZOOM_AMOUNT);
        cinemachineTransposer.m_FollowOffset = Vector3.Lerp(cinemachineTransposer.m_FollowOffset, targetZoomPosition, Time.deltaTime * zoomAmount);
    }
}
