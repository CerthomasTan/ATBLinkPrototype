using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// will control the display for current move being done in battle
/// </summary>
public class MoveInfoUI : MonoBehaviour
{
    /// <summary>
    /// will display the move for 2 secounds and the disappear. 
    /// </summary>
    /// <param name="moveInfo"></param>
    /// <returns></returns>
    public IEnumerator displayMove(string moveInfo)
    {
        this.gameObject.SetActive(true);
        this.GetComponentInChildren<Text>().text = moveInfo;
        yield return new WaitForSeconds(2.0f);
        this.gameObject.SetActive(false);

        
    }
}
