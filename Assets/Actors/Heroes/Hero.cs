using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

/// <summary>
/// Hero class that is a daughter class of actor. This class contains specific methods that will be used for saving and creating skills
/// </summary>
public class Hero : Actor
{

    /// <summary>
    /// Awake method that will create a skillset, get the skills that the hero have, and load the current lvl.
    /// </summary>
    void Awake()
    {
        skills = new SkillSet();
        updateSkillList();
        loadLvl();
    }

    /// <summary>
    /// This will get a file and get the skills this character have access to. 
    /// </summary>
    void updateSkillList()
    {
        StringReader reader = new StringReader(base.skillData.text);
        while (reader.Peek() >= 0)
        {
            string[] data = reader.ReadLine().Split(',');
            if (!data[0].Equals(string.Empty))
            {
                this.skills.Insert(new Key(int.Parse(data[0])));
            }
        }
    }

    /// <summary>
    /// Method that will generate an attack. 
    /// </summary>
    /// <param name="skill"></param>
    /// <param name="heros"></param>
    /// <param name="targets"></param>
    /// <returns></returns>
    public Attack attack(Skill skill, List<Hero> heros, List<Actor> targets)
    {
        List<Actor> actors = new List<Actor>();
        foreach(Actor actor in heros)
        {
            actors.Add(actor);
        }
        return base.attack(skill, actors, targets);
    }

    /// <summary>
    /// Method will lvlup the character. Will update json file to seralize and save.
    /// </summary>
    public void lvlUp()
    {
        string json = JsonUtility.ToJson(new HeroData(base.level + 1));
        print("file created");
        File.WriteAllText(Application.dataPath +"/"+ base.actorName + ".json", json);
    }

    /// <summary>
    /// Read json file and update character level.
    /// </summary>
    public void loadLvl()
    {
        if (File.Exists(Application.dataPath + "/" + base.actorName + ".json"))
        {
            string json = File.ReadAllText(Application.dataPath + "/" + base.actorName + ".json");
            HeroData temp = JsonUtility.FromJson<HeroData>(json);
            this.level = temp.level;
        }
    }
}

/// <summary>
/// Container that will be used to specifiy variables that are to be serialized. 
/// </summary>
class HeroData
{
    public int level;
    
    public HeroData(int level)
    {
        this.level = level;
    }
}