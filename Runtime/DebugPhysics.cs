using UnityEngine;

public static class DebugPhysics
{
    public static readonly float DefaultDrawDuration = .2f;
    public static float DrawDuration { get; set; } = 0f;
    public static Color ColorHit { get; set; } = new Color(0.94f, 0.23f, 0.24f);
    public static Color ColorNoHit { get; set; } = new Color(0.44f, 0.94f, 0.32f);

    public static bool Raycast(
        Vector3 origin,
        Vector3 direction,
        out RaycastHit hitInfo,
        float maxDistance = Mathf.Infinity,
        int layerMask = Physics.DefaultRaycastLayers,
        QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.UseGlobal)
    {
        var didHit = Physics.Raycast(origin, direction, out hitInfo, maxDistance, layerMask,
            queryTriggerInteraction);
#if DEBUG
        direction.Normalize();
        if (didHit)
        {
            var hitPoint = hitInfo.point;
            Debug.DrawLine(origin, hitPoint, ColorHit, DrawDuration);
            var restOfLineColor = ColorHit;
            restOfLineColor.a = .5f;
            Debug.DrawLine(hitPoint, origin + direction * maxDistance, restOfLineColor, DrawDuration);

            DebugDraw.WireDisc(hitPoint, hitInfo.normal, .2f * maxDistance, ColorHit, DrawDuration);
        }
        else
        {
            Debug.DrawLine(origin, origin + direction * maxDistance, ColorNoHit, DrawDuration);
        }
#endif
        return didHit;
    }

    public static bool Raycast(
        Ray ray,
        out RaycastHit hitInfo,
        float maxDistance = Mathf.Infinity,
        int layerMask = Physics.DefaultRaycastLayers,
        QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.UseGlobal)
    {
        return Raycast(ray.origin, ray.direction, out hitInfo, maxDistance, layerMask,
            queryTriggerInteraction);
    }

    public static bool CheckCapsule(
        Vector3 start,
        Vector3 end,
        float radius,
        int layerMask = Physics.DefaultRaycastLayers,
        QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.UseGlobal
    )
    {
        var didHit = Physics.CheckCapsule(start, end, radius, layerMask, queryTriggerInteraction);
#if DEBUG
        DebugDraw.WireCapsule(start, end, radius, didHit ? ColorHit : ColorNoHit, DrawDuration);
#endif
        return didHit;
    }

    public static bool CheckSphere(
        Vector3 position,
        float radius,
        int layerMask = Physics.DefaultRaycastLayers,
        QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.UseGlobal
    )
    {
        var didHit = Physics.CheckSphere(position, radius, layerMask, queryTriggerInteraction);
#if DEBUG
        DebugDraw.WireSphere(position, radius, didHit ? ColorHit : ColorNoHit, DrawDuration);
#endif
        return didHit;
    }

    public static bool CheckBox(
        Vector3 center,
        Vector3 extents,
        Quaternion rotation,
        LayerMask layerMask,
        QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.UseGlobal)
    {
        var didHit = Physics.CheckBox(center, extents, rotation, layerMask);
#if DEBUG
        DebugDraw.WireCube(center, extents, rotation, didHit ? ColorHit : ColorNoHit, DrawDuration);
#endif
        return didHit;
    }

    public static int OverlapSphereNonAlloc(
        Vector3 position,
        float radius,
        Collider[] results,
        LayerMask layerMask,
        QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.UseGlobal)
    {
        var numHits =
            Physics.OverlapSphereNonAlloc(position, radius, results, layerMask, queryTriggerInteraction);
#if DEBUG
        DebugDraw.WireSphere(position, radius, Color.white, DrawDuration);
        for (int i = 0; i < numHits; i++)
        {
            var collider = results[i];
            DebugDraw.WireCube(collider.bounds.center, collider.bounds.size, Quaternion.identity, ColorHit,
                DrawDuration);
        }
#endif

        return numHits;
    }

    public static bool SphereCast(
        Vector3 origin,
        float radius,
        Vector3 direction,
        out RaycastHit hitInfo,
        float maxDistance = Mathf.Infinity,
        int layerMask = Physics.DefaultRaycastLayers,
        QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.UseGlobal)
    {
        var didHit = Physics.SphereCast(origin, radius, direction, out hitInfo, maxDistance, layerMask,
            queryTriggerInteraction);
#if DEBUG
        direction.Normalize();
        if (didHit)
        {
            var hitPoint = hitInfo.point;
            Debug.DrawLine(origin, hitPoint, Color.white, DrawDuration);
            var restOfLineColor = new Color(1, 1, 1, .5f);
            Debug.DrawLine(hitPoint, origin + direction.normalized * maxDistance, restOfLineColor,
                DrawDuration);

            DebugDraw.WireDisc(hitPoint, hitInfo.normal, .1f, ColorHit, DrawDuration);
            DebugDraw.WireCapsule(origin, origin + direction * (hitInfo.distance), radius, Color.red,
                DrawDuration);
        }
        else
        {
            DebugDraw.WireCapsule(origin, origin + direction * (maxDistance), radius, Color.green,
                DrawDuration);
        }
#endif
        return didHit;
    }

    public static bool SphereCast(
        Ray ray,
        float radius,
        out RaycastHit hitInfo,
        float maxDistance = Mathf.Infinity,
        int layerMask = Physics.DefaultRaycastLayers,
        QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.UseGlobal)
    {
        return SphereCast(ray.origin, radius, ray.direction, out hitInfo, maxDistance, layerMask,
            queryTriggerInteraction);
    }
}