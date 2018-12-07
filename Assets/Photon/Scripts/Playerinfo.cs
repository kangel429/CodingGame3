using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Playerinfo : Photon.PunBehaviour {

	private string myName;
	public string masterName;
	void Start () {

		if (!PhotonNetwork.isMasterClient) {

		}
		// Debug.Log("p name = " + photonView.name );  // 게임오브젝트 이름 
	}

	[PunRPC]
	void setname() {
		myName = masterName;
	}

	public void sendName() {

		Debug.Log("send RPC name ");

		photonView.RPC("setname", PhotonTargets.All);

	}
	
	
}
