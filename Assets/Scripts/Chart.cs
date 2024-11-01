using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Stage", menuName = "New Stage")]
public class Chart : ScriptableObject
{
    public float bpm;
    public Sprite[] iconSprite;
    public TextAsset chart;
    public AudioClip[] sounds;
}
