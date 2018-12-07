using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.UIElements;

public class TutorialMapManager : MonoBehaviour {

    
    public BoxCollider[] wall;


    private void Start()
    {
        wall = GameObject.Find("Ground").GetComponentsInChildren<BoxCollider>();
    }



    public void ShowAllWall()
    {
        for (int x = 0; x < wall.Length; x++)
        {
            wall[x].gameObject.SetActive(true);
        }
    }


}
