using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSkillUIController : MonoBehaviour
{
    [SerializeField] GameObject playerSkillBook;
    [SerializeField] GameObject playerButtonList;
    List<Skill> skillList;
    SkillSet playerLinkedSkills;
    Cursor cursor;
    SkillButton[] buttonArray;

    [SerializeField] Hero hero1;
    [SerializeField] Hero hero2;
    [SerializeField] Hero hero3;
    Queue<Hero> heroList;

    int playerCursorScroll = 0;
    int playerCursorSelection = 0;
    

    // Start is called before the first frame update
    void Start()
    {
        clearData();
        heroList = new Queue<Hero>();
        heroList.Enqueue(hero1);
        heroList.Enqueue(hero2);
        heroList.Enqueue(hero3);

        playerLinkedSkills = new SkillSet();
        buttonArray = this.GetComponentsInChildren<SkillButton>();
        cursor = this.GetComponentInChildren<Cursor>();

        Hero t = heroList.Dequeue();
        heroList.Enqueue(t);
        updateList(t.skills);
        //playerSkillBook.GetComponent<SkillBook>().book.debugCommands();
    }

    // Update is called once per frame
    void Update()
    {
        playerInput();
    }

    void updateList(SkillSet skillSet)
    {
        Skill[] skills = new Skill[skillSet.Size()];
        SkillBook skillBook = playerSkillBook.GetComponent<SkillBook>();
        for (int i = 0; i < skillSet.Size(); i++)
        {
            skills[i] = skillBook.book.Find(skillSet.ToArray()[i].data);
        }
        updateList(skills);
    }

    void updateList(Skill[] skills)
    {
        foreach(Skill s in skills)
        {
            skillList.Add(s);
        }
        updateButtons();
    } 

    void updateButtons()
    {
        for(int i = 0; i < 3; i++)
        {
            if (i + playerCursorScroll > skillList.Count)
            {
                
                buttonArray[i].skill = skillList[i + playerCursorScroll - skillList.Count];
                buttonArray[i].updateText();
            }
            else
            {
                buttonArray[i].gameObject.GetComponent<SkillButton>().skill = skillList[i + playerCursorScroll];
                buttonArray[i].updateText();
            }
        }

        updateCombineSkillText();
    }

    private void updateCombineSkillText()
    {
        Text[] buttonArray = this.GetComponentsInChildren<Text>();
        List<Text> textList = new List<Text>();
        foreach (Text o in buttonArray)
        {
            if (o.gameObject.tag.Equals("CombinedSkill"))
            {
                textList.Add(o);
            }
        }

        if(playerLinkedSkills.Size() <= 0)
        {
            foreach(Text t in textList)
            {
                t.enabled = false;
            }
        }
        else
        {
            for(int i = 0; i < 3; i++)
            {
                SkillSet searchSet = new SkillSet(playerLinkedSkills);
                searchSet.Insert(skillList[i + playerCursorScroll]);
                Skill combinedSkill = playerSkillBook.GetComponent<SkillBook>().book.Find(searchSet);
                if (combinedSkill == null)
                {
                    textList[i].enabled = false;
                }
                else
                {
                    textList[i].enabled = true;
                    textList[i].text = combinedSkill.name;

                }
            }
        }

    }

    public void linkSkill(Skill skill)
    {
        playerLinkedSkills.Insert(playerSkillBook.GetComponent<SkillBook>().book.Find(skill));
        clearData();
        Hero temp = heroList.Dequeue();
        heroList.Enqueue(temp);
        updateList(temp.skills);
        return;
    }

    public void addSkillToQueue(Skill skill)
    {
        playerLinkedSkills.Insert(playerSkillBook.GetComponent<SkillBook>().book.Find(skill));
        Debug.Log(playerSkillBook.GetComponent<SkillBook>().book.Find(playerLinkedSkills) + " was added to queue") ;
        clearData();
        playerLinkedSkills = new SkillSet();
        Hero temp = heroList.Dequeue();
        heroList.Enqueue(temp);
        updateList(temp.skills);
        return;
    }

    private void clearData()
    {
        skillList = new List<Skill>();
    }

    private void playerInput()
    {

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (playerLinkedSkills.Size() < 2)
            {
                linkSkill(buttonArray[playerCursorSelection].skill);
            }
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            SkillButton[] buttonArray = this.GetComponentsInChildren<SkillButton>();
            addSkillToQueue(buttonArray[playerCursorSelection].skill);
        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if(playerCursorSelection < 2)
            {
                playerCursorSelection++;
                Vector3 v = buttonArray[playerCursorSelection].transform.position;
                cursor.moveCursor(v);
            }

            else
            {
                playerCursorSelection = 0;
                Vector3 v = buttonArray[playerCursorSelection].transform.position;
                cursor.moveCursor(v);
            }
        }

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (playerCursorSelection > 0)
            {
                playerCursorSelection--;
                Vector3 v = buttonArray[playerCursorSelection].transform.position;
                cursor.moveCursor(v);
            }

            else
            {
                playerCursorSelection = 2;
                Vector3 v = buttonArray[playerCursorSelection].transform.position;
                cursor.moveCursor(v);
            }
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            clearData();
            Hero temp = heroList.Dequeue();
            this.updateList(temp.skills);
            heroList.Enqueue(temp);
        }
    }
}
