using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Texture", menuName = "New Button Textures")]
public class ButtonTextures: ScriptableObject
{
    public Sprite[] iconSprite = new Sprite[6];
}
