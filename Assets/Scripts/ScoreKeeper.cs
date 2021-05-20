using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreKeeper : MonoBehaviour
{
    float score = 0f;
    Text scoreText;

    private void Start()
    {
        scoreText = GetComponent<Text>();
    }

    public void Reset()
    {
        score = 0;
        scoreText.text = "SC: " + score.ToString();
    }

    public void UpdateScore(float addingScore)
    {
        score += addingScore;
        scoreText.text = "HI: " + score.ToString();
    }
}
