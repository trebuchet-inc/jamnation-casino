using UnityEngine;
using NewtonVR;

public class NetworkManager : Photon.PunBehaviour
{
    public static NetworkManager Instance;
    
    public string serverIp;
    public GUIStyle skin;

    bool _roomCreator = false;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        PhotonNetwork.ConnectToMaster(serverIp, 5055, "64d0546d-f744-41eb-8817-1db17103b312", "0.1");
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("JoinRandom");
        PhotonNetwork.JoinRandomRoom();
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected");
        PhotonNetwork.JoinRandomRoom();
    }

    public void OnPhotonRandomJoinFailed()
    {
        Debug.Log("No Room Found");
        PhotonNetwork.CreateRoom(null);
        _roomCreator = true;
        Debug.Log("Room Created");
    }

    public override void OnJoinedRoom()
    {
        int id = _roomCreator ? 0 : 1;
        NetworkPlayerManager.Instance.personalID = id;
        NetworkPlayerManager.Instance.photonView.RPC("SpawnNetworkPlayer", PhotonTargets.OthersBuffered, Vector3.zero, Quaternion.identity, id);
    }
    
    public override void OnLeftRoom()
    {
        NetworkPlayerManager.Instance.photonView.RPC("DestroyNetworkPlayer", PhotonTargets.Others, NetworkPlayerManager.Instance.personalID);
        print("DestroyNetworkPlayer");
    }

    public void OnGUI()
    {
        GUI.skin.label = skin;
        GUILayout.Label(PhotonNetwork.connectionStateDetailed.ToString());
    }
}
