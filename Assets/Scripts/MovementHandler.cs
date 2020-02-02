using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; //Temporary

public class MovementHandler : MonoBehaviour
{
    public Animator anim;
    public Rigidbody2D rb;
    public BoxCollider2D pushBox;
    public BoxCollider2D pushTrigger;
    public AcceptInputs Actions;
    public HitDetector HitDetect;
    public MaxInput MaxInput;

    public Transform opponent;
    public CharacterProperties OpponentProperties;

    AnimatorStateInfo currentState;

    public float weight;
    public float walkSpeed;
    public float walkBackSpeed;
    public float backDashForce;
    public float runAcceleration;
    public float jumpPower;
    public float maxVelocity;
    public int maxJumps;
    public int jumps = 0;
    public float minPosY;
    public int justDefenseTime = 4;
    public bool hittingWall = false;
    public bool playing;

    float inputTime = 0.3f;
    float runInputTime = 0.3f;
    int dashButtonCount = 0;
    int buttonCount = 0;
    public int wallStickTimer;
    public float jumping = 0;
    public bool jumped = false;
    public bool backDash = false;
    private bool jumpRight = false;
    private bool jumpLeft = false;
    bool vertAxisInUse = false;
    bool horiAxisInUse = false;

    //pushbox Sizes and center depending on state
    public Vector2 pushCenter;
    public Vector2 pushSize;
    public Vector2 airPushCenter;
    public Vector2 airPushSize;

    public bool facingRight = true;
    public string Horizontal = "Horizontal_P1";
    public string Vertical = "Vertical_P1";
    public string L3 = "L3_P1";

    static int airID;
    static int crouchID;
    static int runID;
    static int walkFID;
    static int walkBID;
    static int backDashID;
    static int jumpID;
    static int landID;   
    static int lowGuardID;
    static int highGuardID;
    static int airGuardID;
    static int wallStickID;
    static int groundBounceID;
    static int wallBounceID;
    static int yVeloID;

    // Set Up inputs, anim variable hashes, and opponent in awake
    void Awake()
    {
        //Original system to use in original Training Stage
        if (SceneManager.GetActiveScene().name == "TrainingStage")
        {
           Horizontal = "Horizontal_P1";
            Vertical = "Vertical_P1";
            L3 = "L3_P1";
            opponent = GameObject.Find("Player2").transform.GetChild(0).transform;
        }
        else
        {
            Horizontal = "Horizontal_P2";
            Vertical = "Vertical_P2";
            L3 = "L3_P2";
            opponent = GameObject.Find("Player1").transform.GetChild(0).transform;
        }
        OpponentProperties = opponent.GetComponent<CharacterProperties>();
    }

    void Start()
    {
        //Added for character loading system. Needs to start here for it to work
        if (SceneManager.GetActiveScene().name == "TrainingStage2")
        {
            if (transform.parent.name == "Player1")
            {
                Horizontal = "Horizontal_P1";
                Vertical = "Vertical_P1";
                L3 = "L3_P1";
                opponent = GameObject.Find("Player2").transform.GetChild(0).transform;
            }
            else
            {
                Horizontal = "Horizontal_P2";
                Vertical = "Vertical_P2";
                L3 = "L3_P2";
                opponent = GameObject.Find("Player1").transform.GetChild(0).transform;
            }
            OpponentProperties = opponent.GetComponent<CharacterProperties>();
        }
        //
        Application.targetFrameRate = 60;

        pushBox.enabled = true;
        pushBox.offset = pushCenter;
        pushBox.size = pushSize;

        airID = Animator.StringToHash("Airborne");
        crouchID = Animator.StringToHash("Crouch");
        runID = Animator.StringToHash("Run");
        walkFID = Animator.StringToHash("WalkForward");
        walkBID = Animator.StringToHash("WalkBack");
        backDashID = Animator.StringToHash("Backdash");
        jumpID = Animator.StringToHash("Jump");
        landID = Animator.StringToHash("Land");
        lowGuardID = Animator.StringToHash("LowGuard");
        highGuardID = Animator.StringToHash("HighGuard");
        airGuardID = Animator.StringToHash("AirGuard");
        wallStickID = Animator.StringToHash("WallStick");
        groundBounceID = Animator.StringToHash("GroundBounce");
        wallBounceID = Animator.StringToHash("WallBounce");
        yVeloID = Animator.StringToHash("VertVelocity");
    }

