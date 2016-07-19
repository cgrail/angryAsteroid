using UnityEngine;
using System.Collections;

public class ProtectileDragging : MonoBehaviour {

	public float maxStretch = 3.0f;
	public LineRenderer catapultLineFront;
	public LineRenderer catapultLineBack;

	private SpringJoint2D spring;
	private Rigidbody2D ball;
	private Ray rayToMouse;
	private Ray leftCatapultToProjectile;
	private float maxStretchSqr;
	private float circleRadius;
	private Transform catapult;

	private Vector2 prevVelocicty;

	private bool clickedOn;

	void Awake () {
		spring = GetComponent <SpringJoint2D> ();
		ball = GetComponent <Rigidbody2D> ();
		catapult = spring.connectedBody.transform;
	}

	// Use this for initialization
	void Start () {
		LineRendererSetup();
		rayToMouse = new Ray (catapult.position, Vector3.zero);
		leftCatapultToProjectile = new Ray (catapultLineFront.transform.position, Vector3.zero);
		maxStretchSqr = maxStretch * maxStretch;
		CircleCollider2D circle = GetComponent <CircleCollider2D> ();
		circleRadius = circle.radius;
	}
	
	// Update is called once per frame
	void Update () {
		if (clickedOn)
			Dragging ();

		if (spring != null) {
			if (!ball.isKinematic && prevVelocicty.sqrMagnitude > ball.velocity.sqrMagnitude) {
				Destroy (spring);
				ball.velocity = prevVelocicty;
			}

			if (!clickedOn) {
				prevVelocicty = ball.velocity;
			}
			LineRendererUpdate ();
		}
	}

	void LineRendererSetup() {
		catapultLineFront.SetPosition (0, catapultLineFront.transform.position);
		catapultLineBack.SetPosition (0, catapultLineBack.transform.position);

		catapultLineBack.sortingLayerName = "Foreground";
		catapultLineFront.sortingLayerName = "Foreground";

		catapultLineFront.sortingOrder = 3;
		catapultLineBack.sortingOrder = 1;

	}

	void OnMouseDown () {
		spring.enabled = false;
		clickedOn = true;
	}

	void OnMouseUp () {
		spring.enabled = true;
		ball.isKinematic = false;
		clickedOn = false;
	}

	void Dragging () {
		Vector3 mouseWorldPoint = Camera.main.ScreenToWorldPoint (Input.mousePosition);
		Vector2 catapultToMouse = mouseWorldPoint - catapult.position;

		if (catapultToMouse.sqrMagnitude > maxStretchSqr) {
			rayToMouse.direction = catapultToMouse;
			mouseWorldPoint = rayToMouse.GetPoint (maxStretch);
		}

		mouseWorldPoint.z = 0f;
		transform.position = mouseWorldPoint;
	}

	void LineRendererUpdate () {
		Vector2 catapultToProjectile = transform.position - catapultLineFront.transform.position;
		leftCatapultToProjectile.direction = catapultToProjectile;
		Vector3 holdPoint = leftCatapultToProjectile.GetPoint (catapultToProjectile.magnitude + circleRadius);
		catapultLineBack.SetPosition (1, holdPoint);
		catapultLineFront.SetPosition (1, holdPoint);

	}

}
