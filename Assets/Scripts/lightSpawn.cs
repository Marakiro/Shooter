using UnityEngine;

public class lightSpawn : MonoBehaviour
{
    [Header("Spawn Position")]
    public Vector3 spawnPoint_Camera = new Vector3(-592f, -117f, 42f);
   

    public void Start()
    {
        transform.position = spawnPoint_Camera;
       
    }
}
