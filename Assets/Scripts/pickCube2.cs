using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Linq;

[RequireComponent( typeof( PhotonView ) )]
public class pickCube2 : Photon.PunBehaviour
{
	GameObject PanelFreeze;
	Transform hands;
	bool hasPlayer = false;
	bool beingCarried = false;
	public TextMesh namaPemain;
	GameObject namea;
	private bool touched = false;
	PhotonView pv;
	GameObject balok, tanda;
	GameObject player;
	int ownerBalok;
	int playerID;
	float dist;
	float time = 0.0f;
	float lamaPanelBeku;
	bool freeze = false;

	void Start()
	{

		pv = this.GetComponent<PhotonView>();
		pv.RPC ("BALOK", PhotonTargets.All);
		if (pv.isMine) {
			player = gameObject;
			hands = player.transform.Find ("Hands1");
			PanelFreeze = GameObject.FindWithTag ("freeze");
			PanelFreeze.SetActive (false);
			lamaPanelBeku = 7f;
			namea = player.transform.Find ("nama_player1").gameObject;
		}
	}

	[PunRPC]
	void BALOK(){
		balok = GameObject.FindWithTag ("cube2");
		tanda = balok.transform.Find ("Pointer").gameObject;
	}

	void Update()
	{

		if (pv.isMine) {
			TextMesh namaPlayers = namaPemain.GetComponent<TextMesh> ();
			namaPlayers.text = PhotonNetwork.player.NickName;
		}
		if (!pv.isMine) {
			TextMesh namaPlayers = namaPemain.GetComponent<TextMesh> ();
			namaPlayers.text = pv.owner.NickName;
		}

		if (!pv.isMine && PhotonNetwork.connected == true) {
			return;
		}
		tanda.SetActive (true);
		tangkap ();
		balokDipegang ();
		lepasBalok ();

		if (freeze) {
			time += Time.deltaTime;
			beku ();
			PanelFreeze.SetActive (true);
			if (time >= lamaPanelBeku) {
				PanelFreeze.SetActive (false);
				namea.SetActive(false);
			}
			balok.SetActive (false);
			gameObject.tag = "merahBeku";
		}
	}

	///////////////////////////////////////////////////////////////////////////////
	///////////////////////////////////////////////////////////////////////////////


	[PunRPC]
	void parenting(){
		ownerBalok = balok.GetComponent<PhotonView> ().ownerId;
		playerID = (ownerBalok * 1000) + 1;
		Debug.Log (playerID);
		player = PhotonView.Find (playerID).gameObject;
		hands = player.transform.Find ("Hands1");
		balok.GetComponent<Rigidbody> ().isKinematic = true;
		balok.transform.parent = hands;
		balok.transform.position = hands.transform.position;
		balok.transform.position += Vector3.up;
		balok.transform.position += Vector3.forward;
		balok.tag = "cubee";

	}



	void tangkap()
	{
		pv.RPC("DIST",PhotonTargets.All, balok.transform.position, player.transform.position);
		if (dist <= 2f) {
			hasPlayer = true;
		} else {
			hasPlayer = false;
		}
		if (hasPlayer && Input.GetMouseButtonDown (1)) {
			balok.transform.position += (Vector3.up);
			beingCarried = true;
		}
	}

	[PunRPC]
	void DIST(Vector3 posisi_balok, Vector3 posisi_player){
		dist = Vector3.Distance (posisi_balok, posisi_player);
	}


	void balokDipegang()
	{
		if (beingCarried) {
			if (ownerBalok == PhotonNetwork.player.ID) {
				pv.RPC ("parenting", PhotonTargets.All);
			} else {
				balok.GetComponent<PhotonView> ().TransferOwnership (PhotonNetwork.player.ID);
				pv.RPC ("parenting", PhotonTargets.All);
			}
			if (touched) {
				balok.GetComponent<Rigidbody> ().isKinematic = false;
				balok.GetComponent<Rigidbody> ().useGravity = false;
				transform.parent = null;
				beingCarried = false;
				touched = false;
			}
		}
	}

	[PunRPC]
	void disparenting(){
		balok.transform.parent = null;
		beingCarried = false;
		balok.tag = "cube2";
	}

	void lepasBalok()
	{
		if (Input.GetMouseButtonDown (0) && beingCarried) {   
			balok.GetComponent<Rigidbody> ().isKinematic = false;
			pv.RPC ("disparenting", PhotonTargets.All);
		} 
	}

	void beku(){
		GetComponent<PlayerController> ().enabled = false;
		GetComponent<Animator> ().enabled = false;
	}

	void OnTriggerEnter(Collider other)
	{
		if (other.tag == "cubee") {
			return;
		}

		if (beingCarried && other.tag == "cubee") {
			touched = true;
		}

		if (other.tag == "bola") {
			freeze = true;
		}
	}


	void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) 
	{ 

		if (stream.isWriting) 
		{ 
			stream.SendNext (balok.activeInHierarchy);
			stream.SendNext (GetComponent<PlayerController> ().enabled);
			stream.SendNext (GetComponent<Animator> ().enabled);
			stream.SendNext (gameObject.tag);
		} 
		else
		{ 
			balok.SetActive ((bool)stream.ReceiveNext ());
			GetComponent<PlayerController> ().enabled = (bool)stream.ReceiveNext ();
			GetComponent<Animator> ().enabled = (bool)stream.ReceiveNext ();
			gameObject.tag = (string)stream.ReceiveNext ();
		} 
	}

}