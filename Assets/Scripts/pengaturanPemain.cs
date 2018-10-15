using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class pengaturanPemain : MonoBehaviour{

	public Text AttIndexiing;
	public int index;
	public int attacking;
	public Button pilihPenyerang, pilihBertahan;

	Button TombolGabung, Penyerang, Bertahan;

	// Use this for initialization
	void Start () {
		Penyerang = pilihPenyerang.GetComponent<Button> ();
		Bertahan = pilihBertahan.GetComponent<Button> ();
		Penyerang.onClick.AddListener (delegate {pilihAttacker(1);});
		Bertahan.onClick.AddListener (delegate {pilihPemain(0);});
		attacking = int.Parse(AttIndexiing.text);
	}
		
	// Update is called once per frame
	void Update () {
		PlayerPrefs.SetInt ("indexPemain", index);

		AttIndexiing.text = attacking.ToString ();

		if (index == 1) {
			index = attacking;
			Penyerang.interactable = false;
			Bertahan.interactable = true;
		}

		if (index == 0) {
			Penyerang.interactable = true;
			Bertahan.interactable = false;
		}

	}

	public void pilihPemain(int indexPemain){
		index = indexPemain;
	}

	public void pilihAttacker(int indexPemain){
		index = indexPemain;
		attacking++;
	}
}
