using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// This is the skill book. This is the class that will be used when interacting with the libary.
/// The skill book contains the book and the file I/O to generate the skills for the book container. 
/// </summary>
public class SkillBook : MonoBehaviour
{
    /// <summary>
    /// Skill book conatiner.
    /// </summary>
    public SkillHashMap book;
    /// <summary>
    /// the key counter, this will keep track of the skill counter and assign a new number to each skill starting at 1;
    /// </summary>
    int keyCounter = 1;

    /// <summary>
    /// Runs when object is created. Will create the book and take in the basic skill text file.  
    /// </summary>
    public void Awake()
    {
        //read in file and create skill book
        book = new SkillHashMap();
        var file = Resources.Load<TextAsset>("SerializedFiles/BasicSkills");
        generate(file);
        file = Resources.Load<TextAsset>("SerializedFiles/CombinedSkills");
        generate(file);
    }

    /// <summary>
    /// this will generate the skills to insert into the book container using a textasset. 
    /// </summary>
    /// <param name="file">textasset that will create the skill</param>
    public void generate(TextAsset file)
    {
        //place text into a stringreader
        StringReader reader = new StringReader(file.text);
        
        //if text is available, parse data and create skill
        while(reader.Peek() >= 0)
        {
            string[] line = reader.ReadLine().Split(',');
            string name = line[0];
            int damage = int.Parse(line[1]);
            float castTime = float.Parse(line[2]);
            DamageType damageType = determineDamageType(line[3]);
            List<Effects> effects = determineEffectType(line[4]);
            int targetCount = int.Parse(line[5]);
            SkillSet skills = determineSkillSet(line[6]);

            Skill newSkill = new Skill(keyCounter, name, damage, castTime, damageType, effects, targetCount, skills);
            book.Insert(newSkill);
            keyCounter++;
        }
        //close reader
        reader.Close();
    }

    public DamageType determineDamageType(string text)
    {
        text.ToLower();
        foreach(DamageType dt in Enum.GetValues(typeof(DamageType)))
        {
            if (text.Equals(dt.ToString()))
            {
                return dt;
            }
        }
        return DamageType.attack;
    }

    public List<Effects> determineEffectType(string text)
    {
        string t = text;
        t = t.ToLower();
        t = t.Replace("{",string.Empty);
        t = t.Replace("}", string.Empty);
        t = t.Replace(" ", string.Empty);
        List<Effects> effects = new List<Effects>();
        foreach (string effect in t.Split(';'))
        {
            foreach(Effects effectType in Enum.GetValues(typeof(Effects)))
            {
                if (effect.Equals(effectType.ToString())){
                    effects.Add(effectType);
                }
            }
        }
        return effects;
    }

    public SkillSet determineSkillSet(string text)
    {
        SkillSet skills = new SkillSet();
        string t = text;
        t = t.ToLower();
        t = t.Replace("{", string.Empty);
        t = t.Replace("}", string.Empty);
        t = t.Replace(" ", string.Empty);
        foreach (string skill in t.Split(';'))
        {
            if(skill != "")
            {
                skills.Insert(new Key(int.Parse(skill)));
            }
        }
        return skills;
    }
}
