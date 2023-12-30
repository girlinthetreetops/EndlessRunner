using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerCCMovement : MonoBehaviour
{
    //references
    private CharacterController cc;
    private Animator anim;
    private LevelManager levelManager;
    public InputManager inputManager;
    private GameManager gameManager;

    //Movement bools
    private bool swipeLeft;
    private bool swipeRight;
    private bool swipeUp;
    private bool swipeDown;
    private bool isInRoll;
    private bool isInJump;
    private bool canMove;
    private bool hasCollided;


    //Lane stuff
    private enum Lane { Left, Middle, Right }
    private Lane currentLane = Lane.Middle;
    public float xLaneChangeOffset = 2;
    private float xValueToAdjust = 0f;

    //Other
    private float ColHeight;
    private float ColCenterY;
    private float xVel; //horisontal velocity
    private float yVel; //vertical velocity

    //Adjustments
    public float speedDodge = 10f;
    public float jumpPower = 7f;
    public float groundProximityJumpGrace = 0.8f; // To adjust jump grace

    //Raycast
    [SerializeField] private float playerRadius = .7f;
    [SerializeField] private float playerHeight = 2f;
    [SerializeField] private float reachDistance = 1.2f;

    //Effects
    [SerializeField] ParticleSystem crashParticles;
    [SerializeField] ParticleSystem runningParticles;

    //Sounds
    [SerializeField] AudioSource runningAudioSource;
    [SerializeField] AudioSource jumpAudioSource;
    [SerializeField] AudioSource landingAudioSource;
    [SerializeField] AudioSource coinCollectAudioSource;
    [SerializeField] AudioSource crashAudioSource;

    void Start()
    {
        //Retrieve some references
        cc = GetComponent<CharacterController>();
        anim = GetComponentInChildren<Animator>();
        inputManager = FindObjectOfType<InputManager>();
        levelManager = FindObjectOfType<LevelManager>();
        gameManager = FindObjectOfType<GameManager>();

        //Subscribe to input events and assign correct actions
        inputManager.OnRightSwipe.AddListener(StartRightMove);
        inputManager.OnLeftSwipe.AddListener(StartLeftMove);
        inputManager.OnSwipeUp.AddListener(StartJump);
        inputManager.OnSwipeDown.AddListener(StartRoll);

        levelManager.OnCollision.AddListener(RespondToCollision);
        levelManager.OnDeath.AddListener(Die);
        
        //Set start position
        transform.position = Vector3.zero;
        ColHeight = cc.height;
        ColCenterY = cc.center.y;
        canMove = true;
        hasCollided = false;
    }
    //use input and register action with the local bool
    public void StartRightMove()
    {
        swipeRight = true;
    }
    public void StartLeftMove()
    {
        swipeLeft = true;
    }
    public void StartJump()
    {
        swipeUp = true;
    }
    public void StartRoll()
    {
        swipeDown = true;
    }

    //Handle main movements each frame
    void Update()
    {
        //Move Left
        if (swipeLeft && !isInRoll && canMove)
        {
            swipeLeft = false;

            if (currentLane == Lane.Middle)
            {
                xValueToAdjust = -xLaneChangeOffset;
                anim.Play("MoveLeft");
                currentLane = Lane.Left;
            }
            else if (currentLane == Lane.Right)
            {
                xValueToAdjust = 0;
                anim.Play("MoveLeft");
                currentLane = Lane.Middle;
            }
        }

        //Move Right
        if (swipeRight && !isInRoll && canMove)
        {
            swipeRight = false;

            if (currentLane == Lane.Middle)
            {
                xValueToAdjust = xLaneChangeOffset;
                anim.Play("MoveRight");
                currentLane = Lane.Right;
            }
            else if (currentLane == Lane.Left)
            {
                xValueToAdjust = 0;
                anim.Play("MoveRight");
                currentLane = Lane.Middle;
            }
        }

        //I've somehow managed to make this work... but barely so, so adjustments will be tricky unless I consider this more.
        xVel = Mathf.Lerp(xVel, xValueToAdjust, Time.deltaTime * speedDodge);
        Vector3 moveVector = new Vector3(xVel - transform.position.x, yVel * Time.deltaTime, 0);
        cc.Move(moveVector);

        Jump();
        Roll();
        CheckForCollisions();
        transform.position = new Vector3(transform.position.x, transform.position.y, 0);
    }

    public void Jump()
    {
        if (cc.isGrounded && canMove || Physics.Raycast(transform.position, Vector3.down, groundProximityJumpGrace) && canMove) //inbuilt function for charactercontrollers
        {
            if (anim.GetCurrentAnimatorStateInfo(0).IsName("Falling"))
            {
                anim.Play("Landing");
                isInJump = false;
                runningParticles.Play();
                runningAudioSource.Play();
                landingAudioSource.Play();
            }

            if (swipeUp)
            {
                yVel = jumpPower;
                anim.CrossFadeInFixedTime("JumpUp", 0.1f);
                runningParticles.Stop();
                runningAudioSource.Stop();
                jumpAudioSource.Play();
                isInJump = true;
            }
        }

        else
        {
            yVel -= jumpPower * 2 * Time.deltaTime;

            if (cc.velocity.y < -0.1f)
            {
                anim.Play("Falling");
            }

        }

        swipeUp = false;
    }

    internal float RollCounter;

    public void Roll()
    {
        if (canMove)
        {
            if (RollCounter <= 0f)
            {
                RollCounter = 0;
                cc.center = new Vector3(0, ColCenterY, 0);
                cc.height = ColHeight;
                isInRoll = false;
            }
            if (swipeDown)
            {
                RollCounter = 0.2f; //roll animation duration
                yVel -= 10f;
                cc.center = new Vector3(0, ColCenterY / 2, 0);
                cc.height = ColHeight / 2;
                anim.CrossFadeInFixedTime("Roll", 0.3f);
                isInRoll = true;
                isInJump = false;
            }
        }
        RollCounter -= Time.deltaTime;

        
        swipeDown = false;
    }

    public void CheckForCollisions()
    {
        if (!hasCollided)
        {
            if (Physics.Raycast(transform.position, Vector3.forward, out RaycastHit raycastHit, reachDistance))
            {
                if (raycastHit.collider.CompareTag("Obstacle"))
                {
                    levelManager.InvokeCollision();
                }

                else if (raycastHit.collider.CompareTag("Coin")) {
                    Destroy(raycastHit.collider.gameObject);
                    gameManager.AddGold(1);
                    levelManager.AddCoinsCollectedInLevel(1);
                    coinCollectAudioSource.Play();
                    transform.position = new Vector3(transform.position.x, transform.position.y, 0);
                }
            }
        }
    }

    public void RespondToCollision()
    {
        isInJump = false;
        hasCollided = true;
        crashParticles.Play();
        crashAudioSource.Play();
        runningAudioSource.Stop();
        runningParticles.Stop();
        Die();
    }

    public void Die()
    {
        canMove = false;
        anim.Play("Crashed");
    }
}
