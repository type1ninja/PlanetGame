using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HealthUI : MonoBehaviour {
	CharacterHealth healthScript;
	Slider healthSlider; 

	void Start() {
		healthScript = GetComponent<CharacterHealth> ();
		healthSlider = GameObject.Find ("HUDCanvas").transform.Find("StatsPanel").Find("PlayerHealthBar").GetComponent<Slider> ();
	}

	void Update() {
		healthSlider.value = healthScript.GetHealth ();
		healthSlider.maxValue = healthScript.GetMaxHealth();
	}
}