    // Update is called once per frame
    void Update()
    {
        currentState = anim.GetCurrentAnimatorStateInfo(0);
        anim.SetFloat(yVeloID, rb.velocity.y);
        // rotate based on position relative to opponent
        if (facingRight)
            transform.eulerAngles = Vector3.zero;
        else
            transform.eulerAngles = new Vector3(0, 180, 0);
        if (Actions.acceptMove && Actions.standing)
        {
            if (opponent.transform.position.x < transform.position.x - .1f)
                facingRight = false;
            else if (opponent.transform.position.x > transform.position.x + .1f)
                facingRight = true;
        }

        if (!Actions.airborne)
        {
            pushBox.offset = pushCenter;
            pushBox.size = pushSize;
        }
        else if (currentState.IsName("WallStick"))
        {
            pushBox.offset = pushCenter;
            pushBox.size = pushSize;
        }
        else
        {
            anim.SetBool(crouchID, false);
            anim.SetBool(runID, false);

            pushBox.offset = airPushCenter;
            pushBox.size = airPushSize;

            if(rb.velocity.y < 0)
            {
                pushBox.offset = pushCenter;
                pushBox.size = pushSize;
            }

            if (jumps == 0)
                jumps = 1;
        }

        pushTrigger.offset = new Vector2(pushBox.offset.x, pushBox.offset.y);
        pushTrigger.size = new Vector2(pushBox.size.x, pushBox.size.y + .3f);

        if (transform.position.y < minPosY)
        {
            transform.position = new Vector3(transform.position.x, minPosY, transform.position.z);
            Actions.airborne = false;
        }
        if (playing)
        {
            if ((MaxInput.GetAxis(Vertical) < 0 && Actions.acceptMove && Actions.standing) || (anim.GetBool(crouchID) && !Actions.acceptMove && Actions.standing))
                anim.SetBool(crouchID, true);
            else
                anim.SetBool(crouchID, false);

            if (Actions.acceptMove && ((MaxInput.GetAxis(Horizontal) > 0 && facingRight) || (MaxInput.GetAxis(Horizontal) < 0 && !facingRight)) && !Actions.airborne && !anim.GetBool(runID))
            {
                anim.SetBool(walkFID, true);
            }
            else
            {
                anim.SetBool(walkFID, false);
            }

            if (Actions.acceptMove && ((MaxInput.GetAxis(Horizontal) < 0 && facingRight) || (MaxInput.GetAxis(Horizontal) > 0 && !facingRight)) && !Actions.airborne && !backDash)
            {
                anim.SetBool(walkBID, true);
            }
            else
                anim.SetBool(walkBID, false);

            DoubleTapActions();

            if (Actions.jumpCancel && jumps < maxJumps && MaxInput.GetAxis(Vertical) > 0 && !vertAxisInUse)
            {
                Actions.EnableAll();
                pushBox.isTrigger = true;
                jumps++;
                jumping = .4f;


                if (MaxInput.GetAxis(Horizontal) > 0 && !anim.GetBool(runID))
                    jumpRight = true;
                else if (MaxInput.GetAxis(Horizontal) < 0 && !anim.GetBool(runID))
                    jumpLeft = true;

                vertAxisInUse = true;
            }
        }
        else
        {
            Actions.DisableAll();
            Actions.DisableBlitz();
        }

        if (MaxInput.GetAxisRaw(Vertical) == 0)
        {
            vertAxisInUse = false;
        }
        if (MaxInput.GetAxisRaw(Horizontal) == 0)
        {
            horiAxisInUse = false;
            justDefenseTime = 4;
        }

        if (horiAxisInUse)
            justDefenseTime--;

        Blocking();
        WallStick();
    }

