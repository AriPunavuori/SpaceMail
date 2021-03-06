using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum PlayerState
{
    Landed, Flying, Landing, Launching, GameEnded
}
public class Player : MonoBehaviour
{
    public PlayerState playerState;
    public Transform target;
    public float endOfTheWorld;

    float speed = 10f;
    float ufoCheckSize = 1f;
    float landingTime = .2f;
    float flyTime = .5f;

    bool gameEnded;

    int tweenId;

    void Start()
    {
        playerState = PlayerState.Landed;
        Camera.main.GetComponent<FollowingCamera>().target = this.transform;
    }

    private void Update()
    {
        if (playerState != PlayerState.Flying)
            return;

        if (gameEnded)
            return;

        UfoCheck();
    }

    void Land()
    {
        GameManager.Instance.audioSources[2].Play();

        playerState = PlayerState.Landing;
        
        LeanTween.cancel(tweenId);

        var dir = target.position - transform.position;
        var angleDiff = Vector3.SignedAngle(transform.forward, dir, Vector3.up);
        LeanTween.rotateAroundLocal(gameObject, Vector3.up, angleDiff, landingTime).setEaseOutCirc();

        LeanTween.move(gameObject, target.position, landingTime).setEaseOutQuad().setOnComplete(Landed);
    }

    void Landed()
    {
        if (target.gameObject.name == "Goal")
        {
            gameEnded = true;
            GameManager.Instance.GameWon();
            var angleDiff = Vector3.SignedAngle(transform.forward, Vector3.forward, Vector3.up);
            LeanTween.rotateAroundLocal(gameObject, Vector3.up, angleDiff, landingTime).setEaseOutCirc();
            playerState = PlayerState.GameEnded;
        }
        else
        {
            transform.parent = target;
            playerState = PlayerState.Landed;
        }
    }

    public void Launch()
    {
        if (playerState != PlayerState.Landed)
            return;

        GameManager.Instance.audioSources[3].Play();
        transform.parent = null;
        playerState = PlayerState.Launching;
        LeanTween.move(gameObject, transform.position + transform.forward, flyTime / speed * 5f).setEaseInQuad().setOnComplete(Fly);
    }

    void Fly()
    {
        playerState = PlayerState.Flying;
        tweenId = LeanTween.move(gameObject, transform.position + transform.forward * speed, flyTime).setOnComplete(Fly).id;
    }


    void Spin()
    {
        LeanTween.rotateAroundLocal(gameObject, Vector3.up, 360f, flyTime * 5f).setRepeat(-1);
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
                break;
            }            
        }

        if (Vector3.Distance(Vector3.zero, transform.position) > endOfTheWorld)
        {
            LeanTween.cancel(tweenId);         
            LeanTween.move(gameObject, transform.position + transform.forward * (speed * 20f), flyTime * 30f).setEaseOutSine();
            LeanTween.rotateAroundLocal(gameObject, Vector3.up, 720f, flyTime * 15f).setEaseInSine().setOnComplete(Spin);
            GameManager.Instance.GameOver();
            gameEnded = true;
            playerState = PlayerState.GameEnded;
        }
    }
}
