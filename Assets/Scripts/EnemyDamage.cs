using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
    public float damage;
    public float RangeAttack;
    private void Update()
    {
        Destroy(gameObject, RangeAttack);
    }
    private void OnTriggerEnter(Collider myCollider)
    {

        if(myCollider.tag == "Player")
        {
            myCollider.GetComponent<PlayerMovement>().TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}
