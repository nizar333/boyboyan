using System.Collections;
using UnityEngine.UI;
using UnityEngine;
using System.Linq;

public class pickCube : Photon.PunBehaviour {

	public Transform player1;
	public GameObject[] cubes;
	public GameObject[] cubess;
	Transform balok;
	//Rigidbody[] rbCubes;
	public Transform hands1;
	bool hasPlayer = false;
	bool beingCarried = false;
	public TextMesh namaPemain1;
	//private Slider powerBar;
	//private float powerBarTreshold = 500f;
	//private float powerBarValue = 0f;
	//GameObject barPower;
	//GameObject targetTembak;
	//public GameObject pemain;
	//private float thePower; //current power

	//public bool increasing = false;

	//public float barSpeed = 25; //how fast bar will fill in.
	//public AudioClip[] soundToPlay;
	//private AudioSource audio;
	//public int dmg;
	private bool touched = false;
	PhotonView pv;
	//public int count = 0;

	void Start()
	{
		pv = this.GetComponent<PhotonView>();
		balok = GameObject.FindWithTag ("cube").transform;
		//player1 = GameObject.FindWithTag ("attacker").transform;
		//hands1 = GameObject.FindWithTag ("hands1").transform;
	}


	void Update()
	{
		if (pv.isMine) {
			TextMesh namaPlayers = namaPemain1.GetComponent<TextMesh> ();
			namaPlayers.text = PhotonNetwork.player.NickName;
		}
		if (!pv.isMine) {
			TextMesh namaPlayer = namaPemain1.GetComponent<TextMesh> ();
			namaPlayer.text = pv.owner.NickName;
		}


		if (pv.isMine) {
			AmbilCube ();
			LemparCube ();
		}
	}

	void AmbilCube()
	{
		
		cubes = GameObject.FindGameObjectsWithTag ("cube");
		foreach (GameObject cube in cubes) {
			float dist = Vector3.Distance (cube.transform.position, player1.position);
			if (dist <= 2f) {
				hasPlayer = true;
			} else {
				hasPlayer = false;

			}
			if (hasPlayer && Input.GetMouseButtonDown (1)) {
				cube.GetComponent<Rigidbody> ().isKinematic = true;
				dist = 1.3f;
				cube.transform.parent = hands1;
				cube.transform.position += (Vector3.up) * 2;
				beingCarried = true;
				cube.tag = "cubee";
			}

		}
	}

	void LemparCube()
	{
		cubess = GameObject.FindGameObjectsWithTag ("cubee");
		foreach (GameObject cubee in cubess) {
			if (beingCarried) {
				if (touched) {
					cubee.GetComponent<Rigidbody> ().isKinematic = false;
					//cube.GetComponent<Rigidbody> ().useGravity = false;
					cubee.transform.parent = null;
					beingCarried = false;
					touched = false;
				}
				if (Input.GetMouseButtonDown (0)) {
					if (cubess.Length > 0) {
						beingCarried = true;
					} else {
						beingCarried = false;
					}
					cubee.tag = "cube";
					cubee.GetComponent<Rigidbody> ().isKinematic = false;
					cubee.transform.parent = null;

				} 	
			}
		}
	}

	void OnTriggerEnter(Collider other)
	{
		if (other.tag == "cubee") {
			return;
		}

		if (beingCarried && other.tag == "cubee") {
			touched = true;
		}
	}

	void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) 
	{ 
		if (stream.isWriting) 
		{ 
			//stream.SendNext(balok.position);
			//stream.SendNext(cubess);
		} 
		else
		{ 
			//balok = (Transform)stream.ReceiveNext(); 
			//cubess = (GameObject[])stream.ReceiveNext();
		} 
	}

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
