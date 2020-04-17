using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorSwapDHA : MonoBehaviour
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
        Cape = 103,
        Hair = 222,
        Eyebrows = 196, 
        Eyes = 184,
        Skin = 232,
        Shirt = 223,
        Corset = 83,
        CorsetTrim = 173,
        Sleeve1 = 92,
        Sleeve2 = 211,
        Sleeve3 = 60,
        SolesBraces = 91,
        Shoulders = 129,
        Straps = 71,
        Basket = 182,
        Lining = 134,
        GlovesBoots = 163,
        BootTrim = 220,
        HairRibbon = 116,
        Blades = 213,
        Sin = 114,
        SinHighlight = 177,
        Pastry = 127,
    }

    // Start is called before the first frame update
    void Start()
    {
        if (transform.parent.gameObject.name == "Sprite")
        {
            colorNum = transform.parent.GetComponent<ColorSwapDHA>().colorNum;
            HitDetect = transform.parent.GetComponent<ColorSwapDHA>().HitDetect;
        }
        sprite = transform.GetComponent<SpriteRenderer>();
        InitColorSwapTex();
        ApplyPalette();
    }

    void Update()
    {
        if (recoverFlashTimer > 0)
        {
            recoverFlashTimer -= Time.deltaTime;
            if (recoverFlashTimer <= 0)
                ResetSpriteColors();
        }
        if (armorFlashTimer > 0)
        {
            armorFlashTimer -= Time.deltaTime;
            if (armorFlashTimer <= 0)
                ResetSpriteColors();
        }
        if (HitDetect.armorHit && gameObject.name == "Sprite")
        {
            StartArmorHitFlash();
            HitDetect.armorHit = false;
        }
        if (HitDetect.justDefense && gameObject.name == "Sprite")
        {
            StartRecoverFlash();

            HitDetect.justDefense = false;
        }
    }

    public void InitColorSwapTex()
    {
        Texture2D colorSwapTex = new Texture2D(256, 1, TextureFormat.RGBA32, false, false);
        colorSwapTex.filterMode = FilterMode.Point;

        for (int i = 0; i < colorSwapTex.width; i++)
        {
            if (i != 0 && i != 255)
                colorSwapTex.SetPixel(i, 0, new Color32(0, 0, 0, 255));
            else
                colorSwapTex.SetPixel(i, 0, new Color32(0, 0, 0, 0));
        }
        colorSwapTex.Apply();

        sprite.material.SetTexture("_SwapTex", colorSwapTex);

        mSpriteColors = new Color[colorSwapTex.width];
        mColorSwapTex = colorSwapTex;
    }

    public void ColorSwap(SwapIndex index, Color32 color)
    {
        mSpriteColors[(int)index] = color;
        mColorSwapTex.SetPixel((int)index, 0, color);
    }

    public void SwapAllColorsTemp(Color color)
    {
        for (int i = 0; i < mColorSwapTex.width; i++)
        {
            mColorSwapTex.SetPixel(i, 0, color);
        }
        mColorSwapTex.Apply();
    }

    public void ResetSpriteColors()
    {
        for (int i = 0; i < mColorSwapTex.width; i++)
        {
            mColorSwapTex.SetPixel(i, 0, mSpriteColors[i]);
        }
        mColorSwapTex.Apply();
    }

    public void StartRecoverFlash()
    {
        recoverFlashTimer = flashTime;
        SwapAllColorsTemp(Color.white);
    }

    public void StartArmorHitFlash()
    {
        armorFlashTimer = flashTime;
        SwapAllColorsTemp(Color.red);
    }

    public void ApplyPalette()
    {
        if (colorNum == 2)
        {
            ColorSwap(SwapIndex.Outline, new Color32(0, 0, 0, 255));
            ColorSwap(SwapIndex.Cape, new Color32(49, 62, 108, 255));
            ColorSwap(SwapIndex.Hair, new Color32(240, 222, 171, 255));
            ColorSwap(SwapIndex.Eyebrows, new Color32(215, 196, 146, 255));
            ColorSwap(SwapIndex.Eyes, new Color32(140, 198, 230, 255));
            ColorSwap(SwapIndex.Skin, new Color32(255, 224, 209, 255));
            ColorSwap(SwapIndex.Shirt, new Color32(227, 222, 220, 255));
            ColorSwap(SwapIndex.Corset, new Color32(101, 77, 70, 255));
            ColorSwap(SwapIndex.CorsetTrim, new Color32(201, 169, 123, 255));
            ColorSwap(SwapIndex.Sleeve1, new Color32(101, 89, 86, 255));
            ColorSwap(SwapIndex.Sleeve2, new Color32(221, 209, 196, 255));
            ColorSwap(SwapIndex.Sleeve3, new Color32(63, 59, 55, 255));
            ColorSwap(SwapIndex.SolesBraces, new Color32(113, 84, 70, 255));
            ColorSwap(SwapIndex.Shoulders, new Color32(100, 100, 120, 255));
            ColorSwap(SwapIndex.Straps, new Color32(100, 148, 255, 255));
            ColorSwap(SwapIndex.Basket, new Color32(211, 174, 145, 255));
            ColorSwap(SwapIndex.Lining, new Color32(209, 102, 103, 255));
            ColorSwap(SwapIndex.GlovesBoots, new Color32(199, 152, 123, 255));
            ColorSwap(SwapIndex.BootTrim, new Color32(236, 218, 189, 255));
            ColorSwap(SwapIndex.HairRibbon, new Color32(255, 100, 53, 255));
            ColorSwap(SwapIndex.Blades, new Color32(255, 78, 78, 255));
            ColorSwap(SwapIndex.Sin, new Color32(55, 80, 255, 255));
            ColorSwap(SwapIndex.SinHighlight, new Color32(144, 145, 255, 255));
            ColorSwap(SwapIndex.Pastry, new Color32(193, 107, 58, 255));
        }
        else
        {
            //original color
            ColorSwap(SwapIndex.Outline, new Color32(0, 0, 0, 255));
            ColorSwap(SwapIndex.Cape, new Color32(200, 62, 63, 255));
            ColorSwap(SwapIndex.Hair, new Color32(240, 222, 171, 255));
            ColorSwap(SwapIndex.Eyebrows, new Color32(215, 196, 146, 255));
            ColorSwap(SwapIndex.Eyes, new Color32(140, 198, 230, 255));
            ColorSwap(SwapIndex.Skin, new Color32(255, 224, 209, 255));
            ColorSwap(SwapIndex.Shirt, new Color32(227, 222, 220, 255));
            ColorSwap(SwapIndex.Corset, new Color32(101, 77, 70, 255));
            ColorSwap(SwapIndex.CorsetTrim, new Color32(201, 169, 123, 255));
            ColorSwap(SwapIndex.Sleeve1, new Color32(101, 89, 86, 255));
            ColorSwap(SwapIndex.Sleeve2, new Color32(221, 209, 196, 255));
            ColorSwap(SwapIndex.Sleeve3, new Color32(63, 59, 55, 255));
            ColorSwap(SwapIndex.SolesBraces, new Color32(113, 84, 70, 255));
            ColorSwap(SwapIndex.Shoulders, new Color32(164, 117, 95, 255));
            ColorSwap(SwapIndex.Straps, new Color32(148, 38, 43, 255));
            ColorSwap(SwapIndex.Basket, new Color32(211, 174, 145, 255));
            ColorSwap(SwapIndex.Lining, new Color32(209, 102, 103, 255));
            ColorSwap(SwapIndex.GlovesBoots, new Color32(199, 152, 123, 255));
            ColorSwap(SwapIndex.BootTrim, new Color32(236, 218, 189, 255));
            ColorSwap(SwapIndex.HairRibbon, new Color32(65, 130, 175, 255));
            ColorSwap(SwapIndex.Blades, new Color32(205, 215, 222, 255));
            ColorSwap(SwapIndex.Sin, new Color32(255, 50, 53, 255));
            ColorSwap(SwapIndex.SinHighlight, new Color32(255, 144, 145, 255));
            ColorSwap(SwapIndex.Pastry, new Color32(193, 107, 58, 255));
        }

        mColorSwapTex.Apply();
    }
}
