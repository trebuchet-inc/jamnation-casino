using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraThrottler : MonoBehaviour {

	private Transform[] _mainNodes;
	private int _currentNode;
	[HideInInspector]
	public float translationIntensity;
	[HideInInspector]
	public CameraController cameraBrother;
	[HideInInspector]
	public bool activated;


	// Use this for initialization
	public void Initialize () 
	{

		_mainNodes = new Transform[transform.parent.childCount-2];
		for (int i = 2; i < _mainNodes.Length+2; i++)
		{
			_mainNodes[i-2] = transform.parent.GetChild(i);					
		}
		_currentNode = 0;
		transform.position = _mainNodes[_currentNode].position;
		
	}
	
	// Update is called once per frame
	void Update () {
		if(activated)
		{
			if ((transform.position-_mainNodes[_currentNode].position).magnitude > 0.5f)
			{
				transform.Translate((_mainNodes[_currentNode].position-transform.position).normalized*translationIntensity*Time.deltaTime,Space.World);
			} 
			if ((transform.position-_mainNodes[_currentNode].position).magnitude < 1 && (transform.position-cameraBrother.transform.position).magnitude < 1)
			{
				_currentNode = (_currentNode+1)%_mainNodes.Length;
			}
		}
	}
}
