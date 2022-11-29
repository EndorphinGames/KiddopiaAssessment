using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    [SerializeField]
    GameObject coinPrefab;
    [SerializeField]
    int coinPoolCount;
    [SerializeField]
    GameObject[] powerups;
    [SerializeField]
    int powerupPoolCount;
    [SerializeField]
    GameObject[] obstacles;
    [SerializeField]
    int obstaclePoolCount;

    [SerializeField]
    Transform[] spawnPositions;
    [SerializeField]
    Transform spawnParent;
    [SerializeField]
    int minSpawnDelay,maxSpawnDelay;

    [SerializeField]
    bool isSpawning;

    [SerializeField]
    int coinSpawnChance,obstacleSpawnChance,powerupSpawnChance;

    public static Queue<GameObject> coinPool;
    public static Queue<GameObject> obstaclePool;
    public static  Queue<GameObject> powerupPool;

    bool canSpawnBoost=true, canSpawnShield=true;

    private void OnEnable()
    {
        isSpawning = true;
        EventManager.GameOverEvent += OnGameOver;
        EventManager.BoostPowerupEvent += DisableBoost;
        EventManager.ShieldPowerupEvent += DisableShield;
    }

    private void OnDisable()
    {
        isSpawning = false;
        EventManager.GameOverEvent -= OnGameOver;
        EventManager.BoostPowerupEvent -= DisableBoost;
        EventManager.ShieldPowerupEvent -= DisableShield;
    }

    async void Start()
    {
        
        PoolObjects(); //Create initial pool
        await Task.Yield();

        while (isSpawning) //Position objects at spawn points
        {
            int obstacleCount = 0;
            for (int i = 0; i < spawnPositions.Length; i++)
            {
                int rand = UnityEngine.Random.Range(0, coinSpawnChance); //1 in 3 chances for coin to spawn
                if(rand==0)
                {
                    GameObject coin= coinPool.Dequeue() ;
                    coin.SetActive(true);
                    coin.transform.position = spawnPositions[i].position;
                    continue;
                }

                rand = UnityEngine.Random.Range(0, obstacleSpawnChance); //1 in 3 chances for obstacle to spawn

                if (rand == 0 && obstacleCount < 2) // so we do not have obstacles in all lanes
                {
                    GameObject obstacle = obstaclePool.Dequeue();
                    obstacle.SetActive(true);
                    obstacle.transform.position = spawnPositions[i].position;
                    obstacleCount++;
                    continue;
                }

                rand = UnityEngine.Random.Range(0, powerupSpawnChance); //1 in 3 chances for powerup to spawn
              //  Debug.Log($"Powerup rand num - {rand}");
                if (rand == 0)
                {
                    GameObject powerup = powerupPool.Dequeue();
                    if(!canSpawnBoost && powerup.TryGetComponent(out Boost b))
                        continue;
                    if (!canSpawnShield && powerup.TryGetComponent(out Shield s))
                        continue;
                    powerup.SetActive(true);
                    powerup.transform.position = spawnPositions[i].position;
                    continue;
                }
            }
            await Task.Delay(UnityEngine.Random.Range(minSpawnDelay,maxSpawnDelay));
        }    
    }

    void PoolObjects()
    {
        coinPool = new Queue<GameObject>();
        for (int i = 0; i < coinPoolCount; i++)
        {
            GameObject coin = Instantiate(coinPrefab,  spawnParent) ;
            coin.name = $"Coin{i}";
            coin.SetActive(false);
            coinPool.Enqueue(coin);
        }
        obstaclePool = new Queue<GameObject>();
        for (int i = 0; i < obstaclePoolCount; i++)
        {
            GameObject obstacle = Instantiate(obstacles[UnityEngine.Random.Range(0,obstacles.Length)], spawnParent);
            obstaclePool.Enqueue(obstacle);
            obstacle.SetActive(false);
        }

        powerupPool = new Queue<GameObject>();
        for (int i = 0; i < powerupPoolCount; i++)
        {
            GameObject powerup = Instantiate(powerups[UnityEngine.Random.Range(0, powerups.Length)],spawnParent);
            powerupPool.Enqueue(powerup);
            powerup.SetActive(false);
        }
    }

    void OnGameOver()
    {
        isSpawning = false;
        Debug.Log($"Spawning  - {isSpawning}");
    }

    async void DisableBoost(float time)
    {
        canSpawnBoost = false;
        await Task.Delay(TimeSpan.FromSeconds(time));
        canSpawnBoost = true;
    }

    async void DisableShield(float time)
    {
        canSpawnShield = false;
        await Task.Delay(TimeSpan.FromSeconds(time));
        canSpawnShield = true;
    }

}
