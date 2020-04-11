using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSelectSprites : MonoBehaviour
{
    public Animator P1DHAanim;
    public ColorSwapDHA P1Dhalia;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        P1Dhalia.InitColorSwapTex();
        P1Dhalia.ApplyPalette();
    }

    //Blank Functions to prevent errors
    public void TurnAroundCheck() { }
    public void EnableAll() { }
    public void Standing() { }
    public void ClearHitBox() { }
}
