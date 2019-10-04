// agent.CalculatePath returns a 3D path, but we need a 2D path.
using UnityEngine;
using UnityEngine.AI;

public class NavMeshPath2D
{
    public Vector2[] corners;
    public NavMeshPathStatus status;
}
