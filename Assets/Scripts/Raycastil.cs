using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Raycastil : MonoBehaviour {

	public GameObject raycastObject;

	void Update(){



		Vector3 fwd = raycastObject.transform.TransformDirection(Input.mousePosition);
		//Plane GroundPlane = new Plane (Vector3.up, Vector3.zero);
		RaycastHit objectHit;

		if (Physics.Raycast (transform.position, fwd, out objectHit)) {

			Debug.DrawLine (raycastObject.transform.position, fwd, Color.red);

			transform.LookAt (new Vector3(fwd.x, transform.position.y, fwd.z));
		}
			

	}

}
