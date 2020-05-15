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
    Color noColor;

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
        CorruptedEyes = 57
    }

    // Start is called before the first frame update
    void Start()
    {
        noColor = new Color32(0, 0, 0, 0);
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
            if (i == 0 || i == 255)
                colorSwapTex.SetPixel(i, 0, new Color32(0, 0, 0, 0));
            else
                colorSwapTex.SetPixel(i, 0, new Color32(0, 0, 0, 255));
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
            if (i == 0 || i == 255)
                mColorSwapTex.SetPixel(i, 0, new Color32(0, 0, 0, 0));
            else
                mColorSwapTex.SetPixel(i, 0, new Color32(0, 0, 0, 255));
            if (mSpriteColors[i] != noColor)
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
        switch(colorNum)
        {
            case 5:
                {
                    ColorSwap(SwapIndex.Outline, new Color32(0, 0, 0, 255));
                    ColorSwap(SwapIndex.Cape, new Color32(255, 201, 0, 255));
                    ColorSwap(SwapIndex.Hair, new Color32(244, 248, 132, 255));
                    ColorSwap(SwapIndex.Eyebrows, new Color32(244, 248, 132, 255));
                    ColorSwap(SwapIndex.Eyes, new Color32(214, 180, 0, 255));
                    ColorSwap(SwapIndex.Skin, new Color32(255, 224, 209, 255));
                    ColorSwap(SwapIndex.Shirt, new Color32(59, 59, 59, 255));
                    ColorSwap(SwapIndex.Corset, new Color32(232, 211, 29, 255));
                    ColorSwap(SwapIndex.CorsetTrim, new Color32(78, 211, 53, 255));
                    ColorSwap(SwapIndex.Sleeve1, new Color32(214, 172, 66, 255));
                    ColorSwap(SwapIndex.Sleeve2, new Color32(35, 35, 35, 255));
                    ColorSwap(SwapIndex.Sleeve3, new Color32(191, 135, 33, 255));
                    ColorSwap(SwapIndex.SolesBraces, new Color32(191, 135, 33, 255));
                    ColorSwap(SwapIndex.Shoulders, new Color32(35, 35, 35, 255));
                    ColorSwap(SwapIndex.Straps, new Color32(82, 161, 67, 255));
                    ColorSwap(SwapIndex.Basket, new Color32(199, 199, 172, 255));
                    ColorSwap(SwapIndex.Lining, new Color32(87, 173, 61, 255));
                    ColorSwap(SwapIndex.GlovesBoots, new Color32(238, 202, 57, 255));
                    ColorSwap(SwapIndex.BootTrim, new Color32(34, 176, 8, 255));
                    ColorSwap(SwapIndex.HairRibbon, new Color32(82, 161, 67, 255));
                    ColorSwap(SwapIndex.Blades, new Color32(205, 215, 222, 255));
                    ColorSwap(SwapIndex.Sin, new Color32(243, 195, 31, 255));
                    ColorSwap(SwapIndex.SinHighlight, new Color32(96, 170, 0, 255));
                    ColorSwap(SwapIndex.Pastry, new Color32(193, 107, 58, 255));
                    ColorSwap(SwapIndex.CorruptedEyes, new Color32(57, 57, 57, 255));
                    break;
                }
            case 4:
                {
                    ColorSwap(SwapIndex.Outline, new Color32(0, 0, 0, 255));
                    ColorSwap(SwapIndex.Cape, new Color32(48, 113, 211, 255));
                    ColorSwap(SwapIndex.Hair, new Color32(198, 209, 223, 255));
                    ColorSwap(SwapIndex.Eyebrows, new Color32(198, 209, 223, 255));
                    ColorSwap(SwapIndex.Eyes, new Color32(78, 80, 87, 255));
                    ColorSwap(SwapIndex.Skin, new Color32(255, 238, 226, 255));
                    ColorSwap(SwapIndex.Shirt, new Color32(59, 99, 161, 255));
                    ColorSwap(SwapIndex.Corset, new Color32(102, 131, 151, 255));
                    ColorSwap(SwapIndex.CorsetTrim, new Color32(35, 41, 46, 255));
                    ColorSwap(SwapIndex.Sleeve1, new Color32(109, 125, 152, 255));
                    ColorSwap(SwapIndex.Sleeve2, new Color32(120, 152, 193, 255));
                    ColorSwap(SwapIndex.Sleeve3, new Color32(45, 54, 67, 255));
                    ColorSwap(SwapIndex.SolesBraces, new Color32(185, 181, 182, 255));
                    ColorSwap(SwapIndex.Shoulders, new Color32(185, 181, 182, 255));
                    ColorSwap(SwapIndex.Straps, new Color32(143, 163, 193, 255));
                    ColorSwap(SwapIndex.Basket, new Color32(77, 87, 105, 255));
                    ColorSwap(SwapIndex.Lining, new Color32(142, 156, 179, 255));
                    ColorSwap(SwapIndex.GlovesBoots, new Color32(208, 228, 240, 255));
                    ColorSwap(SwapIndex.BootTrim, new Color32(177, 195, 220, 255));
                    ColorSwap(SwapIndex.HairRibbon, new Color32(65, 130, 175, 255));
                    ColorSwap(SwapIndex.Blades, new Color32(255, 98, 133, 255));
                    ColorSwap(SwapIndex.Sin, new Color32(126, 163, 220, 255));
                    ColorSwap(SwapIndex.SinHighlight, new Color32(203, 205, 208, 255));
                    ColorSwap(SwapIndex.Pastry, new Color32(193, 107, 58, 255));
                    ColorSwap(SwapIndex.CorruptedEyes, new Color32(57, 57, 57, 255));
                    break;
                }
            case 3:
                {
                    ColorSwap(SwapIndex.Outline, new Color32(0, 0, 0, 255));
                    ColorSwap(SwapIndex.Cape, new Color32(168, 81, 165, 255));
                    ColorSwap(SwapIndex.Hair, new Color32(123, 65, 65, 255));
                    ColorSwap(SwapIndex.Eyebrows, new Color32(123, 65, 65, 255));
                    ColorSwap(SwapIndex.Eyes, new Color32(197, 157, 241, 255));
                    ColorSwap(SwapIndex.Skin, new Color32(255, 224, 209, 255));
                    ColorSwap(SwapIndex.Shirt, new Color32(211, 194, 211, 255));
                    ColorSwap(SwapIndex.Corset, new Color32(88, 81, 60, 255));
                    ColorSwap(SwapIndex.CorsetTrim, new Color32(150, 112, 225, 255));
                    ColorSwap(SwapIndex.Sleeve1, new Color32(105, 36, 61, 255));
                    ColorSwap(SwapIndex.Sleeve2, new Color32(216, 187, 217, 255));
                    ColorSwap(SwapIndex.Sleeve3, new Color32(59, 33, 43, 255));
                    ColorSwap(SwapIndex.SolesBraces, new Color32(59, 33, 43, 255));
                    ColorSwap(SwapIndex.Shoulders, new Color32(135, 132, 114, 255));
                    ColorSwap(SwapIndex.Straps, new Color32(97, 51, 97, 255));
                    ColorSwap(SwapIndex.Basket, new Color32(97, 89, 66, 255));
                    ColorSwap(SwapIndex.Lining, new Color32(111, 3, 43, 255));
                    ColorSwap(SwapIndex.GlovesBoots, new Color32(132, 103, 120, 255));
                    ColorSwap(SwapIndex.BootTrim, new Color32(179, 127, 164, 255));
                    ColorSwap(SwapIndex.HairRibbon, new Color32(158, 116, 132, 255));
                    ColorSwap(SwapIndex.Blades, new Color32(255, 98, 133, 255));
                    ColorSwap(SwapIndex.Sin, new Color32(196, 90, 232, 255));
                    ColorSwap(SwapIndex.SinHighlight, new Color32(243, 165, 227, 255));
                    ColorSwap(SwapIndex.Pastry, new Color32(193, 107, 58, 255));
                    ColorSwap(SwapIndex.CorruptedEyes, new Color32(57, 57, 57, 255));
                    break;
                }
            case 2:
                {
                    ColorSwap(SwapIndex.Outline, new Color32(0, 0, 0, 255));
                    ColorSwap(SwapIndex.Cape, new Color32(166, 115, 114, 255));
                    ColorSwap(SwapIndex.Hair, new Color32(64, 59, 56, 255));
                    ColorSwap(SwapIndex.Eyebrows, new Color32(64, 59, 56, 255));
                    ColorSwap(SwapIndex.Eyes, new Color32(175, 134, 90, 255));
                    ColorSwap(SwapIndex.Skin, new Color32(243, 188, 147, 255));
                    ColorSwap(SwapIndex.Shirt, new Color32(230, 230, 230, 255));
                    ColorSwap(SwapIndex.Corset, new Color32(132, 78, 74, 255));
                    ColorSwap(SwapIndex.CorsetTrim, new Color32(201, 169, 123, 255));
                    ColorSwap(SwapIndex.Sleeve1, new Color32(81, 81, 81, 255));
                    ColorSwap(SwapIndex.Sleeve2, new Color32(197, 200, 193, 255));
                    ColorSwap(SwapIndex.Sleeve3, new Color32(129, 84, 81, 255));
                    ColorSwap(SwapIndex.SolesBraces, new Color32(129, 84, 81, 255));
                    ColorSwap(SwapIndex.Shoulders, new Color32(164, 117, 95, 255));
                    ColorSwap(SwapIndex.Straps, new Color32(51, 92, 140, 255));
                    ColorSwap(SwapIndex.Basket, new Color32(85, 65, 59, 255));
                    ColorSwap(SwapIndex.Lining, new Color32(209, 102, 103, 255));
                    ColorSwap(SwapIndex.GlovesBoots, new Color32(130, 114, 114, 255));
                    ColorSwap(SwapIndex.BootTrim, new Color32(164, 117, 97, 255));
                    ColorSwap(SwapIndex.HairRibbon, new Color32(65, 130, 175, 255));
                    ColorSwap(SwapIndex.Blades, new Color32(230, 230, 230, 255));
                    ColorSwap(SwapIndex.Sin, new Color32(255, 120, 180, 255));
                    ColorSwap(SwapIndex.SinHighlight, new Color32(208, 150, 150, 255));
                    ColorSwap(SwapIndex.Pastry, new Color32(193, 107, 58, 255));
                    ColorSwap(SwapIndex.CorruptedEyes, new Color32(57, 57, 57, 255));
                    break;
                }
            default:
                {
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
                    ColorSwap(SwapIndex.CorruptedEyes, new Color32(57, 57, 57, 255));
                    break;
                }
        }
        mColorSwapTex.Apply();
    }
}
