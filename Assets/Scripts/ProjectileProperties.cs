using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileProperties : MonoBehaviour
{
    public Animator anim;
    public Rigidbody2D rb;
    public ProjectileHitDetector PHitDetect;

    public int maxLife;
    public int currentLife;

    public int maxHits;
    public int currentHits;

    public bool projectileActive = true;

    static int deactivateID;

    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 60;

        deactivateID = Animator.StringToHash("Deactivate");
    }

    // Update is called once per frame
    void Update()
    {
        if ((currentLife <= 0 || currentHits >= maxHits) && projectileActive)
        {
            anim.SetTrigger(deactivateID);
            projectileActive = false;
        }

        if (currentLife > 0 && PHitDetect.hitStop == 0)
            currentLife--;        
    }

    void Deactivate()
    {
        transform.gameObject.SetActive(false);
    }
}
