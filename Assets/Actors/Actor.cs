using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

/// <summary>
/// Actor class. Class that contains health, skills, name, level, max health, current health, charge rate, current charge, and max charge.
/// </summary>
[System.Serializable]
public class Actor : MonoBehaviour
{
    [SerializeField] protected TextAsset skillData;
    public SkillSet skills;
    public string actorName;
    public int level = 0;
    public int maxHealth;
    public int currentHealth;
    public int chargeRate = 1;
    public int currentATBCharge = 0;
    public int maxATBCharge = 100;

    /// <summary>
    /// charges the atb the charge rate. 
    /// </summary>
    public void chargeATB()
    {
        if(currentATBCharge < maxATBCharge)
        {
            currentATBCharge += chargeRate;
        }
        if(currentATBCharge > maxATBCharge)
        {
            currentATBCharge = maxATBCharge;
        }
        
    }

    /// <summary>
    /// generates an attack with this actor as the caster.
    /// </summary>
    /// <param name="skill"></param>
    /// <param name="targets"></param>
    /// <returns></returns>
    public Attack attack(Skill skill, List<Actor> targets)
    {
        List<Actor> casters = new List<Actor>();
        casters.Add(this);
        return new Attack(skill, casters, targets);
    }

    /// <summary>
    /// apply damge and reduce current health points
    /// </summary>
    /// <param name="damage"></param>
    public void takeDamage(int damage)
    {
        currentHealth -= damage;
        if(currentHealth < 0)
        {
            currentHealth = 0;
        }
    }

    /// <summary>
    /// creates an attack with a skill, casters, and targets
    /// </summary>
    /// <param name="skill">skill</param>
    /// <param name="actors">casters</param>
    /// <param name="targets">targets</param>
    /// <returns></returns>
    public Attack attack(Skill skill, List<Actor> actors, List<Actor> targets)
    {   
        return new Attack(skill, actors, targets);
    }

}
