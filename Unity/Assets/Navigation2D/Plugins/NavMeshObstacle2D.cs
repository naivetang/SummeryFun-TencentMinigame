// Navigation2D Script (c) noobtuts.com
using UnityEngine;
using UnityEngine.AI;

public class NavMeshObstacle2D : MonoBehaviour
{
    // NavMeshObstacle properties
    public NavMeshObstacleShape shape = NavMeshObstacleShape.Box;
    public Vector2 center;
    public Vector2 size = Vector2.one;
    public bool carve = false; // experimental and hard to debug in 2D

    // the projection
    NavMeshObstacle obst;

    // monobehaviour ///////////////////////////////////////////////////////////
    void Awake()
    {
        // create projection
        var go = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        go.name = "NAVIGATION2D_OBSTACLE";
        go.transform.position = NavMeshUtils2D.ProjectTo3D(transform.position);
        go.transform.rotation = Quaternion.Euler(NavMeshUtils2D.RotationTo3D(transform.eulerAngles));
        obst = go.AddComponent<NavMeshObstacle>();

        // disable mesh and collider (no collider for now)
        Destroy(obst.GetComponent<Collider>());
        Destroy(obst.GetComponent<MeshRenderer>());
    }

    void Update()
    {
        // copy properties to projection all the time
        // (in case they are modified after creating it)
        obst.carving = carve;
        obst.center = NavMeshUtils2D.ProjectTo3D(center);
        obst.size = new Vector3(size.x, 1, size.y);
        
        // scale and rotate to match scaled/rotated sprites center properly
        obst.transform.localScale = new Vector3(transform.localScale.x, 1, transform.localScale.y);
        obst.transform.rotation = Quaternion.Euler(NavMeshUtils2D.RotationTo3D(transform.eulerAngles));
        
        // project position to 3d
        obst.transform.position = NavMeshUtils2D.ProjectTo3D(transform.position);
    }
        
    void OnDestroy()
    {
        // destroy projection if not destroyed yet
        if (obst) Destroy(obst.gameObject);
    }
    
    void OnEnable()
    {
        if (obst) obst.enabled = true;
    }
        
    void OnDisable()
    {
        if (obst) obst.enabled = false;
    }

    // radius gizmo (gizmos.matrix for correct rotation)
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.localRotation, transform.localScale);
        Gizmos.DrawWireCube(center, size);
    }

    // validation
    void OnValidate()
    {
        // force shape to box for now because we would need a separate Editor
        // GUI script to switch between size and radius otherwise
        shape = NavMeshObstacleShape.Box;
    }
    
    // NavMeshAgent proxies ////////////////////////////////////////////////////
    public Vector2 velocity
    {
        get { return NavMeshUtils2D.ProjectTo2D(obst.velocity); }
        // set: is a bad idea
    }
}
