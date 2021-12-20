using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This is a data set structure. This set will in the keys of skills and place them into a hash table. 
/// Data in the set is unordered, contains no duplicates, and only contains keys.
/// </summary>
public class SkillSet
{
    int arraySize = 5;
    //will contain integers, will need to box
    Node<Key>[] keyValuesArray;
    
    /// <summary>
    /// constructor to create empty skillset
    /// </summary>
    public SkillSet()
    {
        keyValuesArray = new Node<Key>[arraySize];
    }

    /// <summary>
    /// copy constructo to create a new skill set
    /// </summary>
    /// <param name="skillSet"></param>
    public SkillSet(SkillSet skillSet)
    {
        keyValuesArray = new Node<Key>[arraySize];
        foreach (Node<Key> node in skillSet.ToArray())
        {
            Insert(node.data);
        }
    }

    /// <summary>
    /// Constructor that will create a set and insert a key 
    /// </summary>
    /// <param name="key"></param>
    public SkillSet(Key key)
    {
        keyValuesArray = new Node<Key>[arraySize];
        Insert(key);
    }

    /// <summary>
    /// Constructor that will create a set and insert all skills
    /// </summary>
    /// <param name="skills"></param>
    public SkillSet(Skill[] skills)
    {
        keyValuesArray = new Node<Key>[arraySize];
        foreach (Skill c in skills)
        {
            Insert(c);
        }
    }

    /// <summary>
    /// Constructor that will create a set and insert a skill by geting the key of that skill
    /// </summary>
    /// <param name="skill"></param>
    public SkillSet(Skill skill)
    {
        keyValuesArray = new Node<Key>[arraySize];
        Insert(skill);
    }

    /// <summary>
    /// Will insert a skill into the set
    /// </summary>
    /// <param name="skill"></param>
    public void Insert(Skill skill)
    {
        Insert(skill.key.value);
    }

    /// <summary>
    /// Insert a key into the set
    /// </summary>
    /// <param name="key"></param>
    public void Insert(Key key)
    {
        Insert(key.value);
    }

    /// <summary>
    /// insert a key into the set
    /// </summary>
    /// <param name="key"></param>
    private void Insert(int key)
    {
        int address = HashIt(key);
        if (keyValuesArray[address] == null)
        {
            keyValuesArray[address] = new Node<Key>(new Key(key));
            return;
        }

        else
        {
            //get first key node
            Node<Key> current = keyValuesArray[address];

            //check if exists, if skill exists in set. Exit out of method
            if (current.data.value == key)
            {
                return;
            }
            
            //check if there is a 2nd skill node to access, 
            while(current.next != null)
            {
                if (current.next.data.value == key){
                    return;
                }
                else
                {
                    current = current.next;
                }
            }
            //link key into the list
            current.next = new Node<Key>(new Key(key));
            return;
        }
    }

    /// <summary>
    /// hash method to return address of key in array
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    private int HashIt(int key)
    {
        int hash = key.GetHashCode();
        return hash % arraySize;
    }

    /// <summary>
    /// Checks if skill is in set
    /// </summary>
    /// <param name="skill"></param>
    /// <returns></returns>
    public bool Contains(Skill skill)
    {
        return Contains(skill.key);
    }

    /// <summary>
    /// checks if a key is located in the set
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public bool Contains(Key key)
    {
        Node<Key> current = keyValuesArray[HashIt(key.value)];
        if (current == null)
        {
            return false;
        }

        while (current != null)
        {
            if (current.data.value == key.value)
            {
                return true;
            }
            current = current.next;
        }
        return false;
    }

    /// <summary>
    /// Compares to Skill sets and return the skill that are located in both
    /// </summary>
    /// <param name="otherSkillSet"></param>
    /// <returns></returns>
    public SkillSet Intersection(SkillSet otherSkillSet)
    {
        SkillSet resultSet = new SkillSet();
        foreach(Node<Key> e in keyValuesArray)
        {
            Node<Key> current = e;
            while (current != null)
            {
                if (otherSkillSet.Contains(current.data))
                {
                    resultSet.Insert(current.data);
                }
                current = current.next;
            }
        }
        return resultSet;
    }

    /// <summary>
    /// creates a set that contains all the skill of both this set and another set that is passed in as 
    /// an argument.
    /// </summary>
    /// <param name="otherSkillSet"></param>
    /// <returns></returns>
    public SkillSet Union(SkillSet otherSkillSet)
    {
        SkillSet resultSet = new SkillSet();
        foreach (Node<Key> e in keyValuesArray)
        {
            Node<Key> current = e;
            while (current != null)
            {
                resultSet.Insert(current.data);
                current = current.next;
            }
        }
        foreach (Node<Key> e in otherSkillSet.keyValuesArray)
        {
            Node<Key> current = e;
            while (current != null)
            {
                resultSet.Insert(current.data);
                current = current.next;
            }
        }
        return resultSet;
    }

    /// <summary>
    /// gets the number of values in the set
    /// </summary>
    /// <returns></returns>
    public int Size()
    {
        int size = 0;
        foreach (Node<Key> e in keyValuesArray)
        {
            Node<Key> current = e;
            while (current != null)
            {
                size++;
                current = current.next;
            }
        }
        return size;
    }

    public Node<Key>[] ToArray()
    {
        Node<Key>[] nodes = new Node<Key>[this.Size()];
        int skillCount = 0;
        foreach (Node<Key> e in keyValuesArray)
        {
            Node<Key> current = e;
            while (current != null)
            {
                nodes[skillCount] = current;
                current = current.next;
                skillCount++;
            }
        }

        return nodes;
    }

    public override string ToString()
    {
        string s = "";
        for(int i = 0; i < keyValuesArray.Length; i++)
        {
            Node<Key> current = keyValuesArray[i];
            while(current != null)
            {
                s = s + " " + current.data.value +",";
                current = current.next;
            }
        }
        s = "{" + s + "}";
        return s;
    }

}
