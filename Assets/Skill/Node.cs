using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// This is a node that will contain a particular data. That data will be declared when object is instantiated.
/// </summary>
/// <typeparam name="T">Generic data type will be defined when creating an object</typeparam>
public class Node<T>
{
    /// <summary>
    /// Varaible that will contain a particular concrete class
    /// </summary>
    public T data { get; set; }
    /// <summary>
    /// this is the next node. This will be used in creating different nodes.
    /// </summary>
    public Node<T> next { get; set; }

    /// <summary>
    /// default constructor 
    /// </summary>
    public Node()
    {

    }

    /// <summary>
    /// Constructor that will store object.
    /// </summary>
    /// <param name="t">object to store in node</param>
    public Node(T t)
    {
        this.data = t;
    }
    
    /// <summary>
    /// copy constructor. Will make a new copy of a node
    /// </summary>
    /// <param name="n">Node to copy</param>
    public Node(Node<T> n)
    {
        this.data = n.data;
    }

}
