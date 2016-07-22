using UnityEngine;
using System.Collections;

public class TargetDamage : MonoBehaviour {

	public int hitPoints = 2;
	public Sprite damageSprite;
	public float damageImpactSpeed;

	private int currentHitPoints;
	private float damageImpactSpeedSqr;
	private SpriteRenderer spriteRenderer;
	private Collider2D myCollider2D;
	private Rigidbody2D myRigidBody2D;

	// Use this for initialization
	void Start () {
		myCollider2D = GetComponent<Collider2D> ();
		myRigidBody2D = GetComponent<Rigidbody2D> ();
		spriteRenderer = GetComponent<SpriteRenderer> ();
		currentHitPoints = hitPoints;
		damageImpactSpeedSqr = damageImpactSpeed * damageImpactSpeed;
	}

	void OnCollisionEnter2D (Collision2D collision) {
		if (collision.collider.tag != "Damager") {
			return;
		}
		if (collision.relativeVelocity.sqrMagnitude < damageImpactSpeedSqr) {
			return;
		}

		spriteRenderer.sprite = damageSprite;

		currentHitPoints--;

		if (currentHitPoints <= 0) {
			Kill ();
		}
	}

	void Kill () {
		spriteRenderer.enabled = false;
		myCollider2D.enabled = false;
		myRigidBody2D.isKinematic = true;
	}
}
