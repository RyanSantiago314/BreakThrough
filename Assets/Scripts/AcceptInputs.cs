using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AcceptInputs : MonoBehaviour
{
    //class that dictates what the character is currently capable of doing
    public bool acceptMove = true;
    public bool acceptGuard = true;
    public bool acceptLight = true;
    public bool acceptMedium = true;
    public bool acceptHeavy = true;
    public bool acceptBreak = true;
    public bool acceptSpecial = true;
    public bool acceptSuper = true;
    public bool jumpCancel = true;
    public bool blitzCancel = true;
    public bool airborne = false;
    public bool standing = true;

    public bool usingSpecial = false;
    public bool shattered = false;
    public int wallStick = 0;
    public bool groundBounce = false;

    public bool grabbed = false;
    public bool throwTech = false;

    public float gravScale = 1f;
    public int comboHits = 0;

    public Animator anim;
    public MovementHandler Move;
    public CharacterProperties CharProp;
    static int airID;
    static int standID;

    float zPos;
    float zPosHit;

    void Start()
    {
        airID = Animator.StringToHash("Airborne");
        standID = Animator.StringToHash("Standing");
        zPos = transform.position.z;
        zPosHit = zPos + .01f;
    }

    void Update()
    {
        //moves the defending character slightly farther back to allow visibility on attacking character
        if (comboHits > 0)
            transform.position = new Vector3(transform.position.x, transform.position.y, zPosHit);
        else
            transform.position = new Vector3(transform.position.x, transform.position.y, zPos);


        //change character properties based on current animation state
        if(airborne || anim.GetCurrentAnimatorStateInfo(0).IsName("SweepHit"))
            standing = false;

        if(anim.GetCurrentAnimatorStateInfo(0).IsName("IdleStand")||anim.GetCurrentAnimatorStateInfo(0).IsName("IdleCrouch")||anim.GetCurrentAnimatorStateInfo(0).IsName("StandUp")||
            anim.GetCurrentAnimatorStateInfo(0).IsName("FUGetup")||anim.GetCurrentAnimatorStateInfo(0).IsName("FDGetup"))
            standing = true;

        if(wallStick == 0)
            anim.SetBool("WallStick", false);

        anim.SetBool(airID, airborne);
        anim.SetBool(standID, standing);

        //increase gravScale based on hitting certain numbers with comboHits
        //keep track of hits in combo for damage and gravity scaling
        comboHits = Move.OpponentProperties.HitDetect.comboCount;
    }

    public void DisableAll()
    {
        acceptMove = false;
        acceptGuard = false;
        acceptLight = false;
        acceptMedium = false;
        acceptHeavy = false;
        acceptBreak = false;        
        acceptSpecial = false;
        acceptSuper = false;
        jumpCancel = false;
        CharProp.HitDetect.allowLight = false;
        CharProp.HitDetect.allowMedium = false;
        CharProp.HitDetect.allowHeavy = false;
        CharProp.HitDetect.allowBreak = false;
        CharProp.HitDetect.allowSpecial = false;
        CharProp.HitDetect.allowSuper = false;
        CharProp.HitDetect.jumpCancellable = false;
    }
    public void EnableAll()
    {
        acceptMove = true;
        acceptGuard = true;
        acceptLight = true;
        acceptMedium = true;
        acceptHeavy = true;
        acceptBreak = true;
        acceptSpecial = true;
        acceptSuper = true;
        jumpCancel = true;
        blitzCancel = true;
        gravScale = 1f;
        Move.HitDetect.hitStun = 0;
        Move.HitDetect.blockStun = 0;
        shattered = false;
        wallStick = 0;
        groundBounce = false;
    }
    public void DisableMovement()
    {
        acceptMove = false;
        acceptGuard = false;
    }

    public void DisableBlitz()
    {
        blitzCancel = false;
    }

    public void EnableHeavy()
    {
        acceptHeavy = true;
    }

    public void Launch()
    {
        airborne = true;
        standing = false;
    }

    public void TurnAroundCheck()
    {
        if(Move.opponent.transform.position.x < transform.position.x - .1f)
            Move.facingRight = false;
        else if (Move.opponent.transform.position.x > transform.position.x + .1f)
            Move.facingRight = true;
    }

    public void Guard()
    {
        acceptMove = false;
        acceptGuard = true;
        acceptLight = false;
        acceptMedium = false;
        acceptHeavy = false;
        acceptBreak = false;        
        acceptSpecial = false;
        acceptSuper = false;
        jumpCancel = false;
    }

    public void Advance(float x)
    {
        Move.rb.velocity = Vector2.zero;
        if (Move.facingRight)
            Move.rb.AddForce(new Vector2(x, 0), ForceMode2D.Impulse);
        else
            Move.rb.AddForce(new Vector2(-x, 0), ForceMode2D.Impulse);
    }

    public void Rise(float y)
    {
        Move.rb.AddForce(new Vector2(0, y), ForceMode2D.Impulse);
    }

    public void Recover()
    {
        Move.rb.velocity = new Vector2(.2f * Move.rb.velocity.x, 0);

        if(Input.GetAxis(Move.Horizontal) > 0)
            Move.rb.AddForce(new Vector2(.5f * Move.backDashForce, .5f*Move.jumpPower), ForceMode2D.Impulse);
        else if (Input.GetAxis(Move.Horizontal) < 0)
            Move.rb.AddForce(new Vector2(-.5f * Move.backDashForce, .5f*Move.jumpPower), ForceMode2D.Impulse);
        else if (Input.GetAxis(Move.Vertical) < 0 && transform.position.y > 1.5f)
            Move.rb.AddForce(new Vector2(0, -.5f*Move.jumpPower), ForceMode2D.Impulse);
        else
            Move.rb.AddForce(new Vector2(0, .5f * Move.jumpPower), ForceMode2D.Impulse);
    }

    public void Dash()
    {
        if (Move.facingRight)
            Move.rb.AddForce(new Vector2(-Move.backDashForce, .4f*Move.backDashForce), ForceMode2D.Impulse);
        else
            Move.rb.AddForce(new Vector2(Move.backDashForce, .4f*Move.backDashForce), ForceMode2D.Impulse);
            Move.backDash = false;
    }

    public void Throwing()
    {
        throwTech = true;
        DisableAll();
    }

    public void ThrowTechFalse()
    {
        throwTech = false;
    }

    public void GetUp()
    {
        if (CharProp.comboTimer < 200)
        {
            CharProp.armor = 2;
            CharProp.durability = 100;
        }
        else
        {
            for (float i = CharProp.comboTimer; i >= 100; i -= 100)
            {
                CharProp.armor++;
            }
            CharProp.durability = (int)CharProp.comboTimer % 100;
        }
    }
}
