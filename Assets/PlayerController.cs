using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerController : MonoBehaviour
{

    public float MovementSpeed = 5;
    public float ProjectileSpeed = 10;
    public GameObject Projectile;
    private SpriteRenderer sr;
    public int score = 0;
    public TextMeshProUGUI TextPro;

    public Sprite slow;
    public Sprite fast;
    public float flameThreshold = 3;

    private Rigidbody2D body;

    private bool alt = false;
    public Transform Point1;
    public Transform Point2;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        body = GetComponent<Rigidbody2D>();
        body.freezeRotation = true;
    }

    void FixedUpdate()
    {
        HandleMovement();
        body.rotation = GetAngle();

        if (Input.GetMouseButtonDown(0))
        {
            Fire();
        }
    }

    void Fire()
    {
        Vector2 direction = new Vector2(-Mathf.Sin(Mathf.Deg2Rad * body.rotation), Mathf.Cos(Mathf.Deg2Rad * body.rotation));

        GameObject p = Instantiate(Projectile);
        alt = !alt;
        if (alt)
        {
            p.transform.position = Point1.position + new Vector3(direction.x, direction.y, 0) * 1.25f;
        }
        else
        {
            p.transform.position = Point2.position + new Vector3(direction.x, direction.y, 0) * 1.25f;

        }
        p.transform.rotation = Quaternion.AngleAxis(transform.GetComponent<Rigidbody2D>().rotation, Vector3.forward);
        p.GetComponent<Rigidbody2D>().velocity = direction * ProjectileSpeed;

        Destroy(p, 5);
    }

    private void HandleMovement()
    {
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
        if (body.velocity.x >= flameThreshold || body.velocity.x <= -flameThreshold || body.velocity.y >= flameThreshold || body.velocity.y <= -flameThreshold)
        {
            sr.sprite = fast;
        }
        else sr.sprite = slow;
    }

    private float GetAngle()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        return Mathf.Atan2(mousePos.y - transform.position.y, mousePos.x - transform.position.x) * Mathf.Rad2Deg - 90;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag.Equals("Point"))
        {
            Debug.Log("+1 point!");
            score += 10;
            TextPro.GetComponent<TextMeshProUGUI>();
            TextPro.text = "SCORE: " + score.ToString();
            Destroy(other.gameObject);
        }
    }
}