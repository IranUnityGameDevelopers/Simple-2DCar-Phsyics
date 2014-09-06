using UnityEngine;
using System.Collections;

public class CarMovement : MonoBehaviour {
	public float acceleration = 1f;
	public float deceleration = 0.5f;
	public float maxSteerSpeed = 2f;
	public float brakeSpeed = 4f;
	private float curSteerSpeed = 0f;
	private Vector3 angle = Vector3.zero;
	private Vector2 carNormal;
	private float speed = 0f;
	private Quaternion rotate = Quaternion.identity;

	void Start(){
		angle.z = transform.rotation.eulerAngles.z;
	}
	
	void Update(){
		carNormal = new Vector2(Mathf.Sin((-angle.z) * Mathf.Deg2Rad), Mathf.Cos((-angle.z) * Mathf.Deg2Rad));
		rotate.eulerAngles = angle;
		transform.rotation = rotate;
		//Debug.Log ("["+ (-angle.z)+"] "+"("+carNormal.x + ", " + carNormal.y + ")");

		//Forward and Backward Motion
		if (Input.GetAxis ("Vertical") > 0) {
			if(curSteerSpeed < maxSteerSpeed)
				curSteerSpeed += 0.035f;

			if (speed < 0) //This will make changing direction better and easier
				speed += acceleration/15;
			else
				speed += acceleration/30; 

			rigidbody2D.velocity = carNormal * speed;;
		}
		else if (Input.GetAxis ("Vertical") < 0) {
			if(speed > 0){
				speed -= brakeSpeed/30;
				if(curSteerSpeed > 0)
					curSteerSpeed -= 0.03f;
			}
			else{
				speed -= deceleration/30; 
				if(curSteerSpeed < maxSteerSpeed)
					curSteerSpeed += 0.01f;
			}
			rigidbody2D.velocity = carNormal * speed;
		}
		else if (Input.GetAxis ("Vertical") == 0) {
			if(curSteerSpeed > 0)
				curSteerSpeed -= 0.02f;
			if (speed > 0){
				speed -= deceleration/10;
				if (speed < 0.01)
					speed = 0;
			}
			else if (speed < 0){
				speed += deceleration/10;
				if (speed > -0.01)
					speed = 0;
			}
			rigidbody2D.velocity = carNormal*speed;
		}
		if (speed < 0.1 && speed > -0.1)
			curSteerSpeed = 0;
		//Steering Motion
		if (speed!=0){
			if (Input.GetAxis ("Horizontal") > 0) {
				if(speed > 0)
					angle += new Vector3(0,0,-curSteerSpeed);
				else if(speed <0)
					angle += new Vector3(0,0,curSteerSpeed);
				rotate.eulerAngles = angle;
				transform.rotation = rotate;
			}
			else if (Input.GetAxis ("Horizontal") < 0) {
				if(speed > 0)
					angle += new Vector3(0,0,+curSteerSpeed);
				if(speed < 0)
					angle += new Vector3(0,0,-curSteerSpeed);
				rotate.eulerAngles = angle;
				transform.rotation = rotate;
			}
		}
	}

	void OnCollisionEnter2D(Collision2D collider){
		if(collider.gameObject.tag != null){
			speed = -speed * 0.3f;
			rigidbody2D.angularVelocity=0;
		}
	}

}