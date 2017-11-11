﻿using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System;
using System.IO;
using UnityEngine;
using NewtonVR;

[Serializable]
public class NetworkPlayerData
{
	public SerializableVector3[] positions;
	public SerializableQuaternion[] rotations;

	public NetworkPlayerData (Vector3[] pos, Quaternion[] rot)
	{
		positions = new SerializableVector3[pos.Length];
		for(int i = 0; i < positions.Length; i++)
		{
			positions[i] = new SerializableVector3(pos[i]);
		}

		rotations = new SerializableQuaternion[rot.Length];
		for(int i = 0; i < rotations.Length; i++)
		{
			rotations[i] = new SerializableQuaternion(rot[i]);
		}
	}
}

public class NetworkPlayerManager : Photon.MonoBehaviour 
{
	public static NetworkPlayerManager Instance;

	public int personalID;
	public GameObject playerPrefab;
	public GameObject mountPrefab;
	public List<NetworkPlayerComponent> players;
	public Transform[] startingPos;

	void Awake()
    {
        Instance = this;
    }

	void FixedUpdate()
	{
		if(!PhotonNetwork.connected) return;

		NetworkPlayerData data = new NetworkPlayerData(
			new Vector3[]{NVRPlayer.Instance.transform.position,NVRPlayer.Instance.Head.transform.localPosition, NVRPlayer.Instance.RightHand.transform.localPosition, NVRPlayer.Instance.LeftHand.transform.localPosition},
			new Quaternion[]{NVRPlayer.Instance.transform.rotation,NVRPlayer.Instance.Head.transform.localRotation, NVRPlayer.Instance.RightHand.transform.localRotation, NVRPlayer.Instance.LeftHand.transform.localRotation});
	
		BinaryFormatter formatter = new BinaryFormatter();
		byte[] serializedData = SerializationToolkit.ObjectToByteArray(data); 

		photonView.RPC("UpdateNetworkPlayer", PhotonTargets.Others, serializedData, personalID);
	}

	public void SetLocalPlayer()
	{
		NVRPlayer.Instance.transform.position = startingPos[personalID].position;
		NVRPlayer.Instance.transform.rotation = startingPos[personalID].rotation;
	}

	[PunRPC]
    void UpdateNetworkPlayer(byte[] data, int id)
    {
        foreach(NetworkPlayerComponent p in players)
		{
			if(p.id == id)
			{
				p.ReceiveData((NetworkPlayerData) SerializationToolkit.ByteArrayToObject(data));
				return;
			}
		}
    }

	[PunRPC]
    void SpawnNetworkPlayer(Vector3 pos, Quaternion rot, int id)
    {
        GameObject _newPlayer = Instantiate(playerPrefab, startingPos[id].position, startingPos[id].rotation);
        NetworkPlayerComponent _networkPlayer = _newPlayer.GetComponent<NetworkPlayerComponent>();

		_networkPlayer.mountModel = Instantiate(mountPrefab, startingPos[id].position, startingPos[id].rotation);
		_networkPlayer.id = id;
		players.Add(_networkPlayer);
    }

	[PunRPC]
    void DestroyNetworkPlayer(int id)
    {
        foreach(NetworkPlayerComponent p in players)
		{
			if(p.id == id)
			{
				players.Remove(p);
				Destroy(p.gameObject);
				return;
			}
		}
    }
}
