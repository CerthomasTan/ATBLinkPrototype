using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillBook : MonoBehaviour
{
    public SkillLinkedHashMap book;
    int keyCounter = 1;
    public void Awake()
    {
        book = new SkillLinkedHashMap();
        var file = Resources.Load<TextAsset>("SerializedFiles/BasicSkills");
        generate(file);
    }

    public void generate(TextAsset file)
    {
        
        StringReader reader = new StringReader(file.text);
        while(reader.Peek() >= 0)
        {
            string[] line = reader.ReadLine().Split(',');

            if(line.Length < 5)
            {
                Skill skill = new Skill(keyCounter, line[0], int.Parse(line[1]), line[2], int.Parse(line[3]));
                if (!book.Contains(skill)) { keyCounter++; }
                book.Insert(skill);
            }
            else if(line.Length >= 5)
            {
                SkillSet skillKeySet = new SkillSet();
                for(int i = 4; i <= line.Length - 1; i++)
                {
                    Key key = new Key(int.Parse(line[i]));
                    skillKeySet.Insert(key);
                }
                Skill skill = new Skill(keyCounter, line[0], int.Parse(line[1]), line[2], int.Parse(line[3]), skillKeySet);
                if (!book.Contains(skill)) { keyCounter++; }
                book.Insert(skill);
            }
        }
        reader.Close();
    }
}
