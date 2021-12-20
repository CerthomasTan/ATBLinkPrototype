using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This class controlls the behavoir of the player skill UI. 
/// </summary>
public class PlayerSkillUIController : MonoBehaviour
{
    //seralized fields that is set in unity
    [SerializeField] GameObject playerSkillBook;
    [SerializeField] GameObject playerButtonList;
    [SerializeField] GameObject playerLinkedSkillsUI;
    [SerializeField] GameObject playerHealthDisplayUI;
    [SerializeField] GameObject battleSystem;
    [SerializeField] GameObject debugWindow;
    [SerializeField] GameObject resultScreen;

    //lists that are needed for UI
    Hero currentHero;
    List<Hero> linkingHeros;
    List<Skill> skillList;
    SkillSet playerLinkedSkills;
    List<Actor> targets;
    SkillButton[] buttonArray;
    Queue<Hero> heroList;

    //objects that will be manipulated by class
    Cursor2d cursor2d;
    public Cursor3d cursor3d;

    //Finite States
    bool isSkillLoaded = false;
    bool isSelectingEnemies = false;
    
    //cursor selection
    int playerCursorScroll = 0;
    int playerCursorSelection = 0;
    

    /// <summary>
    /// At start of game, method will clear all data, find all associate game objects, and instantiate objects. 
    /// </summary>
    void Start()
    {
        //clear data from previous battles and get data
        clearData();
        generateHeroUI();

        //create containers
        linkingHeros = new List<Hero>();
        targets = new List<Actor>();
        playerLinkedSkills = new SkillSet();

        //find gameobjects
        buttonArray = this.GetComponentsInChildren<SkillButton>();
        cursor2d = this.GetComponentInChildren<Cursor2d>();
        cursor3d = FindObjectOfType<Cursor3d>();

        //point to heroturn queue in battle system
        heroList = battleSystem.GetComponent<BattleSystem>().heroTurnQueue;

        //manipulate cursor and reset it
        cursor2d.moveCursor(buttonArray[0].gameObject.transform.position);
        checkCursors();

        //start thread which listens for list updates from battle system.
        StartCoroutine(updateList());
    }

