using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class pengaturanPemain : MonoBehaviour{

	//public Text AttIndexiing;
	public int index;
	public int pilihan;


	// Use this for initialization
	void Start () {
		
	}
		
	// Update is called once per frame
	void Update () {
		PlayerPrefs.SetInt ("indexPemain", index);
		pilihan =  PhotonNetwork.countOfPlayersInRooms;

		if (pilihan == 0 || pilihan == 2 || pilihan == 4 || pilihan == 6 || pilihan == 8 || pilihan == 10) {
			index = 0;
		}
		if (pilihan == 1) {
			index = 1;
		}

		if (pilihan == 3) {
			index = 2;
		}

		if (pilihan == 5) {
			index = 3;
		}

		if (pilihan == 7) {
			index = 4;
		}

		if (pilihan == 9) {
			index = 5;
		}
			
	}
}
