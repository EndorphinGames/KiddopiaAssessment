using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;
using System;
using System.Threading.Tasks;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI scoreText;
    [SerializeField]
    GameObject gameOverPanel, leaderboardContent;
    [SerializeField]
    TextMeshProUGUI gameoverCurrentScoreText, gameoverHighScoreText;
    [SerializeField]
    LeaderboardUI leaderboardTile;
    [SerializeField]
    Transform leaderboardTileParent;
    [SerializeField]
    Button replayBtn,menuBtn;


    private void OnEnable()
    {
        EventManager.ScoreUpdateEvent += UpdateText;
        EventManager.GameOverEvent += ShowGameOverScreen;
        replayBtn.onClick.AddListener(ReplayGame);
        menuBtn.onClick.AddListener(GotoMainMenu);
    }

    private void OnDisable()
    {
        EventManager.ScoreUpdateEvent -= UpdateText;
        EventManager.GameOverEvent -= ShowGameOverScreen;
        replayBtn.onClick.RemoveListener(ReplayGame);
        menuBtn.onClick.RemoveListener(GotoMainMenu);
    }

    void UpdateText(int num)
    {
        scoreText.text = GameManager.currentScore.ToString();
    }

    async void ShowGameOverScreen()
    {
        await Task.Delay(TimeSpan.FromSeconds(2f));
        if (!Application.isPlaying)
            return;
        Debug.Log("Show Game over");
        gameOverPanel.SetActive(true);
        gameoverCurrentScoreText.text = $"Current Score: {GameManager.currentScore}";
        gameoverHighScoreText.text = $"High Score: {GameDataSO.instance.gameData.currentActiveProfile.score}";
        if (GameDataSO.instance.gameData.profileData.Count <= 1)
            return;
        leaderboardContent.SetActive(true);

        List<ProfileData>  leaderboardData = GameDataSO.instance.gameData.profileData;
        leaderboardData= leaderboardData.OrderByDescending(x => x.score).ToList();
        for (int i = 0; i < leaderboardData.Count; i++)
        {
            LeaderboardUI tile = Instantiate(leaderboardTile, leaderboardTileParent);
            tile.UpdateLeaderboardTile(leaderboardData[i].profileName, leaderboardData[i].score);
            if (leaderboardData[i] == GameDataSO.instance.gameData.currentActiveProfile)
                tile.HighlightTile();
        }
    }

    void ReplayGame()
    {
        GameManager.ResetScore();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void GotoMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}

