using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;


public class GameM : Photon.PunBehaviour {

    // private WaitForSeconds m_StartWait;     
    // private WaitForSeconds m_EndWait;     
	//  public float m_StartDelay = 3f;         
    // public float m_EndDelay = 3f;
	// public TankManager[] m_Tanks;         
	// public TextMeshPro[] tmpRoomName;
	// public Material red_material;

	public GameObject camera1, camera2;
	public GameObject playerObj;
	public GameObject[] buttonArr;
	// public TextMeshPro roomNameUI, TimeUI;
	public TextMeshProUGUI roomNameUI, TimeUI;
	private float sumTime;
	private float timer = 10f;
	private void Start()
	{

		if (!PhotonNetwork.inRoom) return;
		
		roomNameUI.text = "Room Name  < " + PhotonNetwork.room.Name + " > ";
		if (userEmpty) {

			 if (PhotonNetwork.isMasterClient) {

				userEmpty = false;
				camera1.SetActive(true);
				Debug.Log ("나는 마스터 클라이언트 ");
				playerObj = PhotonNetwork.Instantiate ("animal",
					Vector3.zero, //TestCube
					Quaternion.identity, 0);
				// Transform cube_tr = master.transform.GetChild(2);
				// cube_tr.GetComponent<MeshRenderer>().material = red_material;
        	} else {
				
				userEmpty = false;
				camera2.SetActive(true);
				Debug.Log ("나는 그냥 클라이언트 ");
				playerObj = PhotonNetwork.Instantiate ("animal",
					Vector3.zero,
					user2ro,
					0);
        	}

		}else {

			// userObj.transform.position = pos;
		}

		playerObj.SetActive(false);

		// Create the delays so they only have to be made once.
		// m_StartWait = new WaitForSeconds(m_StartDelay);
		// m_EndWait = new WaitForSeconds(m_EndDelay);
		// tmpRoomName[0].text = PhotonNetwork.room.Name;
		// tmpRoomName[1].text = PhotonNetwork.room.Name;
		// tmpRoomName[2].text = PhotonNetwork.room.Name;
		// tmpRoomName[3].text = PhotonNetwork.room.Name;
		
		// if (!PhotonNetwork.inRoom)
		// 	return;
	
		// if( PhotonNetwork.isMasterClient ) {
		// 	SpawnMasterTank();
		// } else {
		// 	SpawnTank2();
		// 	Invoke("EnterGameLoop", 3);
		// }
	}

	void Update()
	{
		if (sumTime < 9.99f) {

			sumTime += Time.deltaTime;
			TimeUI.text = "Time  " + string.Format("{0:N2}" , timer - sumTime);
		}
		
	}

	private GameObject userObj;
	private bool userEmpty = true;
	private Quaternion user2ro = Quaternion.Euler(new Vector3(0,180,0));

	public void instantiatePlayer(Vector3 pos) {

		if (!PhotonNetwork.inRoom) return;
		
		if (userEmpty) {

			 if (PhotonNetwork.isMasterClient) {

				userEmpty = false;
				camera1.SetActive(true);
				Debug.Log ("나는 마스터 클라이언트 ");
				userObj = PhotonNetwork.Instantiate ("animal",
					pos, //TestCube
					Quaternion.identity, 0);
				// Transform cube_tr = master.transform.GetChild(2);
				// cube_tr.GetComponent<MeshRenderer>().material = red_material;
        	} else {
				
				userEmpty = false;
				camera2.SetActive(true);
				Debug.Log ("나는 그냥 클라이언트 ");
				userObj = PhotonNetwork.Instantiate ("animal",
					pos,
					user2ro,
					0);
        	}

		}else {

			userObj.transform.position = pos;
		}
       
	}



	void OnLevelWasLoaded (int levelNumber) {
        // Photon 룸 안이 아니라면 네트워크에 문제가 있을지도..

        if (!PhotonNetwork.inRoom) return;
        
        Debug.Log ("게임에 들어왔어요, yay~");
		Debug.Log("levelNumber = " + levelNumber );
        Debug.Log("player name = " + PhotonNetwork.playerName );
        Debug.Log("player AuthValues = " + PhotonNetwork.AuthValues );

        if (PhotonNetwork.isMasterClient) {

			camera1.SetActive(true);
            Debug.Log ("나는 마스터 클라이언트 ");
         	// GameObject master = PhotonNetwork.Instantiate ("Player",
            //     initPosition, //TestCube
            //     Quaternion.identity, 0);
			// Transform cube_tr = master.transform.GetChild(2);
			// cube_tr.GetComponent<MeshRenderer>().material = red_material;
        } else {

			camera2.SetActive(true);
            Debug.Log ("나는 그냥 클라이언트 ");
            // PhotonNetwork.Instantiate ("Player",
            //     initPosition,
            //     Quaternion.identity, 0);
        }

    }

	public void hideButton() {
		for (int i = 0; i < buttonArr.Length; i++ ) {
			buttonArr[i].SetActive(false);
		}
	}

	public void leaveRoomButton() {

		PhotonNetwork.LeaveRoom();
	}

	override public void OnLeftRoom()
    {
        Debug.Log("OnLeftRoom (local)");

        // back to main menu
        SceneManager.LoadScene("SampleScene");
    }

    override public void OnDisconnectedFromPhoton()
    {
        Debug.Log("OnDisconnectedFromPhoton");

        // back to main menu
        SceneManager.LoadScene("SampleScene");
    }

	override public void OnPhotonPlayerConnected(PhotonPlayer newPlayer) {
		Debug.Log(" 원격 사용자 접속 ! ");
	}

	
	// create one Tank, set its player number and references needed for control.
	// private void SpawnMasterTank()
	// {
	// 	int i = 0;
	// 	GameObject tank = PhotonNetwork.Instantiate(
	// 				"Player", 
	// 				m_Tanks[i].m_SpawnPoint.position,
	// 				m_Tanks[i].m_SpawnPoint.rotation,
	// 				0) as GameObject;
				
	// 	tank.name = "MasterTank";
	// 	m_Tanks[i].m_Instance = tank;
	// 	m_Tanks[i].m_PlayerNumber = i+1;
	// 	m_Tanks[i].Setup();
	
	// 	Debug.Log("SpawnMasterTank");
	// }

	// private void SpawnTank2()
	// {
	// 	int i = 1;
	// 	GameObject tank = PhotonNetwork.Instantiate(
	// 				"Player", 
	// 				m_Tanks[i].m_SpawnPoint.position,
	// 				m_Tanks[i].m_SpawnPoint.rotation,
	// 				0) as GameObject;
	
	// 	tank.name = "Tank2";
	// 	m_Tanks[i].m_Instance = tank;
	// 	m_Tanks[i].m_PlayerNumber = i+1;
	// 	m_Tanks[i].Setup();
	
	// 	Debug.Log("SpawnTank2");
	// }



}
