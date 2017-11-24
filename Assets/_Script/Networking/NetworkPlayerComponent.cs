using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NewtonVR;

public class NetworkPlayerComponent : MonoBehaviour 
{
	public int id;

	[HideInInspector] public GameObject mountModel;
	bool _mountFreeze = false;

	[HideInInspector] public GameObject[] playerParts;
	List<NetworkPlayerData> _dataBuffer;
	NetworkPlayerData _lastData;
	[HideInInspector] public NVRVirtualHand[] _hands;

	bool _initialized;

	void Start()
	{
		if(!_initialized) Initialize();
	}

	void Initialize()
	{
		_initialized = true;
		playerParts = new GameObject[4];
		_hands = new NVRVirtualHand[2];
		_dataBuffer = new List<NetworkPlayerData>();
		setPlayerPart();
	}

	void FixedUpdate()
	{
		readData();

		if(!_mountFreeze)
		{
			mountModel.transform.position = transform.position;
			mountModel.transform.rotation = transform.rotation;
		}
	}

	void readData()
	{
		if(_lastData == null) return;

		//Gestion du dernier package

		transform.position = _lastData.positions[0].Deserialize();
		transform.rotation = _lastData.rotations[0].Deserialize();

		for(int i = 0; i < playerParts.Length; i++)
		{
			if(playerParts[i] != null)
			{
				playerParts[i].transform.localPosition = _lastData.positions[i + 1].Deserialize();
				playerParts[i].transform.localRotation =_lastData.rotations[i + 1].Deserialize();
			}
			else
			{
				setPlayerPart();
			} 
		}

		if(_dataBuffer.Count <= 0) return;

		//Gestion du buffer

		_dataBuffer.Clear();
	}

	public void ReceiveData(NetworkPlayerData data)
	{
		if(!_initialized) Initialize();
		_lastData = data;
		_dataBuffer.Add(data);
	}

	void setPlayerPart()
	{
		for(int i = 0; i < transform.childCount; i++)
		{
			switch(transform.GetChild(i).name)
			{
				case "Head" :
				playerParts[0] = transform.GetChild(i).gameObject;
				break;

				case "RightHand" :
				playerParts[1] = transform.GetChild(i).gameObject;
				_hands[1] = playerParts[1].GetComponent<NVRVirtualHand>();
				break;

				case "LeftHand" :
				playerParts[2] = transform.GetChild(i).gameObject;
				_hands[0] = playerParts[2].GetComponent<NVRVirtualHand>();
				break;

				case "Torso" :
				playerParts[3] = transform.GetChild(i).gameObject;
				break;
			}
		}
		print("setPlayerPart");
	}
}
