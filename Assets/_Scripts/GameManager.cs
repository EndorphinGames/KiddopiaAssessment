using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static int currentScore = 0;
    //ProfileData currentProfile;
    public GameDataSO gameData;

    private void OnEnable()
    {
        ResetScore();
        EventManager.ScoreUpdateEvent += UpdateScore;
        if (GameDataSO.instance != null)
            return;
        gameData.InitGameData();
        gameData.LoadData();
        gameData.SetCurrentProfile();
    }

    private void OnDisable()
    {
        EventManager.ScoreUpdateEvent -= UpdateScore;
        GameDataSO.instance.SaveData();
    }

    void UpdateScore(int score)
    {
        currentScore += score;
        if (currentScore>GameDataSO.instance.gameData.currentActiveProfile.score  )
            GameDataSO.instance.gameData.currentActiveProfile.score = currentScore;
    }

    public static void ResetScore()
    {
        currentScore = 0;
    }
}
