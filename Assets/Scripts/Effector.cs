using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effector : MonoBehaviour
{
	[SerializeField] [Range(-5.0f, 5.0f)] float m_strength = 1.0f;
	[SerializeField] Water m_water = null;

	void Update()
	{
		if (Input.GetButton("Fire1"))
		{
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			m_water.Touch(ray, m_strength);
		}
	}
}
