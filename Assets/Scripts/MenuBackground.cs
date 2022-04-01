using UnityEngine;

public class MenuBackground : MonoBehaviour
{
    float backGroundSpeed  = .005f;
    Renderer backgroundRenderer;

    void Start()
    {
        backgroundRenderer = GetComponent<Renderer>();
    }

    void Update()
    {
        Vector2 offset = new Vector2(Time.time, 0f) * backGroundSpeed;
        backgroundRenderer.material.mainTextureOffset = offset;
    }
}
