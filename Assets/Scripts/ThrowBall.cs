using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Linq;

[RequireComponent( typeof( PhotonView ) )]
public class ThrowBall : Photon.PunBehaviour
{
	//private Vector3 syncPosShot = Vector3.zero;
	//GameObject player;
	//Transform bola;
	//GameObject ball;
	public Transform hands;
	//public Transform hand2;
	bool hasPlayer = false;
	bool beingCarried = false;
	bool udahRequestOwnership = true;
	bool udahTransferOwnership = true;
	private Slider powerBar;
	private float powerBarTreshold = 20f;
	private float powerBarValue = 0f;
	GameObject barPower;
	GameObject targetTembak;
	//public GameObject pemain;
	public TextMesh namaPemain;
	//private float thePower; //current power

	public bool increasing = false;

	//public float barSpeed = 25; //how fast bar will fill in.
	//public AudioClip[] soundToPlay;
	//private AudioSource audio;
	//public int dmg;
	private bool touched = false;
	private bool shot = false;
	PhotonView pv;
	GameObject bola; 
	GameObject pemain;

	void Start()
	{
		pv = this.GetComponent<PhotonView>();
		//player = GameObject.FindWithTag ("Player");
		//bola = GameObject.FindWithTag ("bola").transform;
		//ball = GameObject.FindWithTag ("bola");
		//hands = GameObject.FindWithTag ("hands").transform;
		pemain = GameObject.FindWithTag ("Player");
		powerBar = GameObject.Find ("Power Bar").GetComponent<Slider> ();
		powerBar.minValue = 0f;
		powerBar.maxValue = 400f;
		powerBar.value = powerBarValue;
		barPower = GameObject.Find ("Power Bar");
		barPower.SetActive (false);
		targetTembak = GameObject.Find ("target_tembak");
		targetTembak.SetActive (false);
		//audio = GetComponent<AudioSource>();
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
			
		tangkap ();
		if (beingCarried && udahRequestOwnership) {

				bola.GetComponent<PhotonView> ().RequestOwnership ();

				if (bola.GetComponent<PhotonView> ().ownerId == PhotonNetwork.player.ID) {
					udahRequestOwnership = false;
				} 
			} 
			
			if (udahRequestOwnership == false && beingCarried == false && udahTransferOwnership) {
				bola.GetComponent<PhotonView> ().TransferOwnership (0);
				udahTransferOwnership = false;
			}
		if (beingCarried) {
			bolaDipegang ();
			lemparBola ();
		}

	}

	//tangkap bola
	void tangkap()
	{
		float dist = Vector3.Distance (bola.transform.position, pemain.transform.position);
		if (dist <= 2f) {
			hasPlayer = true;
		} else {
			hasPlayer = false;
		}
		if (hasPlayer) {
			//ball.GetComponent<>().
			bola.GetComponent<Rigidbody> ().isKinematic = true;
			dist = 1.3f;
			bola.transform.parent = hands;
			bola.transform.position += (Vector3.up) * 2;
			beingCarried = true;
			udahRequestOwnership = true;
			udahTransferOwnership = true;
		}

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

	void lemparBola()
	{
		if (Input.GetMouseButton (0) && beingCarried) {   
			barPower.SetActive (true);
			increasing = true;
			shot = false;
			//RandomAudio();
		} else if (Input.GetMouseButtonUp (0)) {
			barPower.SetActive (false);
			targetTembak.SetActive (false);
			bola.GetComponent<Rigidbody> ().isKinematic = false;
			bola.transform.parent = null;
			beingCarried = false;
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
			bola.transform.parent = null;
			beingCarried = false;
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
			hands.transform.eulerAngles = new Vector3 (10, hands.eulerAngles.y, hands.eulerAngles.z);
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
			transform.rotation = (Quaternion)stream.ReceiveNext ();
		} 
	}

	/*
	[PunRPC]
	public void ColorRpc( Vector3 col )
	{
		Color color = new Color( col.x, col.y, col.z );
		ball.GetComponent<Renderer>().material.color = color;
	}
	*/

/*
	void RandomAudio()
	{
		if (audio.isPlaying){
			return;
		}
		audio.clip = soundToPlay[Random.Range(0, soundToPlay.Length)];
		audio.Play();

	}
  */

}