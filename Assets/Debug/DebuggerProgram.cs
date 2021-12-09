using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebuggerProgram : MonoBehaviour
{

    [SerializeField] GameObject playerbook;
    
    // Start is called before the first frame update
    void Start()
    {
        SkillSet ss = new SkillSet();
        Skill s = playerbook.GetComponent<SkillBook>().book.Find(1);
        ss = (playerbook.GetComponent<SkillBook>().book.skillIndex.GetSkillsThatContain(s));

        SkillSet ss2 = new SkillSet();
        Skill s2 = playerbook.GetComponent<SkillBook>().book.Find(8);
        ss2 = (playerbook.GetComponent<SkillBook>().book.skillIndex.GetSkillsThatContain(s2));
        Debug.Log(ss);
        Debug.Log(ss2);
        Debug.Log(ss.Intersection(ss2).Size());
        Debug.Log(ss.Intersection(ss2));

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
