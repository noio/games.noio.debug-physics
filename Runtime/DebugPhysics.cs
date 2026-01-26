using UnityEngine;

public static class DebugPhysics
{
    public static readonly float DefaultDrawDuration = .2f;
    public static float DrawDuration { get; set; } = 0f;
    public static Color ColorHit { get; set; } = new Color(0.94f, 0.23f, 0.24f);
    public static Color ColorNoHit { get; set; } = new Color(0.44f, 0.94f, 0.32f);
    public static int MaxDrawnHits { get; set; } = 8;

    /***************
     * Raycast
     ***************/

    #region Raycast

    public static bool Raycast(
        Vector3 origin,
        Vector3 direction,
        float maxDistance = Mathf.Infinity,
        int layerMask = Physics.DefaultRaycastLayers,
        QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.UseGlobal)
    {
        return Raycast(origin, direction, out _, maxDistance, layerMask, queryTriggerInteraction);
    }

    public static bool Raycast(
        Ray ray,
        float maxDistance = Mathf.Infinity,
        int layerMask = Physics.DefaultRaycastLayers,
        QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.UseGlobal)
    {
        return Raycast(ray.origin, ray.direction, out _, maxDistance, layerMask, queryTriggerInteraction);
    }

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

    public static int RaycastNonAlloc(
        Vector3 origin,
        Vector3 direction,
        RaycastHit[] results,
        float maxDistance = Mathf.Infinity,
        int layerMask = Physics.DefaultRaycastLayers,
        QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.UseGlobal)
    {
        var numHits = Physics.RaycastNonAlloc(origin, direction, results, maxDistance, layerMask,
            queryTriggerInteraction);
#if DEBUG
        direction.Normalize();
        if (numHits > 0)
        {
            var drawCount = Mathf.Min(numHits, MaxDrawnHits);
            for (int i = 0; i < drawCount; i++)
            {
                var hitPoint = results[i].point;
                Debug.DrawLine(origin, hitPoint, ColorHit, DrawDuration);
                DebugDraw.WireDisc(hitPoint, results[i].normal, .2f * maxDistance, ColorHit, DrawDuration);
            }
            var restOfLineColor = ColorHit;
            restOfLineColor.a = .5f;
            Debug.DrawLine(origin, origin + direction * maxDistance, restOfLineColor, DrawDuration);
        }
        else
        {
            Debug.DrawLine(origin, origin + direction * maxDistance, ColorNoHit, DrawDuration);
        }
#endif
        return numHits;
    }

    public static int RaycastNonAlloc(
        Ray ray,
        RaycastHit[] results,
        float maxDistance = Mathf.Infinity,
        int layerMask = Physics.DefaultRaycastLayers,
        QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.UseGlobal)
    {
        return RaycastNonAlloc(ray.origin, ray.direction, results, maxDistance, layerMask,
            queryTriggerInteraction);
    }

    #endregion

    /***************
     * Linecast
     ***************/

    #region Linecast

    public static bool Linecast(
        Vector3 start,
        Vector3 end,
        int layerMask = Physics.DefaultRaycastLayers,
        QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.UseGlobal)
    {
        return Linecast(start, end, out _, layerMask, queryTriggerInteraction);
    }

    public static bool Linecast(
        Vector3 start,
        Vector3 end,
        out RaycastHit hitInfo,
        int layerMask = Physics.DefaultRaycastLayers,
        QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.UseGlobal)
    {
        var didHit = Physics.Linecast(start, end, out hitInfo, layerMask, queryTriggerInteraction);
#if DEBUG
        if (didHit)
        {
            var hitPoint = hitInfo.point;
            Debug.DrawLine(start, hitPoint, ColorHit, DrawDuration);
            var restOfLineColor = ColorHit;
            restOfLineColor.a = .5f;
            Debug.DrawLine(hitPoint, end, restOfLineColor, DrawDuration);
            DebugDraw.WireDisc(hitPoint, hitInfo.normal, .2f * (end - start).magnitude, ColorHit, DrawDuration);
        }
        else
        {
            Debug.DrawLine(start, end, ColorNoHit, DrawDuration);
        }
#endif
        return didHit;
    }

    #endregion

    /***************
     * SphereCast
     ***************/

    #region SphereCast

