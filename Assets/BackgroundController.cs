using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BackgroundController : MonoBehaviour {

	public GameObject WarpEffect;
	public GameObject FlashPanel;


	private float WarpTime = -1;
	private GameObject warp = null;
	private float transSpeed;
	private float alpha;
	private float speed;
	private float offset;
	private float shake;

	private Vector3 originalCameraPos;
	private Vector3 originalPos;

	// Use this for initialization
	void Start () {
		originalCameraPos = Camera.main.transform.localPosition;
		originalPos = transform.position;
	}


	public void Warp()
	{
		WarpTime = Time.time;
		alpha = 0;
		speed = 20;
		offset = 10;
		shake = 0;
		transSpeed = 0;
	}
	// Update is called once per frame
	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Space))
		{
			Warp();
		}

		if (WarpTime < 0)
		{
			return;
		}

		float time = Time.time - WarpTime;

		if (time < 9f)
		{
			transSpeed = Time.deltaTime * Mathf.Pow(time, 3);
			transform.Translate(-transSpeed, 0, 0);

			if (time > 3.5f)
			{
				shake += Time.deltaTime * 0.05f;
				Camera.main.transform.localPosition = originalCameraPos + Random.insideUnitSphere * shake;
			}


			if (time > 4f)
			{
				if (warp == null)
				{
					warp = Instantiate(WarpEffect);
				}

				alpha = Mathf.Min(1, alpha + Time.deltaTime * 0.5f);
				speed += Time.deltaTime * 10;
				offset += Time.deltaTime * 10;

				warp.transform.position = new Vector3(offset, 0, 0);
				warp.GetComponent<ParticleSystem>().startColor = new Color(1, 1, 1, alpha);
				GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1 - alpha);
				warp.GetComponent<ParticleSystem>().startSpeed = speed;
			}

			if (time > 8f)
			{
				FlashPanel.GetComponent<Image>().color = Color.Lerp(new Color(1, 1, 1, 0), new Color(1, 1, 1, 1), (time - 8) * 5);
			}
		} else
		{
			if (warp != null)
			{
				Destroy(warp);
				warp = null;
				transform.position = originalPos;
				shake = 0.1f;
				transSpeed = 20;
				GetComponent<SpriteRenderer>().color = Color.white;
			}

			shake = Mathf.Max(0, shake - Time.deltaTime * 0.1f);
			transSpeed = Mathf.Max(0, transSpeed * 0.95f);

			transform.Translate(-transSpeed, 0, 0);

			FlashPanel.GetComponent<Image>().color = Color.Lerp(new Color(1, 1, 1, 1), new Color(1, 1, 1, 0), (time - 9) * 5);
			Camera.main.transform.localPosition = originalCameraPos + Random.insideUnitSphere * shake;

			if (time > 11)
			{
				WarpTime = -1;
			}
		}
	}
}
