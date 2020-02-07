using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileProperties : MonoBehaviour
{
    public Animator anim;
    public Rigidbody2D rb;
    public ProjectileHitDetector PHitDetect;

    PauseMenu pauseScreen;

    public int maxLife;
    public int currentLife;

    public int maxHits;
    public int currentHits;

    public bool projectileActive = true;
    public bool hasLifeSpan = true;

    static int deactivateID;

    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 60;

        deactivateID = Animator.StringToHash("Deactivate");

        pauseScreen = GameObject.Find("PauseManager").GetComponentInChildren<PauseMenu>();
    }

    // Update is called once per frame
    void Update()
    {
        if ((currentLife <= 0 || currentHits >= maxHits) && projectileActive)
        {
            anim.SetTrigger(deactivateID);
            projectileActive = false;
        }

        if (projectileActive && currentLife > 0 && PHitDetect.hitStop == 0 && hasLifeSpan && PHitDetect.Actions.blitzed % 2 == 0 && !pauseScreen.isPaused)
            currentLife--;        
    }

    public void Deactivate()
    {
        transform.gameObject.SetActive(false);
    }
}
