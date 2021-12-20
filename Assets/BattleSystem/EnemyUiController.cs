using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// controller that will handle any ui that are associated with enemies
/// </summary>
public class EnemyUiController : MonoBehaviour
{
    [SerializeField] Enemy enemy;
    [SerializeField] TextMesh textMesh;

    private void Start()
    {
        StartCoroutine(setEnemy());
    }
    // Update is called once per frame
    void Update()
    {
        if (textMesh != null)
        {
            textMesh.text = string.Format("{0}/{1}",enemy.currentHealth, enemy.maxHealth);
        }
    }

    /// <summary>
    /// update the text to display health
    /// </summary>
    /// <returns></returns>
    IEnumerator setEnemy()
    {
        yield return new WaitUntil(() => GetComponent<Enemy>() != null);
        enemy = GetComponent<Enemy>();
        yield return new WaitUntil(() => GetComponentInChildren<Enemy>() != null);
        textMesh = GetComponentInChildren<TextMesh>();
        yield break;
    }

}
