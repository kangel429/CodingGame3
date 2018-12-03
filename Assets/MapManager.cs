using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour {
    public GameObject[] mArrTargetWall;

    public GameObject[] wall;

    public GameObject parentObj;

    private int wall_x,wall_y=2;

    private void Start()
    {
        MapVisualize();
        MapPosition();
    }


    // Use this for initialization
    void MapVisualize () {
        for (int x = 0; x < 50; x++)
        {

          wall[x] = Instantiate(mArrTargetWall[0].gameObject,new Vector3(0,0,0), Quaternion.identity);
          //wall[x].gameObject.SetActive(false);
          wall[x].gameObject.transform.parent = parentObj.transform;

        }
    }

    void MapPosition()
    {
        for (int x = 0; x < 50; x++)
        {
            if(x>0 && x % 10 ==0){
                wall_y--;
            }
            wall_x = -5+(x %10);
            wall[x].transform.localPosition = new Vector3(wall_x, wall_y, 0);

        }
    }

    public void ShowAllWall()
    {
        for (int x = 0; x < 50; x++)
        {
            wall[x].SetActive(true);
        }
    }

    // Update is called once per frame
    void Update () {
		
	}


}
