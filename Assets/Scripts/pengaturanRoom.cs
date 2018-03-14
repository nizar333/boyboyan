using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class pengaturanRoom : MonoBehaviour
{

	public Button tombolGabung;
	public Dropdown dropdownRoom;
	public Text statusKoneksi;
	public int maksPanjangNamaPlayer;
	public int minPanjangNamaPlayer;

	[SerializeField]
	InputField inputNamaPlayer;

	Button TombolGabung;
	Dropdown DropdownRoom;

	//Canvas canvasGabungRoom;
	pengaturanKoneksi pengaturanKoneksi;
	List<AmbilRoom.Room> obyekRoom;

	public string namaSceneRumput;
	public string namaScenePasir;
	string namaRuang = "Padang Rumput";
	// Use this for initialization

	void Awake()
	{
		TombolGabung = tombolGabung.GetComponent<Button>();
		DropdownRoom = dropdownRoom.GetComponent<Dropdown>();
		//canvasGabungRoom = transform.GetComponent<Canvas>();
		pengaturanKoneksi = GameObject.FindGameObjectWithTag("pengaturanKoneksi").GetComponent<pengaturanKoneksi>();
		Debug.Log("Mulai Fungsi Awake");
		statusKoneksi.text = "Mulai Fungsi Awake";
		inputNamaPlayer.interactable = false;
	}
	void Start()
	{
		validasiRoom();
		Debug.Log("Mulai Validasi");
		statusKoneksi.text = "Mulai Validasi";
		PhotonNetwork.automaticallySyncScene = true;
	}

	// Update is called once per frame
	void Update()
	{

	}

	public void GabungSeleksiRoom()
	{
		
		AmbilRoom.Room roomDiseleksi = obyekRoom[DropdownRoom.value];
		pengaturanKoneksi.GabungRoom(roomDiseleksi.namaRoom, inputNamaPlayer.text);
		statusKoneksi.text = "Gabung dengan Room yang diseleksi";
	}
	void OnJoinedRoom()
	{
		statusKoneksi.text = "Bergabung Ke Room : " + PhotonNetwork.room.Name;
		Scene sceneIni = SceneManager.GetActiveScene ();

		if (sceneIni.name != namaSceneRumput && PhotonNetwork.room.Name == namaRuang) {
			PhotonNetwork.LoadLevel (namaSceneRumput);
		} else 
		{
			PhotonNetwork.LoadLevel (namaScenePasir);
		}
	}

	void OnJoinedLobby()
	{
		validasiRoom();
		statusKoneksi.text = "Bergabung ke Lobby";
	}

	public void PerubahanInputNamaPlayer()
	{
		validasiRoom();
		Debug.Log("Perubahan Input Player");
		statusKoneksi.text = "Perubahan Input Player";
	}


	public void SaatPerubahanDaftarRoom(List<AmbilRoom.Room> daftarRoom)
	{
		statusKoneksi.text = "Perubahan Daftar Room";
		obyekRoom = daftarRoom;
		DropdownRoom.ClearOptions();
		foreach (var room in daftarRoom)
		{
			DropdownRoom.options.Add(new Dropdown.OptionData(room.namaRoom));
		}
		DropdownRoom.RefreshShownValue();
		DropdownRoom.onValueChanged.AddListener(delegate
			{
				Debug.Log("Room Terpilih: " + DropdownRoom.options[DropdownRoom.value].text + " | Index: " + DropdownRoom.value);
			});
	}

  

	void BelumBisaGabung()
	{
		TombolGabung.interactable = false;
		Debug.Log("Belum Bisa Gabung");
	}

	void SudahBisaGabung()
	{
		TombolGabung.interactable = true;
		Debug.Log("Bisa Gabung");
	}

	bool ApakahRoomPenuh()
	{
		AmbilRoom.Room roomDiseleksi = obyekRoom[DropdownRoom.value];
		return roomDiseleksi.maksimumJumlahPlayer <= roomDiseleksi.jumlahPlayer;
	}

	public bool validasiRoom()
	{
		if (inputNamaPlayer.text.Length < minPanjangNamaPlayer || inputNamaPlayer.text.Length > maksPanjangNamaPlayer
			|| ApakahRoomPenuh())
		{
			BelumBisaGabung();
			return false;
		}
		SudahBisaGabung();
		return true;
	}

	void OnReceivedRoomListUpdate()
	{
		statusKoneksi.text = "Perubahan Daftar Room";
		inputNamaPlayer.interactable = true;
	}
}