using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartGame : MonoBehaviour
{
    public GameObject blockSpawner;
    public GameObject score;

    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {   //TODO: Refactor so it's not dependent on controller inputs.  There's probably a more elegant way to handle this.
        if(Input.GetKeyDown(KeyCode.Space) || Input.GetButtonDown("JumpPS4") || Input.GetButtonDown("JumpXBone") || Input.GetButtonDown("JumpPS3")
        || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)) {
            score.SetActive(true);
            gameObject.SetActive(false);
            blockSpawner.GetComponent<BlockSpawner>().StartSpawner();
        }
    }
}
