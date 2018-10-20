using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Linq;


public class throwBalok : MonoBehaviour
{
	public float speed = 100f;
	Transform pemutarMeriam;
	public Transform hands, hands1, hands2, hands3, hands4;
	GameObject balok, balok1, balok2, balok3, balok4;
	float time = 0.0f;
	float countdownt = 3f;
	float interpolationPeriod;
	float inactivePeriod;
	public int a = 0;
	public GameObject countdown;
	TextMesh hitungMundur;

	void OnTriggerEnter (Collider other)
	{
		if (other.tag == "bola" ) 
		{
			a = 1;
		}

	}

	void Start()
	{
		hitungMundur = countdown.GetComponent<TextMesh> ();
		countdown.SetActive (false);
		interpolationPeriod = 3f;
		inactivePeriod = interpolationPeriod + 5f;
		
		pemutarMeriam = gameObject.transform.Find("pemutar");
		balok = GameObject.FindGameObjectWithTag ("cube");
		balok1 = GameObject.FindGameObjectWithTag ("cube1");
		balok2 = GameObject.FindGameObjectWithTag ("cube2");
		balok3 = GameObject.FindGameObjectWithTag ("cube3");
		balok4 = GameObject.FindGameObjectWithTag ("cube4");

		balok.GetComponent<Rigidbody> ().isKinematic = true;
		balok.GetComponent<Rigidbody> ().useGravity = false;

		balok1.GetComponent<Rigidbody> ().isKinematic = true;
		balok1.GetComponent<Rigidbody> ().useGravity = false;

		balok2.GetComponent<Rigidbody> ().isKinematic = true;
		balok2.GetComponent<Rigidbody> ().useGravity = false;

		balok3.GetComponent<Rigidbody> ().isKinematic = true;
		balok3.GetComponent<Rigidbody> ().useGravity = false;

		balok4.GetComponent<Rigidbody> ().isKinematic = true;
		balok4.GetComponent<Rigidbody> ().useGravity = false;
						
	}
		

	void Update()
	{
		pemutarMeriam.Rotate (Vector3.up, speed*Time.deltaTime);

		if (a == 1) {
			countdown.SetActive (true);
			time += Time.deltaTime;
			countdownt -= Time.deltaTime;
			hitungMundur.text = Mathf.RoundToInt(countdownt).ToString();
		}
			

		if (time >= interpolationPeriod) {
			countdown.SetActive (false);
			balok.transform.parent = null;
			balok.GetComponent<Rigidbody> ().isKinematic = false;
			balok.GetComponent<Rigidbody> ().useGravity = true;
			balok.GetComponent<Rigidbody> ().AddForce (hands.transform.forward * 1f);


			balok1.transform.parent = null;
			balok1.GetComponent<Rigidbody> ().isKinematic = false;
			balok1.GetComponent<Rigidbody> ().useGravity = true;
			balok1.GetComponent<Rigidbody> ().AddForce (hands1.transform.forward * 1f);


			balok2.transform.parent = null;
			balok2.GetComponent<Rigidbody> ().isKinematic = false;
			balok2.GetComponent<Rigidbody> ().useGravity = true;
			balok2.GetComponent<Rigidbody> ().AddForce (hands2.transform.forward * 1f);


			balok3.transform.parent = null;
			balok3.GetComponent<Rigidbody> ().isKinematic = false;
			balok3.GetComponent<Rigidbody> ().useGravity = true;
			balok3.GetComponent<Rigidbody> ().AddForce (hands3.transform.forward * 0.8f);


			balok4.transform.parent = null;
			balok4.GetComponent<Rigidbody> ().isKinematic = false;
			balok4.GetComponent<Rigidbody> ().useGravity = true;
			balok4.GetComponent<Rigidbody> ().AddForce (hands4.transform.forward * 0.5f);


		}

		if (time >= inactivePeriod) {
			
			if (GameObject.FindWithTag ("attacker") == null) {
				balok.SetActive (false);
			}
			if (GameObject.FindWithTag ("attacker1") == null) {
				balok1.SetActive (false);
			}
			if (GameObject.FindWithTag ("attacker2") == null) {
				balok2.SetActive (false);
			}
			if (GameObject.FindWithTag ("attacker3") == null) {
				balok3.SetActive (false);
			}
			if (GameObject.FindWithTag ("attacker4") == null) {
				balok4.SetActive (false);
			}
			this.gameObject.SetActive (false);
		}


	}
		

}