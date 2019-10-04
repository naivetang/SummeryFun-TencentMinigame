using UnityEngine;
using System.Collections;

public class Move : MonoBehaviour
{
    public Camera camera;

    void Start()
    {
        if (this.camera == null)
            this.camera = Camera.main;
    }
    
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            Vector3 w = camera.ScreenToWorldPoint(Input.mousePosition);
            
            
            GetComponent<NavMeshAgent2D>().destination = w;

            //this.transform.position = w;
        }
    }
}