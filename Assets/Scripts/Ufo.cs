using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ufo : MonoBehaviour
{
    Vector3 rotationAxis = new Vector3(0f, 1f, 0f);
    Vector2 rotationMinMax = new Vector2(2.5f, 3.5f);

    void Start()
    {
        float rotationTime = Random.Range(rotationMinMax.x, rotationMinMax.y);
     
        float rnd = Random.Range(0f, 1f);

        float degrees;

        if (rnd > .5f)
            degrees = 360f;
        else
            degrees = -360f;
   
        LeanTween.rotateAroundLocal(gameObject, rotationAxis, degrees, rotationTime).setRepeat(-1);
    }
}
