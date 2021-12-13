using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cursor : MonoBehaviour
{


    public float delta = 1.5f;  // Amount to move left and right from the start point
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
        v.x += delta * Mathf.Sin(Time.time * speed);
        transform.position = v;
    }
    
    public void moveCursor(Vector3 position)
    {
        this.startPos = position;
        startPos.x = startPos.x - distanceFromButton;
        transform.position = startPos;
    }
 }


