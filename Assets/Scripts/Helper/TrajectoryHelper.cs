using System;
using UnityEngine;

public class TrajectoryHelper
{
    public static Vector3 CalculateVelocity(Vector3 target, Vector3 origin, float time)
    {
        Vector3 distance = target - origin;
        Vector3 distanceXZ = distance;
        distanceXZ.y = 0f;

        float Sy = distance.y;
        float Sxz = distanceXZ.magnitude;

        float Vxz = Sxz / time;
        float Vy = Sy / time + 0.5f * Mathf.Abs(Physics.gravity.y) * time;

        Vector3 result = distanceXZ.normalized;
        result *= Vxz;
        result.y = Vy;

        return result;
    }

    public static Vector3 CalculatePosInTime(Vector3 origin, Vector3 velocity, float time)
    {
        Vector3 Vxz = velocity;
        Vxz.y = 0f;

        Vector3 result = origin + velocity * time;
        float sY = (-0.5f * Math.Abs(Physics.gravity.y) * (time * time)) + (velocity.y * time) + origin.y;

        result.y = sY;

        return result;
    }
}
