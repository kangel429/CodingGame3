using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ConnectDB : MonoBehaviour {

	public InputField nickField;
	// Use this for initialization
	void Start () {
		
	}

	IEnumerator checkNick() {

		WWWForm wWForm = new WWWForm();
		wWForm.AddField("nick", nickField.text );
		WWW www = new WWW("http://cacao.dothome.co.kr/dbconn2.php", wWForm );
		yield return www;
		Debug.Log("www = " + www.text );
	}
	

	public void checkButton() {

		StartCoroutine( checkNick() );
	}

}
