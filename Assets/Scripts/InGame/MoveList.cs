using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoveList : MonoBehaviour
{
    //Asset Images Database
    public Image arrow;
    public Image plus;
    public Image LightButton;
    public Image MediumButton;
    public Image HeavyButton;
    public Image BreakButton;
    public Image Blank;

    //Text and Images to be dynamically changed
    public Text characterIdentifier;
    public Text pageIdentifier;
    public Image pageMoveIconL;
    public Image pageMoveIconR;
    public Image carouselSelected;
    public GameObject selectionMarker;
    public GameObject MovesList;

    public Text Move1;
    public Text Description1;
    public Image move1Input1;
    public Image move1Input2;
    public Image move1Input3;
    public Image move1Input4;
    public Image move1Input5;
    public Image move1Input6;
    public Image move1Input7;
    public Text or1Text;
    public Text inAirText;
    public GameObject Arrows1;
    public Image Arrows1L;
    public Image Arrows1R;

    public Text Move2;
    public Text Description2;
    public GameObject Move2Break;
    public Image move2Input1;
    public Image move2Input2;
    public Image move2Input3;
    public Image move2Input4;
    public Image move2Input5;
    public Image move2Input6;
    public Image move2Input7;
    public Text or2Text;
    public Text inAir2Text;

    public Text Move3;
    public GameObject Move3Break;
    public Text Description3;
    public Image move3Input1;
    public Image move3Input2;
    public Image move3Input3;
    public Image move3Input4;
    public Image move3Input5;
    public Image move3Input6;
    public Image move3Input7;
    public Text or3Text;
    public Text inAir3Text;

    public Text Move4;
    public Text Description4;
    public GameObject Move4Break;
    public Image move4Input1;
    public Image move4Input2;
    public Image move4Input3;
    public Image move4Input4;
    public Image move4Input5;
    public Image move4Input6;
    public Image move4Input7;
    public Text or4Text;
    public Text inAir4Text;

    public Text Move5;
    public Text Description5;
    public GameObject Move5Break;
    public Image move5Input1;
    public Image move5Input2;
    public Image move5Input3;
    public Image move5Input4;
    public Image move5Input5;
    public Image move5Input6;
    public Image move5Input7;
    public Text or5Text;
    public Text inAir5Text;

    public Text Move6;
    public Text Description6;
    public GameObject Move6Break;
    public Image move6Input1;
    public Image move6Input2;
    public Image move6Input3;
    public Image move6Input4;
    public Image move6Input5;
    public Image move6Input6;
    public Image move6Input7;
    public Text or6Text;
    public Text inAir6Text;

    //Carousel References
    public Image carousel1;
    public Image carousel2;
    public Image carousel3;
    public Image carousel4;

    //Reference for Vertical Scrolling
    public int maxVerticalIndex;

    //Functions to set pages
    public void setCharacter(string character)
    {
        characterIdentifier.text = character;
    }

    public void setPage(string page)
    {
        pageIdentifier.text = page;
    }

    public bool topCheck()
    {
        if (selectionMarker.GetComponent<RectTransform>().localPosition.y < 121.3f)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    public bool bottomCheck()
    {
        if (selectionMarker.GetComponent<RectTransform>().localPosition.y > -94.5f)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    //Functions to move selection marker
    public void moveMarkerUp()
    {
        if (selectionMarker.GetComponent<RectTransform>().localPosition.y < 121.3f)
        {
            selectionMarker.GetComponent<RectTransform>().localPosition += new Vector3(0, 72f, 0);
        }
    }

    public void moveMarkerDown()
    {
        if (selectionMarker.GetComponent<RectTransform>().localPosition.y > -94.5)
        {
            selectionMarker.GetComponent<RectTransform>().localPosition -= new Vector3(0, 72f, 0);
        }
    }

    public void disableMarker()
    {
        selectionMarker.SetActive(false);
    }

    public void enableMarker()
    {
        selectionMarker.SetActive(true);
    }

    public void setMarkerPosition(int maxNumber)
    {
        switch (maxNumber)
        {
            case 1:
                selectionMarker.GetComponent<RectTransform>().localPosition = new Vector3(-115.52f, 121.4f, 0);
                break;
            case 2:
                selectionMarker.GetComponent<RectTransform>().localPosition = new Vector3(-115.52f, 49.4f, 0);
                break;
            case 3:
                selectionMarker.GetComponent<RectTransform>().localPosition = new Vector3(-115.52f, -22.6f, 0);
                break;
            case 4:
                selectionMarker.GetComponent<RectTransform>().localPosition = new Vector3(-115.52f, -94.6f, 0);
                break;
        }
    }

    public void resetMarker()
    {
        selectionMarker.GetComponent<RectTransform>().localPosition = new Vector3(-115.52f, 121.4f, 0);
    }

    public void moveListUp()
    {
        MovesList.GetComponent<RectTransform>().localPosition += new Vector3(0, 47f, 0);
    }

    public void moveListDown()
    {
        MovesList.GetComponent<RectTransform>().localPosition += new Vector3(0, -47f, 0);
    }

    public void resetListPosition()
    {
        MovesList.GetComponent<RectTransform>().localPosition = new Vector3(0, 0, 0);
    }

    //Dhalia's Movelist pages
    public void setDhaliaPage1()
    {
        setCharacter("Dhalia Thorne");
        setPage("Command Normals");
        pageMoveIconL.color = new Color(1f, 1f, 1f, .5f);
        pageMoveIconR.color = new Color(1f, 1f, 1f, 1f);
        Move1.text = "Normal 1";
        Description1.text = "Can be used repeatedly.";
        or1Text.text = "";
        inAirText.text = "";
        Arrows1.SetActive(false);
        move1Input1.sprite = arrow.sprite;
        move1Input1.transform.rotation = Quaternion.Euler(0, 0, 0);
        move1Input2.sprite = plus.sprite;
        move1Input2.transform.rotation = Quaternion.Euler(0, 0, 0);
        move1Input3.sprite = LightButton.sprite;
        move1Input3.transform.rotation = Quaternion.Euler(0, 0, 0);
        move1Input4.sprite = Blank.sprite;
        move1Input4.transform.rotation = Quaternion.Euler(0, 0, 0);
        move1Input5.sprite = Blank.sprite;
        move1Input5.transform.rotation = Quaternion.Euler(0, 0, 0);
        move1Input6.sprite = Blank.sprite;
        move1Input6.transform.rotation = Quaternion.Euler(0, 0, 0);
        move1Input7.sprite = Blank.sprite;
        move1Input7.transform.rotation = Quaternion.Euler(0, 0, 0);
        move1Input7.rectTransform.localPosition = new Vector3(295.23f, move1Input7.rectTransform.localPosition.y, 0);
        Move2.text = "Normal 2";
        Description2.text = "Hold B  to Charge attack.";
        or2Text.text = "";
        inAir2Text.text = "";
        Move2Break.SetActive(true);
        move2Input1.sprite = arrow.sprite;
        move2Input1.transform.rotation = Quaternion.Euler(0, 0, 0);
        move2Input2.sprite = plus.sprite;
        move2Input2.transform.rotation = Quaternion.Euler(0, 0, 0);
        move2Input3.sprite = BreakButton.sprite;
        move2Input3.transform.rotation = Quaternion.Euler(0, 0, 0);
        move2Input4.sprite = Blank.sprite;
        move2Input4.transform.rotation = Quaternion.Euler(0, 0, 0);
        move2Input5.sprite = Blank.sprite;
        move2Input5.transform.rotation = Quaternion.Euler(0, 0, 0);
        move2Input6.sprite = Blank.sprite;
        move2Input6.transform.rotation = Quaternion.Euler(0, 0, 0);
        move2Input7.sprite = Blank.sprite;
        move2Input7.transform.rotation = Quaternion.Euler(0, 0, 0);
        Move3.text = "";
        Move3Break.SetActive(false);
        Description3.text = "";
        or3Text.text = "";
        inAir3Text.text = "";
        move3Input1.sprite = Blank.sprite;
        move3Input1.transform.rotation = Quaternion.Euler(0, 0, 0);
        move3Input2.sprite = Blank.sprite;
        move3Input2.transform.rotation = Quaternion.Euler(0, 0, 0);
        move3Input3.sprite = Blank.sprite;
        move3Input3.transform.rotation = Quaternion.Euler(0, 0, 0);
        move3Input4.sprite = Blank.sprite;
        move3Input4.transform.rotation = Quaternion.Euler(0, 0, 0);
        move3Input5.sprite = Blank.sprite;
        move3Input5.transform.rotation = Quaternion.Euler(0, 0, 0);
        move3Input6.sprite = Blank.sprite;
        move3Input6.transform.rotation = Quaternion.Euler(0, 0, 0);
        move3Input7.sprite = Blank.sprite;
        move3Input7.transform.rotation = Quaternion.Euler(0, 0, 0);
        Move4.text = "";
        Description4.text = "";
        or4Text.text = "";
        inAir4Text.text = "";
        Move4Break.SetActive(false);
        move4Input1.sprite = Blank.sprite;
        move4Input1.transform.rotation = Quaternion.Euler(0, 0, 0);
        move4Input2.sprite = Blank.sprite;
        move4Input2.transform.rotation = Quaternion.Euler(0, 0, 0);
        move4Input3.sprite = Blank.sprite;
        move4Input3.transform.rotation = Quaternion.Euler(0, 0, 0);
        move4Input4.sprite = Blank.sprite;
        move4Input4.transform.rotation = Quaternion.Euler(0, 0, 0);
        move4Input5.sprite = Blank.sprite;
        move4Input5.transform.rotation = Quaternion.Euler(0, 0, 0);
        move4Input6.sprite = Blank.sprite;
        move4Input6.transform.rotation = Quaternion.Euler(0, 0, 0);
        move4Input7.sprite = Blank.sprite;
        move4Input7.transform.rotation = Quaternion.Euler(0, 0, 0);
        Move5.text = "";
        Description5.text = "";
        or5Text.text = "";
        inAir5Text.text = "";
        Move5Break.SetActive(false);
        move5Input1.sprite = Blank.sprite;
        move5Input1.transform.rotation = Quaternion.Euler(0, 0, 0);
        move5Input2.sprite = Blank.sprite;
        move5Input2.transform.rotation = Quaternion.Euler(0, 0, 0);
        move5Input3.sprite = Blank.sprite;
        move5Input3.transform.rotation = Quaternion.Euler(0, 0, 0);
        move5Input4.sprite = Blank.sprite;
        move5Input4.transform.rotation = Quaternion.Euler(0, 0, 0);
        move5Input5.sprite = Blank.sprite;
        move5Input5.transform.rotation = Quaternion.Euler(0, 0, 0);
        move5Input6.sprite = Blank.sprite;
        move5Input6.transform.rotation = Quaternion.Euler(0, 0, 0);
        move5Input7.sprite = Blank.sprite;
        move5Input7.transform.rotation = Quaternion.Euler(0, 0, 0);
        Move6.text = "";
        Description6.text = "";
        or6Text.text = "";
        inAir6Text.text = "";
        Move6Break.SetActive(false);
        move6Input1.sprite = Blank.sprite;
        move6Input1.transform.rotation = Quaternion.Euler(0, 0, 0);
        move6Input2.sprite = Blank.sprite;
        move6Input2.transform.rotation = Quaternion.Euler(0, 0, 0);
        move6Input3.sprite = Blank.sprite;
        move6Input3.transform.rotation = Quaternion.Euler(0, 0, 0);
        move6Input4.sprite = Blank.sprite;
        move6Input4.transform.rotation = Quaternion.Euler(0, 0, 0);
        move6Input5.sprite = Blank.sprite;
        move6Input5.transform.rotation = Quaternion.Euler(0, 0, 0);
        move6Input6.sprite = Blank.sprite;
        move6Input6.transform.rotation = Quaternion.Euler(0, 0, 0);
        move6Input7.sprite = Blank.sprite;
        move6Input7.transform.rotation = Quaternion.Euler(0, 0, 0);
        carouselSelected.transform.position = carousel1.transform.position;
        maxVerticalIndex = 3;
    }

    public void setDhaliaPage2()
    {
        setCharacter("Dhalia Thorne");
        setPage("Special Moves");
        pageMoveIconL.color = new Color(1f, 1f, 1f, 1f);
        pageMoveIconR.color = new Color(1f, 1f, 1f, 1f);
        Move1.text = "Patissiere";
        Description1.text = "Hold    or    to change trajectory.";
        or1Text.text = "";
        inAirText.text = "";
        Arrows1.SetActive(true);
        Arrows1L.transform.rotation = Quaternion.Euler(0, 0, 0);
        Arrows1R.transform.rotation = Quaternion.Euler(0, 0, -180);
        move1Input1.sprite = arrow.sprite;
        move1Input1.transform.rotation = Quaternion.Euler(0, 0, -90);
        move1Input2.sprite = arrow.sprite;
        move1Input2.transform.rotation = Quaternion.Euler(0, 0, -45);
        move1Input3.sprite = arrow.sprite;
        move1Input3.transform.rotation = Quaternion.Euler(0, 0, 0);
        move1Input4.sprite = plus.sprite;
        move1Input4.transform.rotation = Quaternion.Euler(0, 0, 0);
        move1Input5.sprite = LightButton.sprite;
        move1Input5.transform.rotation = Quaternion.Euler(0, 0, 0);
        move1Input6.sprite = Blank.sprite;
        move1Input6.transform.rotation = Quaternion.Euler(0, 0, 0);
        move1Input7.sprite = Blank.sprite;
        move1Input7.transform.rotation = Quaternion.Euler(0, 0, 0);
        move1Input7.rectTransform.localPosition = new Vector3(295.23f, move1Input7.rectTransform.localPosition.y, 0);
        or2Text.text = "";
        Move2.text = "Head Rush";
        Description2.text = "Hold B  to extend attack.";
        or2Text.text = "";
        inAir2Text.text = "";
        Move2Break.SetActive(true);
        move2Input1.sprite = arrow.sprite;
        move2Input1.transform.rotation = Quaternion.Euler(0, 0, -0);
        move2Input2.sprite = arrow.sprite;
        move2Input2.transform.rotation = Quaternion.Euler(0, 0, -45);
        move2Input3.sprite = arrow.sprite;
        move2Input3.transform.rotation = Quaternion.Euler(0, 0, -90);
        move2Input4.sprite = arrow.sprite;
        move2Input4.transform.rotation = Quaternion.Euler(0, 0, -135);
        move2Input5.sprite = arrow.sprite;
        move2Input5.transform.rotation = Quaternion.Euler(0, 0, -180);
        move2Input6.sprite = plus.sprite;
        move2Input6.transform.rotation = Quaternion.Euler(0, 0, 0);
        move2Input7.sprite = MediumButton.sprite;
        move2Input7.transform.rotation = Quaternion.Euler(0, 0, 0);
        Move3.text = "Mood Swing";
        Move3Break.SetActive(false);
        Description3.text = "";
        or3Text.text = "";
        inAir3Text.text = "(in air)";
        move3Input1.sprite = arrow.sprite;
        move3Input1.transform.rotation = Quaternion.Euler(0, 0, 0);
        move3Input2.sprite = arrow.sprite;
        move3Input2.transform.rotation = Quaternion.Euler(0, 0, -90);
        move3Input3.sprite = arrow.sprite;
        move3Input3.transform.rotation = Quaternion.Euler(0, 0, -45);
        move3Input4.sprite = plus.sprite;
        move3Input4.transform.rotation = Quaternion.Euler(0, 0, 0);
        move3Input5.sprite = MediumButton.sprite;
        move3Input5.transform.rotation = Quaternion.Euler(0, 0, 0);
        move3Input6.sprite = Blank.sprite;
        move3Input6.transform.rotation = Quaternion.Euler(0, 0, 0);
        move3Input7.sprite = Blank.sprite;
        move3Input7.transform.rotation = Quaternion.Euler(0, 0, 0);
        Move4.text = "Blood Brave";
        Description4.text = "Usable in the air.";
        or4Text.text = "";
        inAir4Text.text = "";
        Move4Break.SetActive(false);
        move4Input1.sprite = arrow.sprite;
        move4Input1.transform.rotation = Quaternion.Euler(0, 0, -90);
        move4Input2.sprite = arrow.sprite;
        move4Input2.transform.rotation = Quaternion.Euler(0, 0, -135);
        move4Input3.sprite = arrow.sprite;
        move4Input3.transform.rotation = Quaternion.Euler(0, 0, -180);
        move4Input4.sprite = plus.sprite;
        move4Input4.transform.rotation = Quaternion.Euler(0, 0, 0);
        move4Input5.sprite = HeavyButton.sprite;
        move4Input5.transform.rotation = Quaternion.Euler(0, 0, 0);
        move4Input6.sprite = Blank.sprite;
        move4Input6.transform.rotation = Quaternion.Euler(0, 0, 0);
        move4Input7.sprite = Blank.sprite;
        move4Input7.transform.rotation = Quaternion.Euler(0, 0, 0);
        Move5.text = "Basket Case";
        Description5.text = "Hold B  to charge attack.";
        or5Text.text = "";
        inAir5Text.text = "";
        Move5Break.SetActive(true);
        move5Input1.sprite = arrow.sprite;
        move5Input1.transform.rotation = Quaternion.Euler(0, 0, -90);
        move5Input2.sprite = arrow.sprite;
        move5Input2.transform.rotation = Quaternion.Euler(0, 0, -45);
        move5Input3.sprite = arrow.sprite;
        move5Input3.transform.rotation = Quaternion.Euler(0, 0, 0);
        move5Input4.sprite = plus.sprite;
        move5Input4.transform.rotation = Quaternion.Euler(0, 0, 0);
        move5Input5.sprite = BreakButton.sprite;
        move5Input5.transform.rotation = Quaternion.Euler(0, 0, 0);
        move5Input6.sprite = Blank.sprite;
        move5Input6.transform.rotation = Quaternion.Euler(0, 0, 0);
        move5Input7.sprite = Blank.sprite;
        move5Input7.transform.rotation = Quaternion.Euler(0, 0, 0);
        Move6.text = "";
        Description6.text = "";
        or6Text.text = "";
        inAir6Text.text = "";
        Move6Break.SetActive(false);
        move6Input1.sprite = Blank.sprite;
        move6Input1.transform.rotation = Quaternion.Euler(0, 0, 0);
        move6Input2.sprite = Blank.sprite;
        move6Input2.transform.rotation = Quaternion.Euler(0, 0, 0);
        move6Input3.sprite = Blank.sprite;
        move6Input3.transform.rotation = Quaternion.Euler(0, 0, 0);
        move6Input4.sprite = Blank.sprite;
        move6Input4.transform.rotation = Quaternion.Euler(0, 0, 0);
        move6Input5.sprite = Blank.sprite;
        move6Input5.transform.rotation = Quaternion.Euler(0, 0, 0);
        move6Input6.sprite = Blank.sprite;
        move6Input6.transform.rotation = Quaternion.Euler(0, 0, 0);
        move6Input7.sprite = Blank.sprite;
        move6Input7.transform.rotation = Quaternion.Euler(0, 0, 0);
        carouselSelected.transform.position = carousel2.transform.position;
        maxVerticalIndex = 6;
    }

    public void setDhaliaPage3()
    {
        setCharacter("Dhalia Thorne");
        setPage("Break Triggers");
        pageMoveIconL.color = new Color(1f, 1f, 1f, 1f);
        pageMoveIconR.color = new Color(1f, 1f, 1f, 1f);
        Move1.text = "Toaster";
        Description1.text = "";
        or1Text.text = "";
        inAirText.text = "";
        Arrows1.SetActive(false);
        move1Input1.sprite = arrow.sprite;
        move1Input1.transform.rotation = Quaternion.Euler(0, 0, -90);
        move1Input2.sprite = arrow.sprite;
        move1Input2.transform.rotation = Quaternion.Euler(0, 0, -45);
        move1Input3.sprite = arrow.sprite;
        move1Input3.transform.rotation = Quaternion.Euler(0, 0, 0);
        move1Input4.sprite = plus.sprite;
        move1Input4.transform.rotation = Quaternion.Euler(0, 0, 0);
        move1Input5.sprite = HeavyButton.sprite;
        move1Input5.transform.rotation = Quaternion.Euler(0, 0, 0);
        move1Input6.sprite = plus.sprite;
        move1Input6.transform.rotation = Quaternion.Euler(0, 0, 0);
        move1Input7.sprite = BreakButton.sprite;
        move1Input7.transform.rotation = Quaternion.Euler(0, 0, 0);
        move1Input7.rectTransform.localPosition = new Vector3(295.23f, move1Input7.rectTransform.localPosition.y, 0);
        or2Text.text = "";
        Move2.text = "Judgement Sabre";
        Description2.text = "";
        or2Text.text = "";
        inAir2Text.text = "";
        Move2Break.SetActive(false);
        move2Input1.sprite = arrow.sprite;
        move2Input1.transform.rotation = Quaternion.Euler(0, 0, -90);
        move2Input2.sprite = arrow.sprite;
        move2Input2.transform.rotation = Quaternion.Euler(0, 0, -135);
        move2Input3.sprite = arrow.sprite;
        move2Input3.transform.rotation = Quaternion.Euler(0, 0, -180);
        move2Input4.sprite = plus.sprite;
        move2Input4.transform.rotation = Quaternion.Euler(0, 0, 0);
        move2Input5.sprite = LightButton.sprite;
        move2Input5.transform.rotation = Quaternion.Euler(0, 0, 0);
        move2Input6.sprite = plus.sprite;
        move2Input6.transform.rotation = Quaternion.Euler(0, 0, 0);
        move2Input7.sprite = MediumButton.sprite;
        move2Input7.transform.rotation = Quaternion.Euler(0, 0, 0);
        Move3.text = "";
        Move3Break.SetActive(false);
        Description3.text = "";
        or3Text.text = "";
        inAir3Text.text = "";
        move3Input1.sprite = Blank.sprite;
        move3Input1.transform.rotation = Quaternion.Euler(0, 0, 0);
        move3Input2.sprite = Blank.sprite;
        move3Input2.transform.rotation = Quaternion.Euler(0, 0, 0);
        move3Input3.sprite = Blank.sprite;
        move3Input3.transform.rotation = Quaternion.Euler(0, 0, 0);
        move3Input4.sprite = Blank.sprite;
        move3Input4.transform.rotation = Quaternion.Euler(0, 0, 0);
        move3Input5.sprite = Blank.sprite;
        move3Input5.transform.rotation = Quaternion.Euler(0, 0, 0);
        move3Input6.sprite = Blank.sprite;
        move3Input6.transform.rotation = Quaternion.Euler(0, 0, 0);
        move3Input7.sprite = Blank.sprite;
        move3Input7.transform.rotation = Quaternion.Euler(0, 0, 0);
        Move4.text = "";
        Description4.text = "";
        or4Text.text = "";
        inAir4Text.text = "";
        Move4Break.SetActive(false);
        move4Input1.sprite = Blank.sprite;
        move4Input1.transform.rotation = Quaternion.Euler(0, 0, 0);
        move4Input2.sprite = Blank.sprite;
        move4Input2.transform.rotation = Quaternion.Euler(0, 0, 0);
        move4Input3.sprite = Blank.sprite;
        move4Input3.transform.rotation = Quaternion.Euler(0, 0, 0);
        move4Input4.sprite = Blank.sprite;
        move4Input4.transform.rotation = Quaternion.Euler(0, 0, 0);
        move4Input5.sprite = Blank.sprite;
        move4Input5.transform.rotation = Quaternion.Euler(0, 0, 0);
        move4Input6.sprite = Blank.sprite;
        move4Input6.transform.rotation = Quaternion.Euler(0, 0, 0);
        move4Input7.sprite = Blank.sprite;
        move4Input7.transform.rotation = Quaternion.Euler(0, 0, 0);
        Move5.text = "";
        Description5.text = "";
        or5Text.text = "";
        inAir5Text.text = "";
        Move5Break.SetActive(false);
        move5Input1.sprite = Blank.sprite;
        move5Input1.transform.rotation = Quaternion.Euler(0, 0, 0);
        move5Input2.sprite = Blank.sprite;
        move5Input2.transform.rotation = Quaternion.Euler(0, 0, 0);
        move5Input3.sprite = Blank.sprite;
        move5Input3.transform.rotation = Quaternion.Euler(0, 0, 0);
        move5Input4.sprite = Blank.sprite;
        move5Input4.transform.rotation = Quaternion.Euler(0, 0, 0);
        move5Input5.sprite = Blank.sprite;
        move5Input5.transform.rotation = Quaternion.Euler(0, 0, 0);
        move5Input6.sprite = Blank.sprite;
        move5Input6.transform.rotation = Quaternion.Euler(0, 0, 0);
        move5Input7.sprite = Blank.sprite;
        move5Input7.transform.rotation = Quaternion.Euler(0, 0, 0);
        Move6.text = "";
        Description6.text = "";
        or6Text.text = "";
        inAir6Text.text = "";
        Move6Break.SetActive(false);
        move6Input1.sprite = Blank.sprite;
        move6Input1.transform.rotation = Quaternion.Euler(0, 0, 0);
        move6Input2.sprite = Blank.sprite;
        move6Input2.transform.rotation = Quaternion.Euler(0, 0, 0);
        move6Input3.sprite = Blank.sprite;
        move6Input3.transform.rotation = Quaternion.Euler(0, 0, 0);
        move6Input4.sprite = Blank.sprite;
        move6Input4.transform.rotation = Quaternion.Euler(0, 0, 0);
        move6Input5.sprite = Blank.sprite;
        move6Input5.transform.rotation = Quaternion.Euler(0, 0, 0);
        move6Input6.sprite = Blank.sprite;
        move6Input6.transform.rotation = Quaternion.Euler(0, 0, 0);
        move6Input7.sprite = Blank.sprite;
        move6Input7.transform.rotation = Quaternion.Euler(0, 0, 0);
        carouselSelected.transform.position = carousel3.transform.position;
        maxVerticalIndex = 3;
    }

    //Achealis' Movelist Pages
    public void setAchealisPage1()
    {
        setCharacter("Achealis Thorne");
        setPage("Command Normals");
        pageMoveIconL.color = new Color(1f, 1f, 1f, .5f);
        pageMoveIconR.color = new Color(1f, 1f, 1f, 1f);
        Move1.text = "";
        Description1.text = "";
        or1Text.text = "";
        inAirText.text = "";
        Arrows1.SetActive(false);
        move1Input1.sprite = Blank.sprite;
        move1Input1.transform.rotation = Quaternion.Euler(0, 0, 0);
        move1Input2.sprite = Blank.sprite;
        move1Input2.transform.rotation = Quaternion.Euler(0, 0, 0);
        move1Input3.sprite = Blank.sprite;
        move1Input3.transform.rotation = Quaternion.Euler(0, 0, 0);
        move1Input4.sprite = Blank.sprite;
        move1Input4.transform.rotation = Quaternion.Euler(0, 0, 0);
        move1Input5.sprite = Blank.sprite;
        move1Input5.transform.rotation = Quaternion.Euler(0, 0, 0);
        move1Input6.sprite = Blank.sprite;
        move1Input6.transform.rotation = Quaternion.Euler(0, 0, 0);
        move1Input7.sprite = Blank.sprite;
        move1Input7.transform.rotation = Quaternion.Euler(0, 0, 0);
        move1Input7.rectTransform.localPosition = new Vector3(295.23f, move1Input7.rectTransform.localPosition.y, 0);
        Move2.text = "";
        Description2.text = "";
        or2Text.text = "";
        inAir2Text.text = "";
        Move2Break.SetActive(false);
        move2Input1.sprite = Blank.sprite;
        move2Input1.transform.rotation = Quaternion.Euler(0, 0, 0);
        move2Input2.sprite = Blank.sprite;
        move2Input2.transform.rotation = Quaternion.Euler(0, 0, 0);
        move2Input3.sprite = Blank.sprite;
        move2Input3.transform.rotation = Quaternion.Euler(0, 0, 0);
        move2Input4.sprite = Blank.sprite;
        move2Input4.transform.rotation = Quaternion.Euler(0, 0, 0);
        move2Input5.sprite = Blank.sprite;
        move2Input5.transform.rotation = Quaternion.Euler(0, 0, 0);
        move2Input6.sprite = Blank.sprite;
        move2Input6.transform.rotation = Quaternion.Euler(0, 0, 0);
        move2Input7.sprite = Blank.sprite;
        move2Input7.transform.rotation = Quaternion.Euler(0, 0, 0);
        Move3.text = "";
        Move3Break.SetActive(false);
        Description3.text = "";
        or3Text.text = "";
        inAir3Text.text = "";
        move3Input1.sprite = Blank.sprite;
        move3Input1.transform.rotation = Quaternion.Euler(0, 0, 0);
        move3Input2.sprite = Blank.sprite;
        move3Input2.transform.rotation = Quaternion.Euler(0, 0, 0);
        move3Input3.sprite = Blank.sprite;
        move3Input3.transform.rotation = Quaternion.Euler(0, 0, 0);
        move3Input4.sprite = Blank.sprite;
        move3Input4.transform.rotation = Quaternion.Euler(0, 0, 0);
        move3Input5.sprite = Blank.sprite;
        move3Input5.transform.rotation = Quaternion.Euler(0, 0, 0);
        move3Input6.sprite = Blank.sprite;
        move3Input6.transform.rotation = Quaternion.Euler(0, 0, 0);
        move3Input7.sprite = Blank.sprite;
        move3Input7.transform.rotation = Quaternion.Euler(0, 0, 0);
        Move4.text = "";
        Description4.text = "";
        or4Text.text = "";
        inAir4Text.text = "";
        Move4Break.SetActive(false);
        move4Input1.sprite = Blank.sprite;
        move4Input1.transform.rotation = Quaternion.Euler(0, 0, 0);
        move4Input2.sprite = Blank.sprite;
        move4Input2.transform.rotation = Quaternion.Euler(0, 0, 0);
        move4Input3.sprite = Blank.sprite;
        move4Input3.transform.rotation = Quaternion.Euler(0, 0, 0);
        move4Input4.sprite = Blank.sprite;
        move4Input4.transform.rotation = Quaternion.Euler(0, 0, 0);
        move4Input5.sprite = Blank.sprite;
        move4Input5.transform.rotation = Quaternion.Euler(0, 0, 0);
        move4Input6.sprite = Blank.sprite;
        move4Input6.transform.rotation = Quaternion.Euler(0, 0, 0);
        move4Input7.sprite = Blank.sprite;
        move4Input7.transform.rotation = Quaternion.Euler(0, 0, 0);
        Move5.text = "";
        Description5.text = "";
        or5Text.text = "";
        inAir5Text.text = "";
        Move5Break.SetActive(false);
        move5Input1.sprite = Blank.sprite;
        move5Input1.transform.rotation = Quaternion.Euler(0, 0, 0);
        move5Input2.sprite = Blank.sprite;
        move5Input2.transform.rotation = Quaternion.Euler(0, 0, 0);
        move5Input3.sprite = Blank.sprite;
        move5Input3.transform.rotation = Quaternion.Euler(0, 0, 0);
        move5Input4.sprite = Blank.sprite;
        move5Input4.transform.rotation = Quaternion.Euler(0, 0, 0);
        move5Input5.sprite = Blank.sprite;
        move5Input5.transform.rotation = Quaternion.Euler(0, 0, 0);
        move5Input6.sprite = Blank.sprite;
        move5Input6.transform.rotation = Quaternion.Euler(0, 0, 0);
        move5Input7.sprite = Blank.sprite;
        move5Input7.transform.rotation = Quaternion.Euler(0, 0, 0);
        Move6.text = "";
        Description6.text = "";
        or6Text.text = "";
        inAir6Text.text = "";
        Move6Break.SetActive(false);
        move6Input1.sprite = Blank.sprite;
        move6Input1.transform.rotation = Quaternion.Euler(0, 0, 0);
        move6Input2.sprite = Blank.sprite;
        move6Input2.transform.rotation = Quaternion.Euler(0, 0, 0);
        move6Input3.sprite = Blank.sprite;
        move6Input3.transform.rotation = Quaternion.Euler(0, 0, 0);
        move6Input4.sprite = Blank.sprite;
        move6Input4.transform.rotation = Quaternion.Euler(0, 0, 0);
        move6Input5.sprite = Blank.sprite;
        move6Input5.transform.rotation = Quaternion.Euler(0, 0, 0);
        move6Input6.sprite = Blank.sprite;
        move6Input6.transform.rotation = Quaternion.Euler(0, 0, 0);
        move6Input7.sprite = Blank.sprite;
        move6Input7.transform.rotation = Quaternion.Euler(0, 0, 0);
        carouselSelected.transform.position = carousel1.transform.position;
        maxVerticalIndex = 3;
    }

    public void setAchealisPage2()
    {
        setCharacter("Achealis Thorne");
        setPage("Special Moves");
        pageMoveIconL.color = new Color(1f, 1f, 1f, 1f);
        pageMoveIconR.color = new Color(1f, 1f, 1f, 1f);
        Move1.text = "Tower Leap";
        Description1.text = "Hold    or    to change distance.";
        or1Text.text = "or";
        inAirText.text = "";
        Arrows1.SetActive(true);
        Arrows1L.transform.rotation = Quaternion.Euler(0, 0, -180);
        Arrows1R.transform.rotation = Quaternion.Euler(0, 0, 0);
        move1Input1.sprite = arrow.sprite;
        move1Input1.transform.rotation = Quaternion.Euler(0, 0, -90);
        move1Input2.sprite = arrow.sprite;
        move1Input2.transform.rotation = Quaternion.Euler(0, 0, -135);
        move1Input3.sprite = arrow.sprite;
        move1Input3.transform.rotation = Quaternion.Euler(0, 0, -180);
        move1Input4.sprite = plus.sprite;
        move1Input4.transform.rotation = Quaternion.Euler(0, 0, 0);
        move1Input5.sprite = LightButton.sprite;
        move1Input5.transform.rotation = Quaternion.Euler(0, 0, 0);
        move1Input6.sprite = Blank.sprite;
        move1Input6.transform.rotation = Quaternion.Euler(0, 0, 0);
        move1Input7.sprite = MediumButton.sprite;
        move1Input7.transform.rotation = Quaternion.Euler(0, 0, 0);
        Move2.text = "Level Hell";
        Description2.text = "";
        or2Text.text = "";
        inAir2Text.text = "";
        move2Input1.sprite = arrow.sprite;
        move2Input1.transform.rotation = Quaternion.Euler(0, 0, -90);
        move2Input2.sprite = arrow.sprite;
        move2Input2.transform.rotation = Quaternion.Euler(0, 0, -45);
        move2Input3.sprite = arrow.sprite;
        move2Input3.transform.rotation = Quaternion.Euler(0, 0, 0);
        move2Input4.sprite = plus.sprite;
        move2Input4.transform.rotation = Quaternion.Euler(0, 0, 0);
        move2Input5.sprite = MediumButton.sprite;
        move2Input5.transform.rotation = Quaternion.Euler(0, 0, 0);
        move2Input6.sprite = Blank.sprite;
        move2Input6.transform.rotation = Quaternion.Euler(0, 0, 0);
        move2Input7.sprite = Blank.sprite;
        move2Input7.transform.rotation = Quaternion.Euler(0, 0, 0);
        move2Input7.rectTransform.localPosition = new Vector3(295.23f, move1Input7.rectTransform.localPosition.y, 0);
        Move3.text = "Heaven Climber";
        Description3.text = "Usable in the air.";
        or3Text.text = "or";
        inAir3Text.text = "";
        Move3Break.SetActive(false);    
        move3Input1.sprite = arrow.sprite;
        move3Input1.transform.rotation = Quaternion.Euler(0, 0, 0);
        move3Input2.sprite = arrow.sprite;
        move3Input2.transform.rotation = Quaternion.Euler(0, 0, -90);
        move3Input3.sprite = arrow.sprite;
        move3Input3.transform.rotation = Quaternion.Euler(0, 0, -45);
        move3Input4.sprite = plus.sprite;
        move3Input4.transform.rotation = Quaternion.Euler(0, 0, 0);
        move3Input5.sprite = HeavyButton.sprite;
        move3Input5.transform.rotation = Quaternion.Euler(0, 0, 0);
        move3Input6.sprite = Blank.sprite;
        move3Input6.transform.rotation = Quaternion.Euler(0, 0, 0);
        move3Input7.sprite = BreakButton.sprite;
        move3Input7.transform.rotation = Quaternion.Euler(0, 0, 0);
        Move4.text = "Starfall";
        Description4.text = "Hold B  to delay.";
        or4Text.text = "";
        inAir4Text.text = "(in air)";
        Move4Break.SetActive(true);
        move4Input1.sprite = arrow.sprite;
        move4Input1.transform.rotation = Quaternion.Euler(0, 0, -90);
        move4Input2.sprite = arrow.sprite;
        move4Input2.transform.rotation = Quaternion.Euler(0, 0, -135);
        move4Input3.sprite = arrow.sprite;
        move4Input3.transform.rotation = Quaternion.Euler(0, 0, -180);
        move4Input4.sprite = plus.sprite;
        move4Input4.transform.rotation = Quaternion.Euler(0, 0, 0);
        move4Input5.sprite = BreakButton.sprite;
        move4Input5.transform.rotation = Quaternion.Euler(0, 0, 0);
        move4Input6.sprite = Blank.sprite;
        move4Input6.transform.rotation = Quaternion.Euler(0, 0, 0);
        move4Input7.sprite = Blank.sprite;
        move4Input7.transform.rotation = Quaternion.Euler(0, 0, 0);
        Move5.text = "";
        Description5.text = "";
        or5Text.text = "";
        inAir5Text.text = "";
        Move5Break.SetActive(false);
        move5Input1.sprite = Blank.sprite;
        move5Input1.transform.rotation = Quaternion.Euler(0, 0, 0);
        move5Input2.sprite = Blank.sprite;
        move5Input2.transform.rotation = Quaternion.Euler(0, 0, 0);
        move5Input3.sprite = Blank.sprite;
        move5Input3.transform.rotation = Quaternion.Euler(0, 0, 0);
        move5Input4.sprite = Blank.sprite;
        move5Input4.transform.rotation = Quaternion.Euler(0, 0, 0);
        move5Input5.sprite = Blank.sprite;
        move5Input5.transform.rotation = Quaternion.Euler(0, 0, 0);
        move5Input6.sprite = Blank.sprite;
        move5Input6.transform.rotation = Quaternion.Euler(0, 0, 0);
        move5Input7.sprite = Blank.sprite;
        move5Input7.transform.rotation = Quaternion.Euler(0, 0, 0);
        Move6.text = "";
        Description6.text = "";
        or6Text.text = "";
        inAir6Text.text = "";
        Move6Break.SetActive(false);
        move6Input1.sprite = Blank.sprite;
        move6Input1.transform.rotation = Quaternion.Euler(0, 0, 0);
        move6Input2.sprite = Blank.sprite;
        move6Input2.transform.rotation = Quaternion.Euler(0, 0, 0);
        move6Input3.sprite = Blank.sprite;
        move6Input3.transform.rotation = Quaternion.Euler(0, 0, 0);
        move6Input4.sprite = Blank.sprite;
        move6Input4.transform.rotation = Quaternion.Euler(0, 0, 0);
        move6Input5.sprite = Blank.sprite;
        move6Input5.transform.rotation = Quaternion.Euler(0, 0, 0);
        move6Input6.sprite = Blank.sprite;
        move6Input6.transform.rotation = Quaternion.Euler(0, 0, 0);
        move6Input7.sprite = Blank.sprite;
        move6Input7.transform.rotation = Quaternion.Euler(0, 0, 0);
        carouselSelected.transform.position = carousel2.transform.position;
        maxVerticalIndex = 5;
    }

    public void setAchealisPage3()
    {
        setCharacter("Achealis Thorne");
        setPage("Break Triggers");
        pageMoveIconL.color = new Color(1f, 1f, 1f, 1f);
        pageMoveIconR.color = new Color(1f, 1f, 1f, 1f);
        Move1.text = "Forsythia Marduk";
        Description1.text = "Hold a direction to aim. Press an attack to fire.";
        or1Text.text = "";
        inAirText.text = "";
        Arrows1.SetActive(false);
        move1Input1.sprite = arrow.sprite;
        move1Input1.transform.rotation = Quaternion.Euler(0, 0, -90);
        move1Input2.sprite = arrow.sprite;
        move1Input2.transform.rotation = Quaternion.Euler(0, 0, -45);
        move1Input3.sprite = arrow.sprite;
        move1Input3.transform.rotation = Quaternion.Euler(0, 0, 0);
        move1Input4.sprite = LightButton.sprite;
        move1Input4.transform.rotation = Quaternion.Euler(0, 0, 0);
        move1Input5.sprite = plus.sprite;
        move1Input5.transform.rotation = Quaternion.Euler(0, 0, 0);
        move1Input6.sprite = MediumButton.sprite;
        move1Input6.transform.rotation = Quaternion.Euler(0, 0, 0);
        move1Input7.sprite = Blank.sprite;
        move1Input7.transform.rotation = Quaternion.Euler(0, 0, 0);
        move1Input7.rectTransform.localPosition = new Vector3(295.23f, move1Input7.rectTransform.localPosition.y, 0);
        Move2.text = "";
        Description2.text = "";
        or2Text.text = "";
        inAir2Text.text = "";
        Move2Break.SetActive(false);
        move2Input1.sprite = Blank.sprite;
        move2Input1.transform.rotation = Quaternion.Euler(0, 0, 0);
        move2Input2.sprite = Blank.sprite; ;
        move2Input2.transform.rotation = Quaternion.Euler(0, 0, 0);
        move2Input3.sprite = Blank.sprite; ;
        move2Input3.transform.rotation = Quaternion.Euler(0, 0, 0);
        move2Input4.sprite = Blank.sprite;
        move2Input4.transform.rotation = Quaternion.Euler(0, 0, 0);
        move2Input5.sprite = Blank.sprite;
        move2Input5.transform.rotation = Quaternion.Euler(0, 0, 0);
        move2Input6.sprite = Blank.sprite;
        move2Input6.transform.rotation = Quaternion.Euler(0, 0, 0);
        move2Input7.sprite = Blank.sprite;
        move2Input7.transform.rotation = Quaternion.Euler(0, 0, 0);
        Move3.text = "";
        Move3Break.SetActive(false);
        Description3.text = "";
        or3Text.text = "";
        inAir3Text.text = "";
        move3Input1.sprite = Blank.sprite;
        move3Input1.transform.rotation = Quaternion.Euler(0, 0, 0);
        move3Input2.sprite = Blank.sprite;
        move3Input2.transform.rotation = Quaternion.Euler(0, 0, 0);
        move3Input3.sprite = Blank.sprite;
        move3Input3.transform.rotation = Quaternion.Euler(0, 0, 0);
        move3Input4.sprite = Blank.sprite;
        move3Input4.transform.rotation = Quaternion.Euler(0, 0, 0);
        move3Input5.sprite = Blank.sprite;
        move3Input5.transform.rotation = Quaternion.Euler(0, 0, 0);
        move3Input6.sprite = Blank.sprite;
        move3Input6.transform.rotation = Quaternion.Euler(0, 0, 0);
        move3Input7.sprite = Blank.sprite;
        move3Input7.transform.rotation = Quaternion.Euler(0, 0, 0);
        Move4.text = "";
        Description4.text = "";
        or4Text.text = "";
        inAir4Text.text = "";
        Move4Break.SetActive(false);
        move4Input1.sprite = Blank.sprite;
        move4Input1.transform.rotation = Quaternion.Euler(0, 0, 0);
        move4Input2.sprite = Blank.sprite;
        move4Input2.transform.rotation = Quaternion.Euler(0, 0, 0);
        move4Input3.sprite = Blank.sprite;
        move4Input3.transform.rotation = Quaternion.Euler(0, 0, 0);
        move4Input4.sprite = Blank.sprite;
        move4Input4.transform.rotation = Quaternion.Euler(0, 0, 0);
        move4Input5.sprite = Blank.sprite;
        move4Input5.transform.rotation = Quaternion.Euler(0, 0, 0);
        move4Input6.sprite = Blank.sprite;
        move4Input6.transform.rotation = Quaternion.Euler(0, 0, 0);
        move4Input7.sprite = Blank.sprite;
        move4Input7.transform.rotation = Quaternion.Euler(0, 0, 0);
        carouselSelected.transform.position = carousel3.transform.position;
        Move5.text = "";
        Description5.text = "";
        or5Text.text = "";
        inAir5Text.text = "";
        Move5Break.SetActive(false);
        move5Input1.sprite = Blank.sprite;
        move5Input1.transform.rotation = Quaternion.Euler(0, 0, 0);
        move5Input2.sprite = Blank.sprite;
        move5Input2.transform.rotation = Quaternion.Euler(0, 0, 0);
        move5Input3.sprite = Blank.sprite;
        move5Input3.transform.rotation = Quaternion.Euler(0, 0, 0);
        move5Input4.sprite = Blank.sprite;
        move5Input4.transform.rotation = Quaternion.Euler(0, 0, 0);
        move5Input5.sprite = Blank.sprite;
        move5Input5.transform.rotation = Quaternion.Euler(0, 0, 0);
        move5Input6.sprite = Blank.sprite;
        move5Input6.transform.rotation = Quaternion.Euler(0, 0, 0);
        move5Input7.sprite = Blank.sprite;
        move5Input7.transform.rotation = Quaternion.Euler(0, 0, 0);
        Move6.text = "";
        Description6.text = "";
        or6Text.text = "";
        inAir6Text.text = "";
        Move6Break.SetActive(false);
        move6Input1.sprite = Blank.sprite;
        move6Input1.transform.rotation = Quaternion.Euler(0, 0, 0);
        move6Input2.sprite = Blank.sprite;
        move6Input2.transform.rotation = Quaternion.Euler(0, 0, 0);
        move6Input3.sprite = Blank.sprite;
        move6Input3.transform.rotation = Quaternion.Euler(0, 0, 0);
        move6Input4.sprite = Blank.sprite;
        move6Input4.transform.rotation = Quaternion.Euler(0, 0, 0);
        move6Input5.sprite = Blank.sprite;
        move6Input5.transform.rotation = Quaternion.Euler(0, 0, 0);
        move6Input6.sprite = Blank.sprite;
        move6Input6.transform.rotation = Quaternion.Euler(0, 0, 0);
        move6Input7.sprite = Blank.sprite;
        move6Input7.transform.rotation = Quaternion.Euler(0, 0, 0);
        maxVerticalIndex = 3;
    }

    //Universal Movelist Pages
    public void setUniversal()
    {
        setPage("Universal Moves");
        pageMoveIconL.color = new Color(1f, 1f, 1f, 1f);
        pageMoveIconR.color = new Color(1f, 1f, 1f, .5f);
        Move1.text = "Light";
        Description1.text = "";
        or1Text.text = "";
        inAirText.text = "";
        Arrows1.SetActive(false);
        move1Input1.sprite = LightButton.sprite;
        move1Input1.transform.rotation = Quaternion.Euler(0, 0, 0);
        move1Input2.sprite = Blank.sprite;
        move1Input2.transform.rotation = Quaternion.Euler(0, 0, 0);
        move1Input3.sprite = Blank.sprite;
        move1Input3.transform.rotation = Quaternion.Euler(0, 0, 0);
        move1Input4.sprite = Blank.sprite;
        move1Input4.transform.rotation = Quaternion.Euler(0, 0, 0);
        move1Input5.sprite = Blank.sprite;
        move1Input5.transform.rotation = Quaternion.Euler(0, 0, 0);
        move1Input6.sprite = Blank.sprite;
        move1Input6.transform.rotation = Quaternion.Euler(0, 0, 0);
        move1Input7.sprite = Blank.sprite;
        move1Input7.transform.rotation = Quaternion.Euler(0, 0, 0);
        move1Input7.rectTransform.localPosition = new Vector3(295.23f, move1Input7.rectTransform.localPosition.y, 0);
        Move2.text = "Medium";
        Description2.text = "";
        or2Text.text = "";
        inAir2Text.text = "";
        Move2Break.SetActive(false);
        move2Input1.sprite = MediumButton.sprite;
        move2Input1.transform.rotation = Quaternion.Euler(0, 0, 0);
        move2Input2.sprite = Blank.sprite;
        move2Input2.transform.rotation = Quaternion.Euler(0, 0, 0);
        move2Input3.sprite = Blank.sprite;
        move2Input3.transform.rotation = Quaternion.Euler(0, 0, 0);
        move2Input4.sprite = Blank.sprite;
        move2Input4.transform.rotation = Quaternion.Euler(0, 0, 0);
        move2Input5.sprite = Blank.sprite;
        move2Input5.transform.rotation = Quaternion.Euler(0, 0, 0);
        move2Input6.sprite = Blank.sprite;
        move2Input6.transform.rotation = Quaternion.Euler(0, 0, 0);
        move2Input7.sprite = Blank.sprite;
        move2Input7.transform.rotation = Quaternion.Euler(0, 0, 0);
        Move3.text = "Heavy";
        Move3Break.SetActive(false);
        Description3.text = "";
        or3Text.text = "";
        inAir3Text.text = "";
        move3Input1.sprite = HeavyButton.sprite;
        move3Input1.transform.rotation = Quaternion.Euler(0, 0, 0);
        move3Input2.sprite = Blank.sprite;
        move3Input2.transform.rotation = Quaternion.Euler(0, 0, 0);
        move3Input3.sprite = Blank.sprite;
        move3Input3.transform.rotation = Quaternion.Euler(0, 0, 0);
        move3Input4.sprite = Blank.sprite;
        move3Input4.transform.rotation = Quaternion.Euler(0, 0, 0);
        move3Input5.sprite = Blank.sprite;
        move3Input5.transform.rotation = Quaternion.Euler(0, 0, 0);
        move3Input6.sprite = Blank.sprite;
        move3Input6.transform.rotation = Quaternion.Euler(0, 0, 0);
        move3Input7.sprite = Blank.sprite;
        move3Input7.transform.rotation = Quaternion.Euler(0, 0, 0);
        Move4.text = "Break";
        Description4.text = "";
        or4Text.text = "";
        inAir4Text.text = "";
        Move4Break.SetActive(false);
        move4Input1.sprite = BreakButton.sprite;
        move4Input1.transform.rotation = Quaternion.Euler(0, 0, 0);
        move4Input2.sprite = Blank.sprite;
        move4Input2.transform.rotation = Quaternion.Euler(0, 0, 0);
        move4Input3.sprite = Blank.sprite;
        move4Input3.transform.rotation = Quaternion.Euler(0, 0, 0);
        move4Input4.sprite = Blank.sprite;
        move4Input4.transform.rotation = Quaternion.Euler(0, 0, 0);
        move4Input5.sprite = Blank.sprite;
        move4Input5.transform.rotation = Quaternion.Euler(0, 0, 0);
        move4Input6.sprite = Blank.sprite;
        move4Input6.transform.rotation = Quaternion.Euler(0, 0, 0);
        move4Input7.sprite = Blank.sprite;
        move4Input7.transform.rotation = Quaternion.Euler(0, 0, 0);
        Move5.text = "Blitz Cancel";
        Description5.text = "";
        or5Text.text = "";
        inAir5Text.text = "";
        Move5Break.SetActive(false);
        move5Input1.sprite = MediumButton.sprite;
        move5Input1.transform.rotation = Quaternion.Euler(0, 0, 0);
        move5Input2.sprite = plus.sprite;
        move5Input2.transform.rotation = Quaternion.Euler(0, 0, 0);
        move5Input3.sprite = HeavyButton.sprite;
        move5Input3.transform.rotation = Quaternion.Euler(0, 0, 0);
        move5Input4.sprite = Blank.sprite;
        move5Input4.transform.rotation = Quaternion.Euler(0, 0, 0);
        move5Input5.sprite = Blank.sprite;
        move5Input5.transform.rotation = Quaternion.Euler(0, 0, 0);
        move5Input6.sprite = Blank.sprite;
        move5Input6.transform.rotation = Quaternion.Euler(0, 0, 0);
        move5Input7.sprite = Blank.sprite;
        move5Input7.transform.rotation = Quaternion.Euler(0, 0, 0);
        Move6.text = "Throw";
        Description6.text = "";
        or6Text.text = "";
        inAir6Text.text = "";
        Move6Break.SetActive(false);
        move6Input1.sprite = LightButton.sprite;
        move6Input1.transform.rotation = Quaternion.Euler(0, 0, 0);
        move6Input2.sprite = plus.sprite;
        move6Input2.transform.rotation = Quaternion.Euler(0, 0, 0);
        move6Input3.sprite = BreakButton.sprite;
        move6Input3.transform.rotation = Quaternion.Euler(0, 0, 0);
        move6Input4.sprite = Blank.sprite;
        move6Input4.transform.rotation = Quaternion.Euler(0, 0, 0);
        move6Input5.sprite = Blank.sprite;
        move6Input5.transform.rotation = Quaternion.Euler(0, 0, 0);
        move6Input6.sprite = Blank.sprite;
        move6Input6.transform.rotation = Quaternion.Euler(0, 0, 0);
        move6Input7.sprite = Blank.sprite;
        move6Input7.transform.rotation = Quaternion.Euler(0, 0, 0);
        carouselSelected.transform.position = carousel4.transform.position;
        maxVerticalIndex = 7;
    }   
}


