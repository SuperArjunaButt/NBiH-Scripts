using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SideBarrierPosition : MonoBehaviour
{
    public GameObject blockSpawner;
    private float barrierXPos;
    private Vector3 barrierPosition;

    // Start is called before the first frame update
    void Start()
    {

        //If this is the left barrier, position it based on minXSpawnCoord
        if(transform.position.x < 0)
        {
            barrierXPos = blockSpawner.GetComponent<BlockSpawner>().minXSpawnCoord - 0.5f;
            PositionBarrier(barrierXPos);
        }
        //If this is the right barrier, position it based on maxXSpawnCoord
        if (transform.position.x > 0)
        {
            barrierXPos = blockSpawner.GetComponent<BlockSpawner>().maxXSpawnCoord + 0.5f;
            PositionBarrier(barrierXPos);
        }

    }

    void PositionBarrier(float xBoundary)
    {
        //Match the position to the min or maxSpawnCoord
        barrierPosition = new Vector3(xBoundary, transform.position.y, transform.position.z);
        transform.position = barrierPosition;
    }
}
