using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameEndUI : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Button btn = gameObject.GetComponent<Button> ();
		if (btn == null) {
			Debug.LogError ("not found Button");
			return;
		}
		btn.onClick.AddListener (OnClick);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnClick() {
		Debug.Log ("GameEndUI OnClick");
		Application.LoadLevel ("CongaScene");
	}
}
