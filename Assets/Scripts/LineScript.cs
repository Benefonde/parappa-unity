using System.IO;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LineScript : MonoBehaviour
{
    void Start()
    {
        icon.rectTransform.anchoredPosition = new Vector2(-250, 190);
        rank = FindObjectOfType<RappinMeter>();
        if (rank == null) NoScoring = true;
        turn = 0;
        bpm = 107;
        //bpm = chart.bpm; miko is currently writing the chart system!  
        UpdateDotCount(8);
        iconChar[0] = chart.iconSprite[0];
        iconChar[1] = chart.iconSprite[1];
        for (int i = 0; i < 6; i++)
        {
            buttonIcons[i] = buttonIconSprites.iconSprite[i];
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        icon.rectTransform.Translate(new Vector2(bpm * 2 * Time.deltaTime * transform.parent.GetComponent<CanvasScaler>().scaleFactor, 0), Space.Self);
        if (icon.rectTransform.anchoredPosition.x > -220 + (dots * 30))
        {
            if (!doubleLine)
            {
                icon.rectTransform.anchoredPosition = new Vector2(-250, 190);
                icon.sprite = iconChar[turn];
                turn++;
                EndOfTurnThing();
                return;
            }
            else if (icon.rectTransform.anchoredPosition.y == -220 + (dots * 30))
            {
                icon.rectTransform.anchoredPosition = new Vector2(-250, 190);
                icon.sprite = iconChar[turn];
                turn++;
                EndOfTurnThing();
                return;
            }
            icon.rectTransform.anchoredPosition = new Vector2(-196, 150);
        }
    }

    void EndOfTurnThing()
    {
        if (turn == 2)
        {
            turn = 0;
            score.playerTimings.Clear();
            score.AddScore();
        }
        for (int i = 0; i < transform.GetChild(1).childCount; i++)
        {
            Destroy(transform.GetChild(1).GetChild(i).gameObject);
        }
        if (NoScoring) return;
        if (rank.rank == 12) bpm = 0;
    }

    public void StampButton(int but)
    {
        GameObject a = Instantiate(button, icon.transform.position, icon.transform.rotation, transform.GetChild(1));
        a.GetComponent<Image>().sprite = buttonIcons[but];
        RectTransform b = a.GetComponent<RectTransform>();
        b.anchoredPosition = new Vector2(Mathf.RoundToInt(b.anchoredPosition.x), b.anchoredPosition.y);
        score.playerTimings.Add(Mathf.RoundToInt(b.anchoredPosition.x));
    }

    public void UpdateDotCount(int amount)
    {
        for (int i = 0; i < 18; i++) line[0].transform.GetChild(i).gameObject.SetActive(false);
        for (int i = 0; i < 16; i++) line[1].transform.GetChild(i).gameObject.SetActive(false);
        line[1].SetActive(false);
        dots = amount;
        if (amount > 16)
        {
            line[1].SetActive(true);
            for (int i = 0; i < 18; i++) line[0].transform.GetChild(i).gameObject.SetActive(true);
            doubleLine = true;
            for (int i = 0; i < dots - 16; i++) line[1].transform.GetChild(i).gameObject.SetActive(true);
        }
        else
        {
            doubleLine = false;
            for (int i = 0; i < dots + 2; i++) line[0].transform.GetChild(i).gameObject.SetActive(true);
        }
    }

    //chart stuff
    float bpm;
    Sprite[] iconChar = new Sprite[2]; // 0 parappa 1 teacher
    Sprite[] buttonIcons = new Sprite[6];
    public ButtonTextures buttonIconSprites;
    public int turn; //0 teacher 1 parappa 2
    public int dots = 32;
    public Chart chart;
    RappinMeter rank;
    //ui elements
    public GameObject[] line; // 0 is normal line, 1 is 2nd line
    public Image icon;
    public GameObject button;
    public Score score;
    // other setting
    public bool doubleLine;
    public bool NoScoring;
}