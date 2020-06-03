using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToasterHitbox : MonoBehaviour
{
    public BoxCollider2D hit1;
    public ProjectileHitDetector PHitDetect;

    public BoxCollider2D collide;

    public Light lightSource;

    Animator charAnim;

    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 60;
        AwakenBox();
        charAnim = transform.root.GetChild(0).transform.GetChild(0).GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!PHitDetect.anim.GetCurrentAnimatorStateInfo(0).IsName("BaguetteEnd"))
            charAnim.ResetTrigger("ToasterDeactivate");

        if (PHitDetect.hit)
        {
            ClearHitBox();
            PHitDetect.hit = false;
        }
        if (transform.position.y > 0)
        {
            if (lightSource.range == 9 && lightSource.intensity > 0)
                lightSource.intensity -= .2f;
            else if (lightSource.range == 11)
                lightSource.enabled = false;
            else if (lightSource.range == 5)
                lightSource.intensity = Random.Range(1f, 2.5f);
            else if (lightSource.range == 8 && PHitDetect.hitStop == 0)
                lightSource.intensity = Random.Range(1f, 7f);
        }

        if (PHitDetect.HitDetect.hitStun > 0)
            PHitDetect.ProjProp.Deactivate();
    }

    public void ChargeLight()
    {
        lightSource.enabled = true;
        lightSource.range = 5;
    }

    public void FiringLight()
    {
        lightSource.range = 8;
    }

    public void EndLight()
    {
        lightSource.range = 9;
    }

    public void ClearHitBox()
    {
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
        PHitDetect.allowWallStick = false;
        PHitDetect.allowGroundBounce = false;
        PHitDetect.allowWallBounce = false;
        PHitDetect.shatter = false;
        PHitDetect.usingSuper = false;
        PHitDetect.usingSpecial = false;
    }

    void AwakenBox()
    {
        ClearHitBox();
        hit1.enabled = true;
        hit1.size = new Vector2(1000f, 1000f);
    }

    void InitialHitBox()
    {
        charAnim.ResetTrigger("ToasterDeactivate");
        collide.enabled = true;

        ClearHitBox();
        hit1.enabled = true;
        hit1.offset = new Vector2(18.5f, 2.5f);
        hit1.size = new Vector2(34f, 6f);
        PHitDetect.damage = 35;
        PHitDetect.durabilityDamage = 50;
        PHitDetect.potentialHitStun = 36;
        PHitDetect.potentialHitStop = 1;
        PHitDetect.potentialKnockBack = new Vector2(1f, 1f);
        PHitDetect.potentialAirKnockBack = new Vector2(1f, 1f);
        PHitDetect.attackLevel = 10;
        PHitDetect.guard = "Mid";

        PHitDetect.piercing = true;
        PHitDetect.usingSuper = true;
    }

    void BeamHitBox()
    {
        ClearHitBox();
        hit1.enabled = true;
        hit1.offset = new Vector2(42f, 2.5f);
        hit1.size = new Vector2(73f, 8f);
        PHitDetect.damage = 30;
        PHitDetect.durabilityDamage = 50;
        PHitDetect.potentialHitStun = 36;
        PHitDetect.potentialHitStop = 0;
        PHitDetect.potentialKnockBack = new Vector2(1f, .65f);
        PHitDetect.potentialAirKnockBack = new Vector2(2f, .65f);
        PHitDetect.attackLevel = 10;
        PHitDetect.guard = "Mid";

        PHitDetect.usingSuper = true;
    }

    void FinalHitBox()
    {
        ClearHitBox();
        hit1.enabled = true;
        hit1.offset = new Vector2(42f, 2.5f);
        hit1.size = new Vector2(73f, 8f);
        PHitDetect.damage = 30;
        PHitDetect.durabilityDamage = 50;
        PHitDetect.potentialHitStun = 36;
        PHitDetect.potentialHitStop = 0;
        PHitDetect.potentialKnockBack = new Vector2(4f, .65f);
        PHitDetect.potentialAirKnockBack = new Vector2(4f, .65f);
        PHitDetect.attackLevel = 10;
        PHitDetect.guard = "Mid";

        PHitDetect.allowWallBounce = true;
        PHitDetect.usingSuper = true;
    }

    void RecoveryStart()
    {
        charAnim.SetTrigger("ToasterDeactivate");
        collide.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (hit1.size.x >= 1000 && other.CompareTag("HurtBox") && other.gameObject.transform.parent.parent == PHitDetect.Actions.Move.opponent)
        {
            ClearHitBox();
            transform.gameObject.SetActive(false);
        }
    }
}
