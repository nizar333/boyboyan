using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Linq;

public class  pengaturanMultiplayer  :   Photon.MonoBehaviour {

	GameObject cannon;
	public GameObject info;
	GameObject[] a, b, c, d, e;
	public TextMesh CubeCount;
	public GameObject[] aaa;
	public GameObject[] bbb;
	public GameObject panelMenang;
	public GameObject panelKalah;
	public GameObject panelTunggu;
	public Text namaPlayer;
	public Text jenisPlayer;
	public Text statusAktifitas;
	public Text namaRoom;
	public int indeks;
	public Text jumlahPlayer;
	public Button tombolKeluarLevel;
	Button TombolKeluarLevel;
	public GameObject defender;
	public GameObject attacker;
	public GameObject attacker1;
	public GameObject attacker2;
	public GameObject attacker3;
	public GameObject attacker4;
	public GameObject bola;
	GameObject Pemain;
	GameObject[] players;
	public string namaSceneKeluar;
	PhotonView pv;
	float keluar = 7f, time;
	float closeInfo = 5f, waktu;
	//float waktuInfo = 7f, waktu;


	void Start () {
		pv = this.GetComponent<PhotonView>();
		cannon = GameObject.FindWithTag ("cannon");
		info.SetActive (false);
		panelMenang.SetActive (false);
		panelKalah.SetActive (false);
		panelTunggu.SetActive (true);
		indeks = PlayerPrefs.GetInt ("indexPemain");
		if (PhotonNetwork.connected) {
			MulaiPlayer ();
			statusAktifitas.text += "<color=yellow>Mulai Bergabung</color>\n";
		}
	

		Text namaPlay = namaPlayer.GetComponent<Text> ();
		namaPlay.text = PhotonNetwork.player.NickName;

	

		namaRoom.text = PhotonNetwork.room.Name;
		TombolKeluarLevel = tombolKeluarLevel.GetComponent<Button> ();
		TombolKeluarLevel.onClick.AddListener (() => MeninggalkanRoom());
	}
		
	// Update is called once per frame
	void Update () {
		if (!PhotonNetwork.connected) {
			PhotonNetwork.offlineMode = true;
			PhotonNetwork.ReconnectAndRejoin ();
		}

		jumlahPlayer.text = PhotonNetwork.room.PlayerCount.ToString() + " Player";

		if (PhotonNetwork.room.PlayerCount > 1) {
			panelTunggu.SetActive (false);
			info.SetActive (true);
			waktu += Time.deltaTime;
		}

		if (waktu >= closeInfo ) {
			info.SetActive (false);
		}

		a = GameObject.FindGameObjectsWithTag ("attacker");
		b = GameObject.FindGameObjectsWithTag ("attacker1");
		c = GameObject.FindGameObjectsWithTag ("attacker2");
		d = GameObject.FindGameObjectsWithTag ("attacker3");
		e = GameObject.FindGameObjectsWithTag ("attacker4");


		aaa = a.Concat (b).Concat (c).Concat (d).Concat (e).ToArray();

		int coutinho = int.Parse (CubeCount.text);

		//jika pemain defender menang
		if (cannon.activeInHierarchy == false && Pemain == players [0] && aaa.Length == 0){
			panelMenang.SetActive (true);
		}
		//jika pemain attacker kalah
		if (cannon.activeInHierarchy == false && (Pemain == players [1] || Pemain == players [2] || Pemain == players [3] || Pemain == players [4] || Pemain == players [5]) && aaa.Length == 0){
			panelKalah.SetActive (true);
		}
		//jika pemain attacker menang
		if (cannon.activeInHierarchy == false && (Pemain == players [1] || Pemain == players [2] || Pemain == players [3] || Pemain == players [4] || Pemain == players [5]) && coutinho == aaa.Length && aaa.Length != 0){
			panelMenang.SetActive (true);
		}
		//jika pemain defender kalah
		if (cannon.activeInHierarchy == false && Pemain == players [0] && coutinho == aaa.Length && aaa.Length != 0){
			panelKalah.SetActive (true);
		}

		if (panelKalah.activeInHierarchy == true || panelMenang.activeInHierarchy == true) {
			time += Time.deltaTime;
			if (time >= keluar) {
				MeninggalkanRoom ();
			}
		}


	}
		

	void  OnLeftRoom () {
		pv.RPC ("KirimPesan", PhotonTargets.All , "<color=red>" + PhotonNetwork.player.NickName + " Meninggalkan Room </color>\n");
		Scene sceneIni = SceneManager.GetActiveScene ();
		if (sceneIni.name != namaSceneKeluar){
			PhotonNetwork.LoadLevel (namaSceneKeluar);
		}
	}

	public void MeninggalkanRoom()
	{
		if (PhotonNetwork.connected) {
			PhotonNetwork.LeaveRoom ();
			PhotonNetwork.LeaveLobby ();
			PhotonNetwork.DestroyPlayerObjects (PhotonNetwork.player.ID);
		}
	}

	void MulaiPlayer(){

		SpawnPlayer ();
		pv.RPC ("KirimPesan", PhotonTargets.All , "<color=cyan>" + PhotonNetwork.player.NickName + " Bergabung </color>\n");
	}


	void SpawnPlayer()
	{
		if (PhotonNetwork.connected) {
			Vector3 posisiRandomPlayer = new Vector3(Random.Range(-35, 32), 2f, Random.Range(-21, 21));

			//pilih index pemain
			players = new GameObject[] {defender, attacker, attacker1, attacker2, attacker3, attacker4};

			Pemain = players [indeks];
			PhotonNetwork.Instantiate (Pemain.name, posisiRandomPlayer, transform.rotation, 0);
			if (Pemain == players [0]) {
				jenisPlayer.text = "defender";
			}
			if (Pemain == players [1] || Pemain == players [2] || Pemain == players [3] || Pemain == players [4] || Pemain == players [5]){
				jenisPlayer.text = "attacker";
			}
		}

	}

	[PunRPC]
	void KirimPesan (string pesan) {
		statusAktifitas.text += pesan;
	}


	void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) 
	{ 
		if (stream.isWriting) 
		{ 
			stream.SendNext(statusAktifitas.text);

		} 
		else
		{ 
			statusAktifitas.text = (string)stream.ReceiveNext(); 
	
		} 
	}

}