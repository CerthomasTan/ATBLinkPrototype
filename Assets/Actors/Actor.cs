using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Actor : MonoBehaviour
{
    [SerializeField] protected TextAsset skillData;
    [SerializeField] protected TextAsset actorStats;
    public SkillSet skills;
    public string actorName;
    public int level = 0;
    public int maxHealth;
    public int currentHealth;
    public int chargeRate = 1;
    public int currentATBCharge = 0;
    public int maxATBCharge = 100;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

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
    public Attack attack(Skill skill)
    {
        List<Actor> actor = new List<Actor>();
        actor.Add(this);
        return new Attack(skill, actor);
    }

    public Attack attack(Skill skill, List<Actor> actors)
    {
        
        return new Attack(skill, actors);
    }

}
