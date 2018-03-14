using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lockRotateSlider : MonoBehaviour {



	void Update()
	{
		Vector3 eulerAngles = transform.rotation.eulerAngles;
		eulerAngles = new Vector3(0, 0, 0);
		transform.rotation = Quaternion. Euler(eulerAngles);
	}
}
