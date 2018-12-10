using ExitGames.Client.Photon;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Simple script that uses a property to sync a start time for a multiplayer game.
/// </summary>
/// <remarks>
/// When entering a room, the first player will store the synchronized timestamp.
/// You can't set the room's synchronized time in CreateRoom, because the clock on the Master Server
/// and those on the Game Servers are not in sync. We use many servers and each has it's own timer.
///
/// Everyone else will join the room and check the property to calculate how much time passed since start.
/// You can start a new round whenever you like.
///
/// Based on this, you should be able to implement a synchronized timer for turns between players.
/// </remarks>
public class InRoomRoundTimer : MonoBehaviour
{
    public int SecondsPerTurn = 5;                  // time per round/turn
    public double StartTime;                        // this should could also be a private. i just like to see this in inspector
    public Rect TextPos = new Rect(50,80,400,700);   // default gui position. inspector overrides this!

    private bool startRoundWhenTimeIsSynced;        // used in an edge-case when we wanted to set a start time but don't know it yet.
    private const string StartTimeKey = "st";       // the name of our "start time" custom property.

    public Text timeUI1 , timeUI2;
    public bool joinUser = false;
    private bool user2 = false;
    public GameM gameManager;

    public void StartRoundNow()
    {
        Debug.Log("startRoundNow() call ");
        // in some cases, when you enter a room, the server time is not available immediately.
        // time should be 0.0f but to make sure we detect it correctly, check for a very low value.
        if (PhotonNetwork.time < 0.0001f)
        {
            // we can only start the round when the time is available. let's check that in Update()
            startRoundWhenTimeIsSynced = true;
            return;
        }
        startRoundWhenTimeIsSynced = false;



        ExitGames.Client.Photon.Hashtable startTimeProp = new Hashtable();  // only use ExitGames.Client.Photon.Hashtable for Photon
        startTimeProp[StartTimeKey] = PhotonNetwork.time;
        PhotonNetwork.room.SetCustomProperties(startTimeProp);              // implement OnPhotonCustomRoomPropertiesChanged(Hashtable propertiesThatChanged) to get this change everywhere
    }


    /// <summary>Called by PUN when this client entered a room (no matter if joined or created).</summary>
    public void OnJoinedRoom()
    {
        
        if (PhotonNetwork.isMasterClient)
        {
          
        }
        else
        {
            user2 = true;
            Debug.Log("start inroom");
            this.StartRoundNow();
            // as the creator of the room sets the start time after entering the room, we may enter a room that has no timer started yet
            // Debug.Log("StartTime already set: " + PhotonNetwork.room.CustomProperties.ContainsKey(StartTimeKey));
        }
    }

    /// <summary>Called by PUN when new properties for the room were set (by any client in the room).</summary>
    public void OnPhotonCustomRoomPropertiesChanged(Hashtable propertiesThatChanged)
    {
        Debug.Log(">OnPhotonCustomRoomPropertiesChanged");
        if (propertiesThatChanged.ContainsKey(StartTimeKey))
        {
            StartTime = (double)propertiesThatChanged[StartTimeKey];
        }
    }

    /// <remarks>
    /// In theory, the client which created the room might crash/close before it sets the start time.
    /// Just to make extremely sure this never happens, a new masterClient will check if it has to
    /// start a new round.
    /// </remarks>
    public void OnMasterClientSwitched(PhotonPlayer newMasterClient)
    {
        Debug.Log(">OnMasterClientSwitched");
        if (!PhotonNetwork.room.CustomProperties.ContainsKey(StartTimeKey))
        {
            Debug.Log("The new master starts a new round, cause we didn't start yet.");
            this.StartRoundNow();
        }
    }

    private string countTime;
    private bool timeOver = true;
    private double remainingTime;
    void Update()
    {
        if (timeOver) {

            double elapsedTime = (PhotonNetwork.time - StartTime);
            remainingTime = SecondsPerTurn - (elapsedTime % SecondsPerTurn);
            
            if ( joinUser && remainingTime < 0.02f ) {

                Debug.Log(" join and time over ");
                timeUI1.text = "time: 0.00 ";
                timeOver = false;
                gameManager.hideExitButton();

                if (gameManager.readyP1 == false) {
                    gameManager.randomPos();
                    gameManager.hideButton();
                }
                return;
            }
            if (user2) {

                if (remainingTime < 0.02f) {
                    
                    Debug.Log(" join and time over 2 ");
                    timeUI2.text = "time: 0.00 ";
                    timeOver = false;
                    gameManager.hideExitButton();

                    if (gameManager.readyP2 == false) {
                        gameManager.randomPos();
                        gameManager.hideButton();
                    }
                    return;
                }
            }

            if (startRoundWhenTimeIsSynced)
            {
                this.StartRoundNow();   // the "time is known" check is done inside the method.
            }
            countTime = string.Format("time: {0:0.00}", remainingTime);

            timeUI1.text = countTime;
            timeUI2.text = countTime;
        }
        
    }

    // GUIStyle style = new GUIStyle();
    // public void OnGUI()
    // {

    //     style.fontSize = 40;
    //     // alternatively to doing this calculation here:
    //     // calculate these values in Update() and make them publicly available to all other scripts
    //     // double elapsedTime = (PhotonNetwork.time - StartTime);
    //     // remainingTime = SecondsPerTurn - (elapsedTime % SecondsPerTurn);
       
    //     // int turn = (int)(elapsedTime / SecondsPerTurn);


    //     // simple gui for output
    //     GUILayout.BeginArea(TextPos);
    //     // GUILayout.Label(string.Format("elapsed: {0:0.000}", elapsedTime), style);
    //     // GUILayout.Label(string.Format("remaining: {0:0.000}", remainingTime), style);
    //     // GUILayout.Label(string.Format("turn: {0:0}", turn) , style);
    //     if (GUILayout.Button("new round" , style) )
    //     {
    //         this.StartRoundNow();
    //     }
    //     GUILayout.EndArea();
    // }



}