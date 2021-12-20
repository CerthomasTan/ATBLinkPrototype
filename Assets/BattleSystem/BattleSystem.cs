using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Battle System that will control all game loops (ie. charging and attacking).
/// This method will also ensure proper game state and resolve any collisions that may occur with multip[le thread.
/// </summary>
public class BattleSystem : MonoBehaviour
{
    //unity objects
    [SerializeField] public GameObject heros;
    [SerializeField] public GameObject enemies;
    [SerializeField] GameObject UI;

    //data structures
    public List<Actor> chargeList;
    Queue<Attack> attackingList;
    public Queue<Hero> heroTurnQueue;
    Queue<Enemy> enemyTurn;
    List<Attack> castingList;

    //finite states
    public bool battleIsGoing { get; set; }
    bool performingAttack = false;


    private void Awake()
    {
        generateBattle();
    }

    /// <summary>
    /// creates 2 threads that are for charging and attacking
    /// </summary>
    void Start()
    {
        StartCoroutine(chargeATBLoop());
        StartCoroutine(attackLoop());
        StartCoroutine(castingLoop());
    }

    /// <summary>
    /// Will create all varaibles and find essential objects to start a battle.
    /// </summary>
    void generateBattle()
    {
        battleIsGoing = true;
        chargeList = new List<Actor>();
        attackingList = new Queue<Attack>();
        heroTurnQueue = new Queue<Hero>();
        enemyTurn = new Queue<Enemy>();
        castingList = new List<Attack>();

        foreach (Hero h in heros.GetComponentsInChildren<Hero>())
        {
            moveActorToChargeList(h);
        }
        foreach (Enemy e in enemies.GetComponentsInChildren<Enemy>())
        {
            moveActorToChargeList(e);
        }
    }

    /// <summary>
    /// charging loop that will charge all actors by actors specifed charge rate. If actor is charged, actor is moved and placed into turn queue
    /// </summary>
    /// <returns></returns>
    IEnumerator chargeATBLoop()
    {
        //creates loop
        while (battleIsGoing)
        {
            //wait for a few seounds 
            yield return new WaitForSecondsRealtime(.05f);
            //wait for other systems to release data structures
            yield return new WaitUntil(() => !performingAttack);

            //charge actors ATB
            foreach (Actor actor in chargeList)
            {
                actor.chargeATB();
            }

            //check if any actor is fullycharge. If fully charge, remove from charging list and move to turn queue
            List<Actor> templist = new List<Actor>(chargeList);
            foreach (Actor actor in chargeList)
            {
                if(actor.currentATBCharge == actor.maxATBCharge)
                {
                    templist.Remove(actor);
                    moveActorToTurnList(actor);
                }
            }
            chargeList = templist;
        }

        StartCoroutine(resolveBattle());
    }

    /// <summary>
    /// Creates Attack loop
    /// </summary>
    /// <returns></returns>
    IEnumerator attackLoop()
    {
        //create loop
        while (battleIsGoing)
        {
            //get ui to display move
            MoveInfoUI moveInfoUi = UI.GetComponentInChildren<MoveInfoUI>();

            //wait for queue to contain an attack and wait for other attack to finish
            yield return new WaitUntil(() => attackingList.Count > 0 && !performingAttack);

            //do next attack and resolve damage
            Attack currentattack = attackingList.Dequeue();
            performingAttack = true;
            yield return StartCoroutine(resolveDamage(currentattack, moveInfoUi));
            returnActorsFromAttack(currentattack);

            //set finite state
            performingAttack = false;
        }
        yield break;
    }

    IEnumerator castingLoop()
    {
        while (battleIsGoing)
        {
            //await for item is charging and performing attack is false
            yield return new WaitUntil(() => castingList.Count > 0 && !performingAttack);
            foreach(Attack attack in castingList)
            {
                attack.castCharge += 0.1f;
            }
            List<Attack> tempCastList = new List<Attack>(castingList);
            foreach (Attack attack in tempCastList)
            {
                if(attack.castCharge >= attack.skill.castTime)
                {
                    addAttackingList(attack);
                    castingList.Remove(attack);
                }
            }
            yield return new WaitForSecondsRealtime(0.1f);
        }
    }

    /// <summary>
    /// moves actor into turn list depending on class type
    /// </summary>
    /// <param name="actor"></param>
    void moveActorToTurnList(Actor actor)
    {
        if (actor is Hero)
        {
            heroTurnQueue.Enqueue((Hero)actor);
        }

        else if (actor is Enemy)
        {
            enemyTurn.Enqueue((Enemy)actor);
        }
    }

    /// <summary>
    /// move actor to charge list
    /// </summary>
    /// <param name="actor"></param>
    void moveActorToChargeList(Actor actor)
    {
        actor.currentATBCharge = 0;
        chargeList.Add(actor);
    }

    /// <summary>
    /// adds attack to attack list
    /// </summary>
    /// <param name="attack"></param>
    public void addAttackingList(Attack attack)
    {
        attackingList.Enqueue(attack);
    }

    public void addCastingList(Attack attack)
    {
        castingList.Add(attack);
    }

    /// <summary>
    /// move casters of attack back to charging list. 
    /// <param name="attack"></param>
    public void returnActorsFromAttack(Attack attack)
    {
        foreach (Actor a in attack.casters)
        {
            moveActorToChargeList(a);
        }
    }

    /// <summary>
    /// remove actor from game.
    /// </summary>
    /// <param name="actor"></param>
    public void removeActorFromBattle(Actor actor)
    {
        //set actor to disabled
        actor.gameObject.SetActive(false);

        //remove from lists and queues 
        chargeList.Remove(actor);
        if (enemyTurn.Contains((Enemy)actor))
        {
            Enemy temp = enemyTurn.Dequeue();
            while(temp != actor)
            {
                enemyTurn.Enqueue(temp);
                temp = enemyTurn.Dequeue();
            }
            enemyTurn.Clear();
        }
        if(enemies.GetComponentInChildren<Enemy>() == null)
        {
            battleIsGoing = false;
        }
    }

    /// <summary>
    /// Applies damage and check for if actor is at 0=hp
    /// </summary>
    /// <param name="attack"></param>
    /// <param name="ui"></param>
    /// <returns></returns>
    IEnumerator resolveDamage(Attack attack, MoveInfoUI ui)
    {
        //display move
        StartCoroutine(ui.displayMove(attack.skill.ToString()));

        //applyEffects

        //apply damage and check if still alive
        foreach(Actor a in attack.targets)
        {
            if (a != null)
            {
                a.takeDamage(attack.calculateDamage());
                if (a.currentHealth <= 0)
                {
                    removeActorFromBattle(a);
                }
            }
            else
            {
                Enemy nextEnemy = enemies.GetComponent<Enemy>();
                nextEnemy.takeDamage(attack.calculateDamage());
                if (nextEnemy.currentHealth <= 0)
                {
                   removeActorFromBattle(nextEnemy);
                }
            }
        }

        yield break;
    }


    /// <summary>
    /// plays at end of battle
    /// </summary>
    /// <returns></returns>
    IEnumerator resolveBattle()
    {
        //display win and lvl up message
        UI.GetComponent<PlayerSkillUIController>().displayResultScreen();
        
        //await player input to get past screen
        yield return new WaitUntil(() => Input.anyKeyDown);

        //lvl up all heros
        foreach (Hero h in heros.GetComponentsInChildren<Hero>())
        {
            h.lvlUp();
        }

        //return to title
        SceneManager.LoadScene(0);
    }
}
