using UnityEngine;
using UnityEngine.UI;
using System.Collections;

//Place on an EMPTY containing a SLIDER and a TEXT BOX, with the EMPTY being a CHILD of the thing the health bar should float over
public class FloatingHealth : MonoBehaviour {

	Vector3 offset = new Vector3 (0, 2.5f, 0);
	string charName = "Generic Name";

	Transform character;
	CharacterHealth healthScript;
	Slider slider; 
	Text text;

	void Start() {
		healthScript = GetComponentInParent<CharacterHealth> ();
		//Store the actual character
		character = transform.parent;
		//Set the actual parent to be the canvas; if it stayed with the character
		//it would wiggle all over
		transform.SetParent (GameObject.Find ("EnemyHealthCanvas").transform);

		slider = GetComponentInChildren<Slider> ();
		text = GetComponentInChildren<Text> ();
		text.text = charName;
	}

	void Update() {
		if (CheckIfVisible(character.position + offset)) {
			slider.gameObject.SetActive (true);
			text.gameObject.SetActive (true);

			//TODO - make it fade/resize as you go farther away 
			//(also maybe change vertical offset?)
			transform.position = Camera.main.WorldToScreenPoint (character.position + offset);

			slider.value = healthScript.GetHealth ();
		} else {
			slider.gameObject.SetActive (false);
			text.gameObject.SetActive (false);
		}
	}

	bool CheckIfVisible(Vector3 pos) {
		pos = Camera.main.WorldToViewportPoint (pos);

		if (pos.x > 0 && pos.x < 1 
			&& pos.y > 0 && pos.y < 1
			&& pos.z > 0) {
			return true;
		} else {
			return false;
		}
	}
}