using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlowingTitle : MonoBehaviour
{
    public TMPro.TMP_Text text;
    public float glowChangeSpeed = 0.7f;
    
    private float glowPower = 0.5f;
    private bool goingUp = true;
    private float max = 0.8f;
    private float min = 0.2f;
    // Update is called once per frame
    void Update()
    {
        text.fontSharedMaterial.SetFloat("_GlowPower", glowPower);
        if (goingUp)
        {
            glowPower += glowChangeSpeed;
            if (glowPower > max) goingUp = false;
        }
        else
        {
            glowPower -= glowChangeSpeed;
            if (glowPower < min) goingUp = true;
        }
    }
}
