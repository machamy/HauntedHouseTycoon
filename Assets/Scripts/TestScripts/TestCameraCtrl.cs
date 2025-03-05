using System;
using UnityEngine;

public class TestCameraCtrl : MonoBehaviour
{
    Camera mainCamera;

    public float moveSpeed = 1.0f;
    private float mouseX;
    private float mouseY;


    public float zoomSpeed = 1.0f;
    public float minZoom = 30f;
    public float maxZoom = 60f;
    public float zoomCorrection = 0.4f;

    Vector3 rot0;

    void Awake()
    {
        mainCamera = Camera.main;
        rot0 = transform.rotation.eulerAngles;
    }

    void Update()
    {
        CameraMove();
        CameraZoom();
    }

    void CameraMove()
    {
        Vector3 mousePos = Input.mousePosition;

        if (Input.GetMouseButtonDown(1))
        {
            mouseX = mousePos.x;
            mouseY = mousePos.y;
        }

        if (Input.GetMouseButton(1))
        {
            transform.position = new Vector3(transform.position.x - (mousePos.x-mouseX)*moveSpeed, transform.position.y, transform.position.z - (mousePos.y - mouseY)* moveSpeed * Mathf.Cos(transform.rotation.x));

            mouseX = mousePos.x;
            mouseY = mousePos.y;
        }
    }

    void CameraZoom()
    {
        float wheelInput = Input.GetAxis("Mouse ScrollWheel");

        if (wheelInput != 0)
        {
            Vector3 rot = transform.rotation.eulerAngles;
            rot.x += wheelInput * zoomSpeed;
            rot.x = Math.Max(minZoom, rot.x);
            rot.x = Math.Min(maxZoom, rot.x);

            Quaternion q = Quaternion.Euler(rot);
            q.y = q.z = 0;
            transform.rotation = Quaternion.Slerp(transform.rotation, q, 2f);

            transform.position = new Vector3(transform.position.x,transform.position.y, transform.position.z + (rot.x - rot0.x) * zoomCorrection);
            rot0 = rot;
        }
    }
}