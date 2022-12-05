using UnityEngine;
public class Bullet : MonoBehaviour
{
    public Transform cam;
    public float speed;
    public float range;
    public float rangeAttack;

    private void Awake()
    {
        cam = GameObject.Find("PlayerCam").transform;
        //Random range
        float x = Random.Range(-range, range);
        float y = Random.Range(-range, range);
        float z = Random.Range(-range, range);
        transform.Rotate(new Vector3(x, y, z));
    }
    private void Update()
    {
        
        transform.Translate(cam.forward * speed * Time.deltaTime);
        Destroy(gameObject, rangeAttack);
       
    }
    
}
