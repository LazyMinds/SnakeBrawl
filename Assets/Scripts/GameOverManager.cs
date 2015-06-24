using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameOverManager : MonoBehaviour
{

    public GameObject highScore;
    public GameObject currentScore;

    // Use this for initialization
    void Start()
    {
        Text highScoreValue = highScore.GetComponent<Text>();
        Text currentScoreValue = currentScore.GetComponent<Text>();

        highScoreValue.text = PlayerPrefs.GetInt("highscore").ToString();
        currentScoreValue.text = SnakeManager.score.ToString();
    }
}
