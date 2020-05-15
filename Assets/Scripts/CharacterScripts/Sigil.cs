using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sigil : MonoBehaviour
{
    public SpriteRenderer sigil;
    public AudioSource bounce;
    public HitDetector HitDetect;
    public Color32 tint;
    public float colorChange = 1;
    public float scaleChange = 0;
    public bool rotate = true;

    private void Start()
    {
        HitDetect = transform.root.GetChild(0).GetComponent<MovementHandler>().HitDetect;
    }
    // Update is called once per frame
    void Update()
    {
        if (rotate)
            transform.Rotate(20 * Vector3.forward * Time.deltaTime);
        if (!HitDetect.pauseScreen.isPaused)
            scaleChange += .3f;
        if (transform.eulerAngles.x >= 60)
        {
            if (transform.position.y < .35f)
                transform.position = new Vector3(transform.position.x, .35f, transform.position.z);
            if (transform.position.x < -10f)
                transform.position = new Vector3(-10, transform.position.y, transform.position.z);
            if (transform.position.x > 10f)
                transform.position = new Vector3(10, transform.position.y, transform.position.z);
        }
        else if (transform.eulerAngles.y == 90)
        {
            if (transform.position.y < 1f)
                transform.position = new Vector3(transform.position.x, 1f, transform.position.z);
            if (transform.position.x < -10f)
                transform.position = new Vector3(-10, transform.position.y, transform.position.z);
            if (transform.position.x > 10f)
                transform.position = new Vector3(10, transform.position.y, transform.position.z);
        }

        transform.localScale = Vector3.Lerp(new Vector3(0, 0, 1), new Vector3(.15f, .15f, 1), scaleChange);
        if (transform.localScale == new Vector3(.15f, .15f, 1) && !HitDetect.pauseScreen.isPaused)
        {
            colorChange += .02f;
        }
        sigil.color = Color.Lerp(Color.white, tint, colorChange);
    }

    public void Play()
    {
        bounce.PlayOneShot(bounce.clip, .8f);
    }

    public void WallBouncePlay()
    {
        if (!bounce.isPlaying)
            bounce.Play();
    }
}
