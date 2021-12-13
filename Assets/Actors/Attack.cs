using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack 
{
    public Skill skill;
    public List<Actor> actors;
    //add target

    public Attack(Skill s, List<Actor> a)
    {
        skill = s;
        actors = a;
    }

    public int calculateDamage()
    {
        
        return skill.damage;
    }
}
