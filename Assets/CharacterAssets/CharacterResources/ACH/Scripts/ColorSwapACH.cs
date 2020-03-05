using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorSwapACH : MonoBehaviour
{
    Texture2D mColorSwapTex;
    Color[] mSpriteColors;

    SpriteRenderer sprite;
    public HitDetector HitDetect;

    public int colorNum;

    float recoverFlashTimer = 0;
    float armorFlashTimer = 0;
    const float flashTime = .1f;

    public enum SwapIndex
    {
        Outline = 1,
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
