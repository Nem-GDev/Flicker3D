using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlickInfo
{
    public bool readyToProcess = false;
    public float verticalFlickDistance = 0f;
    Vector2 sPosition, ePosition;
    float sTime, eTime;
    public FlickInfo(Touch t)
    {
        sPosition = t.position;
        sTime = Time.time;
    }
    public void EndFlick(Touch t)
    {
        ePosition = t.position;
        eTime = Time.time;
        CalculateVerticalFLick();
    }
    private void CalculateVerticalFLick()
    {
        float deltaPos = ePosition.y - sPosition.y;
        float deltaT = eTime - sTime;

        if (deltaPos < 0)
        {
            Debug.LogWarning("Invalid Vertical Gesture");
            return;
        }
        verticalFlickDistance = deltaPos;
        readyToProcess = true;
    }
}
