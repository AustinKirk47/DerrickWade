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
    public GameObject HealthFlash;

	static Color HealthyColor = new Color(166/255f, 253/255f, 177/255f);
	static Color DangerColor = new Color(253/255f, 166/255f, 177/255f);

	private int HP;

    void Start () {
		HP = MaxHP;
		SetColor(HealthyColor);
    }

	public void TakeDamage()
	{
        if (HP <= 0)
        {
            return;
        }

        if (gameObject.GetInstanceID() == GameObject.FindGameObjectWithTag("Player").GetInstanceID())
        {
            StartCoroutine("CameraShake");
            StartCoroutine("FlashRed");
        }

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

			EnemyController enemyController = GetComponent<EnemyController>();
			if (enemyController != null)
			{
				enemyController.OnDeath();
			}
		}
	}

	public void Heal()
	{
		if (HP < MaxHP)
		{
			HP = Mathf.Min(MaxHP, HP + 2);
			UpdateUI();
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

    IEnumerator CameraShake()
    {
        Vector3 originalCameraPos = Camera.main.transform.localPosition;
        for (int i = 0; i < 15; i++)
        {
            Camera.main.transform.localPosition = originalCameraPos + Random.insideUnitSphere * 0.5f;
            yield return null;
        }
        Camera.main.transform.localPosition = originalCameraPos;
    }

    IEnumerator FlashRed()
    {
        Image image = HealthFlash.GetComponent<Image>();
        for (float i = 0.9f; i >= 0.05; i*=0.85f)
        {
            if (i < 0.1)
            {
                i *= 0.75f;
            }

            image.color = new Color(image.color.r, image.color.g, image.color.b, i);
            yield return null;
        }

        image.color = new Color(image.color.r, image.color.g, image.color.b, 0);
       
    }

    private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.gameObject.tag.Equals("Projectile"))
		{
			TakeDamage();
			Destroy(other.gameObject);
		} else if (other.gameObject.tag.Equals("Health"))
		{
			if (HP < MaxHP)
			{
				Heal();
				Destroy(other.gameObject);
			}
		}
	}
}
