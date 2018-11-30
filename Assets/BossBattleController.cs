using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BossBattleController : MonoBehaviour {

    public GameObject BossBody;
    public GameObject Explosion;
    public TextMeshProUGUI WinnerText;

    bool dead = false;
    bool won = false;

	void Start () {
		
	}
	
	void Update () {
        Debug.Log(GameObject.FindGameObjectsWithTag("Enemy").Length);
		if (!dead && GameObject.FindGameObjectsWithTag("Enemy").Length == 0)
        {
            Debug.Log("TEST");
            dead = true;
            BossBody.GetComponent<Collider2D>().enabled = true;
            BossBody.AddComponent<Exploder>().Explosion = Explosion;
        }

        if (!won && BossBody == null)
        {
            won = true;
            WinnerText.enabled = true;
        }
	}
}
