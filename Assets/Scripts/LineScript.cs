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
        turn = 0;
        bpm = chart.bpm;
        UpdateDotCount(16);
        iconChar[0] = chart.iconSprite[0];
        iconChar[1] = chart.iconSprite[1];
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        icon.rectTransform.Translate(new Vector2(bpm * 2 * Time.deltaTime, 0), Space.Self);
        if (icon.rectTransform.anchoredPosition.x > 270)
        {
            if (!doubleLine)
            {
                icon.rectTransform.anchoredPosition = new Vector2(-280, 190);
                icon.sprite = iconChar[turn];
                turn++;
                Aaaaa();
                for (int i = 0; i < transform.GetChild(2).childCount; i++)
                {
                    Destroy(transform.GetChild(2).GetChild(i).gameObject);
                }
                if (rank.rank == 12)
                {
                    bpm = 0;
                }
                return;
            }
            else if (icon.rectTransform.anchoredPosition.y == 150)
            {
                icon.rectTransform.anchoredPosition = new Vector2(-280, 190);
                icon.sprite = iconChar[turn];
                turn++;
                Aaaaa();
                for (int i = 0; i < transform.GetChild(2).childCount; i++)
                {
                    Destroy(transform.GetChild(2).GetChild(i).gameObject);
                }
                if (rank.rank == 12)
                {
                    bpm = 0;
                }
                return;
            }
            icon.rectTransform.anchoredPosition = new Vector2(-196, 150);
        }
    }

    void Aaaaa()
    {
        if (turn == 2)
        {
            turn = 0;
            score.playerTimings.Clear();
            score.AddScore();
        }
    }

    public void StampButton(int but)
    {
        GameObject a = Instantiate(buttons[but], icon.transform.position, icon.transform.rotation, transform.GetChild(2));
        RectTransform b = a.GetComponent<RectTransform>();
        b.anchoredPosition = new Vector2(Mathf.RoundToInt(b.anchoredPosition.x), b.anchoredPosition.y);
        score.playerTimings.Add(Mathf.RoundToInt(a.transform.position.x));
    }

    public void UpdateDotCount(int amount)
    {
        dots = amount;
        if (amount > 16)
        {
            line[1].SetActive(true);
            doubleLine = true;
        }
        else
        {
            line[1].SetActive(false);
            doubleLine = false;
        }
    }

    //chart stuff
    float bpm;
    Sprite[] iconChar = new Sprite[2]; // 0 parappa 1 teacher
    public int turn; //0 teacher 1 parappa
    public int dots = 32; //1 line long, charts using the dots thingy should only have buttons from -2 to 32
    public Chart chart;
    RappinMeter rank;
    //ui elements
    public GameObject[] line; // 0 is normal line, 1 is 2nd line
    public Image icon;
    public GameObject[] buttons; //0 triangle/W 1 circle/D 2 cross/S 3 square/A 4 L/Q 5 R/E 6 shieruken (any button/key)
    public Score score;
    public bool doubleLine;
}