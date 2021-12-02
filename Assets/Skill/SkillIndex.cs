using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillIndex
{
    int arraySize = 5;
    Node<SkillSet>[] table;
    SkillLinkedHashMap skillLinkedHashMap;

    public SkillIndex(SkillLinkedHashMap s)
    {
        table = new Node<SkillSet>[arraySize];
        this.skillLinkedHashMap = s;
    }
    public SkillIndex(Skill skill, SkillLinkedHashMap s)
    {
        table = new Node<SkillSet>[arraySize];
        this.skillLinkedHashMap = s;
        Index(skill);
    }

    private int HashIt(int keyValue)
    {
        return keyValue.GetHashCode() % arraySize;
    }

    public void Index(SkillSet skillSet)
    {
        foreach(Node<Key> skill in skillSet.ToArray())
        {
            Index(skillLinkedHashMap.Find(skill.data));
        }
    }

    public void Index(Skill skill)
    {
        foreach (Node<Key> baseSkillKey in skill.skillSet.ToArray())
        {
            int address = HashIt(baseSkillKey.data.value);
            Node<SkillSet> currentSkillSet = table[address];
            //base skill doesn't exists, insert base skill
            if (currentSkillSet == null)
            {
                Node<SkillSet> skillsNode = new Node<SkillSet>(new SkillSet());
                skillsNode.data.Insert(baseSkillKey.data);
                skillsNode.data.Insert(skill);
                table[address] = skillsNode;
                return;
            }

            while(currentSkillSet != null)
            {
                if (currentSkillSet.data.Contains(baseSkillKey.data))
                {
                    currentSkillSet.data.Insert(skill);
                }
                currentSkillSet = currentSkillSet.next;
            }
        }
    }


    public SkillSet GetSkillsThatContain(Node<Key> keyNode)
    {
        if (keyNode != null)
        {
            return GetSkillsThatContain(skillLinkedHashMap.Find(keyNode.data.value));
        }

        return null;
    }

    public SkillSet GetSkillsThatContain(Skill skill)
    {
        if(skill.skillSet.Size() > 1)
        {
            return null;
        }
        else
        {
            int address = HashIt(skill.key.value);
            Node<SkillSet> currentSkillSet = table[address];
            while (currentSkillSet != null)
            {
                if (currentSkillSet.data.Contains(skill))
                {
                    return currentSkillSet.data;
                }
                currentSkillSet = currentSkillSet.next;
            }
        }
        return null;
    }

}
