using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour,ICollectable
{
    bool isCollected;
    [SerializeField]
    int coinValue;

    private void OnBecameVisible()
    {
        isCollected = false;
    }

    private void OnBecameInvisible()
    {
        if (isCollected)
            return;
        gameObject.SetActive(false);
        if(ItemSpawner.coinPool!=null)
            ItemSpawner.coinPool.Enqueue(gameObject);
    }

    public void Collect()
    {
        isCollected = true;
        gameObject.SetActive(false);
        ItemSpawner.coinPool.Enqueue(gameObject);
        EventManager.ScoreUpdateEvent?.Invoke(coinValue);
    }
}
