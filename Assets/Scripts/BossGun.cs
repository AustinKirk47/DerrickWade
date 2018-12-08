using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossGun : MonoBehaviour
{

    public float ProjectileSpeed = 10;
    public float FireRate = 1;
    public float FollowDistance = 10;
    public GameObject Projectile;
    public GameObject[] Loot;

    private Rigidbody2D body;
    private GameObject player;

    private float epsilon = 5f;
    private float FireAllowance = 0;
    private int LastSecond = 0;

    private float SpawnTime;

    public AudioSource ShootSound;

    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        body.freezeRotation = true;

        player = GameObject.FindWithTag("Player");

        SpawnTime = Time.time;
    }

    void FixedUpdate()
    {
        AimAtPlayer();

        if (Time.time - SpawnTime < 2)
        {
            return;
        }

        int second = (int)Time.time;
        if (second != LastSecond)
        {
            LastSecond = second;
            FireAllowance += FireRate;
        }

        if (FireAllowance > 0 && Random.Range(0, 50) < 1)
        {
            Fire();
        }
    }

    public void OnDeath()
    {
        if (Random.Range(0f, 1f) < 0.75f && Loot.Length > 0)
        {
            int i = Random.Range(0, Loot.Length);
            GameObject loot = Instantiate(Loot[i]);
            loot.transform.position = transform.position;
            loot.GetComponent<Rigidbody2D>().velocity = Random.insideUnitCircle * 0.5f;
            loot.GetComponent<Rigidbody2D>().angularVelocity = Random.value * 20;
        }
    }

    void Fire()
    {
        Vector2 direction = new Vector2(-Mathf.Sin(Mathf.Deg2Rad * body.rotation), Mathf.Cos(Mathf.Deg2Rad * body.rotation));

        GameObject p = Instantiate(Projectile);
        p.transform.position = transform.position + new Vector3(direction.x, direction.y, 0) * 3.5f;
        p.GetComponent<Rigidbody2D>().velocity = direction * ProjectileSpeed;
        var a = Instantiate(ShootSound);
        Destroy(a, 1);
        Destroy(p, 5);
        FireAllowance--;
    }
    void AimAtPlayer()
    {
        Vector2 playerPos = player.transform.position;
        float angle = Mathf.Atan2(playerPos.y - transform.position.y, playerPos.x - transform.position.x) * Mathf.Rad2Deg - 90;

        body.rotation = angle;
    }
}
