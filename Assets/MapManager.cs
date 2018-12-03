using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour {
    public GameObject[] mArrTargetWall;

    public GameObject[] wall;

    public GameObject parentObj;

    private void Start()
    {
        MapVisualize();
    }


    // Use this for initialization
    void MapVisualize () {
        for (int x = 0; x < 25; x++)
        {

          wall[x] = Instantiate(mArrTargetWall[0].gameObject, Vector3.zero, Quaternion.identity);
          wall[x].gameObject.SetActive(false);
          wall[x].gameObject.transform.parent = parentObj.transform;

        


        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}


}
