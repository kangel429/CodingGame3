using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class GameM : Photon.PunBehaviour {

	
	public GameObject camera1, camera2;
	/// <summary>
	///  플레이어 오브젝트
	/// </summary>
	public GameObject playerObj;
	public GameObject[] buttonArr1, buttonArr2;
	public TextMeshProUGUI roomNameUI1, TimeUI1, roomNameUI2, TimeUI2;
	private float sumTime;
	/// <summary>
	/// 제한시간 10초
	/// </summary>
	private float tenTime = 10f; 
	private Quaternion user2ro = Quaternion.Euler(new Vector3(0,180,0));
	private bool masterUser = false;
	private bool readyP1, readyP2 = false;
	public GameObject loadingPopUp;
	public Text user11name, user12name;
	public Text user21name, user22name;
	private string masterName;

	// private GameObject userObj;
	// private bool userEmpty = true;

	public InRoomRoundTimer inRoomRoundTimer;

	private void Start()
	{

		if (!PhotonNetwork.inRoom) return;

		masterName = PhotonNetwork.playerName;
		
		// if (userEmpty) {

			 if (PhotonNetwork.isMasterClient) {
				
				roomNameUI1.text = "Room Name  < " + PhotonNetwork.room.Name + " > ";
				TimeUI1.text = "10.00";
				user11name.text = masterName;
				masterUser = true;
				// userEmpty = false;
				camera1.SetActive(true);
				Debug.Log ("나는 마스터 클라이언트 ");
				playerObj = PhotonNetwork.Instantiate ("animal",
					Vector3.zero, //TestCube
					Quaternion.identity, 0);
				// Transform cube_tr = master.transform.GetChild(2);
				// cube_tr.GetComponent<MeshRenderer>().material = red_material;
				

        	} else {
				
				roomNameUI2.text = "Room Name  < " + PhotonNetwork.room.Name + " > ";
				TimeUI2.text = "10.00";
				user21name.text = PhotonNetwork.masterClient.NickName;
				user22name.text = masterName;
				// userEmpty = false;
				camera2.SetActive(true);
				Debug.Log ("나는 그냥 클라이언트 ");
				playerObj = PhotonNetwork.Instantiate ("animal",
					Vector3.zero,
					user2ro,
					0);
        	}
			
			

			playerObj.SetActive(false);

		// }else {

		// 	// userObj.transform.position = pos;
		// }

		

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
	private bool newUser = false;
	private float currentTime;
	void Update()
	{
		if (masterUser) { //마스터 클라이언트 인지 ( 방장 )

			if (newUser) { // 상대방이 접속했을때 카운트 시작

				if (sumTime < 10f) {

					sumTime += Time.deltaTime;
					currentTime = tenTime - sumTime;
					TimeUI1.text = "Time  " + string.Format("{0:N2}" , currentTime );

					if (currentTime <= 0f) {
						TimeUI1.text = "Time  " + "0.00";
						
						if (readyP1 == false) {
							randomPos();
							hideButton();
						}
					}
				}
			}
			
		}else {  // 두번째 플레이어

			if (sumTime < 10f) {

				sumTime += Time.deltaTime;
				currentTime = tenTime - sumTime;
				TimeUI2.text = "Time  " + string.Format("{0:N2}" , currentTime );

				if (currentTime <= 0f) {
					TimeUI2.text = "Time  " + "0.00";
					
					if (readyP2 == false) {
						randomPos();
						hideButton();
					}
				}

			}
		}

		
	}

	// IEnumerator countDown() {

	// 	while ()
	// }



	// public void instantiatePlayer(Vector3 pos) {

	// 	if (!PhotonNetwork.inRoom) return;
		
	// 	if (userEmpty) {

	// 		 if (PhotonNetwork.isMasterClient) {

	// 			userEmpty = false;
	// 			camera1.SetActive(true);
	// 			Debug.Log ("나는 마스터 클라이언트 ");
	// 			userObj = PhotonNetwork.Instantiate ("animal",
	// 				pos, //TestCube
	// 				Quaternion.identity, 0);
	// 			// Transform cube_tr = master.transform.GetChild(2);
	// 			// cube_tr.GetComponent<MeshRenderer>().material = red_material;
    //     	} else {
				
	// 			userEmpty = false;
	// 			camera2.SetActive(true);
	// 			Debug.Log ("나는 그냥 클라이언트 ");
	// 			userObj = PhotonNetwork.Instantiate ("animal",
	// 				pos,
	// 				user2ro,
	// 				0);
    //     	}

	// 	}else {

	// 		userObj.transform.position = pos;
	// 	}
       
	// }



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

		if(masterUser) {

			for (int i = 0; i < buttonArr1.Length; i++ ) {
				buttonArr1[i].SetActive(false);
			}
		}else {
			
			for (int i = 0; i < buttonArr2.Length; i++ ) {
				buttonArr2[i].SetActive(false);
			}
		}
		
	}

	public void selectPos() {

		if (masterUser) {
			readyP1 = true;
		}else {
			readyP2 = true;
		}
	}

	/// <summary>
	/// 10초안에 선택안했을때 랜덤 위치로 선택
	/// </summary>
	private void randomPos() {
		
		int r = Random.Range(0, 10);
		playerObj.SetActive(true);
		if (masterUser) {
			Vector3 rpos = buttonArr1[r].GetComponent<Posinfo>().instantiatePos;
			playerObj.transform.position = rpos;
		}else {
			Vector3 rpos = buttonArr2[r].GetComponent<Posinfo>().instantiatePos;
			playerObj.transform.position = rpos;
		}

		
	}

	public void clickExitButotn() {
		PhotonNetwork.LeaveRoom();
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
		loadingPopUp.SetActive(false);
		newUser = true;
		user12name.text = newPlayer.NickName;
		inRoomRoundTimer.joinUser = true;
		inRoomRoundTimer.StartRoundNow();
		// playerObj.GetComponent<Playerinfo>().masterName = masterName;
		// playerObj.GetComponent<Playerinfo>().sendName();

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
