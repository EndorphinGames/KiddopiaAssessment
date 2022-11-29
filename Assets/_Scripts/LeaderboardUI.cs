using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class LeaderboardUI : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI playerName;
    [SerializeField]
    TextMeshProUGUI score;
    [SerializeField]
    Image tileImage;
    [SerializeField]
    Color highlightColor;

    public void UpdateLeaderboardTile(string name, int sc)
    {
        playerName.text = name;
        score.text = sc.ToString();
    }

    public void HighlightTile()
    {
        tileImage.color = highlightColor;
    }

}
