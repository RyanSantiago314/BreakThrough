﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitboxDHA : MonoBehaviour
{
    public BoxCollider2D hit1;
    public BoxCollider2D hit2;
    public BoxCollider2D hit3;
    public BoxCollider2D hit4;
    public BoxCollider2D hit5;
    public BoxCollider2D hit6;
    public BoxCollider2D hit7;

    public HitDetector HitDetect;

    int hitStunLv1 = 10;
    int hitStunLv2 = 14;
    int hitStunLv3 = 17;
    int hitStunLv4 = 19;

    int hitStopLv1 = 9;
    int hitStopLv2 = 10;
    int hitStopLv3 = 11;
    int hitStopLv4 = 12;

    public int sinCharge; // variable used to charge/enhance break attacks, unique to DHA


    void Start()
    {
        ClearHitBox();
    }

    void Update()
    {
        if (HitDetect.hit)
        {
            ClearHitBox();
            HitDetect.hit = false;
        }
    }

    void ClearHitBox()
    {
        hit1.enabled = false;
        hit2.enabled = false;
        hit3.enabled = false;
        hit4.enabled = false;
        hit5.enabled = false;
        hit6.enabled = false;
        hit7.enabled = false;
        HitDetect.potentialHitStun = 0;
        HitDetect.potentialHitStop = 0;
        HitDetect.potentialKnockBack = Vector2.zero;
        HitDetect.potentialAirKnockBack = Vector2.zero;
        HitDetect.grab = false;
        HitDetect.piercing = false;
        HitDetect.launch = false;
        HitDetect.crumple = false;
        HitDetect.sweep = false;
        HitDetect.forceCrouch = false;
        HitDetect.allowWallStick = false;
        HitDetect.allowGroundBounce = false;
        HitDetect.shatter = false;
    }

    //push damage values, knockback, and proration to hitdetector from hitbox events
    void StandingLHitBox()
    {
        ClearHitBox();
        hit1.enabled = true;
        hit1.offset = new Vector2(.48f, .15f);
        hit1.size = new Vector2(.55f, .15f);
        HitDetect.damage = 25;
        HitDetect.armorDamage = 0;
        HitDetect.durabilityDamage = 50;
        HitDetect.potentialHitStun = hitStunLv1;
        HitDetect.potentialHitStop = hitStopLv1;
        HitDetect.potentialKnockBack = new Vector2(1.2f, 0);
        HitDetect.initialProration = .7f;
        HitDetect.attackLevel = 0;
        HitDetect.guard = "Mid";

        HitDetect.allowLight = true;
        HitDetect.allowMedium = true;
        HitDetect.allowHeavy = true;
        HitDetect.allowSpecial = true;
        HitDetect.allowSuper = true;
    }

    void CrouchingLHitBox()
    {
        ClearHitBox();
        hit1.enabled = true;
        hit1.offset = new Vector2(.5f, -.23f);
        hit1.size = new Vector2(.55f, .15f);
        HitDetect.damage = 20;
        HitDetect.armorDamage = 0;
        HitDetect.durabilityDamage = 50;
        HitDetect.potentialHitStun = hitStunLv1;
        HitDetect.potentialHitStop = hitStopLv1;
        HitDetect.potentialKnockBack = new Vector2(1f, 0);
        HitDetect.initialProration = .65f;
        HitDetect.attackLevel = 0;
        HitDetect.guard = "Mid";

        HitDetect.allowLight = true;
        HitDetect.allowMedium = true;
        HitDetect.allowHeavy = true;
        HitDetect.allowSpecial = true;
        HitDetect.allowSuper = true;
    }

    void JumpLHitBox()
    {
        ClearHitBox();
        hit1.enabled = true;
        hit1.offset = new Vector2(.55f, .3f);
        hit1.size = new Vector2(.57f, .17f);
        HitDetect.damage = 24;
        HitDetect.armorDamage = 0;
        HitDetect.durabilityDamage = 50;
        HitDetect.potentialHitStun = hitStunLv1;
        HitDetect.potentialHitStop = hitStopLv1;
        HitDetect.potentialKnockBack = new Vector2(1f, 0);
        HitDetect.potentialAirKnockBack = new Vector2(.7f, 2f);
        HitDetect.initialProration = .7f;
        HitDetect.attackLevel = 0;
        HitDetect.guard = "Overhead";

        HitDetect.allowLight = true;
        HitDetect.allowMedium = true;
        HitDetect.allowHeavy = true;
        HitDetect.allowSpecial = true;
        HitDetect.allowSuper = true;
        HitDetect.jumpCancellable = true;
    }


    void StandingMHitBox()
    {
        ClearHitBox();
        hit1.enabled = true;
        hit1.offset = new Vector2(.61f, -.41f);
        hit1.size = new Vector2(.81f, .271f);
        HitDetect.damage = 36;
        HitDetect.armorDamage = 0;
        HitDetect.durabilityDamage = 100;
        HitDetect.potentialHitStun = hitStunLv1;
        HitDetect.potentialHitStop = hitStopLv1;
        HitDetect.potentialKnockBack = new Vector2(1.5f, 0);
        HitDetect.initialProration = .75f;
        HitDetect.attackLevel = 1;
        HitDetect.guard = "Low";

        HitDetect.allowMedium = true;
        HitDetect.allowHeavy = true;
        HitDetect.allowBreak = true;
        HitDetect.allowSpecial = true;
        HitDetect.allowSuper = true;
    }

    void CrouchingMHitBox()
    {
        ClearHitBox();
        hit1.enabled = true;
        hit1.offset = new Vector2(.68f, -.78f);
        hit1.size = new Vector2(.92f, .087f);
        HitDetect.damage = 40;
        HitDetect.armorDamage = 0;
        HitDetect.durabilityDamage = 100;
        HitDetect.potentialHitStun = hitStunLv1;
        HitDetect.potentialHitStop = hitStopLv1;
        HitDetect.potentialKnockBack = new Vector2(1.3f, 0);
        HitDetect.initialProration = .85f;
        HitDetect.attackLevel = 1;
        HitDetect.guard = "Low";

        HitDetect.allowMedium = true;
        HitDetect.allowHeavy = true;
        HitDetect.allowBreak = true;
        HitDetect.allowSpecial = true;
        HitDetect.allowSuper = true;
    }

    void JumpMHitBoxFirst()
    {
        ClearHitBox();
        hit1.enabled = true;
        hit2.enabled = true;
        hit1.offset = new Vector2(.51f, -.12f);
        hit1.size = new Vector2(.73f, .26f);
        hit2.offset = new Vector2(1.02f, 0f);
        hit2.size = new Vector2(.295f, .32f);
        HitDetect.damage = 42;
        HitDetect.armorDamage = 0;
        HitDetect.durabilityDamage = 100;
        HitDetect.potentialHitStun = hitStunLv2;
        HitDetect.potentialHitStop = hitStopLv2;
        HitDetect.potentialKnockBack = new Vector2(1.2f, 0);
        HitDetect.potentialAirKnockBack = new Vector2(1.2f, 1.5f);
        HitDetect.initialProration = .85f;
        HitDetect.attackLevel = 1;
        HitDetect.guard = "Overhead";

        HitDetect.allowLight = true;
        HitDetect.allowMedium = true;
        HitDetect.allowHeavy = true;
        HitDetect.allowBreak = true;
        HitDetect.allowSpecial = true;
        HitDetect.allowSuper = true;
        HitDetect.jumpCancellable = true;
    }

    void JumpMHitBoxSecond()
    {
        ClearHitBox();
        hit1.enabled = true;
        hit1.offset = new Vector2(-.44f, -.06f);
        hit1.size = new Vector2(.76f, .33f);

        HitDetect.damage = 44;
        HitDetect.armorDamage = 0;
        HitDetect.durabilityDamage = 100;
        HitDetect.potentialHitStun = hitStunLv3;
        HitDetect.potentialHitStop = hitStopLv2;
        HitDetect.potentialKnockBack = new Vector2(1.3f, 0);
        HitDetect.potentialAirKnockBack = new Vector2(1.2f, 2f);
        HitDetect.initialProration = .85f;
        HitDetect.attackLevel = 1;
        HitDetect.guard = "Overhead";

        HitDetect.allowLight = true;
        HitDetect.allowMedium = true;
        HitDetect.allowHeavy = true;
        HitDetect.allowBreak = true;
        HitDetect.allowSpecial = true;
        HitDetect.allowSuper = true;
    }

    void StandingHHitBoxFirst()
    {
        ClearHitBox();
        hit1.enabled = true;
        hit1.offset = new Vector2(.75f, -.23f);
        hit1.size = new Vector2(.73f, .32f);
        HitDetect.damage = 27;
        HitDetect.armorDamage = 0;
        HitDetect.durabilityDamage = 100;
        HitDetect.potentialHitStun = hitStunLv3;
        HitDetect.potentialHitStop = hitStopLv1;
        HitDetect.potentialKnockBack = new Vector2(1f, 0);
        HitDetect.potentialAirKnockBack = new Vector2(1f, 1.5f);
        HitDetect.initialProration = .9f;
        HitDetect.attackLevel = 2;
        HitDetect.guard = "Mid";

        HitDetect.allowLight = true;
        HitDetect.allowHeavy = true;
        HitDetect.allowBreak = true;
        HitDetect.allowSpecial = true;
        HitDetect.allowSuper = true;
        HitDetect.jumpCancellable = true;
    }

    void StandingHHitBoxSecond()
    {
        ClearHitBox();
        hit1.enabled = true;
        hit1.offset = new Vector2(.86f, .22f);
        hit1.size = new Vector2(.95f, .33f);
        HitDetect.damage = 42;
        HitDetect.armorDamage = 0;
        HitDetect.durabilityDamage = 100;
        HitDetect.potentialHitStun = hitStunLv3;
        HitDetect.potentialHitStop = hitStopLv2;
        HitDetect.potentialKnockBack = new Vector2(1.3f, 0);
        HitDetect.potentialAirKnockBack = new Vector2(1.3f, 1.5f);
        HitDetect.initialProration = 1;
        HitDetect.attackLevel = 2;
        HitDetect.guard = "Mid";

        HitDetect.allowHeavy = true;
        HitDetect.allowBreak = true;
        HitDetect.allowSpecial = true;
        HitDetect.allowSuper = true;
        HitDetect.jumpCancellable = true;
    }

    void CrouchingHHitBox()
    {
        ClearHitBox();
        hit1.enabled = true;
        hit2.enabled = true;
        hit3.enabled = true;
        hit1.offset = new Vector2(.98f, .82f);
        hit1.size = new Vector2(.51f, .775f);
        hit2.offset = new Vector2(1.06f, .38f);
        hit2.size = new Vector2(.5f, 1.1f);
        hit3.offset = new Vector2(.92f, -.4f);
        hit3.size = new Vector2(.63f, .6f);
        HitDetect.damage = 62;
        HitDetect.armorDamage = 0;
        HitDetect.durabilityDamage = 100;
        HitDetect.potentialHitStun = hitStunLv4;
        HitDetect.potentialHitStop = hitStopLv2;
        HitDetect.potentialKnockBack = new Vector2(.5f, 4f);
        HitDetect.potentialAirKnockBack = new Vector2(.5f, 3.5f);
        HitDetect.initialProration = .9f;
        HitDetect.attackLevel = 2;
        HitDetect.guard = "Mid";

        HitDetect.launch = true;
        HitDetect.allowBreak = true;
        HitDetect.allowSpecial = true;
        HitDetect.allowSuper = true;
        HitDetect.jumpCancellable = true;
    }

    void JumpHFirstHitBox()
    {
        ClearHitBox();
        hit1.enabled = true;
        hit2.enabled = true;
        hit3.enabled = true;
        hit4.enabled = true;
        hit5.enabled = true;

        hit1.offset = new Vector2(.59f, -.09f);
        hit1.size = new Vector2(.52f, .26f);
        hit2.offset = new Vector2(.91f, .027f);
        hit2.size = new Vector2(.27f, .26f);
        hit3.offset = new Vector2(.88f, .265f);
        hit3.size = new Vector2(.275f, .228f);
        hit4.offset = new Vector2(.76f, .41f);
        hit4.size = new Vector2(.22f, .08f);
        hit5.offset = new Vector2(.56f, .49f);
        hit5.size = new Vector2(.37f, .087f);

        HitDetect.damage = 60;
        HitDetect.armorDamage = 0;
        HitDetect.durabilityDamage = 100;
        HitDetect.potentialHitStun = hitStunLv3;
        HitDetect.potentialHitStop = 8;
        HitDetect.potentialKnockBack = new Vector2(1.2f, 0f);
        HitDetect.potentialAirKnockBack = new Vector2(.8f, 1.5f);
        HitDetect.initialProration = .8f;
        HitDetect.attackLevel = 2;
        HitDetect.guard = "Overhead";

        HitDetect.allowHeavy = true;
        HitDetect.allowBreak = true;
        HitDetect.allowSpecial = true;
        HitDetect.allowSuper = true;
        HitDetect.jumpCancellable = true;
    }

    void JumpHSecondHitBox()
    {
        ClearHitBox();
        hit1.enabled = true;
        hit2.enabled = true;
        hit3.enabled = true;
        hit4.enabled = true;
        hit5.enabled = true;

        hit1.offset = new Vector2(.01f, -.55f);
        hit1.size = new Vector2(.54f, .36f);
        hit2.offset = new Vector2(.43f, -.48f);
        hit2.size = new Vector2(.31f, .39f);
        hit3.offset = new Vector2(.656f, -.29f);
        hit3.size = new Vector2(.2f, .45f);
        hit4.offset = new Vector2(.74f, -.03f);
        hit4.size = new Vector2(.23f, .73f);
        hit5.offset = new Vector2(.7f, .49f);
        hit5.size = new Vector2(.23f, .33f);

        HitDetect.damage = 40;
        HitDetect.armorDamage = 0;
        HitDetect.durabilityDamage = 100;
        HitDetect.potentialHitStun = hitStunLv3;
        HitDetect.potentialHitStop = 8;
        HitDetect.potentialKnockBack = new Vector2(1.2f, 0f);
        HitDetect.potentialAirKnockBack = new Vector2(.6f, 1f);
        HitDetect.initialProration = .85f;
        HitDetect.attackLevel = 2;
        HitDetect.guard = "Overhead";

        HitDetect.allowHeavy = true;
        HitDetect.allowBreak = true;
        HitDetect.allowSpecial = true;
        HitDetect.allowSuper = true;
    }

    void JumpHThirdHitBox()
    {
        ClearHitBox();
        hit1.enabled = true;
        hit2.enabled = true;
        hit3.enabled = true;
        hit4.enabled = true;
        hit5.enabled = true;

        hit1.offset = new Vector2(.22f, -.21f);
        hit1.size = new Vector2(.25f, .17f);
        hit2.offset = new Vector2(.43f, -.29f);
        hit2.size = new Vector2(.31f, .246f);
        hit3.offset = new Vector2(.78f, -.3f);
        hit3.size = new Vector2(.44f, .4f);
        hit4.offset = new Vector2(.835f, -.031f);
        hit4.size = new Vector2(.2f, .17f);
        hit5.offset = new Vector2(.77f, .14f);
        hit5.size = new Vector2(.16f, .186f);

        HitDetect.damage = 40;
        HitDetect.armorDamage = 0;
        HitDetect.durabilityDamage = 100;
        HitDetect.potentialHitStun = hitStunLv3;
        HitDetect.potentialHitStop = 8;
        HitDetect.potentialKnockBack = new Vector2(1.2f, 0f);
        HitDetect.potentialAirKnockBack = new Vector2(.6f, 1f);
        HitDetect.initialProration = .9f;
        HitDetect.attackLevel = 2;
        HitDetect.guard = "Overhead";

        HitDetect.allowHeavy = true;
        HitDetect.allowBreak = true;
        HitDetect.allowSpecial = true;
        HitDetect.allowSuper = true;
    }

    void JumpHFourthHitBox()
    {
        ClearHitBox();
        hit1.enabled = true;
        hit2.enabled = true;
        hit3.enabled = true;
        hit4.enabled = true;
        hit5.enabled = true;
        hit6.enabled = true;
        hit7.enabled = true;

        hit1.offset = new Vector2(.14f, 1.16f);
        hit1.size = new Vector2(.37f, .33f);
        hit2.offset = new Vector2(.45f, 1.07f);
        hit2.size = new Vector2(.35f, .35f);
        hit3.offset = new Vector2(.64f, .92f);
        hit3.size = new Vector2(.16f, .41f);
        hit4.offset = new Vector2(.76f, .7f);
        hit4.size = new Vector2(.21f, .42f);
        hit5.offset = new Vector2(.85f, .37f);
        hit5.size = new Vector2(.27f, .64f);
        hit6.offset = new Vector2(.72f, -.04f);
        hit6.size = new Vector2(.19f, .23f);
        hit7.offset = new Vector2(.59f, -.17f);
        hit7.size = new Vector2(.14f, .15f);

        HitDetect.damage = 70;
        HitDetect.armorDamage = 1;
        HitDetect.durabilityDamage = 100;
        HitDetect.potentialHitStun = hitStunLv4;
        HitDetect.potentialHitStop = hitStopLv3;
        HitDetect.potentialKnockBack = new Vector2(1f, 3f);
        HitDetect.potentialAirKnockBack = new Vector2(1f, 3f);
        HitDetect.initialProration = 1.2f;
        HitDetect.attackLevel = 3;
        HitDetect.guard = "Overhead";

        HitDetect.launch = true;
        HitDetect.allowSuper = true;
        HitDetect.jumpCancellable = true;
    }

    void BreakCharge()
    {
        sinCharge++;
        HitDetect.anim.SetInteger("SinCharge", sinCharge);
    }

    void StandingBHitBox()
    {
        ClearHitBox();
        hit1.enabled = true;
        hit2.enabled = true;
        hit3.enabled = true;
        hit4.enabled = true;
        hit5.enabled = true;
        hit6.enabled = true;
        hit7.enabled = true;

        hit1.offset = new Vector2(.64f, -.61f);
        hit1.size = new Vector2(.67f, .56f);
        hit2.offset = new Vector2(.81f, -.09f);
        hit2.size = new Vector2(.55f, .6f);
        hit3.offset = new Vector2(.7f, .33f);
        hit3.size = new Vector2(.62f, .25f);
        hit4.offset = new Vector2(.55f, .54f);
        hit4.size = new Vector2(.57f, .21f);
        hit5.offset = new Vector2(0f, .7f);
        hit5.size = new Vector2(1f, .3f);
        hit6.offset = new Vector2(-.55f, .57f);
        hit6.size = new Vector2(.22f, .22f);
        hit7.offset = new Vector2(-.7f, .45f);
        hit7.size = new Vector2(.1f, .12f);

        HitDetect.damage = 85 + (10 * sinCharge);
        HitDetect.armorDamage = sinCharge;
        HitDetect.durabilityDamage = 100;
        HitDetect.potentialKnockBack = new Vector2(1.5f, 0f);
        HitDetect.potentialHitStun = hitStunLv3 + sinCharge;
        HitDetect.potentialHitStop = hitStopLv3 + sinCharge;
        HitDetect.initialProration = 1;
        HitDetect.attackLevel = 3;
        HitDetect.guard = "Mid";

        if(sinCharge > 1)
            HitDetect.forceCrouch = true;
        HitDetect.allowSpecial = true;
        HitDetect.allowSuper = true;
        HitDetect.jumpCancellable = true;

        sinCharge = 0;
        HitDetect.anim.SetInteger("SinCharge", sinCharge);
    }

    void StandingBFullChargeHitBox()
    {
        ClearHitBox();
        hit1.enabled = true;
        hit2.enabled = true;
        hit3.enabled = true;
        hit4.enabled = true;
        hit5.enabled = true;

        hit1.offset = new Vector2(.72f, -.62f);
        hit1.size = new Vector2(.72f, .54f);
        hit2.offset = new Vector2(.92f, -.18f);
        hit2.size = new Vector2(.78f, .8f);
        hit3.offset = new Vector2(.81f, .48f);
        hit3.size = new Vector2(.68f, .56f);
        hit4.offset = new Vector2(-.1f, .82f);
        hit4.size = new Vector2(1.83f, .53f);
        hit5.offset = new Vector2(-.86f, .35f);
        hit5.size = new Vector2(.41f, .44f);

        HitDetect.damage = 150;
        HitDetect.armorDamage = 0;
        HitDetect.durabilityDamage = 0;
        HitDetect.potentialKnockBack = new Vector2(2f, 2f);
        HitDetect.potentialAirKnockBack = new Vector2(2f, -1f);
        HitDetect.potentialHitStun = 60;
        HitDetect.potentialHitStop = hitStopLv4;
        HitDetect.initialProration = 1.1f;
        HitDetect.attackLevel = 5;
        HitDetect.guard = "Mid";

        HitDetect.allowGroundBounce = true;
        HitDetect.allowWallStick = true;
        HitDetect.shatter = true;
        HitDetect.allowSpecial = true;
        HitDetect.allowSuper = true;
        HitDetect.jumpCancellable = true;

        sinCharge = 0;
        HitDetect.anim.SetInteger("SinCharge", sinCharge);
        HitDetect.anim.SetBool("5B", false);
    }

    void CrouchBHitBox()
    {
        ClearHitBox();
        hit1.enabled = true;

        hit1.offset = new Vector2(.91f, -.72f);
        hit1.size = new Vector2(1f, .43f);

        HitDetect.damage = 80 + (10 * sinCharge);
        HitDetect.armorDamage = sinCharge;
        HitDetect.durabilityDamage = 100;
        HitDetect.potentialKnockBack = new Vector2(1f, 1.5f);
        HitDetect.potentialHitStun = 60;
        HitDetect.potentialHitStop = hitStopLv2 + sinCharge;
        HitDetect.initialProration = 1;
        HitDetect.attackLevel = 3;
        HitDetect.guard = "Low";

        HitDetect.sweep = true;
        HitDetect.allowSpecial = true;
        HitDetect.allowSuper = true;

        sinCharge = 0;
        HitDetect.anim.SetInteger("SinCharge", sinCharge);
    }

    void CrouchingBFullChargeHitBox()
    {
        ClearHitBox();
        hit1.enabled = true;
        hit2.enabled = true;
        hit3.enabled = true;

        hit1.offset = new Vector2(.93f, -.76f);
        hit1.size = new Vector2(1.33f, .33f);
        hit2.offset = new Vector2(1.2f, -.41f);
        hit2.size = new Vector2(.88f, .44f);
        hit3.offset = new Vector2(1.08f, -.02f);
        hit3.size = new Vector2(.417f, .35f);

        HitDetect.damage = 130;
        HitDetect.armorDamage = 0;
        HitDetect.durabilityDamage = 0;
        HitDetect.potentialKnockBack = new Vector2(2f, 0f);
        HitDetect.potentialAirKnockBack = new Vector2(2f, 2f);
        HitDetect.potentialHitStun = 60;
        HitDetect.potentialHitStop = hitStopLv4;
        HitDetect.initialProration = 1f;
        HitDetect.attackLevel = 5;
        HitDetect.guard = "Low";

        HitDetect.shatter = true;
        HitDetect.crumple = true;
        HitDetect.allowSpecial = true;
        HitDetect.allowSuper = true;
        HitDetect.jumpCancellable = true;

        sinCharge = 0;
        HitDetect.anim.SetInteger("SinCharge", sinCharge);
        HitDetect.anim.SetBool("5B", false);
    }

    void JumpBHitBox()
    {
        ClearHitBox();
        hit1.enabled = true;
        hit2.enabled = true;
        hit3.enabled = true;
        hit4.enabled = true;
        hit5.enabled = true;

        hit1.offset = new Vector2(-.1f, -.21f);
        hit1.size = new Vector2(.71f, .74f);
        hit2.offset = new Vector2(.26f, .09f);
        hit2.size = new Vector2(.57f, .73f);
        hit3.offset = new Vector2(.47f, .32f);
        hit3.size = new Vector2(.51f, .74f);
        hit4.offset = new Vector2(.61f, .665f);
        hit4.size = new Vector2(.395f, 1.16f);
        hit5.offset = new Vector2(.82f, .71f);
        hit5.size = new Vector2(.21f, .76f);

        HitDetect.damage = 90;
        HitDetect.armorDamage = 1;
        HitDetect.durabilityDamage = 100;
        HitDetect.potentialKnockBack = new Vector2(2f, 0f);
        HitDetect.potentialAirKnockBack = new Vector2(2f, 2f);
        HitDetect.potentialHitStun = 25;
        HitDetect.potentialHitStop = hitStopLv4;
        HitDetect.initialProration = .9f;
        HitDetect.attackLevel = 3;
        HitDetect.guard = "Overhead";

        HitDetect.allowSpecial = true;
        HitDetect.allowSuper = true;
    }
}
