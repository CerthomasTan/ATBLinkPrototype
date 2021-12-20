using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEditor;
/// <summary>
/// This class will create a skill that will be used in game. 
/// A skill will contain a key value, name, damage, damagetype, targetCount, and Skill set.
/// The skillset will contain skill(keys) that will are needed to cast skill.
/// </summary>
/// 

[System.Serializable]
public class Skill
{
    //variables
    /// <summary>
    /// This is the key that will point to this skill
    /// </summary>
    public Key key { get; }
    /// <summary>
    /// Name of the skill
    /// </summary>
    public string name { get; }
    /// <summary>
    /// amount of damage skill does
    /// </summary>
    public int damage { get; }
    
    public float castTime { get; }

    /// <summary>
    /// Type of damage
    /// </summary>
    public DamageType damageType { get; }
    /// <summary>
    /// a list of attack addatives that will alter the effect of the attacck
    /// </summary>
    public List<Effects> effects { get; }
    /// <summary>
    /// amount of targets to hit
    /// </summary>
    public int targetCount { get; }
    /// <summary>
    /// the skillset that contains the need combination of skills to cast skill.
    /// </summary>
    public SkillSet skillSet { get; set; }

    //methods
    /// <summary>
    /// Construcotr that all parameters except a skillset. This Constructor assumes that when there is no 
    /// skillset passed as an argument.The skill is considered a base skill.
    /// </summary>
    /// <param name="keyValue"></param>
    /// <param name="name"></param>
    /// <param name="damage"></param>
    /// <param name="damageType"></param>
    /// <param name="targetCount"></param>
    public Skill(int keyValue, string name, int damage, float castTime,DamageType damageType, List<Effects> effects, int targetCount, SkillSet skillSet) 
    {
        this.key = new Key(keyValue);
        this.name = name;
        this.damage = damage;
        this.castTime = castTime;
        this.damageType = damageType;
        this.effects = effects;
        this.targetCount = targetCount;
        if (skillSet.Size() <= 0) {
            this.skillSet = new SkillSet(key);
        }
        else
        {
            this.skillSet = skillSet;
        }
    }

    /// <summary>
    /// Copy Constructor 
    /// </summary>
    /// <param name="skill"></param>
    public Skill(Skill skill)
    {
        this.key = skill.key;
        this.name = skill.name;
        this.damage = skill.damage;
        this.castTime = skill.castTime;
        this.damageType = skill.damageType;
        this.effects = skill.effects;
        this.targetCount = skill.targetCount;
        this.skillSet = skill.skillSet;
    }

    /// <summary>
    /// To string method. This will override default to string method.
    /// </summary>
    /// <returns>the class name, damage, and target Count</returns>
    public override string ToString()
    {
        return string.Format("{0}, {1}, {2}", name, damage, targetCount);
    }
    
}

/// <summary>
/// Damage structure that will define what type of skill is. 
/// Heal will only target allies and can heal each other.
/// </summary>
public enum DamageType
{
    heal,
    attack
}

/// <summary>
/// Damage effect additives that can be placed on a skill
/// </summary>
public enum Effects
{
    stun
}


