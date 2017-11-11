using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour {
	[HideInInspector]
	public CameraController[] cameras;
	public CameraController activeCamera;
	public Transform cameraTransform;
	public float rotationIntensity;

	// Use this for initialization
	void Start () {
		cameras = FindObjectsOfType<CameraController>();
		InitializeCameras();
		CameraSwitch(1);
		
		
	}
	
	// Update is called once per frame
	void Update () {
		int a = CameraSwitchCheck();
		if (a != 0)
		{
			CameraSwitch(a);			
		}
		
	}

	void InitializeCameras(){
		foreach(CameraController c in cameras)
		{
			c.cameraManager = this;
			c.Initialize();
		
		}
	}

	void CameraSwitch(int value)
	{
		bool foundMatch = false;
			for (int i = 0; i < cameras.Length; i++)
			{
				if (cameras[i].cameraId == value)
				{
					if (activeCamera!=null)
					{
						activeCamera.isActiveCamera = false;
						activeCamera.ResetPosition();
					}
					activeCamera = cameras[i];
					activeCamera.CutToThisCamera();
					foundMatch = true;

				}
				
			}
			if (!foundMatch)
			{
				Debug.Log ("Did not find matching camera");
			}
	}


	int CameraSwitchCheck()
	{
		if (Input.GetKeyDown(KeyCode.Alpha1))
		{
			return 1;
		} else if (Input.GetKeyDown(KeyCode.Alpha2))
		{
			return 2;
		} else if (Input.GetKeyDown(KeyCode.Alpha3))
		{
			return 3;
		} else if (Input.GetKeyDown(KeyCode.Alpha4))
		{
			return 4;
		} else if (Input.GetKeyDown(KeyCode.Alpha5))
		{
			return 5;
		} else if (Input.GetKeyDown(KeyCode.Alpha6))
		{
			return 6;
		} else if (Input.GetKeyDown(KeyCode.Alpha7))
		{
			return 7;
		} else if (Input.GetKeyDown(KeyCode.Alpha8))
		{
			return 8;
		} else if (Input.GetKeyDown(KeyCode.Alpha9))
		{
			return 9;
		} else {
			return 0;
		}
	}

}
