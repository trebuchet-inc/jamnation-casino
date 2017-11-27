using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class PupetPlayer : MonoBehaviour
{
	public bool isLocalPupet;
	[Space]
	public int lerpSpeed = 20;
	public Material[] playerColors;
	[Space]
	public float dyingPartSpeed = 1;

	PupetPart[] _pupetParts;
	Transform[] _playerParts;
	int pupetId;

	bool _killed;

	void Start () 
	{
		_pupetParts = new PupetPart[4];
		_playerParts = new Transform[4];
		setParts();
		GameRefereeManager.Instance.joustPhase.OnJoustHit += OnHitHandler;
		GameRefereeManager.Instance.intermissionPhase.OnRoundReset += OnRounReset;
		if(isLocalPupet) GameRefereeManager.Instance.OnNewGame += setColor;
	}
	
	void Update ()
	{
		if(_killed) return;

		for(int i = 0; i < _pupetParts.Length; i++)
		{
			LerpPart(i);
		}
	}

	void LerpPart(int id)
	{
		_pupetParts[id].transform.position = Vector3.Lerp(_playerParts[id].position, _pupetParts[id].transform.position, lerpSpeed * Time.deltaTime);
		_pupetParts[id].transform.rotation = Quaternion.Lerp(_playerParts[id].rotation, _pupetParts[id].transform.rotation, lerpSpeed * Time.deltaTime);
	}

	void OnHitHandler(HitInfo info)
	{
		if(info.playerHitting == NetworkPlayerManager.Instance.playerID) return;

		_killed = true;
		foreach(PupetPart part in _pupetParts)
		{
			part.Kill(transform.forward * dyingPartSpeed);
		}
	}

	void OnRounReset()
	{
		if(!_killed) return;

		_killed = false;

		foreach(PupetPart part in _pupetParts)
		{
			part.Revive(transform);
		}
	}

	public void setColor(int id)
	{
		pupetId = id;
		MeshRenderer[] renderers = GetComponentsInChildren<MeshRenderer>();
		Material[] temp;

		foreach(MeshRenderer renderer in renderers)
		{
			if(renderer.transform.name.Contains("model"))
			{
				if(renderer.transform.parent.name == "Head" && renderer.transform.name == "modelHelmet")
				{
					temp = renderer.materials;
					Tools.ReplaceMaterial(ref temp, "bleu_mat", playerColors[pupetId]);
					renderer.materials = temp;
				}
				else if (renderer.transform.parent.name.In("LeftHand","Torso","RightHand"))
				{
					temp = renderer.materials;
					Tools.ReplaceMaterial(ref temp, "bleu_mat", playerColors[pupetId]);
					renderer.materials = temp;
				}
			}
		}
	}

	public void setColor()
	{
		pupetId = NetworkPlayerManager.Instance.playerID;
		MeshRenderer[] renderers = GetComponentsInChildren<MeshRenderer>();
		Material[] temp;

		foreach(MeshRenderer renderer in renderers)
		{
			if(renderer.transform.name.Contains("model"))
			{
				if(renderer.transform.parent.name == "Head" && renderer.transform.name == "modelHelmet")
				{
					temp = renderer.materials;
					Tools.ReplaceMaterial(ref temp, "bleu_mat", playerColors[pupetId]);
					renderer.materials = temp;
				}
				else if (renderer.transform.parent.name.In("LeftHand","Torso","RightHand"))
				{
					temp = renderer.materials;
					Tools.ReplaceMaterial(ref temp, "bleu_mat", playerColors[pupetId]);
					renderer.materials = temp;
				}
			}
		}

		GameRefereeManager.Instance.OnNewGame -= setColor;
	}

	void setParts()
	{
		PupetPart[] temp = GetComponentsInChildren<PupetPart>();

		for(int i = 0; i < temp.Length; i++)
		{
			switch(temp[i].transform.name)
			{
				case "Head" :
				_pupetParts[0] = temp[i];
				break;

				case "RightHand" :
				_pupetParts[1] = temp[i];
				break;

				case "LeftHand" :
				_pupetParts[2] = temp[i];
				break;

				case "Torso" :
				_pupetParts[3] = temp[i];
				break;
			}
		}
		
		for(int i = 0; i < transform.parent.childCount; i++)
		{
			switch(transform.parent.GetChild(i).name)
			{
				case "Head" :
				_playerParts[0] = transform.parent.GetChild(i);
				break;

				case "RightHand" :
				_playerParts[1] = transform.parent.GetChild(i);
				break;

				case "LeftHand" :
				_playerParts[2] = transform.parent.GetChild(i);
				break;

				case "Torso" :
				_playerParts[3] = transform.parent.GetChild(i);
				break;
			}
		}
	}
}
