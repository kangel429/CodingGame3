using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Posinfo : MonoBehaviour, IPointerEnterHandler {

	public GameM gameManager;
	public Vector3 instantiatePos; //생성 포지션


	public void clickPosButton() {

		Debug.Log("click pos button ");

		gameManager.playerObj.transform.position = instantiatePos;
		gameManager.hideButton();
		gameManager.selectPos();
		// gameManager.instantiatePlayer(instantiatePos);

		// gameManager.initPosition = instantiatePos;
	}


	public void OnPointerEnter(PointerEventData eventData) {

		gameManager.playerObj.SetActive(true);
		gameManager.playerObj.transform.position = instantiatePos;
		// Debug.Log(eventData.pointerEnter.name);
	}


}
