using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSelectSprites : MonoBehaviour
{
    public GameObject Sprite;
    private string Character;

    // Start is called before the first frame update
    void Start()
    {
        Character = Sprite.name;
    }

    // Update is called once per frame
    void Update()
    {
        switch (Character)
        {
            case "Dhalia":
                Sprite.GetComponent<ColorSwapDHA>().InitColorSwapTex();
                Sprite.GetComponent<ColorSwapDHA>().ApplyPalette();
                break;
            case "Achealis":
                Sprite.GetComponent<ColorSwapACH>().InitColorSwapTex();
                Sprite.GetComponent<ColorSwapACH>().ApplyPalette();
                break;
        }
    }

    //Blank Functions to prevent errors
    public void TurnAroundCheck() { }
    public void EnableAll() { }
    public void Standing() { }
    public void ClearHitBox() { }
}
