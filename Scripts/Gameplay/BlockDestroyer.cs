using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockDestroyer : MonoBehaviour
{

    [SerializeField] private GameObject blockSpawningController;


    private BlockScroller bScroller;
    private BlockSpawner bSpawner;

    private bool destroyedFirstBlock = false;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        //Make sure none of the static environment stuff hit our destroy collider, we're only destroying stuff that it makes sense to destroy
        if(!other.gameObject.CompareTag(Tags.Environment))
        {
            if(other.gameObject.CompareTag(Tags.PlayerCharacter))
            {
                //Destroy the player character
                Destroy(other.gameObject);
                return;
            }
            
            //Debug.Log("BlockDestroyer hit: " + other.gameObject);
            bSpawner.RemoveBlock(other.gameObject);
            if(!destroyedFirstBlock)
            {
                destroyedFirstBlock = true;
                bScroller.ScrollStartingPlatform();
            }
            //We might have just removed a block that was not in the queue, so let's update the array tracking it
            bScroller.RefreshScrollingObjects();
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        //BlockSpawner.cs is on the same object as GameOverScreen.cs, no need to have more than one object
        bSpawner = blockSpawningController.GetComponent<BlockSpawner>();
        bScroller = blockSpawningController.GetComponent<BlockScroller>();
    }

    //// Update is called once per frame
    //void Update()
    //{

    //}
}
