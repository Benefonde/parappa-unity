using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RappinMeter : MonoBehaviour
{
    private void Start()
    {
        aud = GetComponent<AudioSource>();
        rank = 3;
        UpdateRankMeter();
    }

    public void ChangeRank(int scoreFromLine, int buttonsPressed)
    {
        RectTransform rt = transform.GetChild(0).GetComponent<RectTransform>();
        print("ranking!!");
        Invoke(nameof(UpdateRankMeter), 0.01f);
        if (buttonsPressed >= 25)
        {
            if (rank / 3 == Mathf.Floor(rank / 3))
            {
                rank++;
                aud.PlayOneShot(upDown[3]);
            }
            rank += 2;
            if (rank / 3 != Mathf.Floor(rank / 3))
            {
                aud.PlayOneShot(upDown[1]);
            }
            return;
        }
        //cool
        if (scoreFromLine > 84 && (rank == 0))
        {
            aud.PlayOneShot(upDown[0]);
            rt.anchoredPosition = new Vector2(-45, 140);
            return;
        }
        else if (scoreFromLine < 85 && (rank == 0))
        {
            aud.PlayOneShot(upDown[1]);
            rt.anchoredPosition = new Vector2(-45, 140);
            rank++;
            return;
        }
        else if (scoreFromLine < 85 && (rank == 1))
        {
            aud.PlayOneShot(upDown[3]);
            rt.anchoredPosition = new Vector2(-45, 140);
            rank += 2;
            return;
        }
        if (scoreFromLine > 84 && (rank == 1))
        {
            aud.PlayOneShot(upDown[0]);
            rt.anchoredPosition = new Vector2(-45, 140);
            rank--;
            return;
        }
        //good
        if (scoreFromLine > 1 && scoreFromLine < CoolScore() && (rank == 3))
        {
            aud.PlayOneShot(upDown[0]);
            rt.anchoredPosition = new Vector2(-45, 115);
            return;
        }
        if (scoreFromLine > 1 && scoreFromLine < CoolScore() && (rank == 4))
        {
            aud.PlayOneShot(upDown[0]);
            rt.anchoredPosition = new Vector2(-45, 115);
            rank--;
            return;
        }
        if (scoreFromLine > CoolScore() && (rank == 3))
        {
            aud.PlayOneShot(upDown[0]);
            rt.anchoredPosition = new Vector2(-45, 115);
            rank--;
            return;
        }
        if (scoreFromLine < CoolScore() && (rank == 2))
        {
            aud.PlayOneShot(upDown[1]);
            rt.anchoredPosition = new Vector2(-45, 115);
            rank++;
            return;
        }
        if (scoreFromLine > CoolScore() && (rank == 2))
        {
            aud.PlayOneShot(upDown[2]);
            rt.anchoredPosition = new Vector2(-45, 115);
            rank -= 2;
            return;
        }
        if (scoreFromLine < 1 && (rank == 3))
        {
            aud.PlayOneShot(upDown[1]);
            rt.anchoredPosition = new Vector2(-45, 115);
            rank++;
            return;
        }
        if (scoreFromLine < 1 && (rank == 4))
        {
            aud.PlayOneShot(upDown[3]);
            rt.anchoredPosition = new Vector2(-45, 115);
            rank += 2;
            return;
        }
        //bad
        if (scoreFromLine > 0 && (rank == 6 || rank == 7))
        {
            aud.PlayOneShot(upDown[0]);
            rt.anchoredPosition = new Vector2(-45, 90);
            rank--;
            return;
        }
        if (scoreFromLine > 0 && (rank == 5))
        {
            aud.PlayOneShot(upDown[2]);
            rt.anchoredPosition = new Vector2(-45, 90);
            rank -= 2;
            return;
        }
        else if (scoreFromLine < 1 && (rank == 5 || rank == 6))
        {
            aud.PlayOneShot(upDown[1]);
            rt.anchoredPosition = new Vector2(-45, 90);
            rank++;
            return;
        }
        else if (scoreFromLine < 1 && (rank == 7))
        {
            aud.PlayOneShot(upDown[3]);
            rt.anchoredPosition = new Vector2(-45, 90);
            rank += 2;
            return;
        }
        //awful
        if (scoreFromLine > 0 && (rank == 9 || rank == 10))
        {
            aud.PlayOneShot(upDown[0]);
            rt.anchoredPosition = new Vector2(-45, 65);
            rank--;
            return;
        }
        if (scoreFromLine > 0 && (rank == 8))
        {
            aud.PlayOneShot(upDown[2]);
            rt.anchoredPosition = new Vector2(-45, 65);
            rank -= 2;
            return;
        }
        else if (scoreFromLine < 1 && (rank == 8 || rank == 9))
        {
            aud.PlayOneShot(upDown[1]);
            rt.anchoredPosition = new Vector2(-45, 65);
            rank++;
            return;
        }
        if (scoreFromLine < 1 && (rank == 10))
        {
            aud.PlayOneShot(upDown[3]);
            rt.anchoredPosition = new Vector2(-45, 65);
            rank += 2;
            return;
        }
    }

    int CoolScore()
    {
        if (chart.GetComponents<Transform>().Length * 20 + 30 < 60)
        {
            return 60;
        }
        if (chart.GetComponents<Transform>().Length * 20 + 30 > 135)
        {
            if (Mathf.RoundToInt(chart.GetComponents<Transform>().Length * 8.5f + 30) > 135)
            {
                if (Mathf.RoundToInt(chart.GetComponents<Transform>().Length * 4 + 30) > 135)
                {
                    return Mathf.RoundToInt(chart.GetComponents<Transform>().Length * 2.5f + 30);
                }
                return Mathf.RoundToInt(chart.GetComponents<Transform>().Length * 4 + 30);
            }
            return Mathf.RoundToInt(chart.GetComponents<Transform>().Length * 8.5f + 30);
        }
        return chart.GetComponents<Transform>().Length * 20 + 30;
    }

    void UpdateRankMeter()
    {
        switch (rank)
        {
            case 0: BlinkyBlinkRank(2); mus.ChangeMusic(0); break;
            case 1: BlinkyBlinkRank(2, 1); mus.ChangeMusic(0); break;
            case 2: BlinkyBlinkRank(1, 2); mus.ChangeMusic(1); break;
            case 3: BlinkyBlinkRank(0, 2); mus.ChangeMusic(1); break;
            case 4: BlinkyBlinkRank(0, 2, 1); mus.ChangeMusic(1); break;
            case 5: BlinkyBlinkRank(0, 1, 2); mus.ChangeMusic(2); break;
            case 6: BlinkyBlinkRank(0, 0, 2); mus.ChangeMusic(2); break;
            case 7: BlinkyBlinkRank(0, 0, 2, 1); mus.ChangeMusic(2); break;
            case 8: BlinkyBlinkRank(0, 0, 1, 2); mus.ChangeMusic(3); break;
            case 9: BlinkyBlinkRank(0, 0, 0, 2); mus.ChangeMusic(3); break;
            case 10: BlinkyBlinkRank(0, 0, 0, 1); mus.ChangeMusic(3); break;
            case 11: BlinkyBlinkRank(0, 0, 0, 1); mus.ChangeMusic(4); break;
            case 12: BlinkyBlinkRank(0, 0, 0, 1); mus.ChangeMusic(4); break;
            case 13: BlinkyBlinkRank(0, 0, 0, 0); mus.ChangeMusic(4); break;
        }
    }

    void BlinkyBlinkRank(int a = 0, int b = 0, int c = 0, int d = 0)
    {
        ranks[0].SetInteger("a", a);
        ranks[1].SetInteger("a", b);
        ranks[2].SetInteger("a", c);
        ranks[3].SetInteger("a", d);
    }

    public int rank; // 0 cool 3 good 6 bad 9 awful

    public Animator[] ranks;
    public AudioClip[] upDown; // 0 good 1 bad 2 better 3 worse
    AudioSource aud;

    public GameObject chart;
    public Music mus;
}
