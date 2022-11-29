using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    bool hasCollided;

    private void OnBecameVisible()
    {
        hasCollided = false;
    }

    private void OnBecameInvisible()
    {
        if (hasCollided)
            return;
        if (ItemSpawner.obstaclePool != null)
            ItemSpawner.obstaclePool.Enqueue(gameObject);
    }

    public void Crashed()
    {
        hasCollided = true;
        gameObject.SetActive(false);
        ItemSpawner.obstaclePool.Enqueue(gameObject);
        EventManager.GameOverEvent?.Invoke();

    }
   
}
