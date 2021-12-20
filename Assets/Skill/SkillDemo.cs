using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// skill demo used for debbuging skill, node, skillhashmap, skill index, and skill set
/// </summary>
public class SkillDemo : MonoBehaviour
{
    [SerializeField] SkillBook skillBook;

    /// <summary>
    /// demo 1 creates skillbook with hard coded values (ie. doesn't use file I/O
    /// </summary>
    public void Demo1()
    {
        
    }

    /// <summary>
    /// Demo 2 tests wether the skill book and the file I/O works 
    /// </summary>
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