    /// <summary>
    /// Listens to players input
    /// </summary>
    void Update()
    {
        //depending on finite state, change controlls
        if (isSelectingEnemies == true)
        {
            playerTargetInput();
        }

        if ((herosAreAvaliable() || isSkillLoaded) && isSelectingEnemies == false)
        {
            playerSkillInput();
        }

        //check if cursors are on or off
        checkCursors();

        //listens for exit key
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    /// <summary>
    /// This will create a loop that listens for updates from battle system.
    /// If hero is avaible, update buttons and UI to present for player.
    /// </summary>
    /// <returns></returns>
    IEnumerator updateList()
    {
        while (battleSystem.GetComponent<BattleSystem>().battleIsGoing)
        {
            //wait for both herolist to be populated, previous skill seletion to be done.  
            yield return new WaitUntil(() => !isSkillLoaded && !isSelectingEnemies);
            yield return new WaitUntil(() => heroList.Count > 0);

            clearData();
            
            //load next hero and present skills 
            currentHero = heroList.Dequeue();
            updateList(currentHero.skills);

            //change finite state
            isSkillLoaded = true;
        }
    }

    /// <summary>
    /// get skills set and updates the possible options that a player can select.
    /// </summary>
    /// <param name="skillSet"></param>
    void updateList(SkillSet skillSet)
    {
        //create container
        Skill[] skills = new Skill[skillSet.Size()];
        SkillBook skillBook = playerSkillBook.GetComponent<SkillBook>();

        //find skill and place in skill array
        for (int i = 0; i < skillSet.Size(); i++)
        {
            skills[i] = skillBook.book.Find(skillSet.ToArray()[i].data);
        }
        updateList(skills);
    }

    /// <summary>
    /// Get skill array and updates the possible options that a player can select.
    /// </summary>
    /// <param name="skills"></param>
    void updateList(Skill[] skills)
    {
        //add each skill into skillList and update buttons
        foreach(Skill s in skills)
        {
            skillList.Add(s);
        }
        updateButtons();
    } 

    /// <summary>
    /// Update the button depeing on the skills loaded in the button array
    /// </summary>
    void updateButtons()
    {
        //for every button, update the button base skill text. If scroll is greater that a certain number, change the set. 
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

    /// <summary>
    /// updates the combine skill text 
    /// </summary>
    private void updateCombineSkillText()
    {
        //find all text in child
        Text[] buttonArray = this.GetComponentsInChildren<Text>();
        List<Text> textList = new List<Text>();

        //filter the list to just contain combined skill
        foreach (Text o in buttonArray)
        {
            if (o.gameObject.tag.Equals("CombinedSkill"))
            {
                textList.Add(o);
            }
        }

        //if no combined skill are available, hide all text
        if(playerLinkedSkills.Size() <= 0)
        {
            foreach(Text t in textList)
            {
                t.enabled = false;
            }
        }

        //if combined skill is found, set text to name of the skill. If combined skill is not found, hide text. 
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

    /// <summary>
    /// Will get current skill, and place it into the skill set. This will set up a link system.
    /// </summary>
    /// <param name="skill"></param>
    void linkSkill(Skill skill)
    {
        //insert skill to skill set
        playerLinkedSkills.Insert(playerSkillBook.GetComponent<SkillBook>().book.Find(skill));
        //update the text ui.
        Text[] textUI = playerLinkedSkillsUI.GetComponentsInChildren<Text>();
        //UI position will occur at order of entry. 
        textUI[playerLinkedSkills.Size() - 1].text = skill.name;
        //clear data and load next available hero
        clearData();
        return;
    }

    /// <summary>
    /// method will create an attack and place it into the attacking queue in battle system. 
    /// </summary>
    /// <param name="skill"></param>
    /// <returns></returns>
    public IEnumerator addSkillToQueue(Skill skill)
    {
        //change state
        isSelectingEnemies = true;

        //find skill from player selected skills
        playerLinkedSkills.Insert(playerSkillBook.GetComponent<SkillBook>().book.Find(skill));
        Skill linkedSkill = playerSkillBook.GetComponent<SkillBook>().book.Find(playerLinkedSkills);

        //await target to be selected
        if (skill == null)
        {
            yield return new WaitUntil(() => targets.Count > 0);
        }
        else
        {
            yield return new WaitUntil(() => targets.Count >= skill.targetCount);
        }

        //reset cursor
        playerCursorScroll = 0;

        //create copy of target list
        List<Actor> tempTargetList = new List<Actor>(targets);

        //add attack in attack queue
        if (linkedSkill != null)
        {
            battleSystem.GetComponent<BattleSystem>().addCastingList(currentHero.attack(linkedSkill, linkingHeros, tempTargetList));
            linkingHeros.Clear();
            targets.Clear();
        }
        else
        {
            Skill failedSkill = new Skill(9999, "failed linked", 1, 1, DamageType.attack, new List<Effects>(), 1 ,new SkillSet());
            battleSystem.GetComponent<BattleSystem>().addCastingList(currentHero.attack(failedSkill, linkingHeros, tempTargetList));
            linkingHeros.Clear();
            targets.Clear();
        }

        //clears button
        foreach(Text t in playerLinkedSkillsUI.GetComponentsInChildren<Text>())
        {
            t.text = "";
        }

        //clear data 
        playerLinkedSkills = new SkillSet();

        //change state
        isSelectingEnemies = false;
        yield break;
    }

    /// <summary>
    /// Will find heros and assign each a hero ui 
    /// </summary>
    private void generateHeroUI()
    {
        //find all heros
        foreach (Hero hero in battleSystem.GetComponent<BattleSystem>().heros.GetComponentsInChildren<Hero>())
        {
            bool setUI = false;
            foreach (HeroUIController heroUI in playerHealthDisplayUI.GetComponentsInChildren<HeroUIController>())
            {   
                //sets UI by finding empty container
                if(heroUI.hero == null && !setUI)
                {
                    heroUI.hero = hero;
                    setUI = true;
                }
            }
        }
    }

    /// <summary>
    /// determines if there is a hero that is available
    /// </summary>
    /// <returns></returns>
    private bool herosAreAvaliable()
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

    /// <summary>
    /// add target to target set
    /// </summary>
    void addTarget()
    {
        Enemy[] enemies = battleSystem.GetComponent<BattleSystem>().enemies.GetComponentsInChildren<Enemy>();
        targets.Add(enemies[playerCursorSelection]);
        playerCursorSelection = 0;
        cursor3d.moveCursor(enemies[playerCursorSelection].gameObject);
    }

    /// <summary>
    /// check if cursors are active and set visible if there are active. 
    /// </summary>
    void checkCursors()
    {
        //check state and display cursor if condidtions are met. 
        if (isSkillLoaded)
        {
            cursor2d.gameObject.SetActive(true);
        }
        else
        {
            cursor2d.gameObject.SetActive(false);
        }

        if (isSelectingEnemies)
        {
            cursor3d.gameObject.SetActive(true);
        }
        else
        {
            cursor3d.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// clears skill list
    /// </summary>
    private void clearData()
    {
        skillList = new List<Skill>();
    }

    /// <summary>
    /// Clears buttons text
    /// </summary>
    void clearButtons()
    {
        for(int i = 0; i < 3; i++)
        {
            buttonArray[i].clearText();
        }

        clearCombinedSkills();
    }

    /// <summary>
    /// Clears any saved skills from previous linking actions.
    /// </summary>
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

    /// <summary>
    /// Displays win screen 
    /// </summary>
    public void displayResultScreen()
    {
        StartCoroutine(fadeInResultScreen());
    }

    /// <summary>
    /// will fade in black screen to hide battle, displays you win message
    /// </summary>
    /// <returns></returns>
    IEnumerator fadeInResultScreen()
    {
        resultScreen.SetActive(true);
        var tempcolor = resultScreen.GetComponent<Image>().color;

        while (resultScreen.GetComponent<Image>().color.a <= 1)
        {
            tempcolor.a += 0.02f;
            resultScreen.GetComponent<Image>().color = tempcolor;
            yield return new WaitForSeconds(0.02f);
        }

        tempcolor = resultScreen.GetComponentInChildren<Text>().color;
        while (resultScreen.GetComponentInChildren<Text>().color.a <= 1)
        {
            tempcolor.a += 0.02f;
            resultScreen.GetComponentInChildren<Text>().color = tempcolor;
            yield return new WaitForSeconds(0.02f);
        }

    }

    /// <summary>
    /// player input listener during skill selection
    /// </summary>
    private void playerSkillInput()
    {

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (playerLinkedSkills.Size() < 2)
            {
                linkingHeros.Add(currentHero);
                linkSkill(buttonArray[playerCursorSelection].skill);
                cursor2d.moveCursor(buttonArray[0].gameObject.transform.position);
                playerCursorSelection = 0;
                clearButtons();
                isSkillLoaded = false;
            }
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            SkillButton[] buttonArray = this.GetComponentsInChildren<SkillButton>();
            linkingHeros.Add(currentHero);
            StartCoroutine(addSkillToQueue(buttonArray[playerCursorSelection].skill));
            cursor2d.moveCursor(buttonArray[0].gameObject.transform.position);
            Enemy[] enemies = battleSystem.GetComponent<BattleSystem>().enemies.GetComponentsInChildren<Enemy>();
            playerCursorSelection = 0;
            cursor3d.moveCursor(enemies[playerCursorSelection].gameObject);
            clearButtons();
            isSkillLoaded = false;
        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (playerCursorSelection < 2)
            {
                playerCursorSelection++;
                Vector3 v = buttonArray[playerCursorSelection].transform.position;
                cursor2d.moveCursor(v);
            }

            else
            {
                playerCursorSelection = 0;
                Vector3 v = buttonArray[playerCursorSelection].transform.position;
                cursor2d.moveCursor(v);
            }
        }

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (playerCursorSelection > 0)
            {
                playerCursorSelection--;
                Vector3 v = buttonArray[playerCursorSelection].transform.position;
                cursor2d.moveCursor(v);
            }

            else
            {
                playerCursorSelection = 2;
                Vector3 v = buttonArray[playerCursorSelection].transform.position;
                cursor2d.moveCursor(v);
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

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            debugWindow.SetActive(!debugWindow.activeSelf);
        }
    }

    /// <summary>
    /// player input listener during target slection 
    /// </summary>
    private void playerTargetInput()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if(playerCursorSelection <= 0)
            {
                Enemy[] enemies = battleSystem.GetComponent<BattleSystem>().enemies.GetComponentsInChildren<Enemy>();
                playerCursorSelection = battleSystem.GetComponent<BattleSystem>().enemies.GetComponentsInChildren<Enemy>().Length - 1;
                cursor3d.moveCursor(enemies[playerCursorSelection].gameObject);
            }
            else
            {
                playerCursorSelection--;
                Enemy[] enemies = battleSystem.GetComponent<BattleSystem>().enemies.GetComponentsInChildren<Enemy>();
                cursor3d.moveCursor(enemies[playerCursorSelection].gameObject);
            }
            
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (playerCursorSelection >= battleSystem.GetComponent<BattleSystem>().enemies.GetComponentsInChildren<Enemy>().Length - 1)
            {
                Enemy[] enemies = battleSystem.GetComponent<BattleSystem>().enemies.GetComponentsInChildren<Enemy>();
                playerCursorSelection = 0;
                cursor3d.moveCursor(enemies[playerCursorSelection].gameObject);
            }
            else
            {
                playerCursorSelection++;
                Enemy[] enemies = battleSystem.GetComponent<BattleSystem>().enemies.GetComponentsInChildren<Enemy>();
                cursor3d.moveCursor(enemies[playerCursorSelection].gameObject);
            }
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            addTarget();
        }
    }


}
