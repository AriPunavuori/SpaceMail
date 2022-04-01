using UnityEngine;

public class Menu : MonoBehaviour
{
    RectTransform playerRect;
    float wobbleTime = 1.5f;
    float wobbleAmount = 15f;
    void Start()
    {
        playerRect = GameObject.Find("Player").GetComponent<RectTransform>();
        LeanTween.rotateAroundLocal(playerRect, Vector3.forward, -wobbleAmount / 2, 0f);
        LeanTween.rotateAroundLocal(playerRect, Vector3.forward, wobbleAmount, wobbleTime).setEaseInOutSine().setLoopPingPong(-1);
    }
}
