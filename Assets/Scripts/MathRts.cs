using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MathRts
{
    public static float FindDistanceNotSqrt(Vector3 a, Vector3 b)
    {
        float num = a.x - b.x;
        float num2 = a.y - b.y;
        float num3 = a.z - b.z;
        return num * num + num2 * num2 + num3 * num3;
    }

    public static float GetArcHeight(float time)
    {
        return -(time * time) + time;
    }
}
