using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exploder : MonoBehaviour {

    public GameObject Explosion;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void Explode()
    {
        TrailRenderer trail = GetComponentInChildren<TrailRenderer>();
        if (trail != null)
        {
            trail.transform.SetParent(null, true);
        }
        GameObject splode = Instantiate(Explosion, transform.position, Quaternion.identity);
        Destroy(splode, splode.GetComponent<ParticleSystem>().duration);
    }

    void OnDestroy()
    {
        Explode();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag.Equals("Projectile"))
        {
            Destroy(other.gameObject);
            Destroy(gameObject);
        }
    }
}
