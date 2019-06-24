using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * GameplayController component responsible for spawning blocks* 
 */
public class BlockSpawner : MonoBehaviour
{

    //Defaults to play around with depth and how far above the camera they spawn
    //[SerializeField] private float ZSpawnCoord = 2.0f;
    [SerializeField] private float YSpawnCoord = 10.0f;
    private float XSpawnCoord = 2.0f;

    //First block to enter is the first block to leave
    //private Stack<GameObject> blockStack = new Stack<GameObject>();
    
    //Last block to enter is the first block to leave
    private List<GameObject> blockList = new List<GameObject>();

    //Farthest left X coordinate it can spawn
    public float minXSpawnCoord = -9.0f;

    //Farthest right X coordinate it can spawn
    public float maxXSpawnCoord = 9.0f;

    //Array containing obstacle blocks and deadly objects
    [SerializeField] private GameObject[] GameBlockObjects;

    //Array containing powerups
    [SerializeField] private GameObject[] powerupObjects;

    public int randomSpawnTries = 15;

    //Spawn numBlocksToSpawn blocks every spawnFrequency seconds
    public int numBlocksToSpawn = 2;

    //REQUIRE: spawnFrequencies.Length == timers.Length
    public float[] blockSpawnFrequencies;
    [SerializeField] private float[] blockTimers;

    //REQUIRE: powerupSpawnFrequencies.Length == powerupTimers.Length
    public float[] powerupSpawnFrequencies;
    [SerializeField] private float[] powerupTimers;

    private bool gameStarted = false;

    public List<GameObject> GetBlockList()
    {
        return blockList;
    }

    //Places a block just above the camera view
    //Returns true if spawn successful
    //see https://answers.unity.com/questions/1506835/how-to-prevent-3d-object-spawn-overlapping.html
    //and example code for Physics.OverlapBox
    bool PlaceBlock(GameObject objectToSpawn)
    {
        Vector2 blockCoord;
        float XCoord;
        CompositeHitbox cHitbox = objectToSpawn.GetComponent<CompositeHitbox>();
        //for number of tries
        LayerMask defaultMask = LayerMask.GetMask("Default");
            for (int i = 0; i < randomSpawnTries; i++)
            {
                
                //Pick a random X coordinate
                XCoord = Random.Range(minXSpawnCoord, maxXSpawnCoord);
                blockCoord = new Vector2(XCoord, YSpawnCoord);

                //Check for collision
                Collider2D[] hitColliders = Physics2D.OverlapBoxAll(blockCoord + cHitbox.GetOffset(), cHitbox.GetSize(), 0.0f, defaultMask);
                if (hitColliders.Length == 0)     //If no collision, spawn a new block and return
                {
                    Object newBlock = Instantiate(objectToSpawn, blockCoord + new Vector2(objectToSpawn.transform.position.x, objectToSpawn.transform.position.y), Quaternion.identity);
                    blockList.Add((GameObject)newBlock);
                    return true;
                }
            }
        
        return false;     
    }

    public void RemoveBlock(GameObject blockToRemove)
    {
        if(blockList.Contains(blockToRemove))
        {
            blockList.Remove(blockToRemove);
        }
        Destroy(blockToRemove);
 
    }

    public void StartSpawner()
    {
        gameStarted = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (gameStarted)
        {
            //Increment each timer by the amount of time passed
            for (int j = 0; j < blockTimers.Length; j++)
            {
                blockTimers[j] += Time.deltaTime;  
                if(j < powerupTimers.Length)       
                    powerupTimers[j] += Time.deltaTime;         
            }

            //Check each timer to see if it's time to spawn a new block
            for (int i = 0; i < blockSpawnFrequencies.Length; i++)
            {
                if (blockTimers[i] >= blockSpawnFrequencies[i])
                {
                    PlaceBlock(GameBlockObjects[(int)Random.Range(0.0f, (float)GameBlockObjects.Length)]);
                    blockTimers[i] = 0.0f;
                }

                if(i < powerupTimers.Length)
                {
                    if (powerupTimers[i] >= powerupSpawnFrequencies[i])
                    {
                        bool spawnTry = PlaceBlock(powerupObjects[(int)Random.Range(0.0f, (float)powerupObjects.Length)]);
                        powerupTimers[i] = 0.0f;
                        if(spawnTry) Debug.Log("Powerup Spawn Succeeded");
                        else Debug.Log("Powerup Spawn Failed");
                    }
                }
            }

            // //Check each timer to see if it's time to spawn a new powerup
            // for (int k = 0; k < powerupSpawnFrequencies.Length; k++)
            // {
            //     if (powerupTimers[k] >= powerupSpawnFrequencies[k])
            //     {
            //         PlaceBlock(powerupObjects[(int)Random.Range(0.0f, (float)powerupObjects.Length)]);
            //         powerupTimers[k] = 0.0f;
            //     }
            // }
        }
    }
}
