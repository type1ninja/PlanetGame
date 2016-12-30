using UnityEngine;
using System.Collections;

public class CharacterHealth : MonoBehaviour {

	//Void damage per second
	float VOIDDMGPERSEC = -75;
	
	CharacterMove move;

	float maxHealth = 100f;
	float health;
	//Optional regen
	//float healthRegen = 5.0f;

	void Start() {
		move = GetComponent<CharacterMove> ();
		health = maxHealth;
	}

	void FixedUpdate() {

		//Void Death
		if (transform.position.y <= -4) {
			ModHealth(VOIDDMGPERSEC * Time.fixedDeltaTime);
		}

		if (health > maxHealth) {
			health = maxHealth;
		} else if (health < 0) {
			Die ();
		}

		//Regen (optional)
		//ModHealth(healthRegen * Time.fixedDeltaTime);
	}

	//Damage is inputted as a negative number
	//and Regen as a positive number
	public void ModHealth(float diff) {
		health += diff;
	}
	
	void Die() {
		transform.position = new Vector3 (0, 5, 0);
		health = maxHealth;
		move.StopMotion ();

	}

	public float GetMaxHealth() {
		return maxHealth;
	}
	public float GetHealth() {
		return health;
	}
}