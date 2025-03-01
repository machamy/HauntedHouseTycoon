using System;
using UnityEngine;

public class TestLight : MonoBehaviour
{
    Camera mainCamera;
    Light light;
    
    public float deltaY = 0.5f;
    
    void Awake()
    {
        mainCamera = Camera.main;
        light= GetComponent<Light>();
    }

    private void LateUpdate()
    {
        Vector3 mousePos = Input.mousePosition;
        // print(mousePos);
        // print($"{Screen.width} {Screen.height}");
        // print($"{mousePos.x < 0 } {mousePos.x > Screen.width} {mousePos.y < 0} {mousePos.y > Screen.height}");
        if (mousePos.x < 0 || mousePos.x > Screen.width || mousePos.y < 0 || mousePos.y > Screen.height)
        {
            light.enabled = false;
        }
        else
        {
            light.enabled = true;
        }

        if (Physics.Raycast(mainCamera.ScreenPointToRay(mousePos), out RaycastHit hit, 100f,
                LayerMask.GetMask("Ground")))
        {
            Vector3 hitPos = hit.point;
            // print(hitPos);
            // print(hit.transform.name);
            
            transform.position = hitPos;
            transform.localPosition += Vector3.up * deltaY;
        }

    }
}
