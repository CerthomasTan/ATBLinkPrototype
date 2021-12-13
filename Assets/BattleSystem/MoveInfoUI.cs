using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoveInfoUI : MonoBehaviour
{
    // Start is called before the first frame update

    public IEnumerator displayMove(string moveInfo)
    {
        this.gameObject.SetActive(true);
        this.GetComponentInChildren<Text>().text = moveInfo;
        yield return new WaitForSeconds(2.0f);
        this.gameObject.SetActive(false);

        
    }
}
