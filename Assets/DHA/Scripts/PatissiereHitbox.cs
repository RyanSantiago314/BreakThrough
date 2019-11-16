using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatissiereHitbox : MonoBehaviour
{
    public BoxCollider2D hit1;

    public ProjectileHitDetector PHitDetect;

    // Start is called before the first frame update
    void Start()
    {
        AwakenBox();
    }

    // Update is called once per frame
    void Update()
    {
        if (PHitDetect.hit)
        {
            ClearHitBox();
            PHitDetect.currentVelocity *= new Vector2(-.25f, .75f);
            PHitDetect.hit = false;
        }
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
        hit1.size = new Vector2(10f, 10f);
    }

    void InitialHitBox()
    {
        ClearHitBox();

        hit1.enabled = true;
        hit1.offset = Vector2.zero;
        hit1.size = new Vector2(3.5f, 3.5f);
        PHitDetect.damage = 20;
        PHitDetect.durabilityDamage = 100;
        PHitDetect.potentialHitStun = 15;
        PHitDetect.potentialHitStop = 8;
        PHitDetect.potentialKnockBack = new Vector2(1.2f, 0);
        PHitDetect.potentialAirKnockBack = new Vector2(1.5f, 2);
        PHitDetect.initialProration = .75f;
        PHitDetect.attackLevel = 1;
        PHitDetect.guard = "Mid";

        PHitDetect.allowSuper = true;
    }

    void ExplosionHitBox()
    {
        ClearHitBox();

        hit1.enabled = true;
        hit1.offset = Vector2.zero;
        hit1.size = new Vector2(20f, 20f);
        PHitDetect.damage = 95;
        PHitDetect.durabilityDamage = 100;
        PHitDetect.potentialHitStun = 42;
        PHitDetect.potentialHitStop = 8;
        PHitDetect.potentialKnockBack = new Vector2(1.5f, 3.2f);
        PHitDetect.potentialAirKnockBack = new Vector2(1.5f, 3.2f);
        PHitDetect.initialProration = .85f;
        PHitDetect.forcedProration = 1.1f;
        PHitDetect.attackLevel = 3;
        PHitDetect.guard = "Mid";

        PHitDetect.allowSuper = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Floor") && PHitDetect.ProjProp.currentHits == 0)
        {
            ClearHitBox();
            PHitDetect.rb.velocity = new Vector2(0.5f * PHitDetect.rb.velocity.x, PHitDetect.rb.velocity.y);
            PHitDetect.rb.angularVelocity *= .5f;
            PHitDetect.ProjProp.currentHits++;
        }
        else if (other.CompareTag("Wall"))
        {
            PHitDetect.rb.velocity = new Vector2(0.7f * PHitDetect.rb.velocity.x, PHitDetect.rb.velocity.y);
            PHitDetect.rb.angularVelocity *= .7f;
        }
        else if (other.gameObject.transform.parent.parent == PHitDetect.Actions.Move.opponent)
        {
            PHitDetect.rb.velocity = new Vector2(-0.2f * PHitDetect.rb.velocity.x, PHitDetect.rb.velocity.y);
        }
    }
}
