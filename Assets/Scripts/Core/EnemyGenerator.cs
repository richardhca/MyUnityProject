using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGenerator : MonoBehaviour
{
    [SerializeField] private List<GameObject> enemies;
    //[SerializeField] private int numberOfEnemies = 10;

    private const int areaRadius = 230;

    public void GenerateEnemies(int n)
    {
        if (enemies.Count != 0)
            StartCoroutine(generateEnemies(enemies[0], n));
    }

    public void ClearEnemies()
    {
        while (transform.childCount != 0)
        {
            GameObject o = transform.GetChild(0).gameObject;
            Destroy(o);
        }
    }

    IEnumerator generateEnemies(GameObject enemy, int number)
    {
        for (int i=1; i<=number; i++)
        {
            Vector3 randomPosition = new Vector3(Random.Range(-1*areaRadius-1, areaRadius-1), 0, Random.Range(-1*areaRadius-1, areaRadius-1));
            GameObject avatar = Instantiate(enemy, randomPosition, Quaternion.identity, transform);
            avatar.transform.LookAt(Vector3.zero);
            yield return new WaitForSeconds(0.5f);
        }
    }
}
