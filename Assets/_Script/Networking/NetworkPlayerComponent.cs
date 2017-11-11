using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NewtonVR;

public class NetworkPlayerComponent : MonoBehaviour 
{
	public int id;

	[HideInInspector] public GameObject mountModel;
	bool _mounFreeze = false;

	GameObject[] _playerParts;
	List<NetworkPlayerData> _dataBuffer;
	NetworkPlayerData _lastData;
	NVRVirtualHand[] _hands;

	int lerpSpeed = 20;

	void Start()
	{
		_playerParts = new GameObject[3];
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

		for(int i = 0; i < _playerParts.Length; i++)
		{
			if(_playerParts[i] != null)
			{
				_playerParts[i].transform.localPosition = Vector3.Lerp(_playerParts[i].transform.localPosition, _lastData.positions[i + 1].Deserialize(), Time.deltaTime * lerpSpeed);
				_playerParts[i].transform.localRotation = Quaternion.Lerp(_playerParts[i].transform.localRotation, _lastData.rotations[i + 1].Deserialize(), Time.deltaTime * lerpSpeed);
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
				_playerParts[0] = transform.GetChild(i).gameObject;
				break;

				case "rightHand" :
				_playerParts[1] = transform.GetChild(i).gameObject;
				_hands[1] = _playerParts[1].GetComponent<NVRVirtualHand>();
				break;

				case "leftHand" :
				_playerParts[2] = transform.GetChild(i).gameObject;
				_hands[0] = _playerParts[2].GetComponent<NVRVirtualHand>();
				break;
			}
		}
		print("setPlayerPart");
	}
}
