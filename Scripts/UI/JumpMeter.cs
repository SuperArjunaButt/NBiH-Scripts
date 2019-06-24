using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JumpMeter : MonoBehaviour
{
    float jumpMeterTotal = 100f;
    float jumpMeterCount = 100f;
    int i = 0;
    // TODO: Can I set an editor requirement that a variable has to be divisble by another number?
    // Would ensure we don't run into problems where IncreaseJumpMeter doesn't reach exactly 100.
    public float jumpMeterRefill;
    public float jumpMeterDeplete;

    public float minimumJumpRegen = 20.0f;

    public GameObject jumpMeter;

    private bool pressingUp = false;

    [SerializeField] private GameObject playerObject;
    private PlayerInput player;

    // Start is called before the first frame update
    void Start()
    {
        player = playerObject.GetComponent<PlayerInput>();
    }

    void Update()
    {
        
        if(jumpMeterCount > minimumJumpRegen)
        {
            player.SetFloatUp(true);
        }
        
        if (jumpMeterCount < 100)
        {
            jumpMeter.SetActive(true);
        } 
        else
        {
            jumpMeter.SetActive(false);
        }

        // //TODO: Make this work on our two gamepads
        // if (Input.GetKeyDown(KeyCode.UpArrow)) {
        //     DecreaseJumpMeter();
        // }
        // if(Input.GetKeyUp(KeyCode.UpArrow))
        // {
        //     IncreaseJumpMeter();
        // }

        #if UNITY_ANDROID || UNITY_IOS
            if(Input.touchCount > 0)
            {
                Touch jumpTouch = Input.GetTouch(0);
                if(jumpTouch.phase != TouchPhase.Ended && jumpTouch.phase != TouchPhase.Canceled)
                {
                    pressingUp = true;
                }
                else
                {
                    
                    Debug.Log("Jump ended or canceled");
                    pressingUp = false;
                }
            }
            else
            {
                pressingUp = false;
            }
        #else
            // if(Input.GetKeyDown(KeyCode.UpArrow))
            // {
            //     pressingUp = true;
            // }
            // else if(Input.GetKeyUp(KeyCode.UpArrow))
            // {
            //     pressingUp = false;
            // 
            if(player.floatingUpUnclamped())
            {
                pressingUp = true;
            }
            else
            {
                pressingUp = false;
            }

        #endif



        if(pressingUp) DecreaseJumpMeter();
        else {
            jumpMeter.SetActive(false);
            IncreaseJumpMeter();
        }

        jumpMeter.GetComponent<Image>().fillAmount = jumpMeterCount / jumpMeterTotal; 
    }

    public void DecreaseJumpMeter()
    {
        if (jumpMeterCount > 0)
        {
            jumpMeterCount -= jumpMeterDeplete;
        }
        else
        {
            player.SetFloatUp(false);
        }
    }

    public void IncreaseJumpMeter()
    {
        if (jumpMeterCount < jumpMeterTotal)
        {
            jumpMeterCount += jumpMeterRefill;
        }
    }
}
