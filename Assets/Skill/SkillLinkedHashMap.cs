using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillLinkedHashMap 
{
    int arraySize = 5;
    Node<Skill>[] skillTable;
    public SkillIndex skillIndex;

    public SkillLinkedHashMap()
    {
        skillTable = new Node<Skill>[arraySize];
        skillIndex = new SkillIndex(this);
    }
    public SkillLinkedHashMap(Skill skill)
    {
        skillTable = new Node<Skill>[arraySize];
        this.Insert(skill);
        skillIndex = new SkillIndex(this);
    }

    public SkillLinkedHashMap(Skill[] skills)
    {
        foreach (Skill s in skills)
        {
            Insert(s);
        }
    }

    public void Insert(Skill[] skills)
    {
        foreach (Skill e in skills)
        {
            if (e != null)
            {
                this.Insert(e);
            }
        }
    }

    public void Insert(Skill skill)
    {
        int address = HashIt(skill);
        if (skillTable[address] == null)
        {
            skillTable[address] = new Node<Skill>(skill);
            skillIndex.Index(skill);
            return;
        }

        else
        {
            Node<Skill> current = skillTable[address];
            //check if exists, if skill exists in set. Exit out of method
            if (current.data.key.value == skill.key.value)
            {
                return;
            }

            //check if there is a 2nd skill node to access, 
            while (current.next != null)
            {
                //checks if next node already exists 
                if (current.next.data.key.value == skill.key.value)
                {
                    return;
                }
                //get next node in the linked list
                else
                {
                    current = current.next;
                }
            }
            //if at end, insert new skill into 
            current.next = new Node<Skill>(skill);
            skillIndex.Index(skill);
            return;
        }
    }

    public Skill Find(Node<Key> keyNode)
    {
        return Find(keyNode.data);
    }

    public Skill Find(Skill skill)
    {
        return Find(skill.key);
    }

    public Skill Find(Key key)
    {
        return Find(key.value);
    }

    public Skill Find(int keyValue)
    {
        int address = HashIt(keyValue);
        Node<Skill> current = skillTable[address];
        if(current == null) { return null; }
        while(current != null)
        {
            if(current.data.key.value == keyValue)
            {
                return current.data;
            }
            current = current.next;
        }
        return null;
    }

    public int HashIt(Skill skill)
    {
        return HashIt(skill.key.value);
    }

    private int HashIt(int key)
    {
        int hash = key.GetHashCode();
        return hash % arraySize;
    }

    public bool Contains(Skill skill)
    {
        Node<Skill> current = skillTable[HashIt(skill.key.value)];
        while(current != null)
        {
            if (current.data.key.value == skill.key.value)
            {
                return true;
            }
            current = current.next;
        }
        return false;
    }

    public SkillSet FindAllThatContain(Skill skill)
    {
        return FindAllThatContain(new SkillSet(skill));
    }

    public SkillSet FindAllThatContain(SkillSet skills)
    {
        SkillSet[] skillSetArray = new SkillSet[skills.Size()];

        int i = 0;
        foreach (Node<Key> e in skills.ToArray())
        {
            if (e == null) { return null; }
            skillSetArray[i] = this.skillIndex.GetSkillsThatContain(e);
            i++;
        }

        i = 0;
        SkillSet SkillsThatContain = null;
        foreach (SkillSet e in skillSetArray)
        {
            if(e == null) { return null; }
            else if(i == 0)
            {
                SkillsThatContain = skillSetArray[i];
            }
            else
            {
                SkillsThatContain = SkillsThatContain.Intersection(skillSetArray[i]);
            }
            i++;
        }
        return SkillsThatContain;
    }
}
