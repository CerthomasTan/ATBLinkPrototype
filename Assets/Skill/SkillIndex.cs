using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This is the index that will contain keys. This index will keep track of all the skills that a base appears in. 
/// </summary>
public class SkillIndex
{

    int arraySize = 5;
    Node<SkillSet>[] table;
    /// <summary>
    /// the skill hashmap that the index is attached to.
    /// </summary>
    SkillHashMap skillHashMap;

    /// <summary>
    /// constructor that will take skillhashmap
    /// </summary>
    /// <param name="s">hashmap that the index is contained in</param>
    public SkillIndex(SkillHashMap s)
    {
        table = new Node<SkillSet>[arraySize];
        this.skillHashMap = s;
    }

    /// <summary>
    /// constructor that will take skillhashmap and a skill
    /// </summary>
    /// <param name="s">hashmap that the index is contained in</param>
    public SkillIndex(Skill skill, SkillHashMap s)
    {
        table = new Node<SkillSet>[arraySize];
        this.skillHashMap = s;
        Index(skill);
    }

    /// <summary>
    /// hashing values and getting remainder depeing on arraysize varaible
    /// </summary>
    /// <param name="keyValue"></param>
    /// <returns></returns>
    private int HashIt(int keyValue)
    {
        return keyValue.GetHashCode() % arraySize;
    }

    /// <summary>
    /// This will index all the skills in a set into the index
    /// </summary>
    /// <param name="skillSet"></param>
    public void Index(SkillSet skillSet)
    {
        foreach(Node<Key> skill in skillSet.ToArray())
        {
            Index(skillHashMap.Find(skill.data));
        }
    }

    /// <summary>
    /// index skill
    /// </summary>
    /// <param name="skill"></param>
    public void Index(Skill skill)
    {
        foreach (Node<Key> baseSkillKey in skill.skillSet.ToArray())
        {
            Index(baseSkillKey, skill);
        }
    }

    /// <summary>
    /// index a skill. All other public index methods will flow to this method. Meother will a skill and insert it into the index for a base skill
    /// </summary>
    /// <param name="baseSkillKey"></param>
    /// <param name="skill"></param>
    private void Index(Node<Key> baseSkillKey, Skill skill)
    {
        //find header skillset
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

        //if at curent the base skill is found, insert skill into set
        else if (currentSkillSet.data.Contains(baseSkillKey.data))
        {
            currentSkillSet.data.Insert(skill);
            return;
        }

        //iterate to next skill set and see if the next skill set contains the base skill. 
        //If skill is found, insert skill into skill set. If not, iterate thru rest of 
        //skill set and check for base skill. 
        //If skill is not found and at end of list. Insert both the base skill set and the skill into a new set and attach it to list.
        else
        {
            Node<SkillSet> previous = table[address];
            while (currentSkillSet != null)
            {
                if (currentSkillSet.data.Contains(baseSkillKey.data))
                {
                    currentSkillSet.data.Insert(skill);
                    return;
                }
                previous = currentSkillSet;
                currentSkillSet = currentSkillSet.next;
            }

            Node<SkillSet> skillsNode = new Node<SkillSet>(new SkillSet());
            skillsNode.data.Insert(baseSkillKey.data);
            skillsNode.data.Insert(skill);
            previous.next = skillsNode;
            return;

        }
    }

    /// <summary>
    /// this will return all the skills that contain a skill. Skill will be found using a key
    /// </summary>
    /// <param name="keyNode"></param>
    /// <returns></returns>
    public SkillSet GetSkillsThatContain(Node<Key> keyNode)
    {
        if (keyNode != null)
        {
            return GetSkillsThatContain(skillHashMap.Find(keyNode.data.value));
        }

        return null;
    }

    /// <summary>
    /// This will find all the skills that contain a skill. 
    /// </summary>
    /// <param name="skill"></param>
    /// <returns></returns>
    public SkillSet GetSkillsThatContain(Skill skill)
    {
        //if skillset size greater than 1, then this is a combined skill and is not used in combinations. 
        //return null
        if(skill.skillSet.Size() > 1)
        {
            return null;
        }

        //hash to find head of list in array and iterate thru list. if the base skill is found in the set, then that set is returned
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
