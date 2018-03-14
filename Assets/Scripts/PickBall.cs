using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickBall : MonoBehaviour {

	public GameObject BallOnHand;
	public GameObject BallOnGround;
	//public GameObject pemain;
	//public float BallThrowingForce = 5f;
	//private bool holdBall = true;

	// Use this for initialization
	void Start () {
		BallOnHand.SetActive(false);
		BallOnHand.GetComponent<Rigidbody> ().useGravity = false; 
	}

	void OnTriggerEnter(Collider _collider)
	{
		if(_collider.gameObject.tag == "Player")
		{
			BallOnHand.SetActive(true);
			BallOnGround.SetActive(false);
			//BallOnHand.transform.position = pemain.transform.position + pemain.transform.forward*0.8f;
		}
			
	}
		
}
