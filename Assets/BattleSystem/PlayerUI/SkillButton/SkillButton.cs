using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillButton : MonoBehaviour
{

    public Skill skill { get; set; }
    [SerializeField] GameObject playerController;
    public Text buttonText;
    public Text combineText;
    // Update is called once per frame

    void Awake()
    {
        Text[] textArr = this.GetComponentsInChildren<Text>();
        foreach(Text t in textArr)
        {
            if (t.gameObject.tag.Equals("CombinedSkill"))
            {
                combineText = t;
            }
            else
            {
                buttonText = t;
            }
        }
        if (playerController == null)
        {
            playerController = FindObjectOfType<PlayerSkillUIController>().gameObject;
        }
    }

    void Update()
    {

    }


    public void updateText()
    {
        buttonText.text = skill.name;
    }
}
