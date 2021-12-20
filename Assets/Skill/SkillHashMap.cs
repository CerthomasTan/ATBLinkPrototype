using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class is the container that will hold all the skills. The skills are contained in a hashmap. In addition, this 
/// class will also contain an index to find a skills that contain a skill set. 
/// </summary>
public class SkillHashMap 
{
    /// <summary>
    /// Size of the hash table
    /// </summary>
    int arraySize = 5;
    /// <summary>
    /// A skill table that will contain nodes of the skill. 
    /// </summary>
    Node<Skill>[] skillTable;
    /// <summary>
    /// An index that will hold all occurences of base skills
    /// </summary>
    public SkillIndex skillIndex;

    /// <summary>
    /// constructor that will create the hashtable and set its size. 
    /// </summary>
    public SkillHashMap()
    {
        skillTable = new Node<Skill>[arraySize];
        skillIndex = new SkillIndex(this);
    }

    /// <summary>
    /// Constructor that will create hashtable, set table size, and insert the skill.
    /// </summary>
    /// <param name="skill">skill to be inserted</param>
    public SkillHashMap(Skill skill)
    {
        skillTable = new Node<Skill>[arraySize];
        this.Insert(skill);
        skillIndex = new SkillIndex(this);
    }

    /// <summary>
    /// Constructor that will create the hashtable, set table size, and insert all the skill.
    /// </summary>
    /// <param name="skills">skills to be inserted</param>
    public SkillHashMap(Skill[] skills)
    {
        foreach (Skill s in skills)
        {
            Insert(s);
        }
    }

    /// <summary>
    /// A class that will insert skills into the container
    /// </summary>
    /// <param name="skills">skills to be inserted</param>
    public void Insert(Skill[] skills)
    {
        //insert each skill into container
        foreach (Skill e in skills)
        {
            if (e != null)
            {
                this.Insert(e);
            }
        }
    }

