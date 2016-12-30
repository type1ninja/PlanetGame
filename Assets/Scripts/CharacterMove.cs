using UnityEngine;
using System.Collections;

//Taken from the Unity3D documention
//http://docs.unity3d.com/ScriptReference/CharacterController.Move.html
//Some slight modifications were made for this game
public class CharacterMove : MonoBehaviour {

	CharacterController controller;

	float speed = 10.0f;
	float jumpSpeed = 5.0f;
	float gravity = Physics.gravity.y;

	Vector3 moveDirection = Vector3.zero;

	bool canMove = true;

	void Start() {
		controller = GetComponent<CharacterController>();
	}

	void FixedUpdate() {

		if (controller.isGrounded) {
			moveDirection = GetInput ();
			moveDirection = transform.TransformDirection(moveDirection);
			moveDirection *= speed;
			//If the character is moving diagonally, divide magnitude by 1.4 to prevent huge speed buffs from diagonal walking
			if (moveDirection.x != 0 && moveDirection.z != 0) {
				moveDirection /= 1.4f;
			}
			if (GetJump()) {
				moveDirection.y = jumpSpeed;
			}
		}

		//ADD gravity because it's negative
		moveDirection.y += gravity * Time.deltaTime;

		controller.Move(moveDirection * Time.deltaTime);
	}

	public void StopMotion() {
		moveDirection = Vector3.zero;
	}

	//Enemies can add knockback by basically adding a vector to the player's move, then lifting them into the air
	//Then, since the player can't move around in midair, they've taken knockback
	//Force isn't really a force, it's more like a new move direction
	public void AddKnockback(Vector3 force) {
		controller.Move(new Vector3(0, .03f, 0));
		//add force to the motion
		moveDirection += force;
	}

	public Vector3 GetMoveDir() {
		return moveDirection;
	}

	private Vector3 GetInput() { 
		if (canMove) {
			return new Vector3 (Input.GetAxis ("Horizontal"), 0, Input.GetAxis ("Vertical"));
		} else {
			return Vector3.zero;
		}
	}
	private bool GetJump() {
		if (canMove) {
			return Input.GetButton ("Jump");
		} else {
			return false;
		}
	}
}