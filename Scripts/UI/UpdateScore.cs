using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdateScore : MonoBehaviour
{
    //Timer Variables
    private float timer = 0.0f;
    private float second = 1.0f;

    //Score Variables
    public int score;
    string scoreString;

    //Is the player dead?  Relevant to stop counting up score
    public GameObject startScreen;
    private bool isPlayerDead = false;

    // Start is called before the first frame update
    void Start()
    {

    }

    public void PlayerDead()
    {
        isPlayerDead = true;
    }

    public void ChangeScoreValue(int amount)
    {
        score += amount;
        UpdateScoreText();
        //If amount is far enough into the negative, it might cause the blocks to scroll upwards, since the scrolling speed is a function of the score.
    }

    private void Update()
    {
        //Update Score Each Second
        if (!startScreen.activeInHierarchy) { 
            timer += Time.deltaTime;
            if (timer >= second)
            {
                if (!isPlayerDead)
                {
                    score++;
                    UpdateScoreText();
                }
                timer = 0.0f;
            }
        }

    }

    public int Score()
    {
        return score;
    }

    void UpdateScoreText()
    {
            scoreString = score.ToString();

            //Define Score string
            /*if(score > 999)
            {
                gameObject.GetComponent<Text>().text = scoreString;
            }*/
            if(score > 99)
            {
                gameObject.GetComponent<Text>().text = scoreString;
            }
            else if (score > 9)
            {
                gameObject.GetComponent<Text>().text = "0" + scoreString;
            }
            else
            {
                gameObject.GetComponent<Text>().text = "00" + scoreString;
            }
    }
}
