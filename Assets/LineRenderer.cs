using UnityEngine;
using UnityEngine.UI;
public class LineRenderer : MonoBehaviour
{
    private LineRenderer lineRenderer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.positionCount = 0;

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
