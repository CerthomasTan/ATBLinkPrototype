using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// This class is a Key. This container will hold one int value. 
/// Author: Certhomas Tan
/// </summary>
public class Key 
{
    /// <summary>
    /// This is the value of the key
    /// </summary>
    /// <value>This is the key</value>
    public int value { get; set; }

    /// <summary>
    /// This is the constructor of the key. The key requires a int value
    /// </summary>
    /// <param name="value">the number that the key is.</param>
    public Key(int value)
    {
        this.value = value;
    }

    /// <summary>
    /// Copy Constructor
    /// </summary>
    /// <param name="key">The key to copy</param>
    public Key(Key key)
    {
        this.value = key.value;
    }

}
