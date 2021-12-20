using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

/// <summary>
/// This class is for create enemies. This is an daughter class of actor
/// </summary>
public class Enemy : Actor
{

    void updateSkillList()
    {
        StringReader reader = new StringReader(skillData.text);
        while (reader.Peek() >= 0)
        {
            string[] data = reader.ReadLine().Split(',');
            if (!data[0].Equals(string.Empty))
            {
                this.skills.Insert(new Key(int.Parse(data[0])));
            }
        }
    }

}
