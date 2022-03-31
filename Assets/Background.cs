using UnityEngine;

public class Background : MonoBehaviour
{
    public float backGroundSpeed;
    Renderer backgroundRenderer;
    Transform cam;

    void Start()
    {
        backgroundRenderer = GetComponent<Renderer>();
        cam = Camera.main.transform;
    }

    void Update()
    {
        Vector2 offset = new Vector2(cam.position.x, cam.position.z) * backGroundSpeed;
        backgroundRenderer.material.mainTextureOffset = offset;
    }
}
