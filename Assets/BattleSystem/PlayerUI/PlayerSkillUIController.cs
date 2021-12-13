using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSkillUIController : MonoBehaviour
{
    [SerializeField] GameObject playerSkillBook;
    [SerializeField] GameObject playerButtonList;
    [SerializeField] GameObject playerLinkedSkillsUI;
    [SerializeField] GameObject playerHealthDisplayUI;

    [SerializeField] GameObject battleSystem;

    Hero currentHero;
    List<Hero> linkingHeros;
    List<Skill> skillList;
    SkillSet playerLinkedSkills;
    Cursor cursor;
    SkillButton[] buttonArray;
    bool isSkillLoaded = false;
    Queue<Hero> heroList;

    int playerCursorScroll = 0;
    int playerCursorSelection = 0;
    

    // Start is called before the first frame update
    void Start()
    {
        clearData();
        generateHeroUI();
        linkingHeros = new List<Hero>();
        playerLinkedSkills = new SkillSet();
        buttonArray = this.GetComponentsInChildren<SkillButton>();
        cursor = this.GetComponentInChildren<Cursor>();
        heroList = battleSystem.GetComponent<BattleSystem>().heroTurnQueue;
        cursor.moveCursor(buttonArray[0].gameObject.transform.position);
        StartCoroutine(updateList());
    }

    // Update is called once per frame
    void Update()
    {
        if (skillsAreAvaliable() || isSkillLoaded)
        {
            playerInput();
        }
        
    }

    IEnumerator updateList()
    {
        while (battleSystem.GetComponent<BattleSystem>().battleIsGoing)
        {
            yield return new WaitUntil(() => !isSkillLoaded);
            yield return new WaitUntil(() => heroList.Count > 0);
            currentHero = heroList.Dequeue();
            updateList(currentHero.skills);
            isSkillLoaded = true;
        }
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

    void linkSkill(Skill skill)
    {
        playerLinkedSkills.Insert(playerSkillBook.GetComponent<SkillBook>().book.Find(skill));
        Text[] textUI = playerLinkedSkillsUI.GetComponentsInChildren<Text>();
        textUI[playerLinkedSkills.Size() - 1].text = skill.name;
        clearData();
        return;
    }

    public void addSkillToQueue(Skill skill)
    {
        playerLinkedSkills.Insert(playerSkillBook.GetComponent<SkillBook>().book.Find(skill));
        Skill linkedSkill = playerSkillBook.GetComponent<SkillBook>().book.Find(playerLinkedSkills);
        if(linkedSkill != null)
        {
            battleSystem.GetComponent<BattleSystem>().addAttackingList(currentHero.attack(linkedSkill, linkingHeros));
        }
        else
        {
            Skill failedSkill = new Skill(9999,"failed linked",1, "attack", 1);
            battleSystem.GetComponent<BattleSystem>().addAttackingList(currentHero.attack(failedSkill, linkingHeros));
        }
        clearData();
        foreach(Text t in playerLinkedSkillsUI.GetComponentsInChildren<Text>())
        {
            t.text = "";
        }
        playerLinkedSkills = new SkillSet();
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
                linkingHeros.Add(currentHero);
                linkSkill(buttonArray[playerCursorSelection].skill);
                cursor.moveCursor(buttonArray[0].gameObject.transform.position);
                playerCursorSelection = 0;
                clearButtons();
                isSkillLoaded = false;
            }
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            SkillButton[] buttonArray = this.GetComponentsInChildren<SkillButton>();
            linkingHeros.Add(currentHero);
            addSkillToQueue(buttonArray[playerCursorSelection].skill);
            linkingHeros.Clear();
            cursor.moveCursor(buttonArray[0].gameObject.transform.position);
            playerCursorSelection = 0;
            clearButtons();
            isSkillLoaded = false;
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
            if (isSkillLoaded)
            {
                clearData();
                heroList.Enqueue(currentHero);
                isSkillLoaded = false;
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    private void generateHeroUI()
    {
        foreach (Hero hero in battleSystem.GetComponent<BattleSystem>().heros.GetComponentsInChildren<Hero>())
        {
            bool setUI = false;
            foreach (HeroUIController heroUI in playerHealthDisplayUI.GetComponentsInChildren<HeroUIController>())
            {   
                if(heroUI.hero == null && !setUI)
                {
                    heroUI.hero = hero;
                    setUI = true;
                }
            }
        }
    }

    private bool skillsAreAvaliable()
    {
        if(heroList.Count > 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    void clearButtons()
    {
        for(int i = 0; i < 3; i++)
        {
            buttonArray[i].clearText();
        }

        clearCombinedSkills();
    }

    void clearCombinedSkills()
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

        foreach (Text t in textList)
        {
            t.enabled = false;
        }

    }
}
