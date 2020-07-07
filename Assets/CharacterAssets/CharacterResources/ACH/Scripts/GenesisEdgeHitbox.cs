using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenesisEdgeHitbox : MonoBehaviour
{
    public BoxCollider2D hit1;

    public SpriteRenderer sprite;
    public Light flash;

    public ProjectileHitDetector PHitDetect;

    public AttackHandlerACH AttackHandler;
    public Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 60;
        AttackHandler = transform.root.GetComponentInChildren<AttackHandlerACH>();
        AwakenBox();
    }

    // Update is called once per frame
    void Update()
    {
        if (PHitDetect.Actions.superFlash > 0)
        {
            PHitDetect.hitStop = (float)2 / 60;
            if (PHitDetect.currentVelocity == Vector2.zero)
            {
                PHitDetect.currentVelocity = PHitDetect.rb.velocity;
                PHitDetect.currentAngularVelocity = PHitDetect.rb.angularVelocity;
            }
        }

        if (PHitDetect.HitDetect.hitStun > 0)
        {
            anim.SetTrigger("Deactivate");
            anim.SetInteger("ProjectileLevel", 0);
        }

        if (PHitDetect.hit)
        {
            ClearHitBox();
            PHitDetect.hit = false;
        }

        if (anim.GetCurrentAnimatorStateInfo(0).IsName("GenesisProjS1End") || anim.GetCurrentAnimatorStateInfo(0).IsName("GenesisProjS2End"))
            StopVelocity();
    }

    public void Advance()
    {
        if (PHitDetect.HitDetect.Actions.Move.facingRight)
            PHitDetect.rb.velocity = new Vector2(7,0);
        else
            PHitDetect.rb.velocity = new Vector2(-7f, 0);
    }

    public void StopVelocity()
    {
        PHitDetect.rb.velocity = Vector2.zero;
    }

    public void ClearHitBox()
    {
        anim.SetInteger("ProjectileLevel", 0);

        hit1.enabled = false;
        PHitDetect.potentialHitStun = 0;
        PHitDetect.potentialHitStop = 0;
        PHitDetect.damage = 0;
        PHitDetect.armorDamage = 0;
        PHitDetect.durabilityDamage = 0;
        PHitDetect.potentialKnockBack = Vector2.zero;
        PHitDetect.potentialAirKnockBack = Vector2.zero;
        PHitDetect.forcedProration = 0;
        PHitDetect.initialProration = 1;
        PHitDetect.attackLevel = 0;
        PHitDetect.blitz = false;
        PHitDetect.piercing = false;
        PHitDetect.launch = false;
        PHitDetect.crumple = false;
        PHitDetect.sweep = false;
        PHitDetect.forceCrouch = false;
        PHitDetect.forceStand = false;
        PHitDetect.allowWallStick = false;
        PHitDetect.allowGroundBounce = false;
        PHitDetect.allowWallBounce = false;
        PHitDetect.shatter = false;
        PHitDetect.forceShatter = false;
        PHitDetect.usingSuper = false;
        PHitDetect.usingSpecial = false;
        PHitDetect.slash = false;
    }

    void AwakenBox()
    {
        ClearHitBox();
        hit1.enabled = true;
        hit1.size = new Vector2(1000f, 1000f);
    }

    void GenesisS1Hitbox()
    {
        ClearHitBox();

        hit1.enabled = true;
        hit1.offset = new Vector2(10f, -.15f);
        hit1.size = new Vector2(15.5f, 3.8f);
        PHitDetect.damage = 43;
        PHitDetect.durabilityDamage = 100;
        PHitDetect.potentialHitStun = 42;
        PHitDetect.potentialHitStop = 5;
        if  (PHitDetect.HitDetect.Actions.Move.facingRight)
            PHitDetect.potentialKnockBack = new Vector2(2.5f, 1f);
        else
            PHitDetect.potentialKnockBack = new Vector2(-2.5f, 1f);
        PHitDetect.attackLevel = 1;
        PHitDetect.guard = "Mid";

        if (PHitDetect.HitDetect.Actions.airborne)
        {
            PHitDetect.allowWallBounce = true;
            PHitDetect.damage = 85;
            PHitDetect.potentialHitStun = 32;
        }

        PHitDetect.allowSuper = true;
        PHitDetect.usingSpecial = true;

        if (AttackHandler.Hitboxes.install)
            PHitDetect.allowSpecial = true;
    }

    void GenesisS2Hitbox()
    {
        ClearHitBox();

        hit1.enabled = true;
        hit1.offset = new Vector2(10.775f, -1.275f);
        hit1.size = new Vector2(15.4f, 4.5f);
        PHitDetect.damage = 45;
        PHitDetect.durabilityDamage = 0;
        if (PHitDetect.HitDetect.Actions.airborne)
            PHitDetect.potentialHitStun = 60;
        else
        {
            PHitDetect.potentialHitStun = 42;
        }
        PHitDetect.potentialHitStop = 5;
        if (PHitDetect.HitDetect.Actions.Move.facingRight)
            PHitDetect.potentialKnockBack = new Vector2(2.5f, 1f);
        else
            PHitDetect.potentialKnockBack = new Vector2(-2.5f, 1f);
        PHitDetect.attackLevel = 4;
        PHitDetect.guard = "Mid";

        if (PHitDetect.HitDetect.Actions.airborne)
            PHitDetect.allowWallBounce = true;
        else if (PHitDetect.ProjProp.currentHits >= 2 || PHitDetect.ProjProp.currentLife <= 6f/60f)
        {
                PHitDetect.allowWallStick = true;
                PHitDetect.allowWallBounce = true;
        }
        PHitDetect.slash = true;
        PHitDetect.shatter = true;
        PHitDetect.allowSuper = true;
        PHitDetect.usingSpecial = true;

        if (AttackHandler.Hitboxes.install)
            PHitDetect.allowSpecial = true;
    }

    void GenesisS3Hitbox1()
    {
        ClearHitBox();

        hit1.enabled = true;
        hit1.offset = new Vector2(13.5f, -1.275f);
        hit1.size = new Vector2(24.5f, 11.85f);
        PHitDetect.damage = 45;
        PHitDetect.durabilityDamage = 0;
        PHitDetect.potentialHitStun = 60;
        PHitDetect.potentialHitStop = 5;
        if (PHitDetect.HitDetect.Actions.Move.facingRight)
            PHitDetect.potentialKnockBack = new Vector2(3.5f, 1.5f);
        else
            PHitDetect.potentialKnockBack = new Vector2(-3.5f, 1.5f);
        PHitDetect.attackLevel = 7;
        PHitDetect.guard = "Mid";

        PHitDetect.slash = true;
        PHitDetect.shatter = true;
        PHitDetect.allowSuper = true;
        PHitDetect.usingSpecial = true;
    }

    void GenesisS3Hitbox2()
    {
        ClearHitBox();

        hit1.enabled = true;
        hit1.offset = new Vector2(22f, 0f);
        hit1.size = new Vector2(39.5f, 10f);
        PHitDetect.damage = 200;
        PHitDetect.durabilityDamage = 0;
        PHitDetect.potentialHitStun = 60;
        PHitDetect.potentialHitStop = 5;
        if (PHitDetect.HitDetect.Actions.Move.facingRight)
            PHitDetect.potentialKnockBack = new Vector2(5f, 2.5f);
        else
            PHitDetect.potentialKnockBack = new Vector2(-5f, 2.5f);
        PHitDetect.attackLevel = 7;
        PHitDetect.guard = "Mid";

        PHitDetect.allowWallStick = true;
        PHitDetect.allowWallBounce = true;

        PHitDetect.slash = true;
        PHitDetect.shatter = true;
        PHitDetect.allowSuper = true;
        PHitDetect.usingSpecial = true;

        if (AttackHandler.Hitboxes.install)
            PHitDetect.allowSpecial = true;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("HurtBox") && other.gameObject.transform.parent.parent == PHitDetect.Actions.Move.opponent && hit1.size.x >= 1000)
        {
            ClearHitBox();
            transform.gameObject.SetActive(false);
        }
    }
}
