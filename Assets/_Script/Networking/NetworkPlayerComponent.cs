using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NewtonVR;

public class NetworkPlayerComponent : MonoBehaviour 
{
	public int id;

	[HideInInspector] public GameObject mountModel;
	bool _mounFreeze = false;

	[HideInInspector] public GameObject[] playerParts;
	List<NetworkPlayerData> _dataBuffer;
	NetworkPlayerData _lastData;
	[HideInInspector] public NVRVirtualHand[] _hands;

	int lerpSpeed = 20;

	bool _initialized;

	void Start()
	{
		Initialize();
		
	}

	void Initialize()
	{
		_initialized = true;
		playerParts = new GameObject[3];
		_hands = new NVRVirtualHand[2];
		_dataBuffer = new List<NetworkPlayerData>();
		setPlayerPart();
	}

	void FixedUpdate()
	{
		readData();

		if(!_mounFreeze)
		{
			mountModel.transform.position = transform.position;
			mountModel.transform.rotation = transform.rotation;
		}
	}

	void readData()
	{
		if(_lastData == null) return;

		//Gestion du dernier package

		transform.position = Vector3.Lerp(transform.position, _lastData.positions[0].Deserialize(), Time.deltaTime * lerpSpeed);
		transform.rotation = Quaternion.Lerp(transform.rotation,  _lastData.rotations[0].Deserialize(), Time.deltaTime * lerpSpeed);

		for(int i = 0; i < playerParts.Length; i++)
		{
			if(playerParts[i] != null)
			{
				playerParts[i].transform.localPosition = Vector3.Lerp(playerParts[i].transform.localPosition, _lastData.positions[i + 1].Deserialize(), Time.deltaTime * lerpSpeed);
				playerParts[i].transform.localRotation = Quaternion.Lerp(playerParts[i].transform.localRotation, _lastData.rotations[i + 1].Deserialize(), Time.deltaTime * lerpSpeed);
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
				case "head" :
				playerParts[0] = transform.GetChild(i).gameObject;
				break;

				case "rightHand" :
				playerParts[1] = transform.GetChild(i).gameObject;
				_hands[1] = playerParts[1].GetComponent<NVRVirtualHand>();
				break;

				case "leftHand" :
				playerParts[2] = transform.GetChild(i).gameObject;
				_hands[0] = playerParts[2].GetComponent<NVRVirtualHand>();
				break;
			}
		}
		print("setPlayerPart");
	}
}
