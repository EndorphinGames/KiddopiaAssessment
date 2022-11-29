using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour,ICollectable
{
    bool isCollected;
    [SerializeField]
    float shieldTime = 15f;

    private void OnBecameVisible()
    {
        isCollected = false;
    }

    private void OnBecameInvisible()
    {
        if (isCollected)
            return;
        if (ItemSpawner.powerupPool!= null)
            ItemSpawner.powerupPool.Enqueue(gameObject);
    }

    public void Collect()
    {
        isCollected = true;
        gameObject.SetActive(false);
        ItemSpawner.powerupPool.Enqueue(gameObject);
        EventManager.ShieldPowerupEvent?.Invoke(shieldTime);
    }
}
