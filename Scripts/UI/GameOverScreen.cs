using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOverScreen : MonoBehaviour
{
    public GameObject selector;
    public GameObject selection;
    public int selectionIndex;
    public GameObject[] menu;
    public float selectorX;
    public GameObject highScoreList;
    public AudioSource menuClick;

    //input delay
    private float timer = 0.0f;
    private float delay = 0.5f;
    private bool clickOn = false;
    private bool lockDPadButton = false;


    private void Start()
    {
        selection = menu[0];
        selector.transform.SetParent(selection.transform);
    }

    private void Update()
    {

        if (Mathf.Abs(Input.GetAxis("DPadY")) == 0.0f && Mathf.Abs(Input.GetAxis("DPadYXBone")) == 0.0f) lockDPadButton = false;    
    //    if ((Input.GetAxis("DPadY") < 0.01f && Input.GetAxis("DPadY") > -0.01f)
    //        || (Input.GetAxis("DPadYXBone") < 0.01f && Input.GetAxis("DPadYXBone") > -0.01f)) lockDPadButton = false;
        Debug.Log(Input.GetAxis("DPadY"));
        //Input Delay
        timer += Time.deltaTime;
        if (timer >= delay) { clickOn = true; }

        //Change Selection
        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W) || (Input.GetAxis("DPadY") < 0.0f && !lockDPadButton) || (Input.GetAxis("DPadYXBone") < 0.0f && !lockDPadButton))
        {
            ChangeSelectionIndex(-1);
            lockDPadButton = true;
        }
        if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S) || 
            (Input.GetAxis("DPadY") > 0.0f && !lockDPadButton)
            || (Input.GetAxis("DPadYXBone") > 0.0f && !lockDPadButton))
        {
            ChangeSelectionIndex(1);
            lockDPadButton = true;
        }

        //Loop selection
        if(selectionIndex >= menu.Length)
        {
            selectionIndex = 0;
        }
        if(selectionIndex < 0)
        {
            selectionIndex = menu.Length - 1;
        }

        //Change Arrow Position
        selection = menu[selectionIndex];
        selector.transform.SetParent(selection.transform);
        selector.transform.localPosition = new Vector3(selectorX, 2, 0);

        //Select
        if (clickOn == true)
        {
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return) || Input.GetButtonDown("JumpPS4") || Input.GetButtonDown("JumpXBone") || Input.GetButtonDown("JumpPS3"))
            {
                menuClick.Play();
                if (selectionIndex == 0) { GameRestart(); }
                if (selectionIndex == 1) { GameQuit(); }
                if (selectionIndex == 2) { ResetHS(); }
            }
        }
    }

    public void ResetHS()
    {
        highScoreList.GetComponent<HighScoreList>().ResetHighScore();
    }

    public void ChangeSelectionIndex(int adjustment)
    {
        selectionIndex = selectionIndex + adjustment;
        menuClick.Play();

        //Update Color
        for (int i = 0; i < menu.Length; i++)
        {
            if (i == selectionIndex)
            {
                menu[i].GetComponent<Text>().color = new Color(255.0f / 255.0f, 255.0f / 255.0f, 255.0f / 255.0f);
            }
            else
            {
                menu[i].GetComponent<Text>().color = new Color(170.0f / 255.0f, 85.0f / 255.0f, 85.0f / 255.0f);
            }
        }
    }

    public void EnableGameOverScreen()
    {
        gameObject.SetActive(true);
    }

    public void GameQuit()
    {
        Application.Quit();
    }

    public void GameRestart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
