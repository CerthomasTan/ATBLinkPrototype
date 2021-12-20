using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// class that controls the behavoir of the cursor
/// 
/// </summary>
public class Cursor3d : MonoBehaviour
{
    [SerializeField] float distanceFromObject = 10;
    public float Amplitude = 1.5f;  
    public float speed = 2.0f;
    private Vector3 startPos;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        Vector3 v = startPos;
        v.y += Amplitude * Mathf.Sin(Time.time * speed);
        transform.position = v;
    }

    /// <summary>
    /// moves the curos to a gameobject's position
    /// </summary>
    /// <param name="gameObject"></param>
    public void moveCursor(GameObject gameObject)
    {
        Vector3 v = gameObject.transform.position;
        v.y += distanceFromObject;
        startPos = v;
    }

}
