using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{

	public GameObject WarpEffect;
	public GameObject FlashPanel;
	public GameObject Background;

	public string[] Levels;

	private int level = -1;


	private float WarpTime = -1;
	private GameObject warp = null;
	private float transSpeed;
	private float alpha;
	private float speed;
	private float offset;
	private float shake;
	private bool loaded;

	private Vector3 originalCameraPos;
	private Vector3 originalPos;
	private HashSet<GameObject> objectsToRemove;

	// Use this for initialization
	void Start()
	{
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
		loaded = false;


		objectsToRemove = new HashSet<GameObject>();
		foreach (GameObject o in GameObject.FindObjectsOfType(typeof(GameObject)))
		{
			GameObject root = o.transform.root.gameObject;
			if (root.layer != 5 && root.layer != 8)
			{
				objectsToRemove.Add(root);
			}
		}

		level++;
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
			Background.transform.Translate(-transSpeed, 0, 0);

			foreach (GameObject o in objectsToRemove)
			{
				if (o == null)
				{
					continue;
				}
				o.transform.position += new Vector3(-transSpeed, 0, 0);
			}

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
				Background.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1 - alpha);
				warp.GetComponent<ParticleSystem>().startSpeed = speed;
			}

			if (time > 8f)
			{
				FlashPanel.GetComponent<Image>().color = Color.Lerp(new Color(1, 1, 1, 0), new Color(1, 1, 1, 1), (time - 8) * 5);
			}

			if (time > 8.5f && !loaded)
			{
				loaded = true;
				string levelName = Levels[level];
				SceneManager.LoadScene(levelName, LoadSceneMode.Additive);
			}
		}
		else
		{
			if (warp != null)
			{
				Destroy(warp);
				warp = null;
				Background.transform.position = originalPos;
				shake = 0.1f;
				transSpeed = 20;
				Background.GetComponent<SpriteRenderer>().color = Color.white;
				foreach (GameObject o in objectsToRemove)
				{
					Destroy(o, 5);
				}
				objectsToRemove.Clear();
			}

			shake = Mathf.Max(0, shake - Time.deltaTime * 0.1f);
			transSpeed = Mathf.Max(0, transSpeed * 0.95f);

			Background.transform.Translate(-transSpeed, 0, 0);

			FlashPanel.GetComponent<Image>().color = Color.Lerp(new Color(1, 1, 1, 1), new Color(1, 1, 1, 0), (time - 9) * 5);
			Camera.main.transform.localPosition = originalCameraPos + Random.insideUnitSphere * shake;

			if (time > 11)
			{
				WarpTime = -1;
				string levelName = Levels[level];
				foreach (GameObject o in SceneManager.GetSceneByName(levelName).GetRootGameObjects())
				{
					LevelController levelController = o.GetComponent<LevelController>();
					if (levelController != null)
					{
						levelController.LevelReady();
					}
				}
			}
		}
	}
}
