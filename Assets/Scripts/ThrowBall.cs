using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Linq;

[RequireComponent( typeof( PhotonView ) )]
public class ThrowBall : Photon.PunBehaviour
{
	GameObject PanelFreeze;
	Transform hands;
	bool hasPlayer = false;
	bool beingCarried = false;
	private Slider powerBar;
	private float powerBarTreshold = 20f;
	private float powerBarValue = 0f;
	GameObject barPower;
	GameObject targetTembak;
	public TextMesh namaPemain;
	bool increasing = false;
	private bool touched = false;
	private bool shot = false;
	PhotonView pv;
	GameObject bola; 
	GameObject player;
	int ownerBola;
	int playerID;
	float dist;

	void Start()
	{
		
		pv = this.GetComponent<PhotonView>();
		powerBar = GameObject.Find ("Power Bar").GetComponent<Slider> ();
		powerBar.minValue = 0f;
		powerBar.maxValue = 400f;
		powerBar.value = powerBarValue;
		barPower = GameObject.Find ("Power Bar");
		barPower.SetActive (false);
		targetTembak = GameObject.Find ("target_tembak");
		targetTembak.SetActive (false);
		GetComponent<Raycast> ().enabled = false;
		pv.RPC ("BOLA", PhotonTargets.All);
		player = gameObject;
		hands = player.transform.Find ("hands");
		if (pv.isMine) {
			PanelFreeze = GameObject.FindWithTag ("freeze");
			PanelFreeze.SetActive (false);
		}
	}
	[PunRPC]
	void BOLA(){
		bola = GameObject.FindWithTag ("bola");
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

		tangkap ();	
		if (beingCarried) {
			if (ownerBola == PhotonNetwork.player.ID) {
				pv.RPC ("parenting", PhotonTargets.All);
			} else {
				bola.GetComponent<PhotonView> ().TransferOwnership (PhotonNetwork.player.ID);
				pv.RPC ("parenting", PhotonTargets.All);
			}
		}
			
		bolaDipegang ();
		lemparBola ();
	}
		

	[PunRPC]
	void parenting(){
		ownerBola = bola.GetComponent<PhotonView> ().ownerId;
		playerID = (ownerBola * 1000) + 1;
		Debug.Log (playerID);
		player = PhotonView.Find (playerID).gameObject;
		hands = player.transform.Find ("hands");
		bola.GetComponent<Rigidbody> ().isKinematic = true;
		bola.transform.parent = hands;
		bola.transform.position = hands.transform.position;
		bola.transform.position += Vector3.up;
		bola.transform.position += Vector3.forward;

	}
		

	//tangkap bola
	void tangkap()
	{
		pv.RPC("DIST",PhotonTargets.All, bola.transform.position, player.transform.position);
		if (dist <= 2f) {
			hasPlayer = true;
		} else {
			hasPlayer = false;
		}
		if (hasPlayer) {
			bola.transform.position += (Vector3.up);
			beingCarried = true;
		}
	}

	[PunRPC]
	void DIST(Vector3 posisi_bola, Vector3 posisi_player){
		dist = Vector3.Distance (posisi_bola, posisi_player);
	}



	void bolaDipegang()
	{
		if (beingCarried) {
			targetTembak.SetActive (true);
			GetComponent<PlayerController> ().enabled = false;
			GetComponent<Raycast> ().enabled = true;
			if (touched) {

				bola.GetComponent<Rigidbody> ().isKinematic = false;
				bola.GetComponent<Rigidbody> ().useGravity = false;
				transform.parent = null;
				beingCarried = false;
				touched = false;
			}
		}
	}

	[PunRPC]
	void disparenting(){
		bola.transform.parent = null;
		beingCarried = false;
	}
		
	void lemparBola()
	{
		if (Input.GetMouseButton (0) && beingCarried) {   
			barPower.SetActive (true);
			increasing = true;
			shot = false;
		} else if (Input.GetMouseButtonUp (0)) {
			barPower.SetActive (false);
			targetTembak.SetActive (false);
			bola.GetComponent<Rigidbody> ().isKinematic = false;
			pv.RPC ("disparenting", PhotonTargets.All);
			bola.GetComponent<Rigidbody> ().AddForce (hands.forward * powerBarValue);
			increasing = false;
			GetComponent<PlayerController> ().enabled = true;
			GetComponent<Raycast> ().enabled = false;
		}
		if (Input.GetMouseButton (1) && beingCarried) {
			barPower.SetActive (true);
			increasing = true;
			shot = true;
		} else if (Input.GetMouseButtonUp (1)) {
			barPower.SetActive (false);
			targetTembak.SetActive (false);
			bola.GetComponent<Rigidbody> ().isKinematic = false;
			pv.RPC ("disparenting", PhotonTargets.All);
			bola.GetComponent<Rigidbody> ().AddForce (hands.forward * powerBarValue);
			increasing = false;
			GetComponent<PlayerController> ().enabled = true;
			GetComponent<Raycast> ().enabled = false;

		}
		if (increasing) { //if bar is increasing, calculate thepower
			powerBar.value = powerBarValue;
			powerBarValue += powerBarTreshold;
		} else { //else set thepower back to 0.
			powerBarValue = 0;
		}
		if (shot) {
			hands.transform.eulerAngles = new Vector3 (15, hands.eulerAngles.y, hands.eulerAngles.z);
		} else {
			hands.transform.eulerAngles = new Vector3 (-20, hands.eulerAngles.y, hands.eulerAngles.z);
		}	
			

	}
		
		

		private void OnTriggerEnter()
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