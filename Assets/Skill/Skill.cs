using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill
{
    public Key key { get; }
    public string name { get; }
    public int damage { get; }
    public DamageType type { get; }
    public int targetCount { get; }
    public SkillSet skillSet { get; set; }

    /*
     * Construcotr that all parameters except a skillset. This Constructor assumes that when there is no 
     * skillset passed as an argument. The skill is considered a base skill. 
     */
    public Skill(int keyValue, string name, int damage, string damageType, int targetCount) 
    {
        this.key = new Key(keyValue);
        this.name = name;
        this.damage = damage;
        string s = damageType.Trim().ToLower();
        if (s.Equals("heal"))
        {
            this.type = DamageType.heal;
        }
        else
        {
            this.type = DamageType.attack;
        }
        this.targetCount = targetCount;
        this.skillSet = new SkillSet(key);
        skillSet.Insert(this);
    }

    /*
     * Construcotr that all parameters. 
     */
    public Skill(int key, string name, int damage, string damageType, int targetCount, SkillSet skillSet)
    {
        this.key = new Key(key);
        this.name = name;
        this.damage = damage;
        string s = damageType.Trim().ToLower();
        if (s.Equals("heal"))
        {
            this.type = DamageType.heal;
        }
        else
        {
            this.type = DamageType.attack;
        }
        this.targetCount = targetCount;
        this.skillSet = new SkillSet(skillSet);
    }

    /*
     * Copy Constructor
     */
    public Skill(Skill skill)
    {
        this.key = skill.key;
        this.name = skill.name;
        this.damage = skill.damage;
        type = skill.type;
        this.targetCount = skill.targetCount;
        this.skillSet = skill.skillSet;
    }


    public override string ToString()
    {
        return string.Format("{0}, {1}, {2}", name, damage, targetCount);
    }
}
public enum DamageType
{
    heal,
    attack
}
