using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class pindahPanel : MonoBehaviour {

	//public AudioSource buttonSound;
	public GameObject PanelAwal;
	public GameObject PanelTujuan;

	public void GantiKePanelBaru(){
		//buttonSound.PlayOneShot (buttonSound.clip);
		PanelAwal.SetActive (false);
		PanelTujuan.SetActive (true);
	}
}