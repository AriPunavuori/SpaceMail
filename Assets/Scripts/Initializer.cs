using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Initializer : MonoBehaviour
{
    public static Initializer Instance;

    public Color[] colors;

    public GameObject goalPrefab;
    public GameObject ufoPrefab;
    public GameObject playerPrefab;

    public int numberOfCircles;
    public float radiusDelta;
    public float ufoDelta;

    public Vector2 rotationTimeMinMax = new Vector2(2.5f, 3.5f);
    public Vector2 randomSize;
    public float randomDistance;

    public List<GameObject> ufos = new List<GameObject>();

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(this.gameObject);
            return;
        }

        Instance = this;
    }

    void Start()
    {
        Initialize();
    }

    void Initialize()
    {
        GameManager.Instance.GameStart();
        GoalSpawner();
        UfoSpawner();
        PlayerSpawner();
    }

    void GoalSpawner()
    {
        var g = Instantiate(goalPrefab);
        float degrees;
        degrees = Random.Range(0f, 360f);
        var goalPosVector = PositionVector(degrees);
        g.transform.position = RandomVector(goalPosVector.normalized * ((numberOfCircles * radiusDelta) - .5f * radiusDelta));
        g.transform.localScale = g.transform.localScale * 3.5f;
        g.gameObject.name = "Goal";
    }

    void UfoSpawner()
    {
        var startUfo = Instantiate(ufoPrefab);
        Configure(startUfo, Vector3.zero);
        ufos.Add(startUfo);

        for (int i = 0; i < numberOfCircles; i++)
        {
            float radius = (i + 1) * radiusDelta;
            //Debug.Log("Radius: " + radius);

            float circumference = radius * 2 * Mathf.PI;
            //Debug.Log("Circumference: " + circumference);

            float numberOfUfos = circumference / ufoDelta;
            //Debug.Log("Number of ufos: " + numberOfUfos);

            float degrees = 360 / numberOfUfos;
            //Debug.Log("Degrees: " + degrees);

            float rnd = Random.Range(0f, 360f);

            for (int j = 0; j < (int)numberOfUfos; j++)
            {
                var u = Instantiate(ufoPrefab);
                var pos = RandomVector(PositionVector(j * degrees + rnd).normalized * (1 + i) * radiusDelta);
                Configure(u, pos);
                ufos.Add(u);                
            }
        }
    }

    void PlayerSpawner()
    {
        var p = Instantiate(playerPrefab);

        p.transform.parent = ufos[0].transform;

        GameManager.Instance.player = p.GetComponent<Player>();

        GameManager.Instance.player.target = ufos[0].transform;

        GameManager.Instance.player.endOfTheWorld = numberOfCircles * radiusDelta + radiusDelta * 2;
    }

    void Configure(GameObject ufo, Vector3 pos)
    {
        ufo.transform.position = pos;

        ufo.transform.localScale = Vector3.one * Random.Range(randomSize.x, randomSize.y);

        ufo.GetComponentInChildren<SpriteRenderer>().color = UfoColor();

        float rotationTime = Random.Range(rotationTimeMinMax.x, rotationTimeMinMax.y);

        float rnd = Random.Range(0f, 1f);

        float degrees;

        if (rnd > .5f)
            degrees = 360f;
        else
            degrees = -360f;

        LeanTween.rotateAroundLocal(ufo, Vector3.up, degrees, rotationTime).setRepeat(-1);
    }

    Vector3 PositionVector(float degrees)
    {
        Vector3 posVec;
        posVec = new Vector3((float)Mathf.Cos(degrees * Mathf.PI / 180), 0f, (float)Mathf.Sin(degrees * Mathf.PI / 180));

        return posVec;
    }

    Vector3 RandomVector(Vector3 posVec)
    {
        Vector3 randomVector;
        randomVector = new Vector3(Random.Range(-randomDistance, randomDistance), 0f, Random.Range(-randomDistance, randomDistance));

        Vector3 sumVector = posVec + randomVector;

        return sumVector;
    }

    Color UfoColor()
    {
        return colors[Random.Range(0, colors.Length)];
    }
}
