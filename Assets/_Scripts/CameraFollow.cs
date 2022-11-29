using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField]
    Transform target;

    void LateUpdate()
    {
        transform.position = new Vector3(0, target.position.y + 3, -10);
    }
}
