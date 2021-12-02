using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key 
{
    public int value { get; set; }

    public Key(int value)
    {
        this.value = value;
    }

    public Key(Key key)
    {
        this.value = key.value;
    }

}
