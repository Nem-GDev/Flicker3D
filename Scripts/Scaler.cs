using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scaler
{
    public readonly Vector2 WorldBotLeft, WorldTopRight, WorldCenter;
    public readonly float WorldHeight, WorldWidth;
    public readonly double UnitWorldHeight, UnitWorldWidth;
    public readonly double UnitScreenHeight, UnitScreenWidth;
    public readonly float UpperSpeed, LowerSpeed;

    public Scaler(Vector2 scoreSpeedRange)
    {
        Camera cam = Camera.main;
        UnitScreenWidth = cam.pixelWidth / 100f;
        UnitScreenHeight = cam.pixelHeight / 100f;

        Vector3 wbl = cam.ScreenToWorldPoint(new Vector2(0, 0));
        Vector3 wtr = cam.ScreenToWorldPoint(new Vector2(cam.pixelWidth, cam.pixelHeight));
        WorldBotLeft = new Vector2(wbl.x, wbl.y);
        WorldTopRight = new Vector2(wtr.x, wtr.y);
        WorldHeight = WorldTopRight.y - WorldBotLeft.y;
        WorldWidth = WorldTopRight.x - WorldBotLeft.x;
        WorldCenter = new Vector2(wbl.x + WorldWidth/2, wbl.y + WorldHeight/2);

        Debug.Log("SCREEN SPEC:");
        Debug.Log($"W: {WorldWidth}, H: {WorldHeight}");
        Debug.Log($"Wbl: {WorldBotLeft}, Wtr: {WorldTopRight}");
        //Debug.Log("W: "+ cam.pixelWidth + " H: " + cam.pixelHeight);
        UnitWorldHeight = WorldHeight / 100;
        UnitWorldWidth = WorldWidth / 100;

        UpperSpeed = (float) WorldHeight * scoreSpeedRange.y;
        LowerSpeed = (float) WorldHeight * scoreSpeedRange.x;
    }
}
