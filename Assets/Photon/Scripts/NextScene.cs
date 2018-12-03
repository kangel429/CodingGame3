using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class NextScene : MonoBehaviour {

	public void button_click() {
		SceneManager.LoadScene("main");
	}
}
