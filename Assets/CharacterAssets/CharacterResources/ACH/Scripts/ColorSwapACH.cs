using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorSwapACH : MonoBehaviour
{
    Texture2D mColorSwapTex;
    Color[] mSpriteColors;

    SpriteRenderer sprite;
    public HitDetector HitDetect;

    Color noColor;

    public int colorNum;

    float recoverFlashTimer = 0;
    float armorFlashTimer = 0;
    const float flashTime = .1f;

    public enum SwapIndex
    {
        Outline = 1,
        Skin = 200,
        Hair = 60,
        Seals = 85,
        Eyes = 141,
        Wrap = 94,
        ShoesGloves = 119,
        HipArmor = 129,
        HipCloth = 130,
        Belt = 80,
        BeltTrim = 170,
        Pants = 198,
        MetalOrnaments = 201,
        SpearheadSoles = 81,
        SpearBodyStraps = 97,
        SpearEdge = 230,
        SpearCloth = 134,
    }
    // Start is called before the first frame update
    void Start()
    {
        noColor = new Color32(0, 0, 0, 0);

        if (transform.parent.gameObject.name == "Sprite")
        {
            colorNum = transform.parent.GetComponent<ColorSwapACH>().colorNum;
            HitDetect = transform.parent.GetComponent<ColorSwapACH>().HitDetect;
        }
        sprite = transform.GetComponent<SpriteRenderer>();
        InitColorSwapTex();
        ApplyPalette();
    }

    // Update is called once per frame
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
        switch (colorNum)
        {
            case 4:
                {
                    ColorSwap(SwapIndex.Outline, new Color32(0, 0, 0, 255));
                    ColorSwap(SwapIndex.Hair, new Color32(47, 56, 133, 255));
                    ColorSwap(SwapIndex.Eyes, new Color32(202, 205, 248, 255));
                    ColorSwap(SwapIndex.Skin, new Color32(243, 188, 147, 255));
                    ColorSwap(SwapIndex.Seals, new Color32(51, 92, 140, 255));
                    ColorSwap(SwapIndex.Wrap, new Color32(193, 91, 111, 255));
                    ColorSwap(SwapIndex.ShoesGloves, new Color32(208, 96, 112, 255));
                    ColorSwap(SwapIndex.HipArmor, new Color32(93, 87, 173, 255));
                    ColorSwap(SwapIndex.HipCloth, new Color32(230, 200, 127, 255));
                    ColorSwap(SwapIndex.Belt, new Color32(132, 58, 55, 255));
                    ColorSwap(SwapIndex.BeltTrim, new Color32(253, 186, 95, 255));
                    ColorSwap(SwapIndex.Pants, new Color32(230, 138, 117, 255));
                    ColorSwap(SwapIndex.MetalOrnaments, new Color32(239, 155, 108, 255));
                    ColorSwap(SwapIndex.SpearheadSoles, new Color32(38, 39, 87, 255));
                    ColorSwap(SwapIndex.SpearBodyStraps, new Color32(57, 84, 91, 255));
                    ColorSwap(SwapIndex.SpearEdge, new Color32(184, 75, 116, 255));
                    ColorSwap(SwapIndex.SpearCloth, new Color32(208, 102, 102, 255));
                    break;
                }
            case 3:
                {
                ColorSwap(SwapIndex.Outline, new Color32(0, 0, 0, 255));
                ColorSwap(SwapIndex.Hair, new Color32(112, 78, 1, 255));
                ColorSwap(SwapIndex.Eyes, new Color32(234, 220, 21, 255));
                    ColorSwap(SwapIndex.Skin, new Color32(243, 188, 147, 255));
                    ColorSwap(SwapIndex.Seals, new Color32(145, 72, 185, 255));
                ColorSwap(SwapIndex.Wrap, new Color32(213, 140, 46, 255));
                ColorSwap(SwapIndex.ShoesGloves, new Color32(194, 111, 69, 255));
                ColorSwap(SwapIndex.HipArmor, new Color32(180, 77, 0, 255));
                ColorSwap(SwapIndex.HipCloth, new Color32(232, 205, 64, 255));
                ColorSwap(SwapIndex.Belt, new Color32(128, 36, 160, 255));
                ColorSwap(SwapIndex.BeltTrim, new Color32(246, 219, 116, 255));
                ColorSwap(SwapIndex.Pants, new Color32(229, 227, 189, 255));
                ColorSwap(SwapIndex.MetalOrnaments, new Color32(235, 195, 144, 255));
                ColorSwap(SwapIndex.SpearheadSoles, new Color32(214, 167, 39, 255));
                ColorSwap(SwapIndex.SpearBodyStraps, new Color32(164, 66, 37, 255));
                ColorSwap(SwapIndex.SpearEdge, new Color32(252, 212, 130, 255));
                ColorSwap(SwapIndex.SpearCloth, new Color32(180, 76, 195, 255));
                break;
            }
            case 2:
                {
                    ColorSwap(SwapIndex.Outline, new Color32(0, 0, 0, 255));
                    ColorSwap(SwapIndex.Hair, new Color32(240, 222, 171, 255));
                    ColorSwap(SwapIndex.Eyes, new Color32(140, 198, 230, 255));
                    ColorSwap(SwapIndex.Skin, new Color32(255, 224, 209, 255));
                    ColorSwap(SwapIndex.Seals, new Color32(229, 44, 46, 255));
                    ColorSwap(SwapIndex.Wrap, new Color32(227, 222, 220, 255));
                    ColorSwap(SwapIndex.ShoesGloves, new Color32(199, 152, 123, 255));
                    ColorSwap(SwapIndex.HipArmor, new Color32(164, 117, 95, 255));
                    ColorSwap(SwapIndex.HipCloth, new Color32(200, 62, 63, 255));
                    ColorSwap(SwapIndex.Belt, new Color32(65, 130, 175, 255));
                    ColorSwap(SwapIndex.BeltTrim, new Color32(227, 222, 220, 255));
                    ColorSwap(SwapIndex.Pants, new Color32(221, 209, 196, 255));
                    ColorSwap(SwapIndex.MetalOrnaments, new Color32(235, 195, 144, 255));
                    ColorSwap(SwapIndex.SpearheadSoles, new Color32(113, 84, 70, 255));
                    ColorSwap(SwapIndex.SpearBodyStraps, new Color32(113, 84, 70, 255));
                    ColorSwap(SwapIndex.SpearEdge, new Color32(230, 230, 230, 255));
                    ColorSwap(SwapIndex.SpearCloth, new Color32(229, 50, 51, 255));
                    break;
                }
            default:
                {
                    //original color
                    ColorSwap(SwapIndex.Outline, new Color32(0, 0, 0, 255));
                    ColorSwap(SwapIndex.Hair, new Color32(64, 59, 56, 255));
                    ColorSwap(SwapIndex.Eyes, new Color32(175, 134, 90, 255));
                    ColorSwap(SwapIndex.Skin, new Color32(243, 188, 147, 255));
                    ColorSwap(SwapIndex.Seals, new Color32(51, 92, 140, 255));
                    ColorSwap(SwapIndex.Wrap, new Color32(132, 78, 74, 255));
                    ColorSwap(SwapIndex.ShoesGloves, new Color32(130, 114, 114, 255));
                    ColorSwap(SwapIndex.HipArmor, new Color32(164, 117, 97, 255));
                    ColorSwap(SwapIndex.HipCloth, new Color32(166, 115, 114, 255));
                    ColorSwap(SwapIndex.Belt, new Color32(132, 58, 55, 255));
                    ColorSwap(SwapIndex.BeltTrim, new Color32(195, 166, 124, 255));
                    ColorSwap(SwapIndex.Pants, new Color32(197, 200, 193, 255));
                    ColorSwap(SwapIndex.MetalOrnaments, new Color32(235, 195, 144, 255));
                    ColorSwap(SwapIndex.SpearheadSoles, new Color32(81, 81, 81, 255));
                    ColorSwap(SwapIndex.SpearBodyStraps, new Color32(129, 84, 81, 255));
                    ColorSwap(SwapIndex.SpearEdge, new Color32(230, 230, 230, 255));
                    ColorSwap(SwapIndex.SpearCloth, new Color32(208, 102, 102, 255));
                    break;
                }
        }

        mColorSwapTex.Apply();
    }
}
