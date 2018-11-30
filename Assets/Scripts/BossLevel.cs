using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossLevel : MonoBehaviour
{

    public GameObject Enemy;
    public GameObject SpawnEffect;
    public Transform[] SpawnPoints;

    // Use this for initialization
    void Start()
    {

    }
    

    void SpawnEnemy(Vector3 position, Quaternion rotation)
    {
        GameObject effect = Instantiate(SpawnEffect, position, Quaternion.identity);
        Instantiate(Enemy, position, rotation);
        Destroy(effect, 2);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
