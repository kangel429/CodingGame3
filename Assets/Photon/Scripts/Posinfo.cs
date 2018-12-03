using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Posinfo : MonoBehaviour {

	public GameM gameManager;
	public Vector3 instantiatePos; //생성 포지션
	public static Vector3 initPos;

	public void clickPosButton() {

		Debug.Log("click pos button ");
		initPos = instantiatePos;
		gameManager.instantiatePlayer(instantiatePos);

		// gameManager.initPosition = instantiatePos;
	}
}
