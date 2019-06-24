using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HighScoreList : MonoBehaviour
{
    private int myScore;
    public GameObject scoreboard;
    public Text highScoreText;
    public GameObject gameOverScreen;

    void Start()
    {
        DrawHighScore(PlayerPrefs.GetInt("HighScore", 0));
        //highScoreText.text = PlayerPrefs.GetInt("HighScore", 0).ToString();
    }

    void Update()
    {
        myScore = scoreboard.GetComponent<UpdateScore>().Score();
        if(myScore > PlayerPrefs.GetInt("HighScore", 0) && !gameOverScreen.activeInHierarchy)
        {
            PlayerPrefs.SetInt("HighScore", myScore);
            DrawHighScore(myScore);

        }
    }

    public void DrawHighScore(float score)
    {
        //Define Score string
        /*if (score > 999)
        {
            highScoreText.text = score.ToString();
        }*/
        if (score > 99)
        {
            highScoreText.text = score.ToString();
        }
        else if (score > 9)
        {
            highScoreText.text = "0" + score.ToString();
        }
        else
        {
            highScoreText.text = "00" + score.ToString();
        }
    }

    public void ResetHighScore()
    {
        PlayerPrefs.DeleteKey("HighScore");
        highScoreText.text = "000";
    }

    /*public List<GameObject> highScoreLine;
    public List<float> highScore;
    public GameObject scoreboard;
    public float myScore = 0.0f;
    public int myScoreLine;
    public int nextScoreLine;

    // Start is called before the first frame update
    void Start()
    {
        //Set active score to 10th place
        nextScoreLine = highScoreLine.Count;

        //Assign a default score to each high score line
        for(int i = 0; i < highScoreLine.Count; i++)
        {
            highScoreLine[i].GetComponent<Text>().text = highScore[i].ToString();
        }
    }

    // Update is called once per frame
    void Update()
    {
        //match myScore to the scoreboard score
        myScore = scoreboard.GetComponent<UpdateScore>().score;

        if(myScore > highScore[nextScoreLine]){
            TakeNextScoreLine();
        }
    }

    void TakeNextScoreLine()
    {
        nextScoreLine--;
        myScoreLine = nextScoreLine + 1;
        highScore[myScoreLine] = myScore;
    }*/

}
