using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum PlayerState
{
    Landed, Flying, Landing, Launching, Dead
}
public class Player : MonoBehaviour
{
    public PlayerState playerState;
    public Transform target;
    public float endOfTheWorld;

    float speed = 10f;
    float ufoCheckSize = 1f;
    float landingTime = 1f;
    float flyTime = 1f;

    bool dead;

    int tweenId;

    void Start()
    {
        playerState = PlayerState.Landed;
        GameObject.Find("LaunchButton").GetComponent<Button>().onClick.AddListener(Launch);
        Camera.main.GetComponent<FollowingCamera>().target = this.transform;
    }

    private void Update()
    {
        if (playerState != PlayerState.Flying)
            return;

        if (dead)
            return;

        UfoCheck();
    }

    void Land()
    {
        playerState = PlayerState.Landing;
        
        LeanTween.cancel(tweenId);
        transform.LookAt(target);
        LeanTween.move(gameObject, target.position, landingTime).setEaseOutQuad().setOnComplete(Landed);
    }

    void Landed()
    {
        playerState = PlayerState.Landed;
        transform.parent = target;
    }

    public void Launch()
    {
        if (dead)
            GameManager.Instance.Restart();

        if (playerState != PlayerState.Landed)
            return;

        transform.parent = null;
        playerState = PlayerState.Launching;
        LeanTween.move(gameObject, transform.position + transform.forward, flyTime / speed * 5f).setEaseInQuad().setOnComplete(Fly);
    }

    void Fly()
    {
        playerState = PlayerState.Flying;
        tweenId = LeanTween.move(gameObject, transform.position + transform.forward * speed, flyTime).setOnComplete(Fly).id;
    }

    void UfoCheck()
    {
        var colliders = Physics.OverlapSphere(transform.position, ufoCheckSize);

        foreach (var collider in colliders)
        {
            if(collider.transform != target)
            {
                target = collider.transform;
                Land();
            }            
        }

        if (Vector3.Distance(Vector3.zero, transform.position) > endOfTheWorld)
        {
            GameManager.Instance.GameOver();
            dead = true;
        }
    }
}
