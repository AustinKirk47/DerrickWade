using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	public float MovementSpeed = 5;
	public float ProjectileSpeed = 10;
	public GameObject Projectile;

	private Rigidbody2D body;

	// Use this for initialization
	void Start () {
		body = GetComponent<Rigidbody2D>();
		body.freezeRotation = true;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		HandleMovement();
		body.rotation = GetAngle();

		if (Input.GetMouseButtonDown(0))
		{
			Vector2 direction = new Vector2(-Mathf.Sin(Mathf.Deg2Rad * body.rotation), Mathf.Cos(Mathf.Deg2Rad * body.rotation));

			GameObject p = Instantiate(Projectile);
			p.transform.position = transform.position + new Vector3(direction.x, direction.y, 0);
			p.GetComponent<Rigidbody2D>().velocity = direction * ProjectileSpeed;
		}
	}

	private void HandleMovement()
	{`
		if (Input.GetKey(KeyCode.W))
		{
			body.AddForce(Vector2.up * MovementSpeed);
		}
		if (Input.GetKey(KeyCode.A))
		{
			body.AddForce(Vector2.left * MovementSpeed);
		}
		if (Input.GetKey(KeyCode.S))
		{
			body.AddForce(Vector2.down * MovementSpeed);
		}
		if (Input.GetKey(KeyCode.D))
		{
			body.AddForce(Vector2.right * MovementSpeed);
		}
	}

	private float GetAngle()
	{
		Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		return Mathf.Atan2(mousePos.y - transform.position.y, mousePos.x - transform.position.x) * Mathf.Rad2Deg - 90;
	}
}
