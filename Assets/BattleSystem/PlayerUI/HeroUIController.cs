using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeroUIController : MonoBehaviour
{
    public Hero hero;

    void Update()
    {
        string stringbuidler = hero.actorName + System.Environment.NewLine;
        stringbuidler += System.Environment.NewLine;
        stringbuidler += string.Format("Health: {0}/{1}{2}", hero.currentHealth, hero.maxHealth, System.Environment.NewLine);
        stringbuidler += System.Environment.NewLine;
        stringbuidler += string.Format("Charge: {0}/{1}{2}", hero.currentATBCharge, hero.maxATBCharge, System.Environment.NewLine);
        this.gameObject.GetComponentInChildren<Text>().text = stringbuidler;
    }
}
