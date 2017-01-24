using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

	public Animator			animator;
	public Rigidbody2D		rigidbodyPlayer;

	public bool				facingRight;
	public bool				walk;
	public bool				canMove;
	public GameObject   powerrepel;
	public AreaEffector2D areaeffector_pr;
	public GameObject   powerattract;
	public AreaEffector2D areaeffector_pa;
	public GameObject   powerupper;
	public AreaEffector2D areaeffector_pu;
	private float			moveX;
	private float			currTime;
	private bool			alarm_prOn;
	private float			alarm_pr;
	private bool			alarm_paOn;
	private float			alarm_pa;
	private bool			alarm_puOn;
	private float			alarm_pu;
	private bool			alarm_prOn2;
	private float			alarm_pr2;
	private bool			alarm_paOn2;
	private float			alarm_pa2;
	private bool			alarm_puOn2;
	private float			alarm_pu2;

	public float attractTime;
	private bool			key_pr_can_on;
	private bool			key_pa_can_on;
	private bool			key_pu_can_on;
	public float 			maxSpeed;

	void Start () {
		animator = GetComponent<Animator> ();
		facingRight = true;
		canMove = true;
		currTime = 0;
		alarm_prOn = false;
		alarm_pr = 0;
		alarm_paOn = false;
		alarm_pa = 0;
		alarm_puOn = false;
		alarm_pu = 0;

		key_pr_can_on=true;
		key_pa_can_on=true;
		key_pu_can_on=true;
		areaeffector_pr = powerrepel.GetComponent<AreaEffector2D> ();
		areaeffector_pa = powerattract.GetComponent<AreaEffector2D> ();
		areaeffector_pu = powerupper.GetComponent<AreaEffector2D> ();
	}

	void Update () {
		currTime =currTime+ 1*Time.deltaTime;
		animator.SetBool ("walk", walk);
		rigidbodyPlayer.velocity = new Vector2 (moveX * maxSpeed, rigidbodyPlayer.velocity.y);
	
		if (canMove == true) {
			moveX = Input.GetAxis ("Horizontal");
			if (moveX != 0) {
				transform.SetParent (null);
			}

			if (moveX > 0 && !facingRight) { 
				Flip ();
			} else if (moveX < 0 && facingRight) {
				Flip ();
			}

//Set movement true or false
			if (moveX != 0) { 
				walk = true;
			} else {
				walk = false;
			}
		}
		//power reppel
		if(Input.GetKey (KeyCode.L) && key_pr_can_on) {
			if (areaeffector_pr.enabled == false) {
				areaeffector_pr.enabled = true;
				alarm_pr = currTime + 1;
				alarm_prOn = true;
				key_pr_can_on = false;
				
			}
			Debug.Log ("key l");

		}
		//power upper
		if(Input.GetKey (KeyCode.I) && key_pu_can_on) {
			if (areaeffector_pu.enabled == false) {
				areaeffector_pu.enabled = true;
				alarm_pu = currTime + 1;
				alarm_puOn = true;
				key_pu_can_on = false;

			}
			Debug.Log ("key i");

		}
		//power attract
		if(Input.GetKey (KeyCode.J) && key_pa_can_on) {
			if (areaeffector_pa.enabled == false) {
				areaeffector_pa.enabled = true;
				alarm_pa = currTime + attractTime;
				alarm_paOn = true;
				key_pa_can_on = false;

			}
			Debug.Log ("key j");

		}
		if (currTime > alarm_pr && alarm_prOn) {
			alarm_prOn = false;
			areaeffector_pr.enabled = false;
			alarm_pr2 = currTime + 2;
			alarm_prOn2 = true;

		}
		if (currTime > alarm_pa && alarm_paOn) {
			alarm_paOn = false;
			areaeffector_pa.enabled = false;
			alarm_pa2 = currTime + 2;
			alarm_paOn2 = true;

		}
		if (currTime > alarm_pu && alarm_puOn) {
			alarm_puOn = false;
			areaeffector_pu.enabled = false;
			alarm_pu2 = currTime + 2;
			alarm_puOn2 = true;

		}
		if (currTime > alarm_pr2 && alarm_prOn2) {
			alarm_prOn2 = false;
			areaeffector_pr.enabled = false;
			key_pr_can_on = true;

		}
		if (currTime > alarm_pa2 && alarm_paOn2) {
			alarm_paOn2 = false;
			areaeffector_pa.enabled = false;
			key_pa_can_on = true;

		}
		if (currTime > alarm_pu2 && alarm_puOn2) {
			alarm_puOn2 = false;
			areaeffector_pu.enabled = false;
			key_pu_can_on = true;

		}
	}

// Flipping sprites
	void Flip () {
		facingRight = !facingRight;
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}
}