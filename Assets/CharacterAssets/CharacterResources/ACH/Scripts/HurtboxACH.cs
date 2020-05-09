using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HurtboxACH : MonoBehaviour
{
    public BoxCollider2D head;
    public BoxCollider2D body;
    public BoxCollider2D legs1;
    public BoxCollider2D legs2;
    public BoxCollider2D misc1;
    public BoxCollider2D misc2;

    Vector2 headStandSize;
    Vector2 headStandOffset;
    Vector2 bodyStandSize;
    Vector2 bodyStandOffset;
    Vector2 legsStandSize;
    Vector2 legsStandOffset;

    Vector2 headCrouchSize;
    Vector2 headCrouchOffset;
    Vector2 bodyCrouchSize;
    Vector2 bodyCrouchOffset;
    Vector2 legsCrouchSize;
    Vector2 legsCrouchOffset;

    CharacterProperties CharProp;

    void Start()
    {
        head.enabled = false;
        body.enabled = false;
        legs1.enabled = false;
        legs2.enabled = false;
        misc1.enabled = false;
        misc2.enabled = false;

        headStandSize = new Vector2(.33f, .31f);
        headStandOffset = new Vector2(.07f, .68f);
        bodyStandSize = new Vector2(.512f, .55f);
        bodyStandOffset = new Vector2(.041f, .24f);
        legsStandSize = new Vector2(.55f, .87f);
        legsStandOffset = new Vector2(-.03f, -.465f);

        headCrouchOffset = new Vector2(.06f, .345f);
        bodyCrouchSize = new Vector2(.47f, .49f);
        bodyCrouchOffset = new Vector2(.01f, -.03f);
        legsCrouchSize = new Vector2(.91f, .64f);
        legsCrouchOffset = new Vector2(-.034f, -.53f);

        CharProp = transform.GetComponentInParent<CharacterProperties>();
    }

    void Update()
    {
        if ((CharProp.HitDetect.currentState.IsName("FUKnockdown") || CharProp.HitDetect.currentState.IsName("FDKnockdown")) && 
            !CharProp.HitDetect.Actions.hiInvincible && !CharProp.HitDetect.Actions.lowInvincible)
        {
            Knockdown();
        }
    }

    public void Standing()
    {
        ClearHurtBox();
        head.enabled = true;
        body.enabled = true;
        legs1.enabled = true;
        legs2.enabled = true;
        misc1.enabled = true;
        misc2.enabled = true;

        head.offset = new Vector2(.02f, .6f);
        head.size = new Vector2(.33f, .31f);
        body.offset = new Vector2(.024f, .2f);
        body.size = new Vector2(.39f, .497f);
        legs1.offset = new Vector2(-.047f, -.23f);
        legs1.size = new Vector2(.83f, .42f);
        legs2.offset = new Vector2(.125f, -.67f);
        legs2.size = new Vector2(1f, .5f);
        misc1.offset = new Vector2(-.28f, .54f);
        misc1.size = new Vector2(.395f, .21f);
        misc2.offset = new Vector2(.246f, .075f);
        misc2.size = new Vector2(.175f, .54f);
    }

    public void Crouching()
    {
        ClearHurtBox();
        head.enabled = true;
        body.enabled = true;
        legs1.enabled = true;
        misc1.enabled = true;
        misc2.enabled = true;

        head.offset = headCrouchOffset;
        head.size = new Vector2(.21f, .29f);
        body.offset = bodyCrouchOffset;
        body.size = bodyCrouchSize;
        legs1.offset = legsCrouchOffset;
        legs1.size = legsCrouchSize;
        misc1.offset = new Vector2(-.3f, -.1f);
        misc1.size = new Vector2(.15f, .36f);
        misc2.offset = new Vector2(.39f, -.12f);
        misc2.size = new Vector2(.375f, .175f);
    }

    public void Knockdown()
    {
        ClearHurtBox();
        misc1.enabled = true;

        misc1.offset = new Vector2(.04f, -.72f);
        misc1.size = new Vector2(2f, .34f);
    }

    public void Invincible()
    {
        CharProp.HitDetect.Actions.InvincibleHigh();
        CharProp.HitDetect.Actions.InvincibleLow();
    }

    public void ClearHurtBox()
    {
        head.enabled = false;
        body.enabled = false;
        legs1.enabled = false;
        legs2.enabled = false;
        misc1.enabled = false;
        misc2.enabled = false;
    }

    public void Walk()
    {
        ClearHurtBox();
        head.enabled = true;
        body.enabled = true;
        legs1.enabled = true;

        head.offset = new Vector2(.04f, .65f);
        head.size = headStandSize;
        body.offset = new Vector2(.01f, .22f);
        body.size = new Vector2(.5f, .5f);
        legs1.offset = new Vector2(0, -.45f);
        legs1.size = new Vector2(.65f, .9f);
    }

    public void BackDash()
    {
        ClearHurtBox();
        head.enabled = true;
        body.enabled = true;
        legs1.enabled = true;
        legs2.enabled = true;

        head.offset = new Vector2(-.04f, .57f);
        head.size = new Vector2(.19f, .3f);
        body.offset = new Vector2(-.08f, .25f);
        body.size = new Vector2(.3f, .53f);
        legs1.offset = new Vector2(.05f, -.22f);
        legs1.size = new Vector2(.62f, .53f);
        legs2.offset = new Vector2(.44f, -.58f);
        legs2.size = new Vector2(.43f, .39f);
    }

    public void JumpStart()
    {
        ClearHurtBox();
        head.enabled = true;
        body.enabled = true;
        legs1.enabled = true;
        legs2.enabled = true;

        head.offset = new Vector2(0f, .95f);
        head.size = new Vector2(.575f, .59f);
        body.offset = new Vector2(-.022f, .41f);
        body.size = new Vector2(.35f, .53f);
        legs1.offset = new Vector2(0f, -.1f);
        legs1.size = new Vector2(.44f, .69f);
        legs2.offset = new Vector2(-0.04f, -.695f);
        legs2.size = new Vector2(.23f, .53f);
    }

    public void SweepHit()
    {
        ClearHurtBox();
        head.enabled = true;
        body.enabled = true;
        legs1.enabled = true;
        legs2.enabled = true;

        head.offset = new Vector2(.025f, .89f);
        head.size = new Vector2(.22f, .33f);
        body.offset = new Vector2(-.09f, .48f);
        body.size = new Vector2(.43f, .585f);
        legs1.offset = new Vector2(-0.18f, .04f);
        legs1.size = new Vector2(.43f, .51f);
        legs2.offset = new Vector2(-0.52f, -.17f);
        legs2.size = new Vector2(.89f, .3f);
    }

    public void FallForward()
    {
        ClearHurtBox();
        head.enabled = true;
        body.enabled = true;
        legs1.enabled = true;
        legs2.enabled = true;

        head.offset = new Vector2(.71f, -.13f);
        head.size = new Vector2(.2f, .235f);
        body.offset = new Vector2(.36f, -.26f);
        body.size = new Vector2(.6f, .5f);
        legs1.offset = new Vector2(-0.14f, -.125f);
        legs1.size = new Vector2(.5f, .84f);
        legs2.offset = new Vector2(-0.59f, -.17f);
        legs2.size = new Vector2(.44f, .39f);
    }

    public void Jump()
    {
        ClearHurtBox();
        head.enabled = true;
        body.enabled = true;
        legs1.enabled = true;
        misc1.enabled = true;
        misc2.enabled = true;

        head.offset = new Vector2(-.035f, .59f);
        head.size = new Vector2(.24f, .285f);
        body.offset = new Vector2(-.12f, .24f);
        body.size = new Vector2(.39f, .45f);
        legs1.offset = new Vector2(-.07f, -.175f);
        legs1.size = new Vector2(.585f, .54f);
        misc1.offset = new Vector2(-.39f, .365f);
        misc1.size = new Vector2(.32f, .15f);
        misc2.offset = new Vector2(.19f, .16f);
        misc2.size = new Vector2(.4f, .2f);
    }

    public void HitAir()
    {
        ClearHurtBox();
        head.enabled = true;
        body.enabled = true;
        legs1.enabled = true;
        legs2.enabled = true;
        misc1.enabled = true;

        head.offset = new Vector2(-.125f, .58f);
        head.size = new Vector2(.195f, .28f);
        body.offset = new Vector2(-.22f, .24f);
        body.size = new Vector2(.375f, .465f);
        legs1.offset = new Vector2(0f, -.2f);
        legs1.size = new Vector2(.63f, .44f);
        legs2.offset = new Vector2(.29f, -.5f);
        legs2.size = new Vector2(.73f, .26f);
        misc1.offset = new Vector2(.06f, .28f);
        misc1.size = new Vector2(.57f, .3f);
    }

    public void LaunchFall()
    {
        ClearHurtBox();
        head.enabled = true;
        body.enabled = true;
        legs1.enabled = true;
        legs2.enabled = true;

        head.offset = new Vector2(-.066f, -.38f);
        head.size = new Vector2(.23f, .3f);
        body.offset = new Vector2(.07f, .043f);
        body.size = new Vector2(.51f, .645f);
        legs1.offset = new Vector2(.185f, .68f);
        legs1.size = new Vector2(.456f, .69f);
        legs2.offset = new Vector2(.36f, 1.05f);
        legs2.size = new Vector2(.25f, .6f);
    }

    public void Deflected()
    {
        ClearHurtBox();
        head.enabled = true;
        body.enabled = true;
        legs1.enabled = true;
        legs2.enabled = true;
        misc1.enabled = true;

        head.offset = new Vector2(-.5f, .63f);
        head.size = new Vector2(.625f, .33f);
        body.offset = new Vector2(-.3f, .26f);
        body.size = new Vector2(.455f, .45f);
        legs1.offset = new Vector2(-.07f, -.14f);
        legs1.size = new Vector2(.66f, .42f);
        legs2.offset = new Vector2(.04f, -.58f);
        legs2.size = new Vector2(.86f, .67f);
        misc1.offset = new Vector2(-.13f, .66f);
        misc1.size = new Vector2(.24f, .5f);
    }

    public void WallStick()
    {
        ClearHurtBox();
        head.enabled = true;
        body.enabled = true;
        legs1.enabled = true;
        legs2.enabled = true;

        head.offset = new Vector2(-.18f, .88f);
        head.size = new Vector2(.185f, .25f);
        body.offset = new Vector2(-.15f, .5f);
        body.size = new Vector2(.385f, .5f);
        legs1.offset = new Vector2(-.16f, -.01f);
        legs1.size = new Vector2(.48f, .64f);
        legs2.offset = new Vector2(-.2f, -.5f);
        legs2.size = new Vector2(.47f, .51f);
    }


    public void Run()
    {
        ClearHurtBox();
        head.enabled = true;
        body.enabled = true;
        legs1.enabled = true;
        misc1.enabled = true;

        head.offset = new Vector2(.33f, .38f);
        head.size = new Vector2(.32f, .26f);
        body.offset = new Vector2(.05f, .165f);
        body.size = new Vector2(.52f, .54f);
        legs1.offset = new Vector2(0f, -0.5f);
        legs1.size = new Vector2(.78f, .78f);
        misc1.offset = new Vector2(0.3f, 0.1f);
        misc1.size = new Vector2(.6f, .24f);
    }

    public void Brake()
    {
        ClearHurtBox();
        head.enabled = true;
        body.enabled = true;
        legs1.enabled = true;
        misc1.enabled = true;

        head.offset = new Vector2(.42f, .06f);
        head.size = new Vector2(.19f, .26f);
        body.offset = new Vector2(.4f, -.25f);
        body.size = new Vector2(.32f, .47f);
        legs1.offset = new Vector2(-.02f, -0.67f);
        legs1.size = new Vector2(1.43f, .44f);
        misc1.offset = new Vector2(0.03f, 0.13f);
        misc1.size = new Vector2(.6f, .3f);
    }

    public void StandLight()
    {
        ClearHurtBox();
        head.enabled = true;
        body.enabled = true;
        legs1.enabled = true;
        legs2.enabled = true;
        misc1.enabled = true;

        head.offset = new Vector2(.04f, .55f);
        head.size = new Vector2(.21f, .32f);
        body.offset = new Vector2(-.14f, .2f);
        body.size = new Vector2(.52f, .56f);
        legs1.offset = new Vector2(0f, -.14f);
        legs1.size = new Vector2(.89f, .42f);
        legs2.offset = new Vector2(.25f, -.58f);
        legs2.size = new Vector2(1.6f, .67f);
        misc1.offset = new Vector2(.42f, .03f);
        misc1.size = new Vector2(.72f, .53f);
    }

    public void CrouchingLight()
    {
        ClearHurtBox();
        head.enabled = true;
        body.enabled = true;
        legs1.enabled = true;
        legs2.enabled = true;

        head.offset = new Vector2(-.08f, .19f);
        head.size = new Vector2(.19f , .29f);
        body.offset = new Vector2(-.1f, -.1f);
        body.size = new Vector2(.32f, .56f);
        legs1.offset = new Vector2(.12f, -0.6f);
        legs1.size = new Vector2(.945f, .51f);
        legs2.offset = new Vector2(.31f, -0.75f);
        legs2.size = new Vector2(1.3f, .33f);
    }

    public void JumpLight()
    {
        ClearHurtBox();
        head.enabled = true;
        body.enabled = true;
        legs1.enabled = true;
        misc1.enabled = true;

        head.offset = new Vector2(.05f, .5f);
        head.size = new Vector2(.35f, .32f);
        body.offset = new Vector2(-.06f, .21f);
        body.size = new Vector2(.69f, .5f);
        legs1.offset = new Vector2(-.12f, -.11f);
        legs1.size = new Vector2(.6f, .5f);
        misc1.offset = new Vector2(.48f, 0.3f);
        misc1.size = new Vector2(.7f, .15f);
    }

    public void StandMedStartup()
    {
        ClearHurtBox();
        head.enabled = true;
        body.enabled = true;
        legs1.enabled = true;
        misc1.enabled = true;

        head.offset = new Vector2(-.3f, .63f);
        head.size = new Vector2(.2f, .27f);
        body.offset = new Vector2(-.26f, .325f);
        body.size = new Vector2(.54f, .5f);
        legs1.offset = new Vector2(-.2f, -.35f);
        legs1.size = new Vector2(.57f, .92f);
        misc1.offset = new Vector2(.39f, -.12f);
        misc1.size = new Vector2(.375f, .175f);
    }

    public void StandMedActive1()
    {
        ClearHurtBox();
        head.enabled = true;
        body.enabled = true;
        legs1.enabled = true;
        legs2.enabled = true;
        misc1.enabled = true;

        head.offset = new Vector2(-.38f, .54f);
        head.size = new Vector2(.2f, .27f);
        body.offset = new Vector2(-.26f, .325f);
        body.size = new Vector2(.54f, .5f);
        legs1.offset = new Vector2(-.24f, -.35f);
        legs1.size = new Vector2(.49f, .92f);
        legs2.offset = new Vector2(.27f, -.17f);
        legs2.size = new Vector2(.66f, .28f);
        misc1.offset = Vector2.zero;
        misc1.size = new Vector2(.45f, .48f);
    }

    public void StandMedActive2()
    {
        ClearHurtBox();
        head.enabled = true;
        body.enabled = true;
        legs1.enabled = true;
        legs2.enabled = true;
        misc1.enabled = true;

        head.offset = new Vector2(-.44f, .48f);
        head.size = new Vector2(.23f, .26f);
        body.offset = new Vector2(-.26f, .265f);
        body.size = new Vector2(.54f, .38f);
        legs1.offset = new Vector2(-.16f, -.35f);
        legs1.size = new Vector2(.265f, .92f);
        legs2.offset = new Vector2(.56f, .07f);
        legs2.size = new Vector2(1.24f, .425f);
        misc1.offset = new Vector2(-.55f, 0f);
        misc1.size = new Vector2(.5f, .48f);
    }

    public void CrouchMed()
    {
        ClearHurtBox();
        head.enabled = true;
        body.enabled = true;
        legs1.enabled = true;
        legs2.enabled = true;

        head.offset = new Vector2(-.2f, .17f);
        head.size = new Vector2(.37f, .3f);
        body.offset = new Vector2(-.02f, -.16f);
        body.size = new Vector2(.43f, .38f);
        legs1.offset = new Vector2(.06f, -.62f);
        legs1.size = new Vector2(.88f, .6f);
        legs2.offset = new Vector2(.74f, -.68f);
        legs2.size = new Vector2(.485f, .44f);
    }

    public void JumpMedFirst()
    {
        ClearHurtBox();
        head.enabled = true;
        body.enabled = true;
        legs1.enabled = true;
        legs2.enabled = true;
        misc1.enabled = true;

        head.offset = new Vector2(-.03f, .59f);
        head.size = new Vector2(.37f, .48f);
        body.offset = new Vector2(.085f, .2f);
        body.size = new Vector2(.43f, .38f);
        legs1.offset = new Vector2(-.05f, -.28f);
        legs1.size = new Vector2(.56f, .58f);
        legs2.offset = new Vector2(.77f, 0f);
        legs2.size = new Vector2(1.08f, .25f);
        misc1.offset = new Vector2(.41f, .425f);
        misc1.size = new Vector2(.57f, .21f);
    }

    public void JumpMedActive()
    {
        ClearHurtBox();
        head.enabled = true;
        body.enabled = true;
        legs1.enabled = true;
        legs2.enabled = true;
        misc1.enabled = true;

        head.offset = new Vector2(-.03f, .54f);
        head.size = new Vector2(.37f, .38f);
        body.offset = new Vector2(.085f, .2f);
        body.size = new Vector2(.43f, .38f);
        legs1.offset = new Vector2(-.02f, -.22f);
        legs1.size = new Vector2(.62f, .7f);
        legs2.offset = new Vector2(-.525f, 0f);
        legs2.size = new Vector2(.42f, .19f);
        misc1.offset = new Vector2(.4f, .35f);
        misc1.size = new Vector2(.55f, .15f);
    }

    public void StandHeavyActive()
    {
        ClearHurtBox();
        head.enabled = true;
        body.enabled = true;
        legs1.enabled = true;
        misc1.enabled = true;

        head.offset = new Vector2(.15f, .8f);
        head.size = new Vector2(.1875f, .29f);
        body.offset = new Vector2(.12f, .39f);
        body.size = new Vector2(.43f, .57f);
        legs1.offset = new Vector2(-.1f, -.21f);
        legs1.size = new Vector2(.48f, 1.45f);
        misc1.offset = new Vector2(.73f, .5f);
        misc1.size = new Vector2(1.15f, .21f);
    }

    public void StandHeavyRecovery()
    {
        ClearHurtBox();
        head.enabled = true;
        body.enabled = true;
        legs1.enabled = true;
        legs2.enabled = true;

        head.offset = new Vector2(.17f, .47f);
        head.size = new Vector2(.19f, .27f);
        body.offset = new Vector2(.026f, .171f);
        body.size = new Vector2(.8f, .46f);
        legs1.offset = new Vector2(.18f, -.62f);
        legs1.size = new Vector2(1.06f, .61f);
        legs2.offset = new Vector2(.14f, -.19f);
        legs2.size = new Vector2(.83f, .32f);
    }

    public void CrouchHeavyActive()
    {
        ClearHurtBox();
        head.enabled = true;
        body.enabled = true;
        legs1.enabled = true;
        legs2.enabled = true;

        head.offset = new Vector2(.56f, .49f);
        head.size = new Vector2(.42f, .78f);
        body.offset = new Vector2(.45f, .12f);
        body.size = new Vector2(.8f, .41f);
        legs1.offset = new Vector2(.41f, -.26f);
        legs1.size = new Vector2(.79f, .39f);
        legs2.offset = new Vector2(.37f, -.68f);
        legs2.size = new Vector2(1.24f, .45f);
    }

    public void FHeavyFirstStartup()
    {
        ClearHurtBox();
        legs1.enabled = true;
        legs2.enabled = true;

        legs1.offset = new Vector2(-.146f, -.48f);
        legs1.size = new Vector2(.775f, .826f);
        legs2.offset = new Vector2(.278f, -.62f);
        legs2.size = new Vector2(.15f, .53f);
    }

    public void FHeavyActive()
    {
        ClearHurtBox();
        head.enabled = true;
        body.enabled = true;
        legs1.enabled = true;
        legs2.enabled = true;
        misc1.enabled = true;

        head.offset = new Vector2(.04f, .565f);
        head.size = new Vector2(.46f, .31f);
        body.offset = new Vector2(.085f, .16f);
        body.size = new Vector2(.62f, .57f);
        legs1.offset = new Vector2(-.146f, -.48f);
        legs1.size = new Vector2(.775f, .826f);
        legs2.offset = new Vector2(.278f, -.62f);
        legs2.size = new Vector2(.15f, .53f);
        misc1.offset = new Vector2(.63f, .4f);
        misc1.size = new Vector2(.57f, .25f);
    }

    public void JumpHeavyActive()
    {
        ClearHurtBox();
        head.enabled = true;
        body.enabled = true;
        legs1.enabled = true;
        legs2.enabled = true;
        misc1.enabled = true;

        head.offset = new Vector2(.28f, .4f);
        head.size = new Vector2(.3f, .295f);
        body.offset = new Vector2(-.02f, .25f);
        body.size = new Vector2(.58f, .48f);
        legs1.offset = new Vector2(.01f, -.13f);
        legs1.size = new Vector2(.67f, .39f);
        legs2.offset = new Vector2(-.46f, -.36f);
        legs2.size = new Vector2(.29f, .24f);
        misc1.offset = new Vector2(-.67f, -.53f);
        misc1.size = new Vector2(.15f, .25f);
    }

    public void StandBreakStartup()
    {
        ClearHurtBox();
        head.enabled = true;
        body.enabled = true;
        legs1.enabled = true;
        legs2.enabled = true;

        head.offset = new Vector2(0f, .71f);
        head.size = new Vector2(.34f, .38f);
        body.offset = new Vector2(.04f, .3f);
        body.size = new Vector2(.365f, .45f);
        legs1.offset = new Vector2(.14f, .02f);
        legs1.size = new Vector2(.59f, .39f);
        legs2.offset = new Vector2(-.16f, -.25f);
        legs2.size = new Vector2(.82f, .4f);
    }

    public void StandBreakActive()
    {
        ClearHurtBox();
        head.enabled = true;
        body.enabled = true;
        legs1.enabled = true;
        legs2.enabled = true;

        head.offset = new Vector2(.225f, .485f);
        head.size = new Vector2(.26f, .28f);
        body.offset = new Vector2(.14f, .17f);
        body.size = new Vector2(.57f, .5f);
        legs1.offset = new Vector2(.075f, -.25f);
        legs1.size = new Vector2(1.06f, .4f);
        legs2.offset = new Vector2(.115f, -.62f);
        legs2.size = new Vector2(1.23f, .47f);
    }

    public void CrouchBreakActive()
    {
        ClearHurtBox();
        head.enabled = true;
        body.enabled = true;
        legs1.enabled = true;
        legs2.enabled = true;

        head.offset = new Vector2(.47f, -.05f);
        head.size = new Vector2(.47f, .31f);
        body.offset = new Vector2(0.092f, -.21f);
        body.size = new Vector2(.59f, .36f);
        legs1.offset = new Vector2(.19f, -.47f);
        legs1.size = new Vector2(.93f, .17f);
        legs2.offset = new Vector2(.01f, -.72f);
        legs2.size = new Vector2(1.29f, .35f);
    }

    public void FBreak()
    {
        ClearHurtBox();
        body.enabled = true;
        legs1.enabled = true;

        body.offset = bodyStandOffset;
        body.size = bodyStandSize;
        legs1.offset = legsStandOffset;
        legs1.size = legsStandSize;
    }

    public void JumpBreakStartup()
    {
        ClearHurtBox();
        head.enabled = true;
        body.enabled = true;
        legs1.enabled = true;
        legs2.enabled = true;
        misc1.enabled = true;

        head.offset = new Vector2(.3f, .62f);
        head.size = new Vector2(.83f, .26f);
        body.offset = new Vector2(-0.07f, .32f);
        body.size = new Vector2(.515f, .33f);
        legs1.offset = new Vector2(-.165f, -.09f);
        legs1.size = new Vector2(.59f, .52f);
        legs2.offset = new Vector2(-.1f, -.5f);
        legs2.size = new Vector2(.16f, .32f);
        misc1.offset = new Vector2(-.22f, -.77f);
        misc1.size = new Vector2(.18f, .275f);
    }

    public void JumpBreakActive()
    {
        ClearHurtBox();
        head.enabled = true;
        body.enabled = true;
        legs1.enabled = true;
        legs2.enabled = true;
        misc1.enabled = true;

        head.offset = new Vector2(-.03f, .6f);
        head.size = new Vector2(.81f, .35f);
        body.offset = new Vector2(-.19f, .26f);
        body.size = new Vector2(.63f, .35f);
        legs1.offset = new Vector2(-.13f, -.11f);
        legs1.size = new Vector2(.52f, .49f);
        legs2.offset = new Vector2(-.48f, -.29f);
        legs2.size = new Vector2(.18f, .315f);
        misc1.offset = new Vector2(-.657f, -.53f);
        misc1.size = new Vector2(.18f, .29f);
    }

    public void JumpBreakRecovery()
    {
        ClearHurtBox();
        head.enabled = true;
        body.enabled = true;
        legs1.enabled = true;
        legs2.enabled = true;

        head.offset = new Vector2(-.09f, .65f);
        head.size = new Vector2(.45f, .25f);
        body.offset = new Vector2(0.07f, .41f);
        body.size = new Vector2(.65f, .32f);
        legs1.offset = new Vector2(-.13f, 0f);
        legs1.size = new Vector2(.65f, .53f);
        legs2.offset = new Vector2(-.655f, .073f);
        legs2.size = new Vector2(.41f, .22f);
    }
}
