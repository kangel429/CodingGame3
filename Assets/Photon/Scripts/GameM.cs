using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class GameM : Photon.PunBehaviour {

	
	public GameObject camera1, camera2; // 플레이어 1, 2 카메라
	/// <summary>
	///  플레이어 오브젝트
	/// </summary>
	public GameObject playerObj;  // 실제 보여지는 플레이어 오브젝트 프리팹 
	public GameObject[] buttonArr1, buttonArr2;  // 플레이어 1, 2 버튼들 10개씩
	public TextMeshProUGUI roomNameUI1, roomNameUI2;  // 각각 방이름 , 타이머 
	private float sumTime;
	/// <summary>
	/// 제한시간 10초
	/// </summary>
	private float tenTime = 10f; // 선택시간 10초
	private Quaternion user2ro = Quaternion.Euler(new Vector3(0,180,0)); // 플레이어2의 로테이션 마주보게 하려고
	private bool masterUser = false; // 마스터 클라이언트 이면 true
	public bool readyP1, readyP2 = false; // 위치 선택했을때 true
	public GameObject loadingPopUp;  // 방장에게만 보여지는 팝업
	public Text user11name, user12name;  // 플레이어1의 닉네임 ui
	public Text user21name, user22name;  // 플레이어2의 닉네임 ui
	private string masterName; // 자기 자신 닉네임

	// private GameObject userObj;
	// private bool userEmpty = true;

	public InRoomRoundTimer inRoomRoundTimer;
	public GameObject exitButton1, exitButton2;

	private void Start()
	{

		if (!PhotonNetwork.inRoom) return;

		masterName = PhotonNetwork.playerName;
		
		// if (userEmpty) {

			 if (PhotonNetwork.isMasterClient) {
				
				// camera1.SetActive(true);
				roomNameUI1.text = "Room Name  < " + PhotonNetwork.room.Name + " > ";
				// TimeUI1.text = "10.00";
				user11name.text = masterName;
				masterUser = true;
				// userEmpty = false;
				
				// Debug.Log ("나는 마스터 클라이언트 ");
				playerObj = PhotonNetwork.Instantiate ("animal",
					Vector3.zero, //TestCube
					Quaternion.identity, 0);

				playerObj.SetActive(false);

        	} else {
				
				// camera2.SetActive(true);
				roomNameUI2.text = "Room Name  < " + PhotonNetwork.room.Name + " > ";
				// TimeUI2.text = "10.00";
				user21name.text = PhotonNetwork.masterClient.NickName; 
				user22name.text = masterName;
				// userEmpty = false;
				
				// Debug.Log ("나는 그냥 클라이언트 ");
				playerObj = PhotonNetwork.Instantiate ("animal",
					Vector3.zero,
					user2ro,
					0);
				
				playerObj.SetActive(false);
        	}

			

		// }else {

		// 	// userObj.transform.position = pos;
		// }

	}


	private bool newUser = false;
	private float currentTime;
	void Update()
	{
		// if (masterUser) { //마스터 클라이언트 인지 ( 방장 )

		// 	if (newUser) { // 상대방이 접속했을때 카운트 시작

		// 		if (sumTime < 10f) {

		// 			sumTime += Time.deltaTime;
		// 			currentTime = tenTime - sumTime;
		// 			TimeUI1.text = "Time  " + string.Format("{0:N2}" , currentTime );

		// 			if (currentTime <= 0f) {
		// 				TimeUI1.text = "Time  " + "0.00";
						
		// 				if (readyP1 == false) {
		// 					randomPos();
		// 					hideButton();
		// 				}
		// 			}
		// 		}
		// 	}
			
		// }else {  // 두번째 플레이어

		// 	if (sumTime < 10f) {

		// 		sumTime += Time.deltaTime;
		// 		currentTime = tenTime - sumTime;
		// 		TimeUI2.text = "Time  " + string.Format("{0:N2}" , currentTime );

		// 		if (currentTime <= 0f) {
		// 			TimeUI2.text = "Time  " + "0.00";
					
		// 			if (readyP2 == false) {
		// 				randomPos();
		// 				hideButton();
		// 			}
		// 		}

		// 	}
		// }

		
	}

	/// <summary>
	/// start 보다 먼저 호출됨
	/// </summary>

	void OnLevelWasLoaded (int levelNumber) {
        // Photon 룸 안이 아니라면 네트워크에 문제가 있을지도..

        if (!PhotonNetwork.inRoom) return;
        
        Debug.Log ("게임에 들어왔어요, yay~");
		// Debug.Log("levelNumber = " + levelNumber );
        // Debug.Log("player name = " + PhotonNetwork.playerName );
        // Debug.Log("player AuthValues = " + PhotonNetwork.AuthValues );

        if (PhotonNetwork.isMasterClient) {

			camera1.SetActive(true);
            Debug.Log ("나는 마스터 클라이언트 ");
         
        } else {

			camera2.SetActive(true);
            Debug.Log ("나는 그냥 클라이언트 ");
           
        }

    }

	

	/// <summary>
	/// 위치 선택했을때 호출
	/// </summary>
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
	public void randomPos() {
		
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

	/// <summary>
	/// 위치 선택했을때 버튼 10개 숨김
	/// </summary>
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
	/// <summary>
	/// 10초후에 나가기 버튼 사라지게
	/// </summary>
	public void hideExitButton() {

		if (masterUser) {
			exitButton1.SetActive(false);
		}else {
			exitButton2.SetActive(false);
		}

	}


	/// <summary>
	/// 방 나가기 버튼 클릭
	/// </summary>
	public void clickExitButotn() {

		PhotonNetwork.LeaveRoom();
	}

	/// <summary>
	/// 방 나가면 원래 씬으로 돌아감
	/// </summary>
	override public void OnLeftRoom()
    {
        Debug.Log("OnLeftRoom (local)");

        // back to main menu
        SceneManager.LoadScene("SampleScene");
    }


	/// <summary>
	/// 연결안됬을때? 원래 씬으로
	/// </summary>
    override public void OnDisconnectedFromPhoton()
    {
        Debug.Log("OnDisconnectedFromPhoton");

        // back to main menu
        SceneManager.LoadScene("SampleScene");
    }

	
	/// <summary>
	/// 마스터 입장에서 새로운 클라이언트 접속했을때
	/// </summary>

	override public void OnPhotonPlayerConnected(PhotonPlayer newPlayer) {

		Debug.Log(" 원격 사용자 접속 ! ");
		loadingPopUp.SetActive(false);
		newUser = true;
		user12name.text = newPlayer.NickName;
		inRoomRoundTimer.joinUser = true;
		Debug.Log("start gameM ");
		inRoomRoundTimer.StartRoundNow(); // 10초 카운트 시작
		// playerObj.GetComponent<Playerinfo>().masterName = masterName;
		// playerObj.GetComponent<Playerinfo>().sendName();

	}


}
