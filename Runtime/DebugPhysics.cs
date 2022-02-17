using UnityEngine;

public static class DebugPhysics
{
    public static float DrawDuration { get; set; } = 0f;
    public static Color ColorHint { get; set; } = new Color(0.74f, 0.8f, 0.94f);

    public static bool Raycast(
        Vector3        origin,
        Vector3        direction,
        out RaycastHit hitInfo,
        float          maxDistance,
        int            layerMask = Physics.DefaultRaycastLayers)
    {
        var didHit = Physics.Raycast(origin, direction, out hitInfo, maxDistance, layerMask);
#if DEBUG
        direction.Normalize();
        if (didHit)
        {
            var hitPoint = hitInfo.point;
            Debug.DrawLine(origin, hitPoint, ColorHint, DrawDuration);
            DebugDraw.WireDisc(hitPoint, hitInfo.normal, .2f * maxDistance, ColorHint, DrawDuration);
            var color2 = ColorHint;
            color2.a = .5f;
            Debug.DrawLine(hitPoint, origin + direction * maxDistance, color2, DrawDuration);
        }
        else
        {
            Debug.DrawLine(origin, origin + direction * maxDistance, ColorHint, DrawDuration);
        }
#endif
        return didHit;
    }

    public static bool CheckCapsule(
        Vector3 start,
        Vector3 end,
        float   radius,
        int     layerMask = Physics.DefaultRaycastLayers,
        Color?  color     = null
    )
    {
#if !DEBUG
        return Physics.CheckCapsule(start, end, radius, layerMask);
#else
        if (color == null)
        {
            color = Color.green;
        }

        var overlap = Physics.CheckCapsule(start, end, radius, layerMask);
        DebugDraw.WireCapsule(start, end, radius, overlap ? Color.red : color.Value, DrawDuration);
        return overlap;
#endif
    }

    public static bool CheckSphere(
        Vector3 position,
        float   radius,
        int     layerMask = Physics.DefaultRaycastLayers,
        Color?  color     = null
    )
    {
#if !DEBUG
        return Physics.CheckSphere(position, radius, layerMask);
#else
        if (color == null)
        {
            color = Color.green;
        }

        var overlap = Physics.CheckSphere(position, radius, layerMask);
        DebugDraw.WireSphere(position, radius, overlap ? Color.red : color.Value, DrawDuration);
        return overlap;
#endif
    }

    public static int OverlapSphereNonAlloc(Vector3 position, float radius, Collider[] results, LayerMask layerMask)
    {
#if !DEBUG
        return Physics.OverlapSphereNonAlloc(position, radius, results, layerMask);
#endif
        var numFound = Physics.OverlapSphereNonAlloc(position, radius, results, layerMask);
        DebugDraw.WireSphere(position, radius, Color.white, DrawDuration);
        for (int i = 0; i < numFound; i++)
        {
            var collider = results[i];
            DebugDraw.WireCube(collider.bounds.center, collider.bounds.size, Quaternion.identity, Color.green, DrawDuration);
        }

        return numFound;
    }

    public static bool SphereCast(Vector3 origin, float radius, Vector3 direction, out RaycastHit hitInfo, float maxDistance,
        LayerMask                         layerMask)
    {
        var didHit = Physics.SphereCast(origin, radius, direction, out hitInfo, maxDistance, layerMask);
#if DEBUG
        direction.Normalize();
        if (didHit)
        {
            var hitPoint = hitInfo.point;
            Debug.DrawLine(origin, hitPoint, Color.white, DrawDuration);
            DebugDraw.WireDisc(hitPoint, hitInfo.normal, .1f, Color.red, DrawDuration);
            var color2 = new Color(1, 1, 1, .5f);
            Debug.DrawLine(hitPoint, origin + direction.normalized * maxDistance, color2, DrawDuration);
            DebugDraw.WireCapsule(origin, origin + direction * (hitInfo.distance), radius, Color.red, DrawDuration);
        }
        else
        {
            DebugDraw.WireCapsule(origin, origin + direction * (maxDistance), radius, Color.green, DrawDuration);
        }
#endif
        return didHit;
    }
}