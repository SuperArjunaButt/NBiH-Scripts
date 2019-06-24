using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    
    public enum PowerupType {ADD_TO_SCORE, DESTROY_KILLERS};

    public PowerupType pType;

    public int scoreChangeVal = 10;

    //TODO: Replace with a sprite once we have one from the aht depahtment
    [SerializeField] string powerupText;
    
    private GameObject scoreObject;
    private UpdateScore scoreCounter;
    private bool activated = false;

    // Start is called before the first frame update
    void Start()
    {
        if(pType == PowerupType.ADD_TO_SCORE)
        {
            scoreObject = GameObject.FindGameObjectWithTag(Tags.HighScoreObject);
            scoreCounter = scoreObject.GetComponent<UpdateScore>();
        }

    }

    void DisableAllComponents()
    {
        GetComponent<SpriteRenderer>().enabled = false;
    }

    void OnTriggerEnter2D(Collider2D cdr)
    {
        Debug.Log("Powerup.cs: In OnTriggerEnter");
        if(cdr.gameObject.CompareTag(Tags.PlayerCharacter) && !activated)
        {
            activated = true;
            if(pType == PowerupType.ADD_TO_SCORE)
            {
                scoreCounter.ChangeScoreValue(scoreChangeVal);
                Messenger.Broadcast(GameEvent.PLUS_TEN);
            }
            if(pType == PowerupType.DESTROY_KILLERS)
            {
                Messenger.Broadcast(GameEvent.DESTROY_KILLERS);
            }
            DisableAllComponents();
            //cdr.gameObject.GetComponent<PlayerInput>().AnnouncePowerup();
        } 
        if(cdr.gameObject.CompareTag(Tags.BlockDestroyer))
            Destroy(gameObject);   
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
