using UnityEngine;
using System.Collections;

public class MoveUpDown : MonoBehaviour
{    
    // Velocity
    public Vector2 velocity = Vector2.up;
    
    // Direction Change Interval
    public float interval = 1.5f;

    // Use this for initialization
    void Start()
    {
        // Change Direction every now and then
        InvokeRepeating("ChangeDir", interval, interval);
    }
    
    // Update is called once per frame
    void Update()
    {
        transform.position += (Vector3)velocity * Time.deltaTime;
    }
    
    void ChangeDir()
    {
        velocity = -velocity;
    }
}
