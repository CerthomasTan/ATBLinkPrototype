using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// this is a container that creates an instance of attack. This will contain the target, caster, and skill to use.
/// This class will be used mostly in Attacking loop in battlesystem.
/// </summary>
public class Attack 
{
    public Skill skill;
    public List<Actor> casters;
    public List<Actor> targets;
    public float castCharge = 0;

    /// <summary>
    /// Constructor that will take a skill, list of caster, and list of targets
    /// </summary>
    /// <param name="s">skill</param>
    /// <param name="a">caster</param>
    /// <param name="t">target</param>
    public Attack(Skill s, List<Actor> a, List<Actor> t)
    {
        skill = s;
        casters = a;
        targets = t;
    }

    /// <summary>
    /// will get the damge of the skill 
    /// </summary>
    /// <returns></returns>
    public int calculateDamage()
    {
        
        return skill.damage;
    }
}
