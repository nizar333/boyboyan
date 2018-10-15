using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Linq;

[RequireComponent( typeof( PhotonView ) )]
public class atribut_bertahan : Photon.PunBehaviour {

	//atribut bertahan berisi perintah untuk mengambil/menangkap dan melempar bola
	public GameObject namaPemain;
	public GameObject pemain;
	GameObject a, b, d, e;
	GameObject bola;
	Transform c;
	//Transform hands;
	private Slider f;
	bool beingCarried = false;
	bool dekatBola = false;
	private bool touched = false;
	bool increasing = false;
	private bool shot = false;

	private float powerBarTreshold = 20f;
	private float powerBarValue = 0f;
	PhotonView pv;
	int ownerBola, playerID;
	float dist;
	//private bool MengambilBolaExecuted = false;

	void Start(){
		pv = this.GetComponent<PhotonView>();
		//pemain = GameObject.FindWithTag ("Player");
		bola = GameObject.FindWithTag ("bola");
		//namaPemain = pemain.transform.Find ("nama_player").gameObject;
		d = GameObject.Find ("target_tembak");
		d.SetActive (false);
		e = GameObject.Find ("Power Bar");
		e.SetActive (false);
		f = e.GetComponent<Slider> ();
		c = pemain.transform.Find ("hands");
	}
		

	void Update(){

		if (pv.isMine) {
			TextMesh namaPlayers = namaPemain.GetComponent<TextMesh> ();
			namaPlayers.text = PhotonNetwork.player.NickName;
		}
		if (!pv.isMine) {
			TextMesh namaPlayers = namaPemain.GetComponent<TextMesh> ();
			namaPlayers.text = pv.owner.NickName;
		}

		if (pv.isMine == false && PhotonNetwork.connected == true) {
			return;
		}
		ownerBola = bola.GetComponent<PhotonView> ().ownerId;
		MengambilBola (pemain, bola);
		MelemparBola ();
	}

	[PunRPC]
	void OwnerBola(){
		dist = 1.3f;
		ownerBola = b.GetComponent<PhotonView> ().ownerId;
		playerID = (ownerBola * 1000) + 1;
		a = PhotonView.Find (playerID).gameObject;
		c = a.transform.Find ("hands");
		b.GetComponent<Rigidbody> ().isKinematic = true;
		b.transform.parent = c;
		b.transform.position += (Vector3.up);
		beingCarried = true;
	}

	void MengambilBola(GameObject pemain, GameObject bola){
		a = pemain;
		b = bola;

		dist = Vector3.Distance (b.transform.position, a.transform.position);
		if (dist <= 2f) {
			dekatBola = true;
		} else {
			dekatBola = false;
		}

		if (dekatBola) {
			if (ownerBola == PhotonNetwork.player.ID) {
				pv.RPC ("OwnerBola", PhotonTargets.All);
			} 
			else {
				b.GetComponent<PhotonView> ().TransferOwnership (PhotonNetwork.player.ID);
				pv.RPC ("OwnerBola", PhotonTargets.All);
			}

		}



		if (beingCarried) {
			
			d.SetActive (true);
			a.GetComponent<PlayerController> ().enabled = false;
			a.GetComponent<Raycast> ().enabled = true;
			if (touched) {

				b.GetComponent<Rigidbody> ().isKinematic = false;
				b.GetComponent<Rigidbody> ().useGravity = false;
				transform.parent = null;
				beingCarried = false;
				touched = false;
			}
		}

	}


	void MelemparBola(){

		//e = a.transform.Find ("Canvas_player/Power Bar").gameObject;
		f.minValue = 0f;
		f.maxValue = 400f;
		f.value = powerBarValue;


		if (Input.GetMouseButton (0) && beingCarried) {   
			e.SetActive (true);
			increasing = true;
			shot = false;
			//RandomAudio();
		} else if (Input.GetMouseButtonUp (0)) {
			e.SetActive (false);
			d.SetActive (false);
			b.GetComponent<Rigidbody> ().isKinematic = false;
			b.transform.parent = null;
			beingCarried = false;
			b.GetComponent<Rigidbody> ().AddForce (c.forward * powerBarValue);
			increasing = false;
			GetComponent<PlayerController> ().enabled = true;
			GetComponent<Raycast> ().enabled = false;
		}
		if (Input.GetMouseButton (1) && beingCarried) {
			e.SetActive (true);
			increasing = true;
			shot = true;
		} else if (Input.GetMouseButtonUp (1)) {
			e.SetActive (false);
			d.SetActive (false);
			b.GetComponent<Rigidbody> ().isKinematic = false;
			b.transform.parent = null;
			beingCarried = false;
			b.GetComponent<Rigidbody> ().AddForce (c.forward * powerBarValue);
			increasing = false;
			GetComponent<PlayerController> ().enabled = true;
			GetComponent<Raycast> ().enabled = false;

		}
		if (increasing) { //if bar is increasing, calculate thepower
			f.value = powerBarValue;
			powerBarValue += powerBarTreshold;
		} else { //else set thepower back to 0.
			powerBarValue = 0;
		}
		if (shot) {
			c.transform.eulerAngles = new Vector3 (10, c.eulerAngles.y, c.eulerAngles.z);
		} else {
			c.transform.eulerAngles = new Vector3 (-20, c.eulerAngles.y, c.eulerAngles.z);
		}	
	}

	void OnTriggerEnter()
	{
		if (beingCarried) {
			touched = true;
		}
	}


	void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) 
	{ 

		if (stream.isWriting) 
		{ 
			stream.SendNext(transform.position);
			stream.SendNext(transform.rotation);

		} 
		else
		{ 
			transform.position = (Vector3)stream.ReceiveNext(); 
			transform.rotation = (Quaternion)stream.ReceiveNext(); 
		} 
	}
}
