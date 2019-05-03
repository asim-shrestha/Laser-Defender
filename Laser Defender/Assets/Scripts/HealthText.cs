using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HealthText : MonoBehaviour
{
	public void UpdateHealth(int health) {
		GetComponent<TextMeshProUGUI>().text = health.ToString();
	}
}
