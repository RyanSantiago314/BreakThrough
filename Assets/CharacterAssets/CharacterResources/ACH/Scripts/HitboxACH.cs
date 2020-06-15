using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitboxACH : MonoBehaviour
{
    public BoxCollider2D hit1;
    public BoxCollider2D hit2;
    public BoxCollider2D hit3;
    public BoxCollider2D hit4;
    public BoxCollider2D hit5;
    public BoxCollider2D hit6;
    public BoxCollider2D hit7;

    public HitDetector HitDetect;
    public AttackHandlerACH AttackHandler;

    int hitStunLv1 = 10;
    int hitStunLv2 = 14;
    int hitStunLv3 = 17;
    int hitStunLv4 = 19;

    int hitStopLv1 = 8;
    int hitStopLv2 = 9;
    int hitStopLv3 = 10;
    int hitStopLv4 = 11;

    public int sinCharge; // variable used to charge/enhance Genesis Assault special attack, similar to sin charge from DHA


    void Start()
    {
        AwakenBox();
    }

    void Update()
    {
        if (HitDetect.hit)
        {
            ClearHitBox();
            HitDetect.hit = false;
        }

        if (HitDetect.hitStun > 0)
        {
            ClearHitBox();
            AttackHandler.ForsythiaReticle.SetActive(false);
            sinCharge = 0;
            HitDetect.anim.SetInteger("SinCharge", sinCharge);
        }

        if (HitDetect.currentState.IsName("ForsythiaAim") && !AttackHandler.ForsythiaReticle.activeSelf)
            SummonReticle();

        AttackHandler.anim.SetInteger("Shots", AttackHandler.ForsythiaReticle.GetComponent<ProjectileProperties>().currentHits);
    }

    public void ClearHitBox()
    {
        hit1.enabled = false;
        hit2.enabled = false;
        hit3.enabled = false;
        hit4.enabled = false;
        hit5.enabled = false;
        hit6.enabled = false;
        hit7.enabled = false;
        HitDetect.potentialBlockStun = 0;
        HitDetect.potentialHitStun = 0;
        HitDetect.potentialHitStop = 0;
        HitDetect.damage = 0;
        HitDetect.armorDamage = 0;
        HitDetect.durabilityDamage = 0;
        HitDetect.potentialKnockBack = Vector2.zero;
        HitDetect.potentialAirKnockBack = Vector2.zero;
        HitDetect.forcedProration = 0;
        HitDetect.initialProration = 1;
        HitDetect.attackLevel = 0;
        HitDetect.grab = false;
        HitDetect.blitz = false;
        HitDetect.piercing = false;
        HitDetect.launch = false;
        HitDetect.crumple = false;
        HitDetect.sweep = false;
        HitDetect.forceCrouch = false;
        HitDetect.forceStand = false;
        HitDetect.turnOffEffect = false;
        HitDetect.allowWallStick = false;
        HitDetect.allowGroundBounce = false;
        HitDetect.allowWallBounce = false;
        HitDetect.shatter = false;
        HitDetect.forceShatter = false;
        HitDetect.usingSuper = false;
        HitDetect.usingSpecial = false;
        HitDetect.guardCancel = false;
        HitDetect.disableBlitz = false;
        HitDetect.slash = false;
        HitDetect.vertSlash = false;
        HitDetect.horiSlash = false;
    }

    void AwakenBox()
    {
        ClearHitBox();
        hit1.enabled = true;
        hit1.size = new Vector2(10f, 10f);
    }

    public void BlitzCancel()
    {
        if (HitDetect.OpponentDetector.hitStun > 0)
        {
            ClearHitBox();
            hit1.enabled = true;
            hit1.offset = new Vector2(.0f, .0f);
            hit1.size = new Vector2(4f, 4f);

            HitDetect.attackLevel = 0;
            HitDetect.guard = "Unblockable";

            HitDetect.blitz = true;
        }
    }

    public void SummonHCEffect()
    {
        AttackHandler.HCWave.SetActive(true);
        AttackHandler.HCWave.transform.rotation = transform.rotation;
        AttackHandler.HCWave.transform.position = transform.position;
        AttackHandler.HCWave.GetComponent<HeavenClimber>().anim.SetTrigger("Activate");
    }

    public void SummonLHSlideEffect()
    {
        AttackHandler.LHSlide.SetActive(true);
        AttackHandler.LHSlide.transform.rotation = transform.rotation;
        AttackHandler.LHSlide.transform.position = transform.position;
        AttackHandler.LHSlide.GetComponent<HeavenClimber>().anim.SetTrigger("Activate");
    }

    public void SummonLHEffect()
    {
        AttackHandler.LHWave.SetActive(true);
        AttackHandler.LHWave.transform.rotation = transform.rotation;
        AttackHandler.LHWave.transform.position = transform.position;
        AttackHandler.LHWave.GetComponent<HeavenClimber>().anim.SetTrigger("Activate");
    }

    public void SummonSFEffect()
    {
        AttackHandler.SFWave.SetActive(true);
        AttackHandler.SFWave.transform.rotation = transform.rotation;
        AttackHandler.SFWave.transform.position = transform.position;
        AttackHandler.SFWave.GetComponent<HeavenClimber>().anim.SetTrigger("Activate");
    }

    public void SummonReticle()
    {
        AttackHandler.ForsythiaReticle.SetActive(true);
        if (HitDetect.Actions.Move.facingRight)
            AttackHandler.ForsythiaReticle.transform.position = new Vector3(transform.position.x + 1.6f, transform.position.y + .25f, transform.position.z);
        else
            AttackHandler.ForsythiaReticle.transform.position = new Vector3(transform.position.x - 1.6f, transform.position.y + .25f, transform.position.z);
        AttackHandler.ForsythiaReticle.GetComponent<ForsythiaHitbox>().anim.SetTrigger("Activate");
        AttackHandler.ForsythiaReticle.GetComponent<ProjectileProperties>().currentHits = 0;
    }

    //push damage values, knockback, and proration to hitdetector from hitbox events
    void StandingLHitBox()
    {
        ClearHitBox();
        HitDetect.Actions.AttackActive();
        hit1.enabled = true;
        hit2.enabled = true;
        hit1.offset = new Vector2(1.05f, -.45f);
        hit1.size = new Vector2(1.05f, .57f);
        hit2.offset = new Vector2(.92f, -.08f);
        hit2.size = new Vector2(1f, .28f);
        HitDetect.damage = 30;
        HitDetect.armorDamage = 0;
        HitDetect.durabilityDamage = 50;
        HitDetect.potentialHitStun = hitStunLv2;
        HitDetect.potentialHitStop = hitStopLv1;
        HitDetect.potentialKnockBack = new Vector2(1.5f, 0);
        HitDetect.initialProration = .8f;
        HitDetect.attackLevel = 0;
        HitDetect.guard = "Mid";

        HitDetect.slash = true;

        HitDetect.allowLight = true;
        HitDetect.allowMedium = true;
        HitDetect.allowHeavy = true;
        HitDetect.allowSpecial = true;
        HitDetect.allowSuper = true;
        HitDetect.jumpCancellable = true;
    }

    void CrouchingLHitBox()
    {
        ClearHitBox();
        HitDetect.Actions.AttackActive();

        hit1.enabled = true;
        hit1.offset = new Vector2(.49f, -.78f);
        hit1.size = new Vector2(.95f, .25f);
        HitDetect.damage = 20;
        HitDetect.armorDamage = 0;
        HitDetect.durabilityDamage = 50;
        HitDetect.potentialHitStun = hitStunLv1;
        HitDetect.potentialHitStop = hitStopLv1;
        HitDetect.potentialKnockBack = new Vector2(1.2f, 0);
        HitDetect.initialProration = .75f;
        HitDetect.attackLevel = 0;
        HitDetect.guard = "Low";

        HitDetect.allowLight = true;
        HitDetect.allowMedium = true;
        HitDetect.allowHeavy = true;
        HitDetect.allowSpecial = true;
        HitDetect.allowSuper = true;
    }

    void JumpLHitBox()
    {
        ClearHitBox();
        HitDetect.Actions.AttackActive();

        hit1.enabled = true;
        hit2.enabled = true;
        hit1.offset = new Vector2(.53f, .114f);
        hit1.size = new Vector2(.39f, .36f);
        hit2.offset = new Vector2(.27f, .35f);
        hit2.size = new Vector2(.355f, .326f);

        HitDetect.damage = 25;
        HitDetect.armorDamage = 0;
        HitDetect.durabilityDamage = 50;
        HitDetect.potentialHitStun = hitStunLv1;
        HitDetect.potentialHitStop = hitStopLv1;
        HitDetect.potentialKnockBack = new Vector2(1f, 0);
        HitDetect.potentialAirKnockBack = new Vector2(1f, 2f);
        HitDetect.initialProration = .8f;
        HitDetect.attackLevel = 0;
        HitDetect.guard = "Overhead";

        HitDetect.allowLight = true;
        HitDetect.allowMedium = true;
        HitDetect.allowHeavy = true;
        HitDetect.allowSpecial = true;
        HitDetect.allowSuper = true;
        HitDetect.jumpCancellable = true;
    }


    void StandingMHitBox1()
    {
        ClearHitBox();
        HitDetect.Actions.AttackActive();

        hit1.enabled = true;
        hit1.offset = new Vector2(.3f, -.25f);
        hit1.size = new Vector2(.64f, .42f);
        HitDetect.damage = 20;
        HitDetect.armorDamage = 0;
        HitDetect.durabilityDamage = 50;
        HitDetect.potentialHitStun = hitStunLv2;
        HitDetect.potentialHitStop = hitStopLv1;
        HitDetect.potentialKnockBack = new Vector2(.5f, 0);
        HitDetect.potentialAirKnockBack = new Vector2(.25f, 1f);
        HitDetect.attackLevel = 1;
        HitDetect.guard = "Mid";

        HitDetect.allowLight = true;
        HitDetect.allowMedium = true;
        HitDetect.allowHeavy = true;
        HitDetect.allowBreak = true;
        HitDetect.allowSpecial = true;
        HitDetect.allowSuper = true;
    }

    void StandingMHitBox2()
    {
        ClearHitBox();
        HitDetect.Actions.AttackActive();

        hit1.enabled = true;
        hit2.enabled = true;

        hit1.offset = new Vector2(.57f, .15f);
        hit1.size = new Vector2(1.18f, .34f);
        hit2.offset = new Vector2(.53f, -.06f);
        hit2.size = new Vector2(1.07f, .2f);

        HitDetect.damage = 35;
        HitDetect.armorDamage = 0;
        HitDetect.durabilityDamage = 50;
        HitDetect.potentialHitStun = hitStunLv2;
        HitDetect.potentialHitStop = hitStopLv2;
        HitDetect.potentialKnockBack = new Vector2(1.5f, 0);
        HitDetect.potentialAirKnockBack = new Vector2(1f, 2f);
        HitDetect.initialProration = .9f;
        HitDetect.attackLevel = 1;
        HitDetect.guard = "Mid";

        HitDetect.allowMedium = true;
        HitDetect.allowHeavy = true;
        HitDetect.allowBreak = true;
        HitDetect.allowSpecial = true;
        HitDetect.allowSuper = true;
        HitDetect.jumpCancellable = true;
    }

    void CrouchingMHitBox()
    {
        ClearHitBox();
        HitDetect.Actions.AttackActive();

        hit1.enabled = true;
        hit1.offset = new Vector2(1.25f, -.11f);
        hit1.size = new Vector2(1.56f, .22f);
        HitDetect.damage = 45;
        HitDetect.armorDamage = 0;
        HitDetect.durabilityDamage = 100;
        HitDetect.potentialHitStun = hitStunLv2;
        HitDetect.potentialHitStop = hitStopLv1;
        HitDetect.potentialKnockBack = new Vector2(1.5f, 0);
        HitDetect.initialProration = .75f;
        HitDetect.attackLevel = 1;
        HitDetect.guard = "Mid";

        HitDetect.slash = true;

        HitDetect.allowMedium = true;
        HitDetect.allowHeavy = true;
        HitDetect.allowBreak = true;
        HitDetect.allowSpecial = true;
        HitDetect.allowSuper = true;
    }

    void JumpMHitBox()
    {
        ClearHitBox();
        HitDetect.Actions.AttackActive();

        hit1.enabled = true;
        hit1.offset = new Vector2(.51f, .05f);
        hit1.size = new Vector2(1.04f, .35f);
        HitDetect.damage = 45;
        HitDetect.armorDamage = 0;
        HitDetect.durabilityDamage = 100;
        HitDetect.potentialHitStun = hitStunLv2;
        HitDetect.potentialHitStop = hitStopLv2;
        HitDetect.potentialKnockBack = new Vector2(1.2f, 0);
        HitDetect.potentialAirKnockBack = new Vector2(1.5f, 2f);
        HitDetect.initialProration = 1f;
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
        hit1.offset = new Vector2(1.43f, .51f);
        hit1.size = new Vector2(2.26f, .168f);

        HitDetect.damage = 55;
        HitDetect.armorDamage = 0;
        HitDetect.durabilityDamage = 100;
        HitDetect.potentialHitStun = hitStunLv3;
        HitDetect.potentialHitStop = hitStopLv2;
        HitDetect.potentialKnockBack = new Vector2(1.3f, 0);
        HitDetect.potentialAirKnockBack = new Vector2(1.4f, 1f);
        HitDetect.initialProration = .9f;
        HitDetect.attackLevel = 2;
        HitDetect.guard = "Mid";

        HitDetect.slash = true;
        HitDetect.allowHeavy = true;
        HitDetect.allowBreak = true;
        HitDetect.allowSpecial = true;
        HitDetect.allowSuper = true;
    }

    void StandingHHitBoxSecond()
    {
        ClearHitBox();
        HitDetect.Actions.AttackActive();

        hit1.enabled = true;
        hit2.enabled = true;
        hit3.enabled = true;

        hit1.offset = new Vector2(1f, -.04f);
        hit1.size = new Vector2(1.36f, .21f);
        hit2.offset = new Vector2(1.51f, .09f);
        hit2.size = new Vector2(.95f, .18f);
        hit3.offset = new Vector2(1.86f, .24f);
        hit3.size = new Vector2(.71f, .15f);

        HitDetect.damage = 35;
        HitDetect.armorDamage = 0;
        HitDetect.durabilityDamage = 50;
        HitDetect.potentialHitStun = hitStunLv4;
        HitDetect.potentialHitStop = hitStopLv2;
        HitDetect.potentialKnockBack = new Vector2(-1.5f, 0);
        if (Mathf.Abs(HitDetect.Actions.Move.transform.position.x - HitDetect.Actions.Move.opponent.position.x) > 1.75f)
            HitDetect.potentialAirKnockBack = new Vector2(-1.25f, 1.5f);
        else
            HitDetect.potentialAirKnockBack = new Vector2(-1f, 1.5f);
        HitDetect.initialProration = .9f;
        HitDetect.forcedProration = .9f;
        HitDetect.attackLevel = 2;
        HitDetect.guard = "Mid";

        HitDetect.slash = true;
        HitDetect.allowHeavy = true;
        HitDetect.allowBreak = true;
        HitDetect.allowSpecial = true;
        HitDetect.allowSuper = true;
        HitDetect.jumpCancellable = true;
    }

    void CrouchingHHitBox()
    {
        ClearHitBox();
        HitDetect.Actions.AttackActive();

        hit1.enabled = true;
        hit2.enabled = true;
        hit3.enabled = true;
        hit1.offset = new Vector2(1.35f, .4f);
        hit1.size = new Vector2(.46f, 1.32f);
        hit2.offset = new Vector2(1.02f, .34f);
        hit2.size = new Vector2(.44f, 1.15f);
        hit3.offset = new Vector2(.54f, .51f);
        hit3.size = new Vector2(.58f, .54f);
        HitDetect.damage = 35;
        HitDetect.armorDamage = 0;
        HitDetect.durabilityDamage = 100;
        HitDetect.potentialHitStun = hitStunLv2;
        HitDetect.potentialHitStop = hitStopLv1;
        HitDetect.potentialKnockBack = new Vector2(.7f, 0f);
        HitDetect.potentialAirKnockBack = new Vector2(.3f, 2f);
        HitDetect.attackLevel = 2;
        HitDetect.guard = "Mid";

        HitDetect.vertSlash = true;
        HitDetect.allowHeavy = true;
        HitDetect.allowSpecial = true;
        HitDetect.allowSuper = true;
    }

    void CrouchingHHitBox2()
    {
        ClearHitBox();
        HitDetect.Actions.AttackActive();

        hit1.enabled = true;
        hit2.enabled = true;
        hit3.enabled = true;
        hit4.enabled = true;
        hit1.offset = new Vector2(1.51f, 1.125f);
        hit1.size = new Vector2(.34f, .86f);
        hit2.offset = new Vector2(1.31f, .61f);
        hit2.size = new Vector2(.45f, 1.6f);
        hit3.offset = new Vector2(.95f, .6f);
        hit3.size = new Vector2(.34f, 1.32f);
        hit4.offset = new Vector2(.63f, .66f);
        hit4.size = new Vector2(.52f, .82f);
        HitDetect.damage = 40;
        HitDetect.armorDamage = 0;
        HitDetect.durabilityDamage = 100;
        HitDetect.potentialHitStun = hitStunLv4;
        HitDetect.potentialHitStop = hitStopLv3;
        HitDetect.potentialKnockBack = new Vector2(1.5f, 0f);
        HitDetect.potentialAirKnockBack = new Vector2(.7f, 2.75f);
        HitDetect.attackLevel = 3;
        HitDetect.guard = "Mid";

        HitDetect.vertSlash = true;
        HitDetect.forceStand = true;
        HitDetect.allowHeavy = true;
        HitDetect.allowBreak = true;
        HitDetect.allowSpecial = true;
        HitDetect.allowSuper = true;
        HitDetect.jumpCancellable = true;
    }

    void JumpHHitBox()
    {
        ClearHitBox();
        HitDetect.Actions.AttackActive();

        hit1.enabled = true;
        hit2.enabled = true;
        hit3.enabled = true;
        hit4.enabled = true;
        hit5.enabled = true;

        hit1.offset = new Vector2(-.63f, -.168f);
        hit1.size = new Vector2(.97f, .43f);
        hit2.offset = new Vector2(.34f, -.11f);
        hit2.size = new Vector2(.3f, .4f);
        hit3.offset = new Vector2(.635f, .05f);
        hit3.size = new Vector2(.43f, .35f);
        hit4.offset = new Vector2(.79f, .18f);
        hit4.size = new Vector2(.44f, .31f);
        hit5.offset = new Vector2(1.1f, .43f);
        hit5.size = new Vector2(.215f, .59f);

        HitDetect.damage = 75;
        HitDetect.armorDamage = 0;
        HitDetect.durabilityDamage = 100;
        HitDetect.potentialHitStun = hitStunLv4;
        HitDetect.potentialHitStop = hitStopLv3;
        HitDetect.potentialKnockBack = new Vector2(1.5f, 0f);
        HitDetect.potentialAirKnockBack = new Vector2(1.5f, 2.6f);
        HitDetect.attackLevel = 3;
        HitDetect.guard = "Overhead";

        HitDetect.horiSlash = true;

        HitDetect.allowMedium = true;
        HitDetect.allowHeavy = true;
        HitDetect.allowBreak = true;
        HitDetect.allowSpecial = true;
        HitDetect.allowSuper = true;
        HitDetect.jumpCancellable = true;
    }

    void BreakCharge()
    {
        sinCharge++;
        HitDetect.anim.SetInteger("SinCharge", sinCharge);
    }

    void FullBreakCharge()
    {
        sinCharge = 5;
        HitDetect.anim.SetInteger("SinCharge", sinCharge);
    }

    void ResetCharge()
    {
        sinCharge = 0;
        HitDetect.anim.SetInteger("SinCharge", sinCharge);
    }

    void StandingBHitBox()
    {
        ClearHitBox();
        HitDetect.Actions.AttackActive();

        hit1.enabled = true;
        hit2.enabled = true;
        hit3.enabled = true;
        hit4.enabled = true;

        hit1.offset = new Vector2(1.1f, .15f);
        hit1.size = new Vector2(1.33f, .72f);
        hit2.offset = new Vector2(1.15f, .63f);
        hit2.size = new Vector2(1f, .47f);
        hit3.offset = new Vector2(1f, .98f);
        hit3.size = new Vector2(.82f, .41f);
        hit4.offset = new Vector2(.79f, 1.25f);
        hit4.size = new Vector2(.84f, .28f);

        HitDetect.damage = 90;
        HitDetect.armorDamage = 0;
        HitDetect.durabilityDamage = 0;
        HitDetect.potentialKnockBack = new Vector2(2f, 0f);
        HitDetect.potentialAirKnockBack = new Vector2(1f, -2f);
        HitDetect.potentialHitStun = 20;
        HitDetect.potentialHitStop = hitStopLv3;
        if (HitDetect.OpponentDetector.Actions.armorActive)
            HitDetect.initialProration = .75f;
        HitDetect.attackLevel = 1;
        HitDetect.guard = "Overhead";

        HitDetect.piercing = true;
        HitDetect.allowGroundBounce = true;

        HitDetect.vertSlash = true;
        HitDetect.allowSpecial = true;
        HitDetect.allowSuper = true;
    }

    void CrouchBHitBox()
    {
        ClearHitBox();
        HitDetect.Actions.AttackActive();

        hit1.enabled = true;

        hit1.offset = new Vector2(.65f, -.69f);
        hit1.size = new Vector2(1.6f, .445f);

        HitDetect.damage = 35;
        HitDetect.armorDamage = 0;
        HitDetect.durabilityDamage = 0;
        HitDetect.potentialKnockBack = new Vector2(1f, 0f);
        HitDetect.potentialHitStun = hitStunLv2;
        HitDetect.potentialHitStop = hitStopLv2;
        if (HitDetect.OpponentDetector.Actions.armorActive)
            HitDetect.initialProration = .75f;
        HitDetect.attackLevel = 1;
        HitDetect.guard = "Low";

        HitDetect.slash = true;
        HitDetect.piercing = true;
        HitDetect.allowSpecial = true;
        HitDetect.allowSuper = true;
    }

    void CrouchBHitBox2()
    {
        ClearHitBox();
        HitDetect.Actions.AttackActive();

        hit1.enabled = true;
        hit2.enabled = true;

        hit1.offset = new Vector2(.65f, -.69f);
        hit1.size = new Vector2(1.6f, .445f);
        hit2.offset = new Vector2(-.83f, -.57f);
        hit2.size = new Vector2(1.48f, .685f);

        HitDetect.damage = 55;
        HitDetect.armorDamage = 0;
        HitDetect.durabilityDamage = 0;
        HitDetect.potentialKnockBack = new Vector2(.2f, 2f);
        HitDetect.potentialHitStun = 24;
        HitDetect.potentialHitStop = hitStopLv3;
        HitDetect.initialProration = 1;
        HitDetect.attackLevel = 4;
        HitDetect.guard = "Low";

        HitDetect.slash = true;
        HitDetect.sweep = true;
        HitDetect.allowSpecial = true;
        HitDetect.allowSuper = true;
    }

    void JumpBHitBoxInit()
    {
        ClearHitBox();
        HitDetect.Actions.AttackActive();

        hit1.enabled = true;
        hit2.enabled = true;
        hit3.enabled = true;
        hit4.enabled = true;

        hit1.offset = new Vector2(.76f, -.07f);
        hit1.size = new Vector2(1.37f, .62f);
        hit2.offset = new Vector2(.83f, -.38f);
        hit2.size = new Vector2(1.18f, .44f);
        hit3.offset = new Vector2(.73f, -.69f);
        hit3.size = new Vector2(1.01f, .32f);
        hit4.offset = new Vector2(.57f, -.88f);
        hit4.size = new Vector2(.61f, .235f);

        HitDetect.damage = 40;
        HitDetect.durabilityDamage = 0;
        HitDetect.potentialKnockBack = new Vector2(.5f, 2.5f);
        HitDetect.potentialHitStun = 24;
        HitDetect.potentialHitStop = hitStopLv2;
        if (HitDetect.OpponentDetector.Actions.armorActive)
            HitDetect.initialProration = .75f;
        HitDetect.attackLevel = 1;
        HitDetect.guard = "Overhead";

        HitDetect.vertSlash = true;
        HitDetect.piercing = true;
        HitDetect.launch = true;
        HitDetect.allowSpecial = true;
        HitDetect.allowSuper = true;
    }

    void JumpBHitBoxNext()
    {
        ClearHitBox();
        HitDetect.Actions.AttackActive();

        hit1.enabled = true;
        hit2.enabled = true;
        hit3.enabled = true;
        hit4.enabled = true;

        hit1.offset = new Vector2(.3f, .54f);
        hit1.size = new Vector2(.71f, 2.96f);
        hit2.offset = new Vector2(.81f, .59f);
        hit2.size = new Vector2(.45f, 2.38f);
        hit3.offset = new Vector2(1.06f, .47f);
        hit3.size = new Vector2(.46f, 2f);
        hit4.offset = new Vector2(1.35f, .38f);
        hit4.size = new Vector2(.38f, 1.07f);

        HitDetect.damage = 40;
        HitDetect.durabilityDamage = 100;
        HitDetect.potentialKnockBack = new Vector2(.5f, 3f);
        HitDetect.potentialHitStun = 32;
        HitDetect.potentialHitStop = hitStopLv3;
        HitDetect.attackLevel = 1;
        HitDetect.guard = "Overhead";

        HitDetect.vertSlash = true;
        HitDetect.piercing = true;
        HitDetect.launch = true;
        HitDetect.allowSpecial = true;
        HitDetect.allowSuper = true;
    }

    void ThrowInit()
    {
        ClearHitBox();
        HitDetect.Actions.AttackActive();

        hit1.enabled = true;


        hit1.offset = new Vector2(.35f, -.09f);
        hit1.size = new Vector2(.45f, .9f);

        HitDetect.armorDamage = 1;
        HitDetect.durabilityDamage = 50;
        HitDetect.potentialHitStun = 60;
        HitDetect.potentialHitStop = 20;
        HitDetect.attackLevel = 0;
        HitDetect.guard = "Unblockable";

        HitDetect.crumple = true;
        HitDetect.grab = true;

    }

    void ThrowSecondHit()
    {
        ClearHitBox();
        hit1.enabled = true;


        hit1.offset = new Vector2(.56f, .37f);
        hit1.size = new Vector2(.69f, 1.8f);

        HitDetect.potentialHitStun = 60;
        HitDetect.potentialHitStop = 0;
        HitDetect.attackLevel = 0;
        HitDetect.potentialKnockBack = new Vector2(.15f, 3f);
        HitDetect.guard = "Unblockable";

        HitDetect.launch = true;
        HitDetect.turnOffEffect = true;
        HitDetect.disableBlitz = true;

    }

    void ThrowThirdHit()
    {
        ClearHitBox();
        hit1.enabled = true;


        hit1.offset = new Vector2(1.2f, -.19f);
        hit1.size = new Vector2(1.35f, .27f);
        HitDetect.damage = 100;
        HitDetect.initialProration = .7f;
        HitDetect.forcedProration = .7f;
        HitDetect.potentialAirKnockBack = new Vector2(2.5f, 1f);
        HitDetect.potentialHitStun = 60;
        HitDetect.potentialHitStop = hitStopLv4;
        HitDetect.attackLevel = 5;
        HitDetect.guard = "Unblockable";

        HitDetect.horiSlash = true;
        HitDetect.allowSuper = true;
        HitDetect.allowWallStick = true;
    }

    void HCHeavy()
    {
        ClearHitBox();
        HitDetect.Actions.AttackActive();

        hit1.enabled = true;
        hit2.enabled = true;
        hit3.enabled = true;
        hit4.enabled = true;
        hit5.enabled = true;
        hit6.enabled = true;

        hit1.offset = new Vector2(.68f, .58f);
        hit1.size = new Vector2(.73f, 1.5f);
        hit2.offset = new Vector2(.46f, 1.3f);
        hit2.size = new Vector2(.74f, .79f);
        hit3.offset = new Vector2(.25f, 1.42f);
        hit3.size = new Vector2(.89f, .86f);
        hit4.offset = new Vector2(-.5f, .8f);
        hit4.size = new Vector2(.52f, 1.4f);
        hit5.offset = new Vector2(-.99f, .67f);
        hit5.size = new Vector2(.45f, 1f);
        hit6.offset = new Vector2(-1.26f, .45f);
        hit6.size = new Vector2(.73f, .65f);

        HitDetect.damage = 85;
        HitDetect.armorDamage = 0;
        HitDetect.durabilityDamage = 0;
        HitDetect.potentialKnockBack = new Vector2(1f, 3.75f);
        HitDetect.potentialHitStun = 30;
        HitDetect.potentialHitStop = hitStopLv4;
        HitDetect.attackLevel = 4;
        HitDetect.guard = "Mid";
        HitDetect.vertSlash = true;

        HitDetect.launch = true;
        HitDetect.piercing = true;
        HitDetect.usingSpecial = true;
        HitDetect.allowSuper = true;
    }

    void HCBreak()
    {
        ClearHitBox();
        HitDetect.Actions.AttackActive();

        hit1.enabled = true;
        hit2.enabled = true;
        hit3.enabled = true;
        hit4.enabled = true;
        hit5.enabled = true;
        hit6.enabled = true;

        hit1.offset = new Vector2(.68f, .58f);
        hit1.size = new Vector2(.73f, 1.5f);
        hit2.offset = new Vector2(.46f, 1.3f);
        hit2.size = new Vector2(.74f, .79f);
        hit3.offset = new Vector2(.25f, 1.42f);
        hit3.size = new Vector2(.89f, .86f);
        hit4.offset = new Vector2(-.5f, .8f);
        hit4.size = new Vector2(.52f, 1.4f);
        hit5.offset = new Vector2(-.99f, .67f);
        hit5.size = new Vector2(.45f, 1f);
        hit6.offset = new Vector2(-1.26f, .45f);
        hit6.size = new Vector2(.73f, .65f);

        HitDetect.damage = 115;
        HitDetect.armorDamage = 0;
        HitDetect.durabilityDamage = 0;
        HitDetect.potentialKnockBack = new Vector2(1.5f, 5f);
        HitDetect.potentialHitStun = 80;
        HitDetect.potentialHitStop = 15;
        HitDetect.attackLevel = 5;
        HitDetect.guard = "Mid";
        HitDetect.vertSlash = true;

        HitDetect.launch = true;
        HitDetect.shatter = true;
        HitDetect.usingSpecial = true;
        HitDetect.allowSuper = true;
    }

    void LHFirstHit()
    {
        ClearHitBox();

        hit1.enabled = true;

        hit1.offset = new Vector2(.42f, -.71f);
        hit1.size = new Vector2(1f, .41f);

        HitDetect.damage = 45;
        HitDetect.armorDamage = 0;
        HitDetect.durabilityDamage = 100;
        HitDetect.potentialKnockBack = new Vector2(1.5f, 0f);
        HitDetect.potentialAirKnockBack = new Vector2(1.5f, 1.5f);
        if(HitDetect.OpponentDetector.Actions.airborne)
            HitDetect.potentialHitStun = 24;
        else
            HitDetect.potentialHitStun = hitStunLv4;
        HitDetect.potentialHitStop = hitStopLv2;
        HitDetect.attackLevel = 3;
        HitDetect.guard = "Low";

        HitDetect.usingSpecial = true;
        HitDetect.allowSuper = true;
    }

    void LHFinalHit()
    {
        ClearHitBox();
        HitDetect.Actions.AttackActive();

        hit1.enabled = true;
        hit2.enabled = true;

        hit1.offset = new Vector2(.94f, .17f);
        hit1.size = new Vector2(1.35f, .5f);
        hit2.offset = new Vector2(.645f, .15f);
        hit2.size = new Vector2(1.1f, .895f);

        HitDetect.damage = 70;
        HitDetect.armorDamage = 0;
        HitDetect.durabilityDamage = 0;       
        HitDetect.potentialBlockStun = 16;
        HitDetect.potentialHitStun = 42;
        HitDetect.potentialHitStop = 15;
        HitDetect.attackLevel = 5;
        HitDetect.guard = "Mid";

        HitDetect.horiSlash = true;
        HitDetect.shatter = true;
        if (HitDetect.OpponentDetector.Actions.attacking || HitDetect.OpponentDetector.Actions.active || HitDetect.OpponentDetector.Actions.recovering)
        {
            HitDetect.allowWallStick = true;
            HitDetect.allowGroundBounce = true;
            HitDetect.potentialKnockBack = new Vector2(3.5f, 2.5f);
        }
        else
        {
            HitDetect.potentialKnockBack = new Vector2(3f, 2f);
        }
        HitDetect.usingSpecial = true;
        HitDetect.allowSuper = true;
    }

    void StarfallHit()
    {
        ClearHitBox();
        HitDetect.Actions.AttackActive();

        hit1.enabled = true;
        hit2.enabled = true;
        hit3.enabled = true;
        hit4.enabled = true;

        hit1.offset = new Vector2(.65f, -.73f);
        hit1.size = new Vector2(.39f, .385f);
        hit2.offset = new Vector2(.47f, -.45f);
        hit2.size = new Vector2(.39f, .385f);
        hit3.offset = new Vector2(.27f, -.16f);
        hit3.size = new Vector2(.5f, .385f);
        hit4.offset = new Vector2(0.1f, .14f);
        hit4.size = new Vector2(.56f, .385f);

        HitDetect.damage = 120;
        HitDetect.armorDamage = 0;
        HitDetect.durabilityDamage = 0;
        HitDetect.potentialKnockBack = new Vector2(.75f, 3.3f);
        HitDetect.potentialAirKnockBack = new Vector2(2f, -4f);
        HitDetect.potentialHitStun = 32;
        HitDetect.potentialHitStop = 15;
        HitDetect.attackLevel = 5;
        HitDetect.guard = "Overhead";

        if (HitDetect.OpponentDetector.Actions.standing)
            HitDetect.launch = true;

        HitDetect.vertSlash = true;
        HitDetect.allowGroundBounce = true;
        HitDetect.shatter = true;
        HitDetect.usingSpecial = true;
        HitDetect.allowSuper = true;
    }

    void HRSlashHit()
    {
        ClearHitBox();
        HitDetect.Actions.AttackActive();

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

        HitDetect.damage = 50;
        HitDetect.armorDamage = 0;
        HitDetect.durabilityDamage = 100;
        HitDetect.potentialHitStun = hitStunLv4;
        HitDetect.potentialHitStop = hitStopLv1;
        HitDetect.potentialKnockBack = new Vector2(1.2f, 3f);
        HitDetect.potentialAirKnockBack = new Vector2(.8f, 3f);
        HitDetect.attackLevel = 3;
        HitDetect.guard = "Mid";

        HitDetect.slash = true;
        HitDetect.usingSpecial = true;
        HitDetect.allowSuper = true;
    }

    void HRUpperHit1()
    {
        ClearHitBox();
        HitDetect.Actions.AttackActive();

        hit1.enabled = true;
        hit2.enabled = true;

        hit1.offset = new Vector2(.66f, .4f);
        hit1.size = new Vector2(.36f, .74f);
        hit2.offset = new Vector2(.57f, -.08f);
        hit2.size = new Vector2(.25f, .76f);

        HitDetect.damage = 80;
        HitDetect.armorDamage = 0;
        HitDetect.durabilityDamage = 0;
        HitDetect.potentialHitStun = hitStunLv4;
        HitDetect.potentialHitStop = hitStopLv4;
        HitDetect.potentialKnockBack = new Vector2(1.2f, 3f);
        HitDetect.attackLevel = 5;
        HitDetect.guard = "Mid";

        HitDetect.shatter = true;
        HitDetect.usingSpecial = true;
        HitDetect.allowSuper = true;
    }

    void HRUpperHit2()
    {
        ClearHitBox();
        HitDetect.Actions.AttackActive();

        hit1.enabled = true;
        hit2.enabled = true;
        hit3.enabled = true;

        hit1.offset = new Vector2(.4f, .87f);
        hit1.size = new Vector2(.82f, .75f);
        hit2.offset = new Vector2(.75f, .51f);
        hit2.size = new Vector2(.61f, .69f);
        hit3.offset = new Vector2(.79f, -.02f);
        hit3.size = new Vector2(.28f, .56f);

        HitDetect.damage = 120;
        HitDetect.armorDamage = 0;
        HitDetect.durabilityDamage = 0;
        HitDetect.potentialHitStun = 100;
        HitDetect.potentialHitStop = 15;
        HitDetect.potentialKnockBack = new Vector2(1.5f, 5.5f);
        HitDetect.attackLevel = 5;
        HitDetect.guard = "Mid";

        HitDetect.shatter = true;
        HitDetect.launch = true;
        HitDetect.usingSpecial = true;
        HitDetect.allowSuper = true;
    }

    void BCWeakCharge()
    {
        ClearHitBox();
        HitDetect.Actions.AttackActive();

        hit1.enabled = true;
        hit2.enabled = true;

        hit1.offset = new Vector2(1f, .11f);
        hit1.size = new Vector2(.84f, .8f);
        hit2.offset = new Vector2(.59f, .24f);
        hit2.size = new Vector2(.93f, .28f);

        HitDetect.damage = 85;
        HitDetect.armorDamage = 1;
        HitDetect.durabilityDamage = 0;
        HitDetect.potentialHitStun = 48;
        HitDetect.potentialHitStop = hitStopLv4;
        HitDetect.potentialKnockBack = new Vector2(3f, 2f);
        HitDetect.attackLevel = 3;
        HitDetect.guard = "Mid";

        //HitDetect.usingSpecial = true;
        HitDetect.allowSuper = true;
    }

    void BCFullCharge()
    {
        ClearHitBox();
        HitDetect.Actions.AttackActive();

        hit1.enabled = true;
        hit2.enabled = true;
        hit3.enabled = true;

        hit1.offset = new Vector2(1.14f, 018f);
        hit1.size = new Vector2(1f, 1f);
        hit2.offset = new Vector2(.94f, .24f);
        hit2.size = new Vector2(1.6f, .28f);
        hit3.offset = new Vector2(.97f, .5f);
        hit3.size = new Vector2(.64f, .61f);

        HitDetect.damage = 140;
        HitDetect.armorDamage = 0;
        HitDetect.durabilityDamage = 0;
        HitDetect.potentialHitStun = 60;
        HitDetect.potentialHitStop = hitStopLv4;
        HitDetect.potentialKnockBack = new Vector2(4f, 2.5f);
        HitDetect.attackLevel = 5;
        HitDetect.guard = "Mid";

        HitDetect.allowWallStick = true;
        HitDetect.shatter = true;
        //HitDetect.usingSpecial = true;
        HitDetect.allowSuper = true;

        sinCharge = 0;
        HitDetect.anim.SetInteger("SinCharge", sinCharge);
    }

    void JSabreHitbox()
    {
        ClearHitBox();
        HitDetect.Actions.AttackActive();

        hit1.enabled = true;
        hit2.enabled = true;
        hit3.enabled = true;

        hit1.offset = new Vector2(.145f, 1.4f);
        hit1.size = new Vector2(.57f, 1.46f);
        hit2.offset = new Vector2(.58f, .56f);
        hit2.size = new Vector2(.33f, 2.6f);
        hit3.offset = new Vector2(.91f, .63f);
        hit3.size = new Vector2(.375f, 1.8f);

        HitDetect.damage = 300;
        HitDetect.armorDamage = 0;
        HitDetect.durabilityDamage = 0;
        HitDetect.potentialHitStun = 600;
        HitDetect.potentialHitStop = 24;
        HitDetect.potentialKnockBack = new Vector2(.7f, 8f);
        HitDetect.initialProration = .8f;
        HitDetect.forcedProration = .8f;
        HitDetect.attackLevel = 10;
        HitDetect.guard = "Mid";

        HitDetect.vertSlash = true;

        HitDetect.launch = true;
        HitDetect.shatter = true;
        HitDetect.usingSuper = true;
    }
}
