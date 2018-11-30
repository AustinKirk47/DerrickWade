using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level2Controller : LevelController
{

    public GameObject Enemy1;
    public GameObject Enemy2;
    public GameObject SpawnEffect;
    public Transform[] SpawnPoints1;
    public Transform[] SpawnPoints2;

    public int ObstacleSpawnInterval;
    public GameObject[] Obstacles;
    public float ObstacleSpeed;

    public bool ReadyOnStart;

    private float lastSpawnTime;

    void Start()
    {
        if (ReadyOnStart)
        {
            LevelReady();
        }
    }

	public override void LevelReady()
	{
        foreach (Transform t in SpawnPoints1)
        {
            SpawnEnemy1(t.position, t.rotation);
        }

        foreach (Transform t in SpawnPoints2)
        {
            SpawnEnemy2(t.position, t.rotation);
        }

        lastSpawnTime = Time.time;
	}

    void Update()
    {
        if (Time.time > lastSpawnTime + ObstacleSpawnInterval)
        {
            if (Random.value < 0.01f)
            {
                SpawnObstacle();
            }
        }
    }

    void SpawnEnemy1(Vector3 position, Quaternion rotation)
    {
        GameObject effect = Instantiate(SpawnEffect, position, Quaternion.identity);
        Instantiate(Enemy1, position, rotation);
        Destroy(effect, 2);
    }

    void SpawnEnemy2(Vector3 position, Quaternion rotation)
    {
        GameObject effect = Instantiate(SpawnEffect, position, Quaternion.identity);
        Instantiate(Enemy2, position, rotation);
        Destroy(effect, 2);
    }

    void SpawnObstacle()
    {
        int i = Random.Range(0, Obstacles.Length);

        GameObject obstacle = Instantiate(Obstacles[i]);

        int side = Random.Range(0, 4); //0 = Left, 1 = Right, 2 = Top, 3 = Bottom

        float offset = Random.value;
        Vector3 pos = Vector3.zero;

        float buffer = 0.5f;

        if (side == 0)
        {
            pos = new Vector3(-buffer, offset);
        } else if (side == 1)
        {
            pos = new Vector3(1 + buffer, offset);
        } else if (side == 2)
        {
            pos = new Vector3(offset, 1 + buffer);
        } else if (side == 3)
        {
            pos = new Vector3(offset, -buffer);
        }

        pos = Camera.main.ViewportToWorldPoint(pos);
        pos.z = 0;

        Vector3 dir = (GameObject.FindGameObjectWithTag("Player").transform.position - pos).normalized;

        obstacle.transform.position = pos;
        obstacle.GetComponent<Rigidbody2D>().velocity = dir * ObstacleSpeed;
        obstacle.GetComponent<Rigidbody2D>().AddTorque(Random.value * 50);

        lastSpawnTime = Time.time;
    }
}
