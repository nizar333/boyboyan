using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class photonNetworkManager : Photon.MonoBehaviour {

	[SerializeField] private Text connectText;
	[SerializeField] private GameObject player;
	[SerializeField] private Transform spawnPoint;

	TypedLobby lobby1 = new TypedLobby("Lobby1", LobbyType.Default);

    // Use this for initialization
	void Start () {
		PhotonNetwork.ConnectUsingSettings("nizar");
	}

	public virtual void OnConnectedToMaster()
	{
		PhotonNetwork.JoinLobby(lobby1);
		Debug.Log ("konek ke master server");
	}

	public virtual void OnJoinedLobby()
	{
		Debug.Log ("bergabung ke lobby");

		// join a room if it exist, or create one
		PhotonNetwork.JoinOrCreateRoom("New", null, null);
	}
		

	public virtual void OnJoinedRoom()
	{
		//spawn in the player
		PhotonNetwork.Instantiate(player.name, spawnPoint.position, spawnPoint.rotation,0);
	}

	// Update is called once per frame
	void Update () {
		// for testing only
		connectText.text = PhotonNetwork.connectionStateDetailed.ToString ();
	}
}
