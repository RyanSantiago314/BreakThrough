using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForsythiaHitbox : MonoBehaviour
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
        if (PHitDetect.HitDetect.hitStop <= 0 && PHitDetect.HitDetect.Actions.blitzed <= 0 && anim.GetCurrentAnimatorStateInfo(0).IsName("ForsythiaReticle"))
        {
            PHitDetect.rb.velocity = new Vector2(7f * AttackHandler.MaxInput.GetAxis(AttackHandler.Horizontal), 7f * AttackHandler.MaxInput.GetAxis(AttackHandler.Vertical));

            if (AttackHandler.MaxInput.GetButtonDown(AttackHandler.Light) || AttackHandler.MaxInput.GetButtonDown(AttackHandler.Medium) || 
                AttackHandler.MaxInput.GetButtonDown(AttackHandler.Heavy) || AttackHandler.MaxInput.GetButtonDown(AttackHandler.Break) ||
                AttackHandler.MaxInput.GetButtonDown(AttackHandler.LB) || AttackHandler.MaxInput.GetButtonDown(AttackHandler.LM) ||
                AttackHandler.MaxInput.GetButtonDown(AttackHandler.MH) || AttackHandler.MaxInput.GetButtonDown(AttackHandler.HB))
            {
                anim.SetTrigger("Fire");
                PHitDetect.HitDetect.anim.SetTrigger("FMShot");
                PHitDetect.ProjProp.currentHits++;
            }
        }
        else
        {
            PHitDetect.rb.velocity = Vector2.zero;
        }

        if (transform.position.x > 10.4f)
            transform.position = new Vector3(10.4f, transform.position.y, transform.position.z);
        if (transform.position.x < -10.4)
            transform.position = new Vector3(-10.4f, transform.position.y, transform.position.z);


        if (PHitDetect.HitDetect.OpponentDetector.Actions.blitzed > 0)
            anim.SetFloat("AnimSpeed", .5f);
        else if (PHitDetect.hitStop <= 0)
            anim.SetFloat("AnimSpeed", 1f);

        if (PHitDetect.HitDetect.OpponentDetector.Actions.shattered && PHitDetect.HitDetect.hitStop <= 0 && PHitDetect.hitStop > 0)
            PHitDetect.HitDetect.hitStop = PHitDetect.hitStop;


        if (PHitDetect.HitDetect.hitStun > 0 || (PHitDetect.HitDetect.anim.GetCurrentAnimatorStateInfo(0).IsName("ForsythiaEnd") && anim.GetCurrentAnimatorStateInfo(0).IsName("ForsythiaReticle")))
            PHitDetect.ProjProp.Deactivate(0);

        if (flash.intensity > 0)
        {
            flash.intensity -= .2f;
        }
    }

    public void ClearHitBox()
    {
        PHitDetect.hit = false;
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
    }

    void AwakenBox()
    {
        ClearHitBox();
        hit1.enabled = true;
        hit1.size = new Vector2(1000f, 1000f);
    }

    void Explode()
    {
        flash.intensity = 6;
    }

    void ExplosionHitBox()
    {
        ClearHitBox();
        hit1.enabled = true;
        hit1.offset = Vector2.zero;
        hit1.size = new Vector2(15f, 15f);

        if (Mathf.Abs(transform.position.y - PHitDetect.OpponentDetector.Actions.Move.transform.position.y) < .25f &&
            Mathf.Abs(transform.position.x - PHitDetect.OpponentDetector.Actions.Move.transform.position.x) < .25f)
        {
            PHitDetect.damage = 120;
            PHitDetect.durabilityDamage = 0;
            PHitDetect.potentialHitStun = 60;
            PHitDetect.potentialHitStop = 15;

            PHitDetect.potentialKnockBack = Vector2.zero;
            PHitDetect.allowWallBounce = true;

            if (transform.position.x < PHitDetect.OpponentDetector.Actions.Move.transform.position.x && 
                Mathf.Abs(transform.position.x - PHitDetect.OpponentDetector.Actions.Move.transform.position.x) > .05f)
                PHitDetect.potentialAirKnockBack = new Vector2(2f, 0f);
            else if (transform.position.x > PHitDetect.OpponentDetector.Actions.Move.transform.position.x && 
                Mathf.Abs(transform.position.x - PHitDetect.OpponentDetector.Actions.Move.transform.position.x) > .05f)
                PHitDetect.potentialAirKnockBack = new Vector2(-2f, 0f);
            else
            {
                PHitDetect.potentialAirKnockBack = Vector2.zero;
            }

            if (transform.position.y > PHitDetect.OpponentDetector.Actions.Move.transform.position.y &&
                Mathf.Abs(transform.position.y - PHitDetect.OpponentDetector.Actions.Move.transform.position.y) > .15f)
            {
                PHitDetect.potentialAirKnockBack = new Vector2(PHitDetect.potentialAirKnockBack.x, -4f);
                PHitDetect.allowGroundBounce = true;
                PHitDetect.allowWallBounce = false;
                PHitDetect.launch = true;
            }
            else
            {
                PHitDetect.potentialAirKnockBack = new Vector2(PHitDetect.potentialAirKnockBack.x, 4f);
            }


            PHitDetect.initialProration = .95f;
            PHitDetect.forcedProration = 1f;
            PHitDetect.attackLevel = 10;
            if (PHitDetect.OpponentDetector.Actions.airborne)
            {
                PHitDetect.guard = "Unblockable";
            }
            else
                PHitDetect.guard = "Mid";

            if (PHitDetect.OpponentDetector.Actions.standing)
                PHitDetect.crumple = true;
            else
                PHitDetect.launch = true;

            if (PHitDetect.ProjProp.currentHits <= 1 || PHitDetect.OpponentDetector.hitStun <= 0)
            {
                PHitDetect.shatter = true;
            }
            else
                PHitDetect.forceShatter = true;
        }
        else
        {
            PHitDetect.damage = 85;
            PHitDetect.durabilityDamage = 0;
            PHitDetect.potentialHitStun = 48;
            PHitDetect.potentialHitStop = 10;

            PHitDetect.allowWallBounce = true;

            if (transform.position.x < PHitDetect.OpponentDetector.Actions.Move.transform.position.x)
                PHitDetect.potentialKnockBack = new Vector2(1.5f, 0f);
            else if (transform.position.x > PHitDetect.OpponentDetector.Actions.Move.transform.position.x)
                PHitDetect.potentialKnockBack = new Vector2(-1.5f, 0f);

            if (transform.position.y > PHitDetect.OpponentDetector.Actions.Move.transform.position.y &&
                Mathf.Abs(transform.position.y - PHitDetect.OpponentDetector.Actions.Move.transform.position.y) > .3f)
            {
                PHitDetect.potentialKnockBack = new Vector2(PHitDetect.potentialKnockBack.x, -3f);
                PHitDetect.allowGroundBounce = true;
                PHitDetect.allowWallBounce = false;
                PHitDetect.launch = true;
            }
            else
            {
                PHitDetect.potentialKnockBack = new Vector2(PHitDetect.potentialKnockBack.x, 3f);
            }

            PHitDetect.potentialAirKnockBack = PHitDetect.potentialKnockBack;
            PHitDetect.attackLevel = 10;
            PHitDetect.guard = "Mid";

            PHitDetect.shatter = true;
        }
        PHitDetect.usingSuper = true;           
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("HurtBox") && other.gameObject.transform.parent.parent == PHitDetect.Actions.Move.opponent && hit1.size.x >= 1000)
        {
            ClearHitBox();
            transform.gameObject.SetActive(false);
        }
    }
}
