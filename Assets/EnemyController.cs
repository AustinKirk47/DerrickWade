using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour {

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

	void Start () {
		body = GetComponent<Rigidbody2D>();
		body.freezeRotation = true;

		player = GameObject.FindWithTag("Player");
	}
	
	void FixedUpdate () {
		AimAtPlayer();
		MaintainPlayerDistance();

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
		p.transform.position = transform.position + new Vector3(direction.x, direction.y, 0) * 1.5f;
		p.GetComponent<Rigidbody2D>().velocity = direction * ProjectileSpeed;

		Destroy(p, 5);
		FireAllowance--;
	}

	void MaintainPlayerDistance()
	{
		Vector3 delta = player.transform.position + Random.insideUnitSphere * 5 - transform.position;
		Vector2 desiredVelocity;
		if (Mathf.Abs(delta.magnitude - FollowDistance) > epsilon)
		{
			float x = delta.magnitude - FollowDistance;
			desiredVelocity = delta.normalized * x;
		} else
		{
			desiredVelocity = new Vector2(Mathf.Cos(Time.time) * 2, Mathf.Sin(Time.time) * 2);
		}

		body.velocity = body.velocity * 0.8f + desiredVelocity * 0.2f;
	}
	void AimAtPlayer()
	{
		Vector2 playerPos = player.transform.position;
		float angle = Mathf.Atan2(playerPos.y - transform.position.y, playerPos.x - transform.position.x) * Mathf.Rad2Deg - 90;

		body.rotation = angle;
	}
}
