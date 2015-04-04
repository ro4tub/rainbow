using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {
	private Vector3 newPosition;
	private Transform player;
	// Use this for initialization
	void Start () {
		GameObject obj = GameObject.FindGameObjectWithTag ("Player");
		if (obj != null) {
			player = obj.transform;
			newPosition = player.position;
			newPosition.z = transform.position.z;
			transform.position = newPosition;
		}
	}
	
	// Update is called once per frame
	void Update () {
		newPosition = player.position;
		newPosition.z = transform.position.z;
		transform.position = newPosition;
	}
}
