using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class backsound : MonoBehaviour {

	AudioSource audioSource;
	public GameObject SuaraOn;
	public GameObject SuaraOff;
	int nyala = 1;
	Button musiknyala, musikmati;

	void Awake(){
		
	}

	void Start(){
		audioSource = GetComponent<AudioSource>();
		musiknyala = SuaraOn.GetComponent<Button> ();
		musikmati = SuaraOff.GetComponent<Button> ();
		SuaraOn.SetActive (false);

	}

	// Update is called once per frame
	void Update () {
		musiknyala.onClick.AddListener (delegate {musikOn(1);});
		musikmati.onClick.AddListener (delegate {musikOn(0);});
		if (nyala == 1) {
			audioSource.mute = false;
			SuaraOff.SetActive (true);
			SuaraOn.SetActive (false);
		}

		if (nyala == 0) {
			audioSource.mute = true;
			SuaraOff.SetActive (false);
			SuaraOn.SetActive (true);
		}
	}

	void musikOn (int a){
		nyala = a;
	}
}
