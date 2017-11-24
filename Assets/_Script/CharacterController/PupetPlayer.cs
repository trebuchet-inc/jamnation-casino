using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PupetPlayer : MonoBehaviour
{
	public int lerpSpeed = 20;

	Transform[] _pupetParts;
	Transform[] _playerParts;

	void Start () 
	{
		_pupetParts = new Transform[4];
		_playerParts = new Transform[4];
		setParts();
	}
	
	void Update ()
	{
		for(int i = 0; i < _pupetParts.Length; i++)
		{
			LerpPart(i);
		}
	}

	void LerpPart(int id)
	{
		_pupetParts[id].position = Vector3.Lerp(_playerParts[id].position, _pupetParts[id].position, lerpSpeed * Time.deltaTime);
		_pupetParts[id].rotation = Quaternion.Lerp(_playerParts[id].rotation, _pupetParts[id].rotation, lerpSpeed * Time.deltaTime);
	}

	void setParts()
	{
		for(int i = 0; i < transform.childCount; i++)
		{
			switch(transform.GetChild(i).name)
			{
				case "Head" :
				_pupetParts[0] = transform.GetChild(i);
				break;

				case "RightHand" :
				_pupetParts[1] = transform.GetChild(i);
				break;

				case "LeftHand" :
				_pupetParts[2] = transform.GetChild(i);
				break;

				case "Torso" :
				_pupetParts[3] = transform.GetChild(i);
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