    void FixedUpdate()
    {
        //walking
        if(anim.GetBool(walkFID) && !anim.GetBool(crouchID))
        {
            rb.velocity = new Vector2(walkSpeed, rb.velocity.y);

            if(!facingRight)
                rb.velocity *= new Vector2(-1, 1);
        }
        else if (anim.GetBool(walkBID) && !anim.GetBool(crouchID))
        {
            rb.velocity = new Vector2(walkBackSpeed, rb.velocity.y);

            if(facingRight)
                rb.velocity *= new Vector2(-1, 1);
        }

        //Jump logic
        if (jumping > 0 && anim.GetFloat("AnimSpeed") > 0 && HitDetect.hitStun == 0 && HitDetect.blockStun == 0)
        {
            anim.SetTrigger(jumpID);
            Actions.TurnAroundCheck();
            if(Actions.airborne)
                rb.velocity = new Vector2(0, 0);
            else
                rb.velocity = new Vector2(.5f * rb.velocity.x, 0);
            Actions.airborne = true;

            if (MaxInput.GetAxis(Horizontal) > 0 && !anim.GetBool(runID))
                jumpRight = true;
            else if (MaxInput.GetAxis(Horizontal) < 0 && !anim.GetBool(runID))
                jumpLeft = true;

            if(!anim.GetBool(runID))
                rb.velocity = new Vector2(0, rb.velocity.y);

            if (jumpRight)
            {
                rb.velocity = new Vector2(0, rb.velocity.y);
                rb.AddForce(new Vector2(.3f * jumpPower, 0), ForceMode2D.Impulse);
            }
            else if(jumpLeft)
            {
                rb.velocity = new Vector2(0, rb.velocity.y);
                rb.AddForce(new Vector2(-.3f * jumpPower, 0), ForceMode2D.Impulse);
            }

            rb.AddForce(new Vector2(0, jumpPower), ForceMode2D.Impulse);
            
            jumping = 0;
            jumpRight = false;
            jumpLeft = false;
            jumped = true;
        }
        else
        {
            if (HitDetect.hitStun > 0 && HitDetect.blockStun > 0)
            {
                jumping = 0;
                jumpRight = false;
                jumpLeft = false;
            }
            else
                jumping -= Time.fixedDeltaTime;
        }

        //Run acceleration
        if(anim.GetBool(runID) && ((MaxInput.GetAxis(Horizontal) > 0 && facingRight) || (MaxInput.GetAxis(Horizontal) < 0 && !facingRight)) && !anim.GetBool(crouchID))
        {
            if (facingRight && rb.velocity.x < walkSpeed)
                rb.velocity = new Vector2(walkSpeed, rb.velocity.y);
            else if (!facingRight && rb.velocity.x > -walkSpeed)
                rb.velocity = new Vector2(-walkSpeed, rb.velocity.y);

            if (Actions.blitzed > 0)
            {
                if (facingRight)
                    rb.AddForce(new Vector2(runAcceleration, 0), ForceMode2D.Impulse);
                else
                    rb.AddForce(new Vector2(-runAcceleration, 0), ForceMode2D.Impulse);
            }
            else
            {
                if (facingRight)
                    rb.AddForce(new Vector2(runAcceleration, 0), ForceMode2D.Impulse);
                else
                    rb.AddForce(new Vector2(-runAcceleration, 0), ForceMode2D.Impulse);
            }
        }
        else
        { 
            anim.SetBool(runID, false);
        }

        Brake();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Floor"))
        {
            //bouncing against the ground
            if (Actions.groundBounce)
            {
                anim.SetTrigger(groundBounceID);
                rb.velocity = Vector2.zero;
                if (facingRight)
                {
                    HitDetect.KnockBack = new Vector2(-1f, 3.3f);
                }
                else
                {
                    HitDetect.KnockBack = new Vector2(1f, 3.3f);
                }
                Actions.groundBounce = false;
            }
            //for landing on the ground if the opponent is not supposed to bounce
            else
            {
                Actions.airborne = false;
                jumps = 0;
                pushBox.isTrigger = false;
            }
        }
        else if (collision.collider.CompareTag("Wall"))
        {
            WallStates();
            if (!opponent.GetComponent<MovementHandler>().hittingWall)
                hittingWall = true;
        }
        else if (collision.collider.CompareTag("Player") && collision.collider.gameObject.transform.parent.name == opponent.gameObject.transform.parent.name)
        {
            MovementHandler opponentMove = opponent.GetComponent<MovementHandler>();
            if (Actions.airborne && !opponentMove.Actions.airborne)
            {
                pushBox.isTrigger = true;
            }
        }
        else if (collision.collider.CompareTag("Bound") && Actions.wallBounce && Actions.wallStick == 0)
        {
            WallStates();
        }

    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Floor"))
        {
            if (Actions.groundBounce && rb.velocity.y <= 0 && !Actions.standing)
            {
                anim.SetTrigger(groundBounceID);
                Actions.groundBounce = false;
            }
            else
            {
                if (!Actions.standing && Actions.blitzed > 0 && !Actions.groundBounce)
                    Actions.blitzed = 0;
                if (HitDetect.hitStun == 0 && Actions.airborne)
                    Actions.airborne = false;
                if (Actions.standing)
                    jumps = 0;
            }
        }
        else if (collision.collider.CompareTag("Wall"))
        {
            WallStates();
        }
        else if (collision.collider.CompareTag("Player") && collision.collider.gameObject.transform.parent.name == opponent.gameObject.transform.parent.name)
        {
            MovementHandler opponentMove = opponent.GetComponent<MovementHandler>();
            if (Actions.airborne && !opponentMove.Actions.airborne)
            {
                pushBox.isTrigger = true;
                if (rb.velocity.y <= 0 && opponentMove.hittingWall)
                {
                    if (opponentMove.facingRight)
                        transform.position = new Vector3(opponent.position.x + (.5f * opponentMove.pushBox.size.x + .51f * pushBox.size.x), transform.position.y, transform.position.z);
                    else
                        transform.position = new Vector3(opponent.position.x - (.5f * opponentMove.pushBox.size.x + .51f * pushBox.size.x), transform.position.y, transform.position.z);
                }
                if (((opponent.position.x > transform.position.x && facingRight) || (opponent.position.x < transform.position.x && !facingRight)) && rb.velocity.y < 0)
                {
                    rb.velocity = new Vector2(0, rb.velocity.y);
                }
            }
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Floor"))
        {
            Actions.airborne = true;
            if (jumps > maxJumps - 1)
                jumps--;
        }
        else if (collision.collider.CompareTag("Wall"))
        {
            hittingWall = false;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player") && other.gameObject.transform.parent.name == opponent.gameObject.transform.parent.name)
        {
            //keeps characters from intersecting and occupying the same space
                MovementHandler opponentMove = opponent.GetComponent<MovementHandler>();
            if (!Actions.airborne && opponentMove.Actions.airborne && !hittingWall && opponentMove.rb.velocity.y < 0)
            {
                if (opponent.position.x > transform.position.x)
                {
                    if (transform.position.x + .5f * pushBox.size.x > opponent.position.x - .5f * opponentMove.pushBox.size.x)
                    {
                        float translateX = (transform.position.x + .51f * pushBox.size.x) - (opponent.position.x - .51f * opponentMove.pushBox.size.x);
                        transform.position = new Vector3(transform.position.x - translateX, transform.position.y, transform.position.z);
                    }
                    else
                        rb.AddForce(new Vector2(-.25f, 0), ForceMode2D.Impulse);
                }
                else if (opponent.position.x < transform.position.x)
                {
                    if (transform.position.x - .5f * pushBox.size.x < opponent.position.x + .5f * opponentMove.pushBox.size.x)
                    {
                        float translateX = (transform.position.x - .51f * pushBox.size.x) - (opponent.position.x + .51f * opponentMove.pushBox.size.x);
                        transform.position = new Vector3(transform.position.x - translateX, transform.position.y, transform.position.z);
                    }
                    else
                        rb.AddForce(new Vector2(.25f, 0), ForceMode2D.Impulse);
                }
                else if (facingRight)
                {
                    if (transform.position.x - .5f * pushBox.size.x < opponent.position.x + .5f * opponentMove.pushBox.size.x)
                    {
                        float translateX = (transform.position.x - .51f * pushBox.size.x) - (opponent.position.x + .51f * opponentMove.pushBox.size.x);
                        transform.position = new Vector3(transform.position.x - translateX, transform.position.y, transform.position.z);
                    }
                    else
                        rb.AddForce(new Vector2(.25f, 0), ForceMode2D.Impulse);
                }
                else
                {
                    if (transform.position.x + .5f * pushBox.size.x > opponent.position.x - .5f * opponentMove.pushBox.size.x)
                    {
                        float translateX = (transform.position.x + .51f * pushBox.size.x) - (opponent.position.x - .51f * opponentMove.pushBox.size.x);
                        transform.position = new Vector3(transform.position.x - translateX, transform.position.y, transform.position.z);
                    }
                    else
                        rb.AddForce(new Vector2(-.25f, 0), ForceMode2D.Impulse);
                }
            }
            else if (Actions.airborne && opponentMove.Actions.airborne && ((HitDetect.OpponentDetector.hitStun == 0 && HitDetect.hitStun == 0)||(HitDetect.OpponentDetector.hitStun != 0 && HitDetect.hitStun == 0)))
            {
                if (Mathf.Abs(transform.position.x - opponent.position.x) < pushBox.size.x && opponentMove.hittingWall)
                {
                    if (facingRight)
                        transform.position = new Vector3(opponent.position.x - (.51f * pushBox.size.x + .5f * opponentMove.pushBox.size.x), transform.position.y, transform.position.z);
                    else
                        transform.position = new Vector3(opponent.position.x + (.51f * pushBox.size.x + .5f * opponentMove.pushBox.size.x), transform.position.y, transform.position.z);
                }
            }
        }
        else if (other.CompareTag("Floor"))
        {
            //keep character from falling through the floor
            pushBox.isTrigger = false;
        }
        else if (other.CompareTag("Wall"))
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
            if (!opponent.GetComponent<MovementHandler>().hittingWall)
            {
                hittingWall = true;
                pushBox.isTrigger = false;
            }
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if(other.CompareTag("Player") && other.gameObject.transform.parent.name == opponent.gameObject.transform.parent.name)
        {
            MovementHandler opponentMove = opponent.GetComponent<MovementHandler>();
            if (transform.position.y > opponent.position.y && (transform.position.y - opponent.position.y) > (.5f * pushBox.size.y + .5f * opponentMove.pushBox.size.y))
            {
                pushBox.isTrigger = true;
            }
            else if (Actions.airborne && opponentMove.Actions.airborne && HitDetect.OpponentDetector.hitStun == 0 && HitDetect.hitStun == 0)
            {
                pushBox.isTrigger = false;
                if (transform.position.y < opponent.position.y)
                {
                    if (transform.position.x < opponent.position.x - .05f)
                        transform.position = new Vector3(opponent.position.x - (.51f * pushBox.size.x + .5f * opponentMove.pushBox.size.x), transform.position.y, transform.position.z);
                    else if (transform.position.x > opponent.position.x + .05f)
                        transform.position = new Vector3(opponent.position.x + (.51f * pushBox.size.x + .5f * opponentMove.pushBox.size.x), transform.position.y, transform.position.z);
                    else if (facingRight)
                        transform.position = new Vector3(opponent.position.x - (.51f * pushBox.size.x + .5f * opponentMove.pushBox.size.x), transform.position.y, transform.position.z);
                    else
                        transform.position = new Vector3(opponent.position.x + (.51f * pushBox.size.x + .5f * opponentMove.pushBox.size.x), transform.position.y, transform.position.z);
                    pushBox.isTrigger = true;
                }
            }
            else if (Actions.airborne && !opponentMove.Actions.airborne && hittingWall && transform.position.y - opponent.position.y < .5f * pushBox.size.y)
            {
                pushBox.isTrigger = false;
            }
            else if(Actions.airborne && !opponentMove.Actions.airborne)
            {
                pushBox.isTrigger = true;
                if (rb.velocity.y <= 0 && opponentMove.hittingWall)
                {
                   if (opponentMove.facingRight)
                        transform.position = new Vector3(opponent.position.x + (.5f * opponentMove.pushBox.size.x + .51f * pushBox.size.x), transform.position.y, transform.position.z);
                   else
                        transform.position = new Vector3(opponent.position.x - (.5f * opponentMove.pushBox.size.x + .51f * pushBox.size.x), transform.position.y, transform.position.z);
                }
                if (((opponent.position.x > transform.position.x && facingRight)|| (opponent.position.x < transform.position.x && !facingRight)) && rb.velocity.y < 0)
                {
                    rb.velocity = new Vector2(0, rb.velocity.y);
                }
            }
            else
            {
                pushBox.isTrigger = false;
            }

            if (opponentMove.hittingWall && rb.velocity.y < 0)
            {
                if(opponentMove.facingRight)
                    rb.AddForce(new Vector2(.1f, 0), ForceMode2D.Impulse);
                else
                    rb.AddForce(new Vector2(-.1f, 0), ForceMode2D.Impulse);
            }
        }
        else if (other.CompareTag("Wall"))
        {
            hittingWall = true;
            rb.velocity = new Vector2(0, rb.velocity.y);
        }
        else if (other.CompareTag("Bound"))
        {
            if (Actions.wallBounce && HitDetect.hitStun > 0 && transform.position.y > 1.3f)
            {
                Actions.groundBounce = false;
                Actions.wallBounce = false;
                rb.velocity = Vector2.zero;
                if (facingRight)
                {
                    HitDetect.KnockBack = new Vector2(.6f, 1.5f);
                }
                else
                {
                    HitDetect.KnockBack = new Vector2(-.6f, 1.5f);
                }
                anim.SetTrigger(wallBounceID);
                //set off wall hit effect
            }
            else
            {
                rb.velocity = new Vector2(0, rb.velocity.y);
                pushBox.isTrigger = false;
            }
        }
        else if (other.CompareTag("Floor"))
        {
            pushBox.isTrigger = false;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if(other.CompareTag("Player") && other.gameObject.transform.parent.name == opponent.gameObject.transform.parent.name)
        {
            pushBox.isTrigger = false;
        }
        else if (other.CompareTag("Wall"))
        {
            hittingWall = false;
        }
    }

    void DoubleTapActions()
    {
        //double tap backward direction for backdash
        if (((MaxInput.GetAxisRaw(Horizontal) == -1 && facingRight) || (MaxInput.GetAxisRaw(Horizontal) == 1 && !facingRight)) && !Actions.airborne)
        {
            if(!horiAxisInUse)
            {
                if (Actions.acceptMove && Actions.standing && inputTime > 0 && !anim.GetBool(crouchID) && dashButtonCount == 1/*Number of Taps Minus One*/)
                {
                    //Has double tapped
                    anim.SetTrigger(backDashID);
                    backDash = true;
                    dashButtonCount = 0;
                }
                else
                {
                    inputTime = 0.25f; 
                    dashButtonCount += 1;
                }
                horiAxisInUse = true;
            }
        }
        //double tap forward to run
        if (((MaxInput.GetAxisRaw(Horizontal) == 1 && facingRight) || (MaxInput.GetAxisRaw(Horizontal) == -1 && !facingRight)))
        {
            if (!horiAxisInUse)
            {
                if (Actions.acceptMove && runInputTime > 0 && !anim.GetBool(crouchID) && buttonCount == 1/*Number of Taps Minus One*/)
                {
                    //Has double tapped
                    anim.SetBool(runID, true);
                    buttonCount = 0;
                }
                else
                {
                    runInputTime = 0.25f; 
                    buttonCount += 1;
                }
                horiAxisInUse = true;
            }   
        }

        if (MaxInput.GetButton(L3) && MaxInput.GetAxis(Horizontal) != 0)
        {
            if (((MaxInput.GetAxis(Horizontal) > 0 && facingRight) || (MaxInput.GetAxis(Horizontal) < 0 && !facingRight)) && !Actions.airborne)
            {
                if (Actions.acceptMove && !anim.GetBool(crouchID))
                {
                    anim.SetBool(runID, true);
                    buttonCount = 0;
                }
            }
        }

        if (inputTime > 0)
        {
            inputTime -= Time.deltaTime;
        }
        else
        {
            dashButtonCount = 0;
        }
        if(runInputTime > 0)
        {
            runInputTime -= Time.deltaTime;
        }
        else
        {
            buttonCount = 0;
        }
    }

    void Brake()
    {
        if(anim.GetBool(runID) || (Actions.airborne && Actions.acceptMove))
        {
            //keeps character movement speed from reaching insane levels
            if (Actions.blitzed > 0)
            {
                if (rb.velocity.x > maxVelocity/2 && facingRight)
                    rb.velocity = new Vector2(maxVelocity/2, rb.velocity.y);
                else if (rb.velocity.x < -maxVelocity/2)
                    rb.velocity = new Vector2(-maxVelocity/2, rb.velocity.y);
            }
            else
            {
                if (rb.velocity.x > maxVelocity && facingRight)
                    rb.velocity = new Vector2(maxVelocity, rb.velocity.y);
                else if (rb.velocity.x < -maxVelocity)
                    rb.velocity = new Vector2(-maxVelocity, rb.velocity.y);
            } 
        }
        else if (!Actions.airborne && !Actions.acceptMove)
        {
            //friction for on the ground while attacking or getting hit, uses character's walking back speed to determine deceleration
            if (rb.velocity.x > .85f)
            {
                rb.AddForce(new Vector2(-.12f * walkBackSpeed, 0), ForceMode2D.Impulse);
            }
            else if(rb.velocity.x < -.8f)
            {
                rb.AddForce(new Vector2(.12f * walkBackSpeed, 0), ForceMode2D.Impulse);
            }
            else
            {
                rb.velocity = new Vector2(0, rb.velocity.y);
            }
        }
        else if (!Actions.airborne)
        {
            //friction for on the ground, uses character's walking back speed to determine deceleration
            if(rb.velocity.x > .5f)
            {
                rb.AddForce(new Vector2(-.2f * walkBackSpeed, 0), ForceMode2D.Impulse);
            }
            else if(rb.velocity.x < -.5f)
            {
                rb.AddForce(new Vector2(.2f * walkBackSpeed, 0), ForceMode2D.Impulse);
            }
            else
            {
                rb.velocity = new Vector2(0, rb.velocity.y);
            }
        }
        
    }

    void Blocking()
    {
        if (Actions.acceptGuard)
        {
            //change guard type based on state and directions being held
            if (opponent.position.x > transform.position.x) //for when the character is on the left
            {
                if(MaxInput.GetAxis(Horizontal) < 0 && MaxInput.GetAxis(Vertical) < 0 && !Actions.airborne)
                {
                    anim.SetBool(lowGuardID, true);
                    anim.SetBool(highGuardID, false);
                    anim.SetBool(airGuardID, false);
                }
                else if(MaxInput.GetAxis(Horizontal) < 0 && !Actions.airborne)
                {
                    anim.SetBool(lowGuardID, false);
                    anim.SetBool(highGuardID, true);
                    anim.SetBool(airGuardID, false);
                }
                else if (MaxInput.GetAxis(Horizontal) < 0 && Actions.airborne)
                {
                    anim.SetBool(lowGuardID, false);
                    anim.SetBool(highGuardID, false);
                    anim.SetBool(airGuardID, true);
                }
                else
                {
                    anim.SetBool(lowGuardID, false);
                    anim.SetBool(highGuardID, false);
                    anim.SetBool(airGuardID, false);
                }
            }
            else if (opponent.position.x < transform.position.x) //for when the character is on the right
            {
                if(MaxInput.GetAxis(Horizontal) > 0 && MaxInput.GetAxis(Vertical) < 0 && !Actions.airborne)
                {
                    anim.SetBool(lowGuardID, true);
                    anim.SetBool(highGuardID, false);
                    anim.SetBool(airGuardID, false);
                }
                else if(MaxInput.GetAxis(Horizontal) > 0 && !Actions.airborne)
                {
                    anim.SetBool(lowGuardID, false);
                    anim.SetBool(highGuardID, true);
                    anim.SetBool(airGuardID, false);
                }
                else if (MaxInput.GetAxis(Horizontal) > 0 && Actions.airborne)
                {
                    anim.SetBool(lowGuardID, false);
                    anim.SetBool(highGuardID, false);
                    anim.SetBool(airGuardID, true);
                }
                else
                {
                    anim.SetBool(lowGuardID, false);
                    anim.SetBool(highGuardID, false);
                    anim.SetBool(airGuardID, false);
                }
            }
            if (opponent.GetComponent<MovementHandler>().Actions.attacking && Vector3.Distance(transform.position, opponent.position) <= 2 && HitDetect.blockStun == 0)
                anim.SetBool("ForceBlock", true);
            else
                anim.SetBool("ForceBlock", false);
        }
        else
        {
            anim.SetBool(lowGuardID, false);
            anim.SetBool(highGuardID, false);
            anim.SetBool(airGuardID, false);
        }

        if (MaxInput.GetAxis(Horizontal) == 0)
        {
            anim.SetBool(lowGuardID, false);
            anim.SetBool(highGuardID, false);
            anim.SetBool(airGuardID, false);
        }
    }

    void WallStates()
    {
        hittingWall = true;
        //makes characters stick against wall and slowly fall
        if (Actions.wallStick > 0 && HitDetect.hitStun > 0 && rb.velocity.y > 0 && transform.position.y > 1.3f)
        {
            Actions.groundBounce = false;
            rb.velocity = new Vector2(0, rb.velocity.y);
            anim.SetBool(wallStickID, true);
            //set off wall hit effect
        }
        else if (Actions.wallBounce && HitDetect.hitStun > 0  && transform.position.y > 1.3f)
        {
            Actions.groundBounce = false;
            rb.velocity = Vector2.zero;
            if (facingRight)
            {
                HitDetect.KnockBack = new Vector2(.6f, 1.5f);
            }
            else
            {
                HitDetect.KnockBack = new Vector2(-.6f, 1.5f);
            }
            anim.SetTrigger(wallBounceID);
            //set off wall hit effect
        }
    }

    void WallStick()
    {
        if(currentState.IsName("WallStick"))
        {
            if(wallStickTimer == 0)
            {
                anim.SetBool(wallStickID, false);

            }
            wallStickTimer--;
        }
        else if (Actions.blitzed % 2 == 0)
        {
            wallStickTimer = 36;
        }
    }
}
