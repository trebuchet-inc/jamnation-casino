using UnityEngine;
using NewtonVR;

public class NetworkManager : Photon.PunBehaviour
{
    public static NetworkManager Instance;
    
    public bool LAN;
    public string defaultLanIP = "192.168.1.102";
    [Space]
    public bool debuging;
    public GUIStyle debugSkin;

    string _debug;
    string _ipOverride = "";
    bool _roomCreator = false;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        _debug = "Enter IP (default " + (LAN ? "LAN : " + defaultLanIP : "Online") + ")";
    }
    
    void Update()
    {
        if(!PhotonNetwork.connected) ManageSetting();
        ManageConection();
        ManageDebugVisibility();
    }

    void ManageDebugVisibility()
    {
        if(Input.GetKeyDown(KeyCode.D))
        {
            debuging = !debuging;
        } 
    }

    void ManageSetting()
    {
        string input = Tools.GetNumberInput();
        if(input.Length > 0)
        {
            _ipOverride += input;
        }
        if(Input.GetKeyDown(KeyCode.Backspace))
        {
            _ipOverride = "";
        }

        if(Input.GetKeyDown(KeyCode.C))
        {
            LAN = !LAN;
            _debug = "Enter IP (default " + (LAN ? "LAN : " + defaultLanIP : "Online") + ")";
        } 
    }

    void ManageConection()
    {  
        if(!PhotonNetwork.connected && Input.GetKeyDown(KeyCode.Return))
        {
            if(_ipOverride.Length <= 0)
            {
                if(LAN)
                {
                    _ipOverride = defaultLanIP;
                    PhotonNetwork.ConnectToMaster(defaultLanIP, 5055, "64d0546d-f744-41eb-8817-1db17103b312", "0.1");
                } 
                else
                {
                    _ipOverride = "Online";
                    PhotonNetwork.ConnectUsingSettings("0.1");
                }
            }
            else
            {
                PhotonNetwork.ConnectToMaster(_ipOverride, 5055, "64d0546d-f744-41eb-8817-1db17103b312", "0.1");
            }
        }
        else if(PhotonNetwork.connected)
        {
            _debug = PhotonNetwork.connectionStateDetailed.ToString();
        }
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
        PhotonNetwork.CreateRoom(null);
        _roomCreator = true;
        Debug.Log("Room Created");
    }

    public override void OnJoinedRoom()
    {
        int id = _roomCreator ? 0 : 1;
        NetworkPlayerManager.Instance.playerID = id;
        NetworkPlayerManager.Instance.SetLocalPlayer();
        NetworkPlayerManager.Instance.photonView.RPC("SpawnNetworkPlayer", PhotonTargets.OthersBuffered, Vector3.zero, Quaternion.identity, id);

        if (id >= 1)
        {
            GameRefereeManager.Instance.NewGame();
        }
    }
    
    public override void OnLeftRoom()
    {
        NetworkPlayerManager.Instance.photonView.RPC("DestroyNetworkPlayer", PhotonTargets.Others, NetworkPlayerManager.Instance.playerID);
        print("DestroyNetworkPlayer");
    }

    public void OnGUI()
    {
        if(!debuging) return;

        GUI.skin.label = debugSkin;
        GUILayout.Label(_debug + " - " + _ipOverride);
    }
}
