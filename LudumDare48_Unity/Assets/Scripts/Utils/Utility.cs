using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utility
{
    public static Vector2 RadianToVector2(float radian)
    {
        return new Vector2(Mathf.Cos(radian), Mathf.Sin(radian));
    }
    public static Vector2 DegreeToVector2(float degree)
    {
        return RadianToVector2(degree * Mathf.Deg2Rad);
    }
    public static float ClampAngle(float angle, float min = -180, float max = 180)
    {
        float clamped = angle;
        while (clamped < min)
            clamped += 360;
        while (clamped > max)
            clamped -= 360;
        return clamped;
    }
}
