using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : MonoBehaviour {

    public GameObject Explosion;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnDestroy()
    {
        GetComponentInChildren<TrailRenderer>().transform.SetParent(null, true);
        GameObject splode = Instantiate(Explosion, transform.position, Quaternion.identity);
        Destroy(splode, splode.GetComponent<ParticleSystem>().duration);
    }
}
