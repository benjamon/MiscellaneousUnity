using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FPSDisplay : MonoBehaviour
{
    public TextMeshProUGUI fpsText;

    double frameCount = 0;
    double nextUpdate = 0.0;
    double fps = 0.0;
    double updateRate = 4.0;  // 4 updates per sec.

    void Start()
    {
        nextUpdate = Time.time;
    }

    private void Update()
    {
        frameCount++;
        if (Time.time > nextUpdate)
        {
            nextUpdate += 1.0 / updateRate;
            fps = frameCount * updateRate;
            frameCount = 0;
        }

        fpsText.text = $"{fps} fps";
    }
}
