using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSnapper : MonoBehaviour
{

    public enum SnapType { LEFT, RIGHT, TOP, KILL, BOTTOM };

    public SnapType snapType;

    private List<GameObject> players;

    private void OnTriggerEnter2D(Collider2D cdr)
    {
        //Debug.Log("In OnTriggerEnter()");
        GameObject collidedObject = cdr.gameObject;

        if (players.Contains(collidedObject))
        {
            PlayerInput pi = collidedObject.GetComponent<PlayerInput>();
            switch (snapType)
            {
                case SnapType.LEFT:
                    Debug.Log("Snapped left");
                    pi.SnapLeft(transform.parent.gameObject);
                    break;
                case SnapType.RIGHT:
                    Debug.Log("Snapped right");
                    pi.SnapRight(transform.parent.gameObject);
                    break;
                case SnapType.TOP:
                    Debug.Log("Snapped top");
                    pi.GroundPlayer(transform.parent.gameObject);
                    break;
                case SnapType.BOTTOM:
                    pi.HitBottom();
                    break;
                case SnapType.KILL:
                    Destroy(collidedObject);
                    break;

            }
        }
    }

    private void OnTriggerStay2D(Collider2D cdr)
    {
        //Debug.Log("In OnTriggerEnter()");
        GameObject collidedObject = cdr.gameObject;

        if (players.Contains(collidedObject))
        {
            PlayerInput pi = collidedObject.GetComponent<PlayerInput>();
            switch (snapType)
            {
                case SnapType.LEFT:
                    Debug.Log("Snapped left");
                    if(!(Input.GetKeyDown(KeyCode.UpArrow) || !pi.floatingUpAndSnapped()))
                        pi.SnapLeft(transform.parent.gameObject);
                    break;
                case SnapType.RIGHT:
                    Debug.Log("Snapped right");
                    if(!(Input.GetKeyDown(KeyCode.UpArrow) || !pi.floatingUpAndSnapped()))
                        pi.SnapRight(transform.parent.gameObject);
                    break;
                // case SnapType.LEFT:
                //     Debug.Log("Snapped left");
                //     if(pi.movingLeftRightUnsnapped())
                //         pi.SnapLeft(transform.parent.gameObject);
                //     break;
                // case SnapType.RIGHT:
                //     Debug.Log("Snapped right");
                //     if(pi.movingLeftRightUnsnapped())
                //         pi.SnapRight(transform.parent.gameObject);
                //     break;
                case SnapType.TOP:
                    Debug.Log("Snapped top");
                    pi.GroundPlayer(transform.parent.gameObject);
                    break;
                case SnapType.BOTTOM:
                    pi.HitBottom();
                    break;
                case SnapType.KILL:
                    Destroy(collidedObject);
                    break;

            }
        }
    }


    private void OnTriggerExit2D(Collider2D other)
    {
        GameObject collidedObject = other.gameObject;

        if (players.Contains(collidedObject))
        {
            PlayerInput pi = collidedObject.GetComponent<PlayerInput>();
            switch (snapType)
            {
                case SnapType.LEFT:
                    pi.UnsnapFromWall();
                    break;
                case SnapType.RIGHT:
                    pi.UnsnapFromWall();
                    break;
                case SnapType.TOP:
                    pi.Unground();
                    break;
                case SnapType.BOTTOM:
                    pi.LeaveBottom();
                    break;
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        players = new List<GameObject>(GameObject.FindGameObjectsWithTag(Tags.PlayerCharacter));
    }

    //// Update is called once per frame
    //void Update()
    //{

    //}
}
