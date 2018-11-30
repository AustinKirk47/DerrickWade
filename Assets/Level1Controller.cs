using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level1Controller : LevelController
{

	public GameObject Enemy;
	public GameObject SpawnEffect;
	public Transform[] SpawnPoints;

    public int ObstacleSpawnInterval;
    public GameObject[] Obstacles;
    public float ObstacleSpeed;

    private float lastSpawnTime;

	public override void LevelReady()
	{
		foreach (Transform t in SpawnPoints)
		{
			SpawnEnemy(t.position, t.rotation);
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

	void SpawnEnemy(Vector3 position, Quaternion rotation)
	{
		GameObject effect = Instantiate(SpawnEffect, position, Quaternion.identity);
		Instantiate(Enemy, position, rotation);
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
