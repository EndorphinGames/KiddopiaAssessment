using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    bool canMove;
    [SerializeField]
    Rigidbody2D myRigidbody;
    [SerializeField]
    float defaultMoveSpeed;
    [SerializeField]
    float boostSpeed;

    [SerializeField]
    Transform itemsSpawnParent;
    [SerializeField]
    Transform player;
    bool isPositionReset;
    [SerializeField]
    float[] playerPositions;
    int currentPos=1;
    float moveSpeed;

    public PlayerPosition playerPos;

    private void OnEnable()
    {
        SwipeDetector.OnSwipe += HandleSwipe;
        EventManager.PositionResetEvent += OnPositionReset;
        EventManager.BoostPowerupEvent += MoveBoostCollected;
        moveSpeed = defaultMoveSpeed;
        canMove = true;
        EventManager.GameOverEvent += OnGameOver;
    }

    private void OnDisable()
    {
        SwipeDetector.OnSwipe -= HandleSwipe;
        EventManager.PositionResetEvent -= OnPositionReset;
        EventManager.BoostPowerupEvent -= MoveBoostCollected;
        EventManager.GameOverEvent -= OnGameOver;
    }

    void HandleSwipe(SwipeData data)
    {
        Move(data.Direction);
    }

    void Move(SwipeDirection direction)
    {
        //Debug.Log($"Swipe direction  - {direction}");
        if (direction == SwipeDirection.Left)
        {
            currentPos--;
            currentPos = Mathf.Clamp(currentPos, 0, 2);
            player.position = new Vector3(playerPositions[currentPos], player.transform.position.y, 0);
        }
        else if (direction == SwipeDirection.Right)
        {
            currentPos++;
            currentPos = Mathf.Clamp(currentPos, 0, 2);
            player.position = new Vector3(playerPositions[currentPos], player.transform.position.y, 0);
        }
    }



    void Update()
    {
        if (!canMove)
            return;

        transform.Translate(Vector3.up * moveSpeed * Time.deltaTime);
        if (Input.GetKeyDown(KeyCode.LeftArrow))
            Move(SwipeDirection.Left);
        else if (Input.GetKeyDown(KeyCode.RightArrow))
            Move(SwipeDirection.Right);
    }

    private void LateUpdate()
    {
        if (isPositionReset)
        {
            isPositionReset = false;
            itemsSpawnParent.SetParent(transform);
            transform.position = Vector3.zero;
            itemsSpawnParent.SetParent(null);
        }
    }

    void OnPositionReset()
    {
        isPositionReset = true;
    }

    async void MoveBoostCollected(float boostTime)
    {
        moveSpeed = boostSpeed;
        await Task.Delay(TimeSpan.FromSeconds(boostTime));
        moveSpeed = defaultMoveSpeed;
    }

    void OnGameOver()
    {
        Debug.Log("Player movement game over");
        canMove = false;
    }

}

public enum PlayerPosition
{
    Left=0,
    Center,
    Right
}
