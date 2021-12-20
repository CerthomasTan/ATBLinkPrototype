using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Controls the debuging charge list windows
/// </summary>
public class DebugChargeList : MonoBehaviour
{

    [SerializeField] GameObject BattleSystem;

    // Update is called once per frame
    void LateUpdate()
    {
        printChargeList();
    }

    /// <summary>
    /// prints charge all actors in the charging list
    /// </summary>
    void printChargeList()
    {
        if(BattleSystem.GetComponent<BattleSystem>() == null)
        {
            return;
        }
        string temp = "";
        foreach (Actor a in BattleSystem.GetComponent<BattleSystem>().chargeList)
        {
            temp += string.Format("{0}{1}", a.name, System.Environment.NewLine);
        }
        this.GetComponent<Text>().text = temp;
    }
}
