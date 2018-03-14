using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetBall : MonoBehaviour {

	public GameObject bola;
	public GameObject pemain;

	private bool holdBall = true;


	void Start (){
		bola.GetComponent<Rigidbody> ().useGravity = false; 
	}

	void Update (){
		if (holdBall) {
			bola.transform.position = pemain.transform.position + pemain.transform.forward*0.8f;
			if (Input.GetMouseButtonDown (0)) {
				holdBall = false;
				bola.GetComponent<Rigidbody> ().useGravity = true;
			}
		}

	}
}