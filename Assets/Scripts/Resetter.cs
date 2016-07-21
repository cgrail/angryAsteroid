using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class Resetter : MonoBehaviour {

	public Rigidbody2D projectile;

	public float resetSpeed = 0.025f;

	private float resetSpeedSqr;
	private SpringJoint2D spring;

	// Use this for initialization
	void Start () {
		resetSpeedSqr = resetSpeed * resetSpeed;
		spring = projectile.GetComponent<SpringJoint2D> ();
	}

	void Update () {

		if (Input.GetKey (KeyCode.R)) {
			Reset ();
		}

		if (spring == null && projectile.velocity.sqrMagnitude < resetSpeedSqr) {
			Reset ();
		}
	
	}

	void OnTriggerExit2D (Collider2D other) {
		if (other.GetComponent<Rigidbody2D>() == projectile) {
			Reset ();
		}	

	}

	void Reset () {
		SceneManager.LoadScene (SceneManager.GetActiveScene().buildIndex);
	}
}
