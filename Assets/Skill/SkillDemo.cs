using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillDemo : MonoBehaviour
{
    [SerializeField] SkillBook skillBook;
    // Start is called before the first frame update
    void Start()
    {

    }

    public void Demo1()
    {
        Debug.Log("Demo Start, version 1");
        Debug.Log("Creating Skills: fire, water, sword");
        Skill fire = new Skill(1, "fire", 10, "attack", 1);
        Skill water = new Skill(2, "water", 10, "attack", 1);
        Skill sword = new Skill(3, "sword", 10, "attack", 1);

        Debug.Log("Creating Skills and set: flamesword. Inserting into skillbook");
        SkillSet skills = new SkillSet();
        skills.Insert(sword.key);
        skills.Insert(sword.key);
        skills.Insert(fire.key);
        Skill flameSword = new Skill(4, "flameSword", 20, "attack", 2, skills);

        Debug.Log("Creating Skills and set: waterSword. Inserting into skillbook");
        skills = new SkillSet();
        skills.Insert(sword.key);
        skills.Insert(sword.key);
        skills.Insert(water.key);
        Skill waterSword = new Skill(5, "waterSword", 20, "attack", 2, skills);

        Debug.Log("checking to skills class");
        Debug.Log(fire);
        Debug.Log(water);
        Debug.Log(sword);
        Debug.Log(flameSword);

        Debug.Log("checking to SkillSet Class");
        Debug.Log(flameSword.skillSet);
        Debug.Log(flameSword.skillSet.Contains(water));
        Debug.Log(flameSword.skillSet.Contains(fire));

        Debug.Log("checking to SkillSet Class method: intersection and union");
        Debug.Log(flameSword.skillSet.Union(waterSword.skillSet));
        Debug.Log(water.skillSet.Intersection(waterSword.skillSet));

        Debug.Log(flameSword.skillSet.Contains(water));
        Debug.Log(flameSword.skillSet.Contains(fire));

        Debug.Log("checking to SkillBook Class methods");
        SkillLinkedHashMap actionBook = new SkillLinkedHashMap();
        actionBook.Insert(fire);
        actionBook.Insert(water);
        actionBook.Insert(sword);
        actionBook.Insert(flameSword);
        actionBook.Insert(waterSword);

        Debug.Log(actionBook.skillIndex.GetSkillsThatContain(sword));

        Debug.Log(waterSword.skillSet);

        foreach (Node<Key> k in actionBook.FindAllThatContain(fire).ToArray())
        {
            Debug.Log(actionBook.Find(k));
        }

        foreach (Node<Key> k in actionBook.FindAllThatContain(water).ToArray())
        {
            Debug.Log(actionBook.Find(k));
        }

        foreach (Node<Key> k in actionBook.FindAllThatContain(skills).ToArray())
        {
            Debug.Log(actionBook.Find(k));
        }
    }

    public void Demo2()
    {
        Debug.Log("Welcome to the test");

        Debug.Log("Creating player skill book");
        if(skillBook == null)
        {
            return;
        }
        Debug.Log("Finding if key values, searching for skill 1");
        Skill skill = skillBook.book.Find(1);
        Debug.Log(skill);

        Debug.Log("Finding if key values, searching for skill 10");
        Debug.Log(skillBook.book.Find(10));

        Debug.Log("finding all that contain");
        Debug.Log(skillBook.book.FindAllThatContain(skill));
        
    }
}
