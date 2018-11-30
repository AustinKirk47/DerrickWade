using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exploder : MonoBehaviour {

    public GameObject Explosion;

    private bool isQuitting;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnApplicationQuit()
    {
        isQuitting = true;
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
        if (!isQuitting)
        {
            Explode();
        }
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
