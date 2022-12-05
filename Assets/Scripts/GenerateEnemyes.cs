using System.Collections;
using UnityEngine;

public class GenerateEnemyes : MonoBehaviour
{
    public GameObject theEnemy;
    private float xPos;
    private float zPos;
    public static int enemyCount;
    public int howMuch;

    void Start()
    {
        StartCoroutine(enemyDrop());
    }

   IEnumerator enemyDrop()
    {
        while(enemyCount < howMuch)
        {
            xPos = Random.Range(-28.6f, 25.9f);
            zPos = Random.Range(-36.9f, 4.5f);
            Instantiate(theEnemy, new Vector3(xPos, 1.43f, zPos), Quaternion.identity);
            yield return new WaitForSeconds(0.1f);
            enemyCount += 1;
        }
    }
}
