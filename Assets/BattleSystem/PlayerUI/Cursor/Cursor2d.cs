using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// class that controls the behavoir of the 2d cursor
/// 
/// </summary>
public class Cursor2d : MonoBehaviour
{
    public float amplitude = 1.5f;  
    public float speed = 2.0f;
    private Vector3 startPos;
    [SerializeField] float distanceFromButton = 50;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        Vector3 v = startPos;
        v.x += amplitude * Mathf.Sin(Time.time * speed);
        transform.position = v;
    }
    
    /// <summary>
    /// will move cursor to gameobject position
    /// </summary>
    /// <param name="position"></param>
    public void moveCursor(Vector3 position)
    {
        this.startPos = position;
        startPos.x = startPos.x - distanceFromButton;
        transform.position = startPos;
    }
 }


