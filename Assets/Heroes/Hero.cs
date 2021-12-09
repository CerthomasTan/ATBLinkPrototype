using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Hero : MonoBehaviour
{
    [SerializeField] TextAsset skillData;
    [SerializeField] TextAsset heroStats;
    public SkillSet skills;
    int level = 0;
    // Start is called before the first frame update
    void Awake()
    {
        skills = new SkillSet();
        updateSkillList();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

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
