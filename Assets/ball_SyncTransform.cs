using UnityEngine;
using System.Collections;

[RequireComponent(typeof(PhotonView))]
public class ball_SyncTransform : MonoBehaviour {
	PhotonView m_view;
	Vector3 m_networkPosition;
	Quaternion m_networkRotation;
	bool m_isParented;
	int m_holderID;

	protected virtual void Awake() {
		this.m_view = this.GetComponent<PhotonView>();
	}

	protected virtual void Update() {
		if(!this.m_view.isMine && ! this.m_isParented) {
			this.transform.position = Vector3.Lerp(this.transform.position, this.m_networkPosition, 0.1F);
			this.transform.rotation = Quaternion.Lerp(this.transform.rotation, this.m_networkRotation, 0.1F);

		}
	}

	[PunRPC]
	protected virtual void RequestPickUp(int viewID) {
		if(this.m_holderID != -1) return;

		PhotonView view = PhotonView.Find(viewID);

		if(view != null) {
			this.m_isParented = true;
			this.transform.parent = view.transform;
			this.transform.localPosition = Vector3.zero;
			this.transform.rotation = Quaternion.identity;
			this.m_holderID = view.owner.ID;
		}
	}

	[PunRPC]
	protected virtual void RequestDrop() {
		this.transform.parent = null;
		this.m_isParented = false;
		this.m_holderID = -1;
	}

	protected void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) {
		if(stream.isWriting) {
			stream.SendNext(this.transform.position);
			stream.SendNext(this.transform.rotation);
		} else {
			this.m_networkPosition = (Vector3)stream.ReceiveNext();
			this.m_networkRotation = (Quaternion)stream.ReceiveNext();
		}
	}

	protected virtual void OnPhotonPlayerDisconnected(PhotonPlayer otherPlayer) {
		if(this.m_holderID == otherPlayer.ID) {
			this.m_view.RPC("RequestDrop", PhotonTargets.AllBufferedViaServer);
		}
	}

	public virtual PhotonView View {
		get {
			return this.m_view;

		}
	}
}