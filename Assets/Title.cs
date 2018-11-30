using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Title : MonoBehaviour {

    public GameObject CrawlText;
    public Image TitleImage;
    public Text LongTime;
    public AudioSource Music;

    public float CrawlRate;

	void Start () {
        LongTime.color = new Color(LongTime.color.r, LongTime.color.g, LongTime.color.b, 0);
        TitleImage.color = new Color(1, 1, 1, 0);
        Music.PlayDelayed(2.5f);
	}

    void Update() {


        if (Time.time < 3)
        {
            LongTime.color = new Color(LongTime.color.r, LongTime.color.g, LongTime.color.b, Mathf.Min(1, LongTime.color.a + 0.01f));
        } else
        {
            LongTime.color = new Color(LongTime.color.r, LongTime.color.g, LongTime.color.b, LongTime.color.a - 0.01f);
        }

        if (Time.time < 5)
        {
            return;
        }


        float effectiveCrawlRate = CrawlRate;
        if (Input.GetKey(KeyCode.Escape))
        {
            effectiveCrawlRate *= 4;
        }
        CrawlText.transform.Translate(Vector3.up * effectiveCrawlRate);
        TitleImage.transform.Translate(Vector3.forward * effectiveCrawlRate * 5);

        if (TitleImage.transform.position.z > 900)
        {
            TitleImage.color = new Color(1, 1, 1, TitleImage.color.a - 0.1f * effectiveCrawlRate);
        } else
        {
            TitleImage.color = Color.white;
        }

        if (CrawlText.transform.position.z > 1300)
        {
            Music.volume -= 0.0035f;
        }

        if (CrawlText.transform.position.z > 1400)
        {
            SceneManager.LoadScene("MainScene", LoadSceneMode.Single);
        }
	}
}
