using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    bool isUsingShield;
    [SerializeField]
    GameObject shield;
    [SerializeField]
    GameObject boost;
    [SerializeField]
    Sprite deathVfx;
    [SerializeField]
    SpriteRenderer playerRenderer;

    private void OnEnable()
    {
        EventManager.ShieldPowerupEvent += EnableShield;
        EventManager.BoostPowerupEvent += EnableBoost;
    }

    private void OnDisable()
    {
        EventManager.ShieldPowerupEvent -= EnableShield;
        EventManager.BoostPowerupEvent -= EnableBoost;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.TryGetComponent(out ICollectable collectable))
            collectable.Collect();
        else if(collision.gameObject.TryGetComponent(out Obstacle obstacle) && !isUsingShield)
        {
            obstacle.Crashed();
            OnDie();
        }
            
    }

    async void EnableShield(float shieldTime)
    {
        isUsingShield = true;
        shield.SetActive(true);
        await Task.Delay(TimeSpan.FromSeconds(shieldTime));
        if (!Application.isPlaying)
            return;
        isUsingShield = false;
        shield.SetActive(false);
    }

    async void EnableBoost(float boostTime)
    {
        boost.SetActive(true);
        await Task.Delay(TimeSpan.FromSeconds(boostTime));
        if(Application.isPlaying)
            boost.SetActive(false);
    }

    void OnDie()
    {
        Debug.Log("Player Interaction game over");
        playerRenderer.sprite = deathVfx;
        boost.SetActive(false);
        
    }
}
