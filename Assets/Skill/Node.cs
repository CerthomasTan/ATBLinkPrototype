using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node<T>
{
    //Will be unique to the skill node. This is to prevent 
    //duplicated data of large class objects.
    public T data { get; set; }
    public Node<T> next { get; set; }

    public Node()
    {

    }

    public Node(T t)
    {
        this.data = t;
    }
    /*
     * copy constructor
     */
    public Node(Node<T> n)
    {
        this.data = n.data;
    }

}
