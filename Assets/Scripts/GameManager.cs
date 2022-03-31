using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Color[] colors;

    public GameObject goalPrefab;
    public GameObject ufoPrefab;
    public GameObject playerPrefab;

    public int numberOfCircles;
    public float radiusDelta;
    public float ufoDelta;

    public float randomDistance;
    public float randomShiftFactor;

    public List<GameObject> ufos = new List<GameObject>();

    void Start()
    {
        GoalSpawner();
        UfoSpawner();
        PlayerSpawner();
    }

    void GoalSpawner()
    {
        var g = Instantiate(ufoPrefab);
        float degrees;
        degrees = Random.Range(0f, 360f);
        var goalPosVector = PositionVector(degrees);
        g.transform.position = RandomVector(goalPosVector.normalized * ((numberOfCircles * radiusDelta) -.5f * radiusDelta));
        g.transform.localScale = g.transform.localScale * 2;
        g.gameObject.name = "Goal";
    }

    void UfoSpawner()
    {
        var startUfo = Instantiate(ufoPrefab);
        ufos.Add(startUfo);

        for (int i = 0; i < numberOfCircles; i++)
        {
            //Debug.Log("Circle: " + i);

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
                ufos.Add(u);
                var ufoPosVector = PositionVector(j * degrees + rnd);
                u.transform.position = RandomVector(ufoPosVector.normalized * (1 + i) * radiusDelta);
                u.GetComponentInChildren<SpriteRenderer>().color = UfoColor();
            }
        }
    }

    void PlayerSpawner()
    {
        var p = Instantiate(playerPrefab);
        p.transform.parent = ufos[0].transform;
        p.GetComponent<Player>().target = ufos[0].transform;
    }

    Vector3 PositionVector(float degrees)
    {
        Vector3 posVec;
        posVec = new Vector3((float) Mathf.Cos(degrees* Mathf.PI / 180), 0f, (float) Mathf.Sin(degrees* Mathf.PI / 180));

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
