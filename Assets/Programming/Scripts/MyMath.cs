using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MyMath
{
    public static Vector2 RotationToDirection(float angle)
    {
        float rad = angle * Mathf.Deg2Rad;
        return new Vector2(Mathf.Cos(rad), Mathf.Sin(rad)).normalized;
    }
    public static float DirectionToRotation(Vector2 dir)
    {
        return Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
    }
}
