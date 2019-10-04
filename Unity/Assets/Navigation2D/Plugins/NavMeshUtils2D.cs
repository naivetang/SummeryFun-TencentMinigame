using UnityEngine;

public class NavMeshUtils2D
{
    // project agent position to 2D
    public static Vector2 ProjectTo2D(Vector3 v)
    {
        return new Vector2(v.x, v.z);
    }

    // project 2D position to agent position
    public static Vector3 ProjectTo3D(Vector2 v)
    {
        return new Vector3(v.x, 0, v.y);
    }

    // project 2D rotation to 3D
    public static Vector3 RotationTo3D(Vector3 v)
    {
        return new Vector3(0, -v.z, 0);
    }

    // project 2D scale to 3D
    public static Vector3 ScaleTo3D(Vector3 v)
    {
        return new Vector3(v.x, 1, v.y);
    }

    public static Vector2[] AdjustMinMax(Collider2D co, Vector2 min, Vector2 max)
    {
        min.x = Mathf.Min(co.bounds.min.x, min.x);
        min.y = Mathf.Min(co.bounds.min.y, min.y);
        max.x = Mathf.Max(co.bounds.max.x, max.x);
        max.y = Mathf.Max(co.bounds.max.y, max.y);
        return new Vector2[]{min, max};
    }

    public static Vector3 ScaleFromBoxCollider2D(BoxCollider2D co)
    {
        // transform.localScale * collider size (but with components swapped for 3d)
        return Vector3.Scale(ScaleTo3D(co.transform.localScale), new Vector3(co.size.x, 1, co.size.y));
    }

    public static Vector3 ScaleFromCircleCollider2D(CircleCollider2D co)
    {
        // transform.localScale * collider size (but with components swapped for 3d)
        // radius * 2 because diameter := radius * 2
        return Vector3.Scale(ScaleTo3D(co.transform.localScale), new Vector3(co.radius*2, 1, co.radius*2));
    }

    public static Vector3 ScaleFromPolygonCollider2D(PolygonCollider2D co)
    {
        // transform.localScale * collider size (but with components swapped for 3d)
        return ScaleTo3D(co.transform.localScale);
    }

    public static Vector3 ScaleFromEdgeCollider2D(EdgeCollider2D co)
    {
        // transform.localScale * collider size (but with components swapped for 3d)
        return ScaleTo3D(co.transform.localScale);
    }
}
