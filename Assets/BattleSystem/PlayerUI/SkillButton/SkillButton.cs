using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// A class that controls behavoir of skill butoons. 
/// </summary>
public class SkillButton : MonoBehaviour
{

    public Skill skill { get; set; }
    [SerializeField] GameObject playerController;
    public Text buttonText;
    public Text combineText;
    // Update is called once per frame

    /// <summary>
    /// on awake method that finds the text components in child objects, compares the tags and place
    /// them into appropiate varaible.
    /// </summary>
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

    /// <summary>
    /// will update name depending on the skill that is placed into button
    /// </summary>
    public void updateText()
    {
        buttonText.text = skill.name;
    }

    /// <summary>
    /// clears the button text.
    /// </summary>
    public void clearText()
    {
        buttonText.text = "";
    }
}
