using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        digit[0].gameObject.SetActive(true);
        for (int i = 1; i < digit.Length; i++)
        {
            digit[i].gameObject.SetActive(false);
        }
        score = 0;
        digit[0].sprite = nums[0];
        validTimings.Clear();
        for (int i = 0; i < 19; i++)
        {
            validTimings.Add(-195 + i * 30);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (line.turn == 1)
        {
            if (Input.GetButtonDown("Triangle"))
            {
                buttonsPressed++;
                line.StampButton(2);
                QueueScore();
            }
            if (Input.GetButtonDown("Circle"))
            {
                buttonsPressed++;
                line.StampButton(3);
                QueueScore();
            }
            if (Input.GetButtonDown("Cross"))
            {
                buttonsPressed++;
                line.StampButton(4);
                QueueScore();
            }
            if (Input.GetButtonDown("Square"))
            {
                buttonsPressed++;
                line.StampButton(5);
                QueueScore();
            }
            if (Input.GetButtonDown("R"))
            {
                buttonsPressed++;
                line.StampButton(1);
                QueueScore();
            }
            if (Input.GetButtonDown("L"))
            {
                buttonsPressed++;
                line.StampButton(0);
                QueueScore();
            }

            if (Input.GetKey(KeyCode.I))
            {
                //buttonsPressed++;
                line.StampButton(2);
                QueueScore();
            }
        }
    }

    string[] AllowedInputs()
    {
        List<string> inputs = new List<string>();
        for (int i = 0; i < chart.GetComponents<Transform>().Length; i++)
        {
            print(chart.GetComponents<Transform>()[i].name);
            inputs.Add(chart.GetComponents<Transform>()[i].name);
        }
        return inputs.ToArray();
    }

    void QueueScore(string pattern = "X.")
    {
        /*int patternScore = 0;
        switch (pattern)
        {
            case "..": patternScore += 0; break;
            case "X.": patternScore += 3; break;
            case "XX": patternScore += 6; break;
            case ".X": patternScore += 9; break;
        }
        queuedScore += patternScore;*/
        float scaler = transform.parent.GetComponent<CanvasScaler>().scaleFactor; 
        int closest_distance = 8;
        for (int i = 0; i < validTimings.Count; i++)
        {
            int distance = Mathf.RoundToInt(Mathf.Abs(playerTimings[playerTimings.Count - 1]) - Mathf.Abs(validTimings[i]));
            if (distance <= closest_distance && distance >= -closest_distance)
            {
                closest_distance = Mathf.RoundToInt(distance * scaler);
            }
        }
        print(closest_distance);
        if (Mathf.Abs(closest_distance) <= 7)
        {
            queuedScore += 6;
        }
        else
        {
            queuedScore -= 12;
        }
    }

    public void AddScore()
    {
        if (buttonsPressed > line.dots + 4)
        {
            queuedScore = -100;
        }
        if (!NoScoring) FindObjectOfType<RappinMeter>().ChangeRank(queuedScore, buttonsPressed);
        buttonsPressed = 0;
        if ((int)score + (int)queuedScore < 32767) score += queuedScore;
        else score = 32767;
        if (score < 0)
        {
            score = 0;
        }
        for (int i = 0; i < digit.Length; i++)
        {
            digit[i].gameObject.SetActive(false);
        }
        for (int i = 0; i < Math.Floor(Math.Log10(score) + 1); i++)
        {
            digit[i].gameObject.SetActive(true);
            digit[i].sprite = nums[DigitNum(ReverseNum(score), i + 1)];
        }
        if (score == 0)
        {
            digit[0].gameObject.SetActive(true);
            digit[0].sprite = nums[0];
        }
        queuedScore = 0;
    }

    public int DigitNum(float val, int digit)
    {
        return (int)(val /= Mathf.Pow(10, digit - 1)) % 10;
    }
    public static int ReverseNum(int num)
    {
        for (int result = 0; ; result = result * 10 + num % 10, num /= 10) 
            if (num == 0) return result;
    }

    public Sprite[] nums;
    [SerializeField]
    short score;
    [SerializeField]
    short queuedScore;
    public Image[] digit;
    int buttonsPressed;

    public GameObject chart;
    public LineScript line;

    public List<int> playerTimings = new List<int>();

    public List<int> validTimings = new List<int>();
    public bool NoScoring;
    //public List<int> debug = new List<int>();
}
