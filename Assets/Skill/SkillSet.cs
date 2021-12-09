using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillSet
{
    int arraySize = 5;
    //will contain integers, will need to box
    Node<Key>[] keyValuesArray;
    
    public SkillSet()
    {
        keyValuesArray = new Node<Key>[arraySize];
    }

    //copy Constructor 
    public SkillSet(SkillSet skillSet)
    {
        keyValuesArray = new Node<Key>[arraySize];
        foreach (Node<Key> node in skillSet.ToArray())
        {
            Insert(node.data);
        }
    }

    public SkillSet(Key key)
    {
        keyValuesArray = new Node<Key>[arraySize];
        Insert(key);
    }

    public SkillSet(Skill[] skills)
    {
        keyValuesArray = new Node<Key>[arraySize];
        foreach (Skill c in skills)
        {
            Insert(c);
        }
    }

    public SkillSet(Skill skill)
    {
        keyValuesArray = new Node<Key>[arraySize];
        Insert(skill);
    }

    public void Insert(Skill skill)
    {
        Insert(skill.key.value);
    }

    public void Insert(Key key)
    {
        Insert(key.value);
    }

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
            current.next = new Node<Key>(new Key(key));
            return;
        }
    }

    private int HashIt(int key)
    {
        int hash = key.GetHashCode();
        return hash % arraySize;
    }

    public bool Contains(Skill skill)
    {
        return Contains(skill.key);
    }

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
