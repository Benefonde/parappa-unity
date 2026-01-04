using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HudSizeScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        canvasScaler = GetComponent<CanvasScaler>();
    }

    // Update is called once per frame
    void Update()
    {
        canvasScaler.scaleFactor = Mathf.FloorToInt((Screen.height / 0.75f) / 240);
        if (canvasScaler.scaleFactor < 1)
        {
            canvasScaler.scaleFactor = 1f;
        }
    }

    CanvasScaler canvasScaler;
}
