using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{

    //Jumping: https://answers.unity.com/questions/1020197/can-someone-help-me-make-a-simple-jump-script.html
    //This is test text
    public float jumpVelocity = 3.0f;
    public float downJumpVelocity = 1.5f;
    public float moveHorizontalVelocity = 1.5f;
    public float mobileVerticalVelocity = 0.1f;

    public float minXVel = -8.0f;
    public float maxXVel = 8.0f;

    private bool isUpAgainstBlock = false;
    private bool movingLeftRight = false;

    private Vector2 moveHorizontalVector;

    private float moveVertical;
    private float moveHorizontal;

    private Rigidbody2D rb;

    private bool isGrounded = true;
    private bool snappedLeft = false;
    private bool snappedRight = false;
    private bool hitBottom = false;
    private bool UsingXBoneController = false;
    private bool UsingPS3Controller = false;


    [SerializeField] private GameObject scoreCounterObject;
    [SerializeField] private GameObject gameOverScreenObject;
    [SerializeField] private GameObject startMenuObject;
    [SerializeField] private AudioClip jumpSound;
    [SerializeField] private AudioClip snapNoise;
    [SerializeField] private AudioClip groundedNoise;
    

    [SerializeField] private Sprite jumpingSprite;
    [SerializeField] private Sprite fallingSprite;
    [SerializeField] private Sprite standingSprite;
    [SerializeField] private Sprite clampedSprite;

    private UpdateScore scoreUpdater;
    private GameOverScreen gameOverScreen;
    private GameplayController gController;
    private AudioSource playerSoundSrc;
    private SpriteRenderer spriteRenderer;
    private BlockScroller bScroller;
    int a = 1;
    private bool canFloatUp = true;
    private bool floatingUpNotClamped = false;

    private float oldTouchX;
    private float oldTouchY;


    public void SetFloatUp(bool floating)
    {
        canFloatUp = floating;
    }

    //private void OnCollisionStay(Collision collision)
    //{
    //    //Are we colliding against an object that is going to scroll to the bottom of the screen?
    //    //If so, we should prevent the player from moving up while moving sideways to stick to the block
    //    if (collision.collider.gameObject.CompareTag(Tags.ScrollingBlock))
    //        isUpAgainstBlock = true;
    //    else
    //    {
    //        isUpAgainstBlock = false;

    //    }
    //}

    //private void OnCollisionExit(Collision collision)
    //{
    //    if (isUpAgainstBlock) isUpAgainstBlock = false;
    //}



    

    public void SnapLeft(GameObject blockSnappingTo)
    {
        if (hitBottom) return;
        
        //Change sprite to gripping left face animation here
        spriteRenderer.sprite = clampedSprite;
        //Flip the x axis since we don't have a sprite specifically for this
        spriteRenderer.flipX = true;
        //This will involve a call to the player's Runtime Animation Controller most likely

        if(!playerSoundSrc.isPlaying)
        {
            playerSoundSrc.clip = snapNoise;
            playerSoundSrc.Play();
        }

        //Make player character a child object of the block it snapped to
        transform.parent = blockSnappingTo.transform;
        //turn off gravity
        rb.gravityScale = 0.0f;


        //Zero out y velocity so we can't slide up or down the block
        //WRONG!!  y velocity needs to match block we're snapping to
        rb.velocity = new Vector2(rb.velocity.x, 0.0f);
        //rb.velocity = new Vector2(rb.velocity.x, bScroller.BlockYVelocity());


        snappedLeft = true;

        isGrounded = true;
        //Debug.Log("In PlayerInput.SnapLeft()");
    }

    public void SnapRight(GameObject blockSnappingTo)
    {
        if (hitBottom) return;

        //Change sprite to gripping right face animation here
        spriteRenderer.sprite = clampedSprite;
        

        //This will involve a call to the player's Runtime Animation Controller most likely


        //Play snapping noise
        if(!playerSoundSrc.isPlaying)
        {
            playerSoundSrc.clip = snapNoise;
            playerSoundSrc.Play();
        }

        //Make player character a child object of the block it snapped to
        transform.parent = blockSnappingTo.transform;
        //turn off gravity
        rb.gravityScale = 0.0f;

        //Zero out y velocity so we can't slide up or down the block
        //WRONG!!  y velocity needs to match block we're snapping to
        rb.velocity = new Vector2(rb.velocity.x, 0.0f);
        //rb.velocity = new Vector2(rb.velocity.x, bScroller.BlockYVelocity());


        snappedRight = true;

        isGrounded = true;
        //Debug.Log("In PlayerInput.SnapRight()");
    }

    public void UnsnapFromWall()
    {
        spriteRenderer.flipX = false;
        
        //Debug.Log("In UnsnapFromWall()");
        //reset the bools for whether we're snapped to a surface
        snappedLeft = false;
        snappedRight = false;

        //Deparent the player from the wall it was parented to
        transform.parent = null;

        //Turn gravity back on so we're not just scrolling with the block
        rb.gravityScale = 1.0f;
    }

    public void GroundPlayer(GameObject blockSnappingTo)
    {
        if (snappedLeft || snappedRight) return;
        
        if(!isGrounded)
        {
            //Play grounded noise
            playerSoundSrc.clip = groundedNoise;
            playerSoundSrc.Play();
        }

        //Debug.Log("In PlayerInput.GroundPlayer()");
        isGrounded = true;

        //Make player character a child object of the block it snapped to
        transform.parent = blockSnappingTo.transform;
        //turn off gravity
        rb.gravityScale = 0.0f;



    }

    public void Unground()
    {
        //Make player character a child object of the block it snapped to
        transform.parent = null;
        isGrounded = false;
        //turn off gravity
        rb.gravityScale = 1.0f;
    }

    public void HitBottom()
    {
        hitBottom = true;
    }

    public void LeaveBottom()
    {
        hitBottom = false;
    }

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        //moveHorizontalVector = new Vector3(moveHorizontalVelocity, 0.0f, 0.0f);
        moveHorizontalVector = new Vector2(moveHorizontalVelocity, 0.0f);
        //moveRightVector = new Vector3(moveLeftRightVelocity, 0.0f, 0.0f);
    }

    private void Start()
    {
        scoreUpdater = scoreCounterObject.GetComponent<UpdateScore>();
        gameOverScreen = gameOverScreenObject.GetComponent<GameOverScreen>();
        gController = GameObject.FindGameObjectWithTag(Tags.GameController).GetComponent<GameplayController>();
        playerSoundSrc = GetComponent<AudioSource>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        bScroller = GameObject.FindGameObjectWithTag(Tags.GameController).GetComponent<BlockScroller>();

        //Find out which joysticks we have connected, if any
        string[] controllers = Input.GetJoystickNames();
        foreach(string controller in controllers)
        {
            Debug.Log(controller);
            if (controller.Contains("Xbox"))
                UsingXBoneController = true;
            if (controller.Contains("PLAYSTATION"))
            {
                Debug.Log("Detected PS3 Controller");
                UsingPS3Controller = true;
            }
        }

    }

    // Update is called once per frame
    void Update()
    {
        //if Input.GetTouches().length > 0 && TouchPhase.Began
        #if UNITY_ANDROID || UNITY_IOS
            //if Input.GetTouches().length > 0 && TouchPhase.Began
            if(Input.touchCount > 0)
            {
                Touch jumpTouch = Input.GetTouch(0);
                if(jumpTouch.phase == TouchPhase.Began && isGrounded)
                {
                    playerSoundSrc.clip = jumpSound;
                    playerSoundSrc.Play();
                    Unground();
                    rb.AddForce(Vector2.up * jumpVelocity, ForceMode2D.Impulse);    //TODO: Best practice is to add force in FixedUpdate, not Update
                    isGrounded = false;
                    spriteRenderer.sprite = jumpingSprite;
                    UnsnapFromWall();
                }
                
                if(jumpTouch.phase == TouchPhase.Began)
                {
                    oldTouchX = jumpTouch.position.x;
                    oldTouchY = jumpTouch.position.y;
                }

            }
        #else       //EVERYTHING KEYBOARD AND GAMEPAD AND PROPANE ACCESSORIES
            if ((Input.GetKeyDown(KeyCode.Space) && isGrounded) || (!UsingXBoneController && Input.GetButtonDown("JumpPS4") && isGrounded)
                                                                || (UsingXBoneController && Input.GetButtonDown("JumpXBone") && isGrounded)
                                                                || (UsingPS3Controller && Input.GetButtonDown("JumpPS3") && isGrounded))
            {
                playerSoundSrc.clip = jumpSound;
                playerSoundSrc.Play();
                Unground();
                rb.AddForce(Vector2.up * jumpVelocity, ForceMode2D.Impulse);    //TODO: Best practice is to add force in FixedUpdate, not Update
                isGrounded = false;
                spriteRenderer.sprite = jumpingSprite;
                UnsnapFromWall();
            }
        #endif

        if((Input.GetKeyDown(KeyCode.DownArrow) && !isGrounded))
        {
            rb.AddForce(Vector2.down * downJumpVelocity, ForceMode2D.Impulse);  //TODO: Best practice is to add force in FixedUpdate, not Update
            spriteRenderer.sprite = fallingSprite;
        }
        


        float verticalVelocity = rb.velocity.y;

        if (snappedLeft || snappedRight)
        {
            spriteRenderer.sprite = clampedSprite;
            return;
        }

        if(rb.velocity.x > 0.01f)
        {
            spriteRenderer.flipX = true;
        }
        if(rb.velocity.x < -0.01f)
        {
            spriteRenderer.flipX = false;
        }

        if(!snappedLeft && !snappedRight && Mathf.Abs(verticalVelocity) <= 0.01f)
        {
            spriteRenderer.sprite = standingSprite;
            
            return;
        }

        if (Mathf.Abs(verticalVelocity) > 0.01f && verticalVelocity > 0)
        {
            spriteRenderer.sprite = jumpingSprite;
            return;
        }

        if (Mathf.Abs(verticalVelocity) > 0.01f && verticalVelocity < 0)
        {
            spriteRenderer.sprite = fallingSprite;
            return;
        }

        //if(!isGrounded AND (raycast to left is true OR raycast to right is true)) jump?
    }

    private void FixedUpdate()
    {

        moveHorizontal = Input.GetAxis("Horizontal");
        moveVertical = Input.GetAxis("Vertical");

        #if UNITY_ANDROID || UNITY_IOS
            //Get the touch (if it's there) if(TouchPhase.Moved)
            //Figure out the x component of the deltaPosition
            //Convert to a unit vector so we don't have crazy large x values?
            if(Input.touchCount > 0)
            {
                Touch jumpTouch = Input.GetTouch(0);
                if(jumpTouch.phase == TouchPhase.Moved)
                {
                    //If we normalize it, we turn it into something equivalent to gamepad axis data
                    //This is awfully finicky even when normalized.  Should we:
                    //1. Lerp it to the new position, OR
                    //2. divide by the width of the screen?
                    jumpTouch.deltaPosition.Normalize();        //TODO: Since this is reading in input, shouldn't this be in Update()?
                    moveHorizontal = jumpTouch.deltaPosition.x;


                    //If to figure out if we can float up?
                    // moveVertical = jumpTouch.deltaPosition.y;

                }
                if(jumpTouch.phase != TouchPhase.Ended && jumpTouch.phase != TouchPhase.Canceled)
                {
                    moveVertical = 1.0f;
                    floatingUpNotClamped = true;
                }
                else
                {
                    moveVertical = 0.0f;
                    floatingUpNotClamped = false;
                }
            }
        #endif

        if(moveVertical >= 0.1f && !snappedLeft && !snappedRight)
        {
            floatingUpNotClamped = true;
        }
        else
        {
            floatingUpNotClamped = false;
        }

        Vector2 movedVector = moveHorizontalVector * moveHorizontal;
        //if (!isUpAgainstBlock)                                          //Are we not up against a block?  Then we should move upwards if moving sideways
        //    movedVector.y = Mathf.Abs(movedVector.y);
        //else                                                            //Otherwise we're not adding vertical velocity
        //    movedVector.y = 0;
        //Debug.Log(movedVector);
        //Debug.Log(movedVector.x);
        if (movedVector.x < 0 && snappedLeft)   //Moving left and we're snapped right
        {
            Debug.Log("snapped left, moving right");
            UnsnapFromWall();
        }
        if (movedVector.x > 0 && snappedRight)   //Moving right and we're snapped left
        {
            Debug.Log("snapped right, moving left");
            UnsnapFromWall();
        }

        //if ((movedVector.x > 0 && snappedRight) || (movedVector.x < 0 && snappedLeft)) { return; }
        //float moveVertical = Input.GetAxis("Vertical");
        rb.AddForce(movedVector, ForceMode2D.Impulse);
        //TODO: Would this be more readable with a call to clamp?
        if (rb.velocity.x > maxXVel)
        {
            rb.velocity = new Vector2(maxXVel, rb.velocity.y);
            return;
        }
        if(rb.velocity.x < minXVel)
        {
            rb.velocity = new Vector2(minXVel, rb.velocity.y);
            return;
        }


        //TODO: Incorporate fixedDeltatime?
        #if UNITY_ANDROID || UNITY_IOS
            movedVector = Vector2.up * moveVertical * mobileVerticalVelocity;
            if(movedVector.y > 0.0f && canFloatUp)
                rb.AddForce(movedVector, ForceMode2D.Impulse);
        #else
            movedVector = Vector2.down * (downJumpVelocity / 10.0f ) * moveVertical * -1;
            if((movedVector.y > 0.0f && canFloatUp) || movedVector.y <= 0.0f)
            {
                rb.AddForce(movedVector, ForceMode2D.Impulse);                    
            }
            else
            {
                //Debug.Log("Not applying vertical force");
            }
        #endif
        //else
        //{
        //    rb.AddForce(movedVector, ForceMode.VelocityChange);
        //}
        //Debug.Log("Velocity: " + rb.velocity);

        //So we don't slide up the side of a block with the clamped sprite
        if((Input.GetKeyDown(KeyCode.UpArrow) && (snappedLeft || snappedRight))
        || (Mathf.Abs(moveHorizontal) <= 0.1f && moveVertical > 0.3f && (snappedLeft || snappedRight)))
        {
            Debug.Log("Floating while snapped");
            UnsnapFromWall();
            floatingUpNotClamped = true;
        }

        if(Mathf.Abs(rb.velocity.x) >= 0.1f)
            movingLeftRight = true;

    }

    public bool floatingUpUnclamped()
    {
        return floatingUpNotClamped;
    }

    public bool floatingUpAndSnapped()
    {
        return (Input.GetKeyDown(KeyCode.UpArrow) && (snappedLeft || snappedRight))
        || (Mathf.Abs(moveHorizontal) <= 0.1f && moveVertical > 0.1f && (snappedLeft || snappedRight));
    }

    public bool movingLeftRightUnsnapped()
    {
        #if UNITY_ANDROID || UNITY_IOS
            return movingLeftRight && rb.velocity.y < 0.1f;
        #else
        return  Input.GetKeyDown(KeyCode.LeftArrow) 
                || Input.GetKeyDown(KeyCode.RightArrow)
                || movingLeftRight == true
                && (!snappedLeft || !snappedRight)
                && rb.velocity.y < 0.1f;
        #endif
    }

    private void OnDestroy()
    {
        //Play death sound
        gController.StopBackgroundMusic();
        
        //Turn on game over screen
        gameOverScreen.EnableGameOverScreen();
        if(startMenuObject.activeSelf == true)
            startMenuObject.SetActive(false);

        //Stop the score timer
        scoreUpdater.PlayerDead();

    }


}
