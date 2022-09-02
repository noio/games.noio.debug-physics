using UnityEngine;

public static class DebugDraw
{
    public static void Axes(Vector3 position, Quaternion rotation, float duration = 0.1f)
    {
        Debug.DrawRay(position, rotation * Vector3.right, Color.red, duration, false);
        Debug.DrawRay(position, rotation * Vector3.up, Color.green, duration, false);
        Debug.DrawRay(position, rotation * Vector3.forward, Color.blue, duration, false);
    }

    public static void WireCube(Vector3 center, Vector3 size, Quaternion rotation, Color color,
        float                           duration = 0.1f)
    {
        /*
         The points are laid out in the array as follows

              4----5
             /    /|
            0----1 |
            | 6  | 7
            |    |/
            2----3

         */

        var points = new[]
        {
            new Vector3(-size.x, +size.y, -size.z), // Front top left
            new Vector3(+size.x, +size.y, -size.z), // Front top right
            new Vector3(-size.x, -size.y, -size.z), // Front bottom left
            new Vector3(+size.x, -size.y, -size.z), // Front bottom right
            new Vector3(-size.x, +size.y, +size.z), // Back top left
            new Vector3(+size.x, +size.y, +size.z), // Back top right
            new Vector3(-size.x, -size.y, +size.z), // Back bottom left
            new Vector3(+size.x, -size.y, +size.z)  // Back bottom right
        };

        for (var i = 0; i < points.Length; i++)
        {
            points[i] = rotation * points[i] + center;
        }

        Debug.DrawLine(points[0], points[1], color, duration);
        Debug.DrawLine(points[1], points[3], color, duration);
        Debug.DrawLine(points[3], points[2], color, duration);
        Debug.DrawLine(points[2], points[0], color, duration);

        Debug.DrawLine(points[4], points[5], color, duration);
        Debug.DrawLine(points[5], points[7], color, duration);
        Debug.DrawLine(points[7], points[6], color, duration);
        Debug.DrawLine(points[6], points[4], color, duration);

        Debug.DrawLine(points[0], points[4], color, duration);
        Debug.DrawLine(points[1], points[5], color, duration);
        Debug.DrawLine(points[3], points[7], color, duration);
        Debug.DrawLine(points[2], points[6], color, duration);
    }

    public static void WireSphere(Vector3 center, float radius, Color color, float duration = 0)
    {
        WireDisc(center, Vector3.up, radius, color, duration);
        WireDisc(center, Vector3.right, radius, color, duration);
        WireDisc(center, Vector3.forward, radius, color, duration);
    }

    public static void WireSphere(Vector3 center, float radius, Quaternion rotation, Color color,
        float                             duration = 0)
    {
        WireDisc(center, rotation * Vector3.up, radius, color, duration);
        WireDisc(center, rotation * Vector3.right, radius, color, duration);
        WireDisc(center, rotation * Vector3.forward, radius, color, duration);
    }

    public static void WireCapsule(
        Vector3 start,
        Vector3 end,
        float   radius,
        Color   color,
        float   duration = 0)
    {
        var forward = end - start;
        /*
         * Capsule is so short it is actually a sphere
         */
        var magnitude = forward.magnitude;
        if (magnitude < 1f / 1000)
        {
            WireSphere(start, radius, color, duration);
        }
        else
        {
            forward /= magnitude;
            var up = Vector3.Cross(forward, Vector3.up);
            if (up.sqrMagnitude < 1f / 1000)
            {
                up = Vector3.Cross(forward, Vector3.right);
            }

            up.Normalize();

            var right = Vector3.Cross(forward, up);

            WireArc(start, forward, up, 360, radius, color, duration);
            WireArc(end, forward, up, 360, radius, color, duration);

            WireArc(start, up, -right, 180, radius, color, duration);
            WireArc(start, right, up, 180, radius, color, duration);
            WireArc(end, up, right, 180, radius, color, duration);
            WireArc(end, right, -up, 180, radius, color, duration);

            up *= radius;
            right *= radius;
            Debug.DrawLine(start + up, end + up, color, duration);
            Debug.DrawLine(start - up, end - up, color, duration);
            Debug.DrawLine(start + right, end + right, color, duration);
            Debug.DrawLine(start - right, end - right, color, duration);
        }
    }

    /// <summary>
    /// Draws a wire arc
    /// </summary>
    /// <param name="center">The center of the arc</param>
    /// <param name="normal">Normal direction, perpendicular to the disc</param>
    /// <param name="from">Vector that points in the direction of one end of the arc. Must be normalized</param>
    /// <param name="angle">Total angle width of the arc</param>
    /// <param name="radius">Radius of the arc</param>
    /// <param name="color"></param>
    /// <param name="duration"></param>
    public static void WireArc(
        Vector3 center,
        Vector3 normal,
        Vector3 from,
        float   angle,
        float   radius,
        Color   color,
        float   duration = 0,
        bool drawCaps = false)
    {
        from *= radius;
        var prev = center + from;
        var steps = Mathf.CeilToInt(angle / 20);
        var anglePerStep = angle / steps;
        var rotation = Quaternion.AngleAxis(anglePerStep, normal);
        if (drawCaps)
        {
            Debug.DrawLine(center, prev, color, duration);
        }
        for (var i = 0; i < steps; i++)
        {
            from = rotation * from;
            var point = center + from;
            Debug.DrawLine(prev, point, color, duration);
            prev = point;
        }
        if (drawCaps)
        {
            Debug.DrawLine(center, prev, color, duration);
        }
    }

    public static void WireDisc(
        Vector3 center,
        Vector3 normal,
        float   radius,
        Color   color,
        float   duration = 0)
    {
        var from = Vector3.Cross(normal, Vector3.up);
        if (from.sqrMagnitude < 1f / 1000)
        {
            from = Vector3.Cross(normal, Vector3.right);
        }

        from.Normalize();
        WireArc(center, normal, from, 360, radius, color, duration);
    }

    public static void Arrow(Vector3 start, Vector3 end, Vector3 up, Color color, float duration = 0)
    {
        var dir = end - start;
        var length = dir.magnitude;
        if (length > 0.001f)
        {
            Debug.DrawLine(start, end, color, duration);
            dir /= length;
            var left = Vector3.Cross(dir, up).normalized;
            var right = -left;
            var arrowSize = length * .25f;
            Debug.DrawLine(end, end - (dir - left) * arrowSize, color, duration);
            Debug.DrawLine(end, end - (dir - right) * arrowSize, color, duration);
        }
    }

    public static void ArrowV(Vector3 start, Vector3 dir, Vector3 up, Color color, float duration = 0)
    {
        Arrow(start, start + dir, up, color, duration);
    }
    //
    // /// <summary>
    // /// Draws a grid on the XZ plane (with normal in the Y dir)
    // /// </summary>
    // /// <param name="pos"></param>
    // /// <param name="rotation"></param>
    // /// <param name="size"></param>
    // /// <param name="subdivisions"></param>
    // /// <param name="color"></param>
    // /// <param name="duration"></param>
    // /// <returns></returns>
    // public static void Grid(
    //     Vector3 pos,
    //     Quaternion rotation,
    //     Vector2 size,
    //     Vector2Int subdivisions,
    //     Color color,
    //     float duration = 0)
    // {
    //     var start = pos - rotation * size * .5f;
    //     var xIncr = rotation * Vector3.right * size.x / subdivisions.x;
    //     var yIncr = rotation * Vector3.forward * size.y / subdivisions.y;
    //     for (int i = 0; i <= subdivisions.x; i++)
    //     {
    //         Debug.DrawLine()
    //     }
    // }
}