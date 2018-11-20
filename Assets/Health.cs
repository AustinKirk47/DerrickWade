using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour {

	public int MaxHP = 8;
	public GameObject HealthbarFG;
	public GameObject HealthbarBG;
	public ParticleSystem ExplosionSystem;
	public bool FollowObject;

	static Color HealthyColor = new Color(166/255f, 253/255f, 177/255f);
	static Color DangerColor = new Color(253/255f, 166/255f, 177/255f);

	private int HP;

	void Start () {
		HP = MaxHP;
		SetColor(HealthyColor);
	}

	public void TakeDamage()
	{
		if (HP > 0)
		{
			HP--;
			UpdateUI();
		}

		if (HP == 0)
		{
			Instantiate(ExplosionSystem, transform.position, transform.rotation);

			if (FollowObject)
			{
				Destroy(transform.parent.gameObject);
			} else
			{
				Destroy(gameObject);
			}
		}
	}

	void UpdateUI()
	{
		float scale = (float)HP / MaxHP;
		HealthbarBG.GetComponent<RectTransform>().localScale = new Vector2(scale, 1);

		if (scale <= 0.3f)
		{
			SetColor(DangerColor);
		} else
		{
			SetColor(HealthyColor);
		}
	}

	void SetColor(Color color)
	{
		HealthbarBG.GetComponent<Image>().color = color;
		HealthbarFG.GetComponent<Image>().color = color;
	}

	void Update()
	{
		if (FollowObject)
		{
			Vector3 offset = new Vector3(-1.5f, 2, 0);

			HealthbarBG.GetComponent<RectTransform>().position = transform.position + offset;
			HealthbarFG.GetComponent<RectTransform>().position = transform.position + offset;
		}
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.gameObject.tag.Equals("Projectile"))
		{
			TakeDamage();
			Destroy(other.gameObject);
		}
	}
}
