using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    IEnumerator setEnemy()
    {
        yield return new WaitUntil(() => GetComponent<Enemy>() != null);
        enemy = GetComponent<Enemy>();
        yield return new WaitUntil(() => GetComponentInChildren<Enemy>() != null);
        textMesh = GetComponentInChildren<TextMesh>();
    }

}
