using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{
    Vector2 startPos, endPos;
    [SerializeField]
    float minimumSwipeDistance = 20f;
    [SerializeField]
    PlayerMovement playerMovement;
   
    void Update()
    {
        Vector2 touchPos = Input.GetTouch(0).position;
        if (Input.GetTouch(0).phase==TouchPhase.Began)
            startPos = touchPos;
        if (Input.GetTouch(0).phase == TouchPhase.Ended)
            endPos= touchPos;

    }
}
