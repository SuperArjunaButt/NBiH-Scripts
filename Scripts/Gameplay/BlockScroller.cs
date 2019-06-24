using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockScroller : MonoBehaviour
{

    public float baseScrollSpeed = 2.0f;
    private float scrollSpeedup = 0.0f;
    public float scrollSpeedupDivisor = 100.0f;

    [SerializeField] private BlockSpawner bSpawner;
    [SerializeField] private GameObject ScoreCounterObject;

    private List<GameObject> gList;
    private GameObject[] scrollingObjects;
    private UpdateScore scoreCounter;

    private bool destroyedFirstBlock = false;

    // Start is called before the first frame update
    void Start()
    {
        gList = bSpawner.GetBlockList();
        scoreCounter = ScoreCounterObject.GetComponent<UpdateScore>();
        //scrollingObjects = GameObject.FindGameObjectsWithTag(Tags.NonProcedurals);
    }

    public void ScrollStartingPlatform()
    {
        destroyedFirstBlock = true;
    }

    public void RefreshScrollingObjects()
    {
        scrollingObjects = GameObject.FindGameObjectsWithTag(Tags.NonProcedurals);
    }

    public float BlockYVelocity()
    {
        return (baseScrollSpeed + scrollSpeedup) * Time.deltaTime * -1;
    }

    // Update is called once per frame
    void Update()
    {
        scrollSpeedup = scoreCounter.Score() / scrollSpeedupDivisor;
        Vector2 scrollVector = new Vector3(0.0f, (baseScrollSpeed + scrollSpeedup) * Time.deltaTime * -1);
        
        //Scroll the blocks
        foreach (GameObject gameBlock in gList)
        {
            try
            {
            gameBlock.transform.Translate(scrollVector); 
            }
            catch(MissingReferenceException mre) {}
        }
        //gQueue = bScroller.GetBlockQueue();

        //Scroll objects that will never be in the blockQueue because they aren't procedurally generated
        //(E.g. starting platform)
        if (scrollingObjects != null && destroyedFirstBlock)
        {
            foreach (GameObject scroller in scrollingObjects)
            {
                if (scroller)
                    scroller.transform.Translate(scrollVector);
            }
        }
    }
}
