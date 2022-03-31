using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowingCamera : MonoBehaviour
{
    public Transform target;

    void Update()
    {
        if (target == null)
            return;

        Vector3 camPos = target.position;
        camPos.y = 10f;
        transform.position = camPos;
    }
}
