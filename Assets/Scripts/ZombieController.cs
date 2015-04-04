using UnityEngine;
using System.Collections;



public class ZombieController : MonoBehaviour {
	public float moveSpeed;
	public float turnSpeed;
	private Vector3 moveDirection;
	enum CONTROL_STATE {
		STATE_NOT_IN_CONTROL = 1, // not control
		STATE_IN_CONTROL = 2, // control by touch
	}
	enum GAME_STATE {
		GAME_STATE_IDLE = 1,
		GAME_STATE_PAUSE = 2,
		GAME_STATE_OVER = 3,
	}
	private CONTROL_STATE state = CONTROL_STATE.STATE_NOT_IN_CONTROL;
	private GAME_STATE gameState = GAME_STATE.GAME_STATE_IDLE;
	private Vector3 lastTouchPos;




	// Use this for initialization
	void Start () {
		moveDirection = Vector3.right;
		Debug.LogFormat ("touchSupported={0}, simulateMouseWithTouches={1}\n", Input.touchSupported, Input.simulateMouseWithTouches);
	}
	
	// Update is called once per frame
	void Update () {
		if (gameState != GAME_STATE.GAME_STATE_IDLE)
			return;

		// 1
		Vector3 currentPosition = transform.position;
		// 2
		if (state == CONTROL_STATE.STATE_IN_CONTROL && Input.GetMouseButton (0) == true) {
			Vector3 curTouchPos = Camera.main.ScreenToWorldPoint( Input.mousePosition );
			moveDirection = curTouchPos - lastTouchPos;
			moveDirection.z = 0; 
			moveDirection.Normalize();
		} else if(state == CONTROL_STATE.STATE_NOT_IN_CONTROL) {
			// not controlled state, forward
			moveDirection.x = 1;
			moveDirection.y = 0;
			moveDirection.z = 0;
			moveDirection.Normalize();
		}
		Vector3 target = moveDirection * moveSpeed + currentPosition;
		transform.position = Vector3.Lerp( currentPosition, target, Time.deltaTime );

		float targetAngle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg;
		transform.rotation = 
			Quaternion.Slerp( transform.rotation, 
			                 Quaternion.Euler( 0, 0, targetAngle ), 
			                 turnSpeed * Time.deltaTime );

		if (Input.GetMouseButtonDown (0)) {
			Debug.Log ("touch began");
			state = CONTROL_STATE.STATE_IN_CONTROL;
			lastTouchPos = Camera.main.ScreenToWorldPoint( Input.mousePosition );
		} else if (Input.GetMouseButtonUp (0)) {
			Debug.Log ("touch ended");
			state = CONTROL_STATE.STATE_NOT_IN_CONTROL;
		}

//		EnforceBounds ();
	}
	
	private void EnforceBounds() {
		Vector3 newPosition = transform.position;
		Camera mainCamera = Camera.main;
		Vector3 cameraPosition = mainCamera.transform.position;

		float xDist = mainCamera.aspect * mainCamera.orthographicSize;
		float xMax = cameraPosition.x + xDist;
		float xMin = cameraPosition.x - xDist;

		if (newPosition.x < xMin || newPosition.x > xMax) {
			newPosition.x = Mathf.Clamp (newPosition.x, xMin, xMax);
			moveDirection.x = -moveDirection.x;
		}
		transform.position = newPosition;
	}

	void OnTriggerEnter2D( Collider2D other ) {
		Debug.Log ("Hit " + other.gameObject);

		GameObject gameEndUI = Resources.Load ("GameEndUI") as GameObject;
		if (gameEndUI == null) {
			Debug.LogError ("not found GameEndUI prefab");
			return;
		}
		gameState = GAME_STATE.GAME_STATE_OVER;
//		Time.timeScale = 0; // FIXME !!! important !!!
		Instantiate(gameEndUI);
	}
}
