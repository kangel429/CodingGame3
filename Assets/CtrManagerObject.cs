using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CtrManagerObject : MonoBehaviour {

    private Vector3 initMousePos;

    public bool mouseDrag=true;

    private void Update()
    {
        if(mouseDrag){
            Vector3 worldPoint = Input.mousePosition;
            worldPoint.z = 10;
            worldPoint = Camera.main.ScreenToWorldPoint(worldPoint);

            transform.position =
            new Vector3(Mathf.Clamp(worldPoint.x, -4.5f, 4.5f),
            transform.position.y,
            transform.position.z);
        }
    }

    //처음마우스 클릭시
    void OnMouseDown()
    {

        mouseDrag = false;
    }




    ////마우스 드래그시
    //void OnMouseDrag()
    //{
    //    //mouseDrag = true;
    //    //Vector3 worldPoint = Input.mousePosition;
    //    //worldPoint.z = 10;
    //    //worldPoint = Camera.main.ScreenToWorldPoint(worldPoint);

    //    //Vector3 diffPos = worldPoint - initMousePos;
    //    //diffPos.z = 0;
    //    //diffPos.y = 0;

    //    //initMousePos = Input.mousePosition;
    //    //initMousePos.z = 10;
    //    //initMousePos = Camera.main.ScreenToWorldPoint(initMousePos);

    //    //transform.position =
    //        //new Vector3(Mathf.Clamp(transform.position.x + diffPos.x, -4.5f, 4.5f),
    //                    //transform.position.y,
    //                    //transform.position.z);


    //     //Debug.Log("mouse drag" + diffPos);
    //}


}
