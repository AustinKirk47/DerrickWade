using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

	void Start () {
	}
	
	void Update () {
		Bounds bounds = new Bounds();

		GameObject player = GameObject.FindGameObjectWithTag("Player");
		bounds.Encapsulate(player.transform.position);

		GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
		foreach (GameObject enemy in enemies)
		{
			bounds.Encapsulate(enemy.transform.position);
		}

		//camera.transform.position = bounds.center;
		//camera.orthographicSize = Mathf.Max(bounds.extents.x, bounds.extents.y) * 1.5f;
	}
}