    /// <summary>
    /// Class that will insert a skill into the container
    /// </summary>
    /// <param name="skill">skill to insert</param>
    public void Insert(Skill skill)
    {
        //hash the skill to get adress for array
        int address = HashIt(skill);

        //if skill at address is empty. insert skill as head of the linked list in the array. Skill is 
        // indexed
        if (skillTable[address] == null)
        {
            skillTable[address] = new Node<Skill>(skill);
            skillIndex.Index(skill);
            return;
        }

        // if not empty, iterate thru each node and compare values. If same skill is found, exit method.
        // If skill is not found, insert skill into the list.
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

    /// <summary>
    /// Class finds a certain skill depending on the skill set that is passed into the method. 
    /// The method uses the index and find the intersection of the skills. Then from that set of 
    /// possible skills, method will compare sizes to find appropiate skill.
    /// </summary>
    /// <param name="skillSet">the set of skills that is contained in the skill that the method is trying to find</param>
    /// <returns></returns>
    public Skill Find(SkillSet skillSet)
    {
        //if an empty set is passed into method, return null
        if(skillSet == null)
        {
            return null;
        }

        //if size of skill set is 1, skillset is refering to base skill. Search container for key and return skill. 
        if(skillSet.Size() == 1)
        { 
            return Find(skillSet.ToArray()[0]); 
        }

        // If skill is larger than 1, Use index to find a list of skills that contain both skills. Then check size to see if skill matches.
        // Return skill if skill is found. Return null if not found. 
        else
        {
            SkillSet resultSet = FindAllThatContain(skillSet);
            if (resultSet == null)
            {
                return null;
            }
            foreach(Node<Key> key in FindAllThatContain(skillSet).ToArray())
            {
                Skill skill = Find(key);
                if (key != null && skill.skillSet.Size() == skillSet.Size())
                {
                    return skill;
                }
            }
            return null;
        }
    }

    /// <summary>
    /// Find skill in skill container using a node that contains a key
    /// </summary>
    /// <param name="keyNode">Node to be used to find skill</param>
    /// <returns></returns>
    public Skill Find(Node<Key> keyNode)
    {
        return Find(keyNode.data);
    }

    /// <summary>
    /// Find skill in skill container using a skill
    /// </summary>
    /// <param name="skill"></param>
    /// <returns></returns>
    public Skill Find(Skill skill)
    {
        return Find(skill.key);
    }

    /// <summary>
    /// find skill in skill container using a key
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public Skill Find(Key key)
    {
        return Find(key.value);
    }

    /// <summary>
    /// Find skill in skill container using the value of the skill key
    /// </summary>
    /// <param name="keyValue">Key value that will be used to search the container</param>
    /// <returns></returns>
    public Skill Find(int keyValue)
    {
        //timing data for testing
        float startTime = Time.realtimeSinceStartup;
        
        //find head of list
        int address = HashIt(keyValue);
        Node<Skill> current = skillTable[address];

        // if null, return null
        if(current == null) { return null; }

        // if not null, iterate thru list. If keys matches, skill is found in returned
        while(current != null)
        {
            if(current.data.key.value == keyValue)
            {
                
                //Debug.Log("FIND IN HASHMAP: Time in Secs to find: " + current.data.name + " " + (Time.realtimeSinceStartup - startTime).ToString("F13"));
                return current.data;
            }
            current = current.next;
        }

        //if not found, null is returned
        return null;
    }

    /// <summary>
    /// Hashing that takes a skill as aurgument
    /// </summary>
    /// <param name="skill"></param>
    /// <returns></returns>
    public int HashIt(Skill skill)
    {
        return HashIt(skill.key.value);
    }

    /// <summary>
    /// Hashing that takes a integer as aurgument
    /// </summary>
    /// <param name="skill"></param>
    /// <returns></returns>
    private int HashIt(int key)
    {
        int hash = key.GetHashCode();
        return hash % arraySize;
    }

    /// <summary>
    /// This method will find a skill in the container and returns true or false.
    /// </summary>
    /// <param name="skill"></param>
    /// <returns></returns>
    public bool Contains(Skill skill)
    {
        //get head of linked list
        Node<Skill> current = skillTable[HashIt(skill.key.value)];
        float startTime = Time.realtimeSinceStartup;

        //iterate thru list and check if value is contained in the container
        while (current != null)
        {
            if (current.data.key.value == skill.key.value)
            {
                Debug.Log("CONTAINS SEARCH: time in secs to find skill: " + current.data.name + " was " + (Time.realtimeSinceStartup - startTime).ToString("F13"));
                return true;
            }
            current = current.next;
        }

        // if not found, return false and print to debug console
        Debug.Log("CONTAINS SEARCH(null): time in secs " + (Time.realtimeSinceStartup - startTime).ToString("F13"));
        return false;
    }

    /// <summary>
    /// This method will search the index for all the possible skills that contain the skill.
    /// </summary>
    /// <param name="skill"></param>
    /// <returns></returns>
    public SkillSet FindAllThatContain(Skill skill)
    {
        return FindAllThatContain(new SkillSet(skill));
    }

    /// <summary>
    /// FInd all the possible skill that contains a skillset
    /// </summary>
    /// <param name="skills"></param>
    /// <returns></returns>
    public SkillSet FindAllThatContain(SkillSet skills)
    {
        float startTime = Time.realtimeSinceStartup;
        SkillSet[] skillSetArray = new SkillSet[skills.Size()];
        
        //iterator
        int i = 0;
        //for each node, get all possible skills. Then assign the skillset into array
        foreach (Node<Key> e in skills.ToArray())
        {
            if (e == null) { return null; }
            skillSetArray[i] = this.skillIndex.GetSkillsThatContain(e);
            i++;
        }

        // this will compare all the skills sets and return the intersecting skills in a skill set.
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

        //Debug.Log("SKILLSET SEARCH: time in secs to find set:" + SkillsThatContain + " was " + (Time.realtimeSinceStartup - startTime).ToString("F13"));
        return SkillsThatContain;
    }

    /// <summary>
    /// debug method to aid in finding errors between skill container and skill index
    /// </summary>
    public void debugCommands()
    {
        for(int i = 1; i < 13; i++)
        {
            Debug.Log(skillIndex.GetSkillsThatContain(Find(i)));
        }
    }
}
