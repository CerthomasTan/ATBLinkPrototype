using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleSystem : MonoBehaviour
{
    [SerializeField] public GameObject heros;
    [SerializeField] GameObject enemies;
    [SerializeField] GameObject UI;

    List<Actor> chargeList;
    Queue<Attack> attackingList;
    public Queue<Hero> heroTurnQueue;
    Queue<Enemy> enemyTurn;
    List<Actor> castingList;

    public bool battleIsGoing { get; set; }


    private void Awake()
    {
        generateBattle();
    }

    void Start()
    {
        
        StartCoroutine(chargeATBLoop());
        StartCoroutine(attackLoop());
    }

    void generateBattle()
    {
        battleIsGoing = true;
        chargeList = new List<Actor>();
        attackingList = new Queue<Attack>();
        heroTurnQueue = new Queue<Hero>();
        enemyTurn = new Queue<Enemy>();
        castingList = new List<Actor>();

        foreach (Hero h in heros.GetComponentsInChildren<Hero>())
        {
            moveActorToChargeList(h);
        }
        foreach (Enemy e in enemies.GetComponentsInChildren<Enemy>())
        {
            moveActorToChargeList(e);
        }
    }

    IEnumerator chargeATBLoop()
    {

        while (battleIsGoing)
        {
            yield return new WaitForSecondsRealtime(.05f);
            foreach(Actor actor in chargeList)
            {
                actor.chargeATB();
            }
            List<Actor> templist = new List<Actor>(chargeList);
            foreach(Actor actor in chargeList)
            {
                if(actor.currentATBCharge == actor.maxATBCharge)
                {
                    templist.Remove(actor);
                    moveActorToTurnList(actor);
                }
            }
            chargeList = templist;
        }
    }

    void moveActorToTurnList(Actor actor)
    {
        if(actor is Hero)
        {
            heroTurnQueue.Enqueue((Hero)actor);
        }

        else if (actor is Enemy)
        {
            enemyTurn.Enqueue((Enemy)actor);
        }
    }

    void moveActorToChargeList(Actor actor)
    {
        actor.currentATBCharge = 0;
        chargeList.Add(actor);
    }

    public void addAttackingList(Attack attack)
    {
        attackingList.Enqueue(attack);
    }


    IEnumerator attackLoop()
    {
        bool performingAttack = false;
        MoveInfoUI moveInfoUi = UI.GetComponentInChildren<MoveInfoUI>();
        while (battleIsGoing)
        {
            yield return new WaitUntil(() => attackingList.Count > 0 && !performingAttack);
            Attack currentattack = attackingList.Dequeue();
            print(currentattack.actors.Count);
            performingAttack = true;
            yield return moveInfoUi.displayMove(currentattack.skill.ToString());
            foreach(Actor a in currentattack.actors)
            {
                print("moving actor");
                moveActorToChargeList(a);
            }
            performingAttack = false;
        }
        yield break;
    }


    void gameStartUpdate()
    {
        foreach(Hero h in heros.GetComponentsInChildren<Hero>())
        {
            moveActorToChargeList(h);
        }

        foreach(Enemy e in enemies.GetComponentsInChildren<Enemy>())
        {
            moveActorToChargeList(e);
        }
    }

}
