using UnityEngine;
using System.Collections;

public class CharacterHealth : MonoBehaviour {
	
	//CharacterMove move; TODO - REPLACE WITH RELEVANT SUPERCHARACTERCONTROLLER THINGY

	float maxHealth = 100f;
	float health;
	//Optional regen
	//float healthRegen = 5.0f;

	void Start()
    {
        //TODO - SCC REPLACEMENT
		//move = GetComponent<CharacterMove> ();
		health = maxHealth;
	}

	void FixedUpdate()
    {
		if (health > maxHealth)
        {
			health = maxHealth;
		}
        else if (health < 0)
        {
			Die ();
		}
		//Regen (optional)
		//ModHealth(healthRegen * Time.fixedDeltaTime);
	}

	//Damage is inputted as a negative number
	//and Regen as a positive number
	public void ModHealth(float diff)
    {
		health += diff;
	}
	
	void Die()
    {
		transform.position = new Vector3 (0, 5, 0);
		health = maxHealth;
        //TODO - STOP MOTION ON DEATH
	}

	public float GetMaxHealth()
    {
		return maxHealth;
	}
	public float GetHealth()
    {
		return health;
	}
}