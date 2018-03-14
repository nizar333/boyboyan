using UnityEngine;
using System.Collections;

[RequireComponent( typeof( PhotonView ) )]
public class OnTouchReqOwn : Photon.MonoBehaviour
{
	public GameObject player;
	public GameObject bola;
	bool hasPlayer = false;
	bool beingCarried = false;
	private bool touched = false;

	void Start()
	{
		//player = GameObject.FindWithTag ("Player");
		//bola = GameObject.FindWithTag ("bola");
	}
		

	void Update()
	{
		float dist = Vector3.Distance (bola.transform.position, player.transform.position);
		if (dist <= 2f) {
			hasPlayer = true;
		} else {
			hasPlayer = false;
		}
		if (hasPlayer) {
			beingCarried = true;
		}
		if (beingCarried && touched) 
		{
			if( this.photonView.ownerId == PhotonNetwork.player.ID )
			{
				Debug.Log( "Not requesting ownership. Already mine." );
				return;
			}

			this.photonView.RequestOwnership();
		}
	}

	void OnTriggerEnter()
	{
		if (beingCarried) {
			touched = true;
		}
	}

	[PunRPC]
	public void ColorRpc( Vector3 col )
	{
		Color color = new Color( col.x, col.y, col.z );
		this.gameObject.GetComponent<Renderer>().material.color = color;
	}
}