    public static bool SphereCast(
        Vector3 origin,
        float radius,
        Vector3 direction,
        float maxDistance = Mathf.Infinity,
        int layerMask = Physics.DefaultRaycastLayers,
        QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.UseGlobal)
    {
        return SphereCast(origin, radius, direction, out _, maxDistance, layerMask, queryTriggerInteraction);
    }

    public static bool SphereCast(
        Ray ray,
        float radius,
        float maxDistance = Mathf.Infinity,
        int layerMask = Physics.DefaultRaycastLayers,
        QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.UseGlobal)
    {
        return SphereCast(ray.origin, radius, ray.direction, out _, maxDistance, layerMask,
            queryTriggerInteraction);
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
            Debug.DrawLine(origin, hitPoint, ColorHit, DrawDuration);
            var restOfLineColor = ColorHit;
            restOfLineColor.a = .5f;
            Debug.DrawLine(hitPoint, origin + direction.normalized * maxDistance, restOfLineColor,
                DrawDuration);

            DebugDraw.WireDisc(hitPoint, hitInfo.normal, .1f, ColorHit, DrawDuration);
            DebugDraw.WireCapsule(origin, origin + direction * (hitInfo.distance), radius, ColorHit,
                DrawDuration);
        }
        else
        {
            DebugDraw.WireCapsule(origin, origin + direction * (maxDistance), radius, ColorNoHit,
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

    public static int SphereCastNonAlloc(
        Vector3 origin,
        float radius,
        Vector3 direction,
        RaycastHit[] results,
        float maxDistance = Mathf.Infinity,
        int layerMask = Physics.DefaultRaycastLayers,
        QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.UseGlobal)
    {
        var numHits = Physics.SphereCastNonAlloc(origin, radius, direction, results, maxDistance, layerMask,
            queryTriggerInteraction);
#if DEBUG
        direction.Normalize();
        if (numHits > 0)
        {
            var drawCount = Mathf.Min(numHits, MaxDrawnHits);
            for (int i = 0; i < drawCount; i++)
            {
                var hitPoint = results[i].point;
                Debug.DrawLine(origin, hitPoint, ColorHit, DrawDuration);
                DebugDraw.WireDisc(hitPoint, results[i].normal, .1f, ColorHit, DrawDuration);
                DebugDraw.WireCapsule(origin, origin + direction * results[i].distance, radius, ColorHit,
                    DrawDuration);
            }
            var restOfLineColor = ColorHit;
            restOfLineColor.a = .5f;
            Debug.DrawLine(origin, origin + direction * maxDistance, restOfLineColor, DrawDuration);
        }
        else
        {
            DebugDraw.WireCapsule(origin, origin + direction * maxDistance, radius, ColorNoHit, DrawDuration);
        }
#endif
        return numHits;
    }

    public static int SphereCastNonAlloc(
        Ray ray,
        float radius,
        RaycastHit[] results,
        float maxDistance = Mathf.Infinity,
        int layerMask = Physics.DefaultRaycastLayers,
        QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.UseGlobal)
    {
        return SphereCastNonAlloc(ray.origin, radius, ray.direction, results, maxDistance, layerMask,
            queryTriggerInteraction);
    }

    #endregion

    /***************
     * CapsuleCast
     ***************/

    #region CapsuleCast

    public static bool CapsuleCast(
        Vector3 point1,
        Vector3 point2,
        float radius,
        Vector3 direction,
        float maxDistance = Mathf.Infinity,
        int layerMask = Physics.DefaultRaycastLayers,
        QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.UseGlobal)
    {
        return CapsuleCast(point1, point2, radius, direction, out _, maxDistance, layerMask,
            queryTriggerInteraction);
    }

    public static bool CapsuleCast(
        Vector3 point1,
        Vector3 point2,
        float radius,
        Vector3 direction,
        out RaycastHit hitInfo,
        float maxDistance = Mathf.Infinity,
        int layerMask = Physics.DefaultRaycastLayers,
        QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.UseGlobal)
    {
        var didHit = Physics.CapsuleCast(point1, point2, radius, direction, out hitInfo, maxDistance, layerMask,
            queryTriggerInteraction);
#if DEBUG
        direction.Normalize();
        var capsuleCenter = (point1 + point2) * 0.5f;
        if (didHit)
        {
            var hitPoint = hitInfo.point;
            Debug.DrawLine(capsuleCenter, hitPoint, ColorHit, DrawDuration);
            var restOfLineColor = ColorHit;
            restOfLineColor.a = .5f;
            Debug.DrawLine(hitPoint, capsuleCenter + direction * maxDistance, restOfLineColor, DrawDuration);

            DebugDraw.WireDisc(hitPoint, hitInfo.normal, .1f, ColorHit, DrawDuration);
            var offset = direction * hitInfo.distance;
            DebugDraw.WireCapsule(point1 + offset, point2 + offset, radius, ColorHit, DrawDuration);
        }
        else
        {
            var offset = direction * maxDistance;
            DebugDraw.WireCapsule(point1 + offset, point2 + offset, radius, ColorNoHit, DrawDuration);
        }
#endif
        return didHit;
    }

    public static int CapsuleCastNonAlloc(
        Vector3 point1,
        Vector3 point2,
        float radius,
        Vector3 direction,
        RaycastHit[] results,
        float maxDistance = Mathf.Infinity,
        int layerMask = Physics.DefaultRaycastLayers,
        QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.UseGlobal)
    {
        var numHits = Physics.CapsuleCastNonAlloc(point1, point2, radius, direction, results, maxDistance,
            layerMask, queryTriggerInteraction);
#if DEBUG
        direction.Normalize();
        var capsuleCenter = (point1 + point2) * 0.5f;
        if (numHits > 0)
        {
            var drawCount = Mathf.Min(numHits, MaxDrawnHits);
            for (int i = 0; i < drawCount; i++)
            {
                var hitPoint = results[i].point;
                Debug.DrawLine(capsuleCenter, hitPoint, ColorHit, DrawDuration);
                DebugDraw.WireDisc(hitPoint, results[i].normal, .1f, ColorHit, DrawDuration);
                var offset = direction * results[i].distance;
                DebugDraw.WireCapsule(point1 + offset, point2 + offset, radius, ColorHit, DrawDuration);
            }
            var restOfLineColor = ColorHit;
            restOfLineColor.a = .5f;
            Debug.DrawLine(capsuleCenter, capsuleCenter + direction * maxDistance, restOfLineColor, DrawDuration);
        }
        else
        {
            var offset = direction * maxDistance;
            DebugDraw.WireCapsule(point1 + offset, point2 + offset, radius, ColorNoHit, DrawDuration);
        }
#endif
        return numHits;
    }

    #endregion

    /***************
     * BoxCast
     ***************/

    #region BoxCast

    public static bool BoxCast(
        Vector3 center,
        Vector3 halfExtents,
        Vector3 direction,
        Quaternion orientation = default,
        float maxDistance = Mathf.Infinity,
        int layerMask = Physics.DefaultRaycastLayers,
        QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.UseGlobal)
    {
        return BoxCast(center, halfExtents, direction, out _, orientation, maxDistance, layerMask,
            queryTriggerInteraction);
    }

    public static bool BoxCast(
        Vector3 center,
        Vector3 halfExtents,
        Vector3 direction,
        out RaycastHit hitInfo,
        Quaternion orientation = default,
        float maxDistance = Mathf.Infinity,
        int layerMask = Physics.DefaultRaycastLayers,
        QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.UseGlobal)
    {
        var didHit = Physics.BoxCast(center, halfExtents, direction, out hitInfo, orientation, maxDistance,
            layerMask, queryTriggerInteraction);
#if DEBUG
        direction.Normalize();
        if (didHit)
        {
            var hitPoint = hitInfo.point;
            Debug.DrawLine(center, hitPoint, ColorHit, DrawDuration);
            var restOfLineColor = ColorHit;
            restOfLineColor.a = .5f;
            Debug.DrawLine(hitPoint, center + direction * maxDistance, restOfLineColor, DrawDuration);

            DebugDraw.WireDisc(hitPoint, hitInfo.normal, .1f, ColorHit, DrawDuration);
            DebugDraw.WireCube(center + direction * hitInfo.distance, halfExtents, orientation, ColorHit,
                DrawDuration);
        }
        else
        {
            DebugDraw.WireCube(center + direction * maxDistance, halfExtents, orientation, ColorNoHit,
                DrawDuration);
        }
#endif
        return didHit;
    }

    public static int BoxCastNonAlloc(
        Vector3 center,
        Vector3 halfExtents,
        Vector3 direction,
        RaycastHit[] results,
        Quaternion orientation = default,
        float maxDistance = Mathf.Infinity,
        int layerMask = Physics.DefaultRaycastLayers,
        QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.UseGlobal)
    {
        var numHits = Physics.BoxCastNonAlloc(center, halfExtents, direction, results, orientation, maxDistance,
            layerMask, queryTriggerInteraction);
#if DEBUG
        direction.Normalize();
        if (numHits > 0)
        {
            var drawCount = Mathf.Min(numHits, MaxDrawnHits);
            for (int i = 0; i < drawCount; i++)
            {
                var hitPoint = results[i].point;
                Debug.DrawLine(center, hitPoint, ColorHit, DrawDuration);
                DebugDraw.WireDisc(hitPoint, results[i].normal, .1f, ColorHit, DrawDuration);
                DebugDraw.WireCube(center + direction * results[i].distance, halfExtents, orientation, ColorHit,
                    DrawDuration);
            }
            var restOfLineColor = ColorHit;
            restOfLineColor.a = .5f;
            Debug.DrawLine(center, center + direction * maxDistance, restOfLineColor, DrawDuration);
        }
        else
        {
            DebugDraw.WireCube(center + direction * maxDistance, halfExtents, orientation, ColorNoHit,
                DrawDuration);
        }
#endif
        return numHits;
    }

    #endregion

    /***************
     * Check Methods
     ***************/

    #region Check Methods

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

    #endregion

    /***************
     * Overlap Methods
     ***************/

    #region Overlap Methods

    public static int OverlapSphereNonAlloc(
        Vector3 position,
        float radius,
        Collider[] results,
        int layerMask = Physics.DefaultRaycastLayers,
        QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.UseGlobal)
    {
        var numHits =
            Physics.OverlapSphereNonAlloc(position, radius, results, layerMask, queryTriggerInteraction);
#if DEBUG
        var sphereColor = numHits > 0 ? ColorHit : ColorNoHit;
        sphereColor.a = .5f;
        DebugDraw.WireSphere(position, radius, sphereColor, DrawDuration);
        var drawCount = Mathf.Min(numHits, MaxDrawnHits);
        for (int i = 0; i < drawCount; i++)
        {
            var collider = results[i];
            DebugDraw.WireCube(collider.bounds.center, collider.bounds.size, Quaternion.identity, ColorHit,
                DrawDuration);
        }
#endif

        return numHits;
    }

    public static int OverlapBoxNonAlloc(
        Vector3 center,
        Vector3 halfExtents,
        Collider[] results,
        Quaternion orientation = default,
        int layerMask = Physics.DefaultRaycastLayers,
        QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.UseGlobal)
    {
        var numHits =
            Physics.OverlapBoxNonAlloc(center, halfExtents, results, orientation, layerMask, queryTriggerInteraction);
#if DEBUG
        var boxColor = numHits > 0 ? ColorHit : ColorNoHit;
        boxColor.a = .5f;
        DebugDraw.WireCube(center, halfExtents, orientation, boxColor, DrawDuration);
        var drawCount = Mathf.Min(numHits, MaxDrawnHits);
        for (int i = 0; i < drawCount; i++)
        {
            var collider = results[i];
            DebugDraw.WireCube(collider.bounds.center, collider.bounds.size, Quaternion.identity, ColorHit,
                DrawDuration);
        }
#endif

        return numHits;
    }

    public static int OverlapCapsuleNonAlloc(
        Vector3 point0,
        Vector3 point1,
        float radius,
        Collider[] results,
        int layerMask = Physics.DefaultRaycastLayers,
        QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.UseGlobal)
    {
        var numHits =
            Physics.OverlapCapsuleNonAlloc(point0, point1, radius, results, layerMask, queryTriggerInteraction);
#if DEBUG
        var capsuleColor = numHits > 0 ? ColorHit : ColorNoHit;
        capsuleColor.a = .5f;
        DebugDraw.WireCapsule(point0, point1, radius, capsuleColor, DrawDuration);
        var drawCount = Mathf.Min(numHits, MaxDrawnHits);
        for (int i = 0; i < drawCount; i++)
        {
            var collider = results[i];
            DebugDraw.WireCube(collider.bounds.center, collider.bounds.size, Quaternion.identity, ColorHit,
                DrawDuration);
        }
#endif

        return numHits;
    }

    #endregion
}