using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Class that controls the behavoir of the HeroUI
/// </summary>
public class HeroUIController : MonoBehaviour
{
    public Hero hero;

    /// <summary>
    /// gets called every frame. Updates the Hero UI with infomation.
    /// </summary>
    void Update()
    {
        string stringbuidler = hero.actorName + " lvl: " + hero.level + System.Environment.NewLine;
        stringbuidler += System.Environment.NewLine;
        stringbuidler += string.Format("Health: {0}/{1}{2}", hero.currentHealth, hero.maxHealth, System.Environment.NewLine);
        stringbuidler += System.Environment.NewLine;
        stringbuidler += string.Format("Charge: {0}/{1}{2}", hero.currentATBCharge, hero.maxATBCharge, System.Environment.NewLine);
        stringbuidler += System.Environment.NewLine;

        this.gameObject.GetComponentInChildren<Text>().text = stringbuidler;
    }
}
