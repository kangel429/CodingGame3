using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PhotonManager : Photon.PunBehaviour {
    public static PhotonManager instance;
    public static GameObject localPlayer;
    public static string roomName = "room1";
    public PhotonLogLevel logLevel;
    public Button join_button;
    public GameObject roomCell;
    public Transform listContent;
    public GameObject noRoomImg;
    private TypedLobby lobby;

    public InputField inputRoomName;
    public GameObject roomPopup;
    void Awake () {
 
        // if (instance != null) {
        //     Debug.Log("instance not null");
        //     // DestroyImmediate (gameObject);
        //     return;
        // }
        Debug.Log("photonManager Awake ");
        // DontDestroyOnLoad (gameObject);
        // instance = this;

        // this makes sure we can use PhotonNetwork.LoadLevel() on the master client and all clients in the same room sync their level automatically
        PhotonNetwork.automaticallySyncScene = true;
        PhotonNetwork.autoJoinLobby = false;
        // the following line checks if this client was just created (and not yet online). if so, we connect
        if (PhotonNetwork.connectionStateDetailed == ClientState.PeerCreated) {
            PhotonNetwork.ConnectUsingSettings ("1.0");
        }
        
        // PhotonNetwork.automaticallySyncScene = true;
    }

    void Start () {
        // PhotonNetwork.playerName = "aa";
        if (join_button) {
            join_button.enabled = false;
        }
        
        // PhotonNetwork.ConnectUsingSettings ("TanksPUN_v1.0");
        // ScrollRect scrollRect = GameObject.Find("Scroll View").GetComponent<ScrollRect>();
        // scrollRect.verticalScrollbar.value = 0.5f; //스크롤 중간 1.0 맨위?
    }

    public override void OnConnectedToMaster () {
        // MasterServer와 연결 후,  Button (UI) 이 생기면 그외 다른 액션도 실행할 수 있습니다.
        Debug.Log ("Master Server에 연결되었습니다");
        if (join_button) {
            join_button.enabled = true;
        }
        lobby = new TypedLobby ("my lobby", LobbyType.Default);
        PhotonNetwork.JoinLobby (lobby);
        // getRooms ();
    }

    public void JoinCreateRoom() {
        
        string roomname = inputRoomName.text;
        if (roomname == "") {
            return;
        }
        RoomOptions options = new RoomOptions ();
        options.MaxPlayers = 2;
        PhotonNetwork.JoinOrCreateRoom (roomname, options, lobby);

        // PhotonNetwork.JoinLobby()
        // PhotonNetwork.JoinRoom()
        // PhotonNetwork.JoinRandomRoom()
        // PhotonNetwork.CreateRoom()
        // PhotonNetwork.LeaveLobby()
        // PhotonNetwork.LeaveRoom()

        // PhotonNetwork.countOfPlayers;
        // PhotonNetwork.countOfRooms;
    }

    public override void OnJoinedRoom () {

        Debug.Log ("OnJoinedRoom 당신은 이미 게임 룸에 진입했습니다!!");

        // if(PhotonNetwork.isMasterClient)
        // {
        //    PhotonNetwork.LoadLevel("GameRoomScene");
        // }

        // PhotonNetwork.LoadLevel ("GameRoomScene");
        PhotonNetwork.LoadLevel ("MultiBattle");
    }

    // void OnLevelWasLoaded (int levelNumber) {
    //     // Photon 룸 안이 아니라면 네트워크에 문제가 있을지도..

    //     if (!PhotonNetwork.inRoom) return;
        
    //     Debug.Log ("게임에 들어왔어요, yay~");
    //     Debug.Log("player name = " + PhotonNetwork.playerName );
    //     Debug.Log("player AuthValues = " + PhotonNetwork.AuthValues );
    //     if (PhotonNetwork.isMasterClient) {
    //         Debug.Log ("나는 마스터 클라이언트 ");
    //         localPlayer = PhotonNetwork.Instantiate ("Player",
    //             new Vector3 (0, 0.5f, 0),
    //             Quaternion.identity, 0);
    //     } else {
    //         Debug.Log ("나는 그냥 클라이언트 ");
    //         PhotonNetwork.Instantiate ("Player",
    //             new Vector3 (0, 0.5f, 0),
    //             Quaternion.identity, 0);
    //     }

    // }

    public void OnPhotonCreateRoomFailed () {
        Debug.Log ("OnPhotonCreateRoomFailed");
        //이미 방이름 사용중일때 다른 방 이름 사용하세요
    }

    public void OnPhotonJoinRoomFailed () {
        Debug.Log ("OnPhotonJoinRoomFailed");
        //방이 없거나 가득차있거나 닫힌 경우
    }

    public void OnPhotonRandomJoinFailed () {
        Debug.Log ("OnPhotonRandomJoinFailed");
        //임의의 방에 참여할수 없거나 사용할 수 있는 방이 없을때
    }

    override public void OnReceivedRoomListUpdate () {
        Debug.Log ("OnReceivedRoomListUpdate");
        getRooms ();
    }

    public void getRooms () {
        int count = listContent.childCount;
        for (int i = 0; i < count; i++ ) {
            Destroy( listContent.GetChild(i).gameObject );
        }
        if (PhotonNetwork.GetRoomList ().Length == 0) {
            Debug.Log ("no room ");
            if (noRoomImg != null) {
                noRoomImg.SetActive (true);
            }
            return;
        }
        if (noRoomImg != null) {
            noRoomImg.SetActive (false);
        }
        foreach (RoomInfo roomInfo in PhotonNetwork.GetRoomList ()) {
            GameObject cell = Instantiate (roomCell);
            RoomCell rcell = cell.GetComponent<RoomCell> ();
            rcell.roomName.text = roomInfo.Name;
            rcell.playerCount.text = roomInfo.PlayerCount + " / " + roomInfo.MaxPlayers;
            rcell.joinButton.onClick.AddListener (() => {
                PhotonNetwork.JoinRoom (roomInfo.Name);
            });
            cell.transform.SetParent (listContent);
        }
    }

    override public void OnDisconnectedFromPhoton () {
        Debug.Log ("Disconnected from Photon.");
    }

    override public void OnCreatedRoom () {
        Debug.Log ("OnCreatedRoom");
        // PhotonNetwork.LoadLevel(SceneNameGame);
    }

    public void addCellButton () {

        GameObject cell = Instantiate (roomCell);
        RoomCell rcell = cell.GetComponent<RoomCell> ();
        rcell.roomName.text = "room name";
        rcell.playerCount.text = "0명" + " / " + "5명";
        rcell.joinButton.onClick.AddListener (() => {
            PhotonNetwork.JoinRoom ("roomInfo.Name");
        });
        cell.transform.SetParent (listContent);
    }


    public void showRoomPopUp() {
        
        roomPopup.SetActive(true);

    }

    public void hideRoomPopUp() {
        
        roomPopup.SetActive(false);

    }

}