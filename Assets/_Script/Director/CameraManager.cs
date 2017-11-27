using UnityEngine;
using System;

[Serializable]
public class AimCameraSpeadsheet
{
	public int cameraId;
	public Transform[] targets;
}

public class CameraManager : MonoBehaviour {
	[HideInInspector]
	public CameraController[] cameras;
	public CameraController activeCamera;
	public Transform cameraTransform;
	public float rotationIntensity;
	public float translationIntensity;
	public float apertureIntense;
	public AimCameraSpeadsheet[] aimCameraTargets;

	Camera cam;

	// Use this for initialization

	private void Awake()
	{
		cameras = FindObjectsOfType<CameraController>();
		
		foreach(CameraController c in cameras)
		{
			Destroy(c.transform.GetComponent<Camera>());
		}
		cam = cameraTransform.GetComponentInChildren<Camera>();
	}

	private void Start () 
	{
		InitializeCameras();
		CameraSwitch(1);
	}
	
	private void Update () 
	{
		int a = CameraSwitchCheck();
		
		if (a != 0)
		{
			CameraSwitch(a);			
		}

		if(Input.GetKeyDown(KeyCode.P))
		{
			cam.depth *= -1;
		}
	}

	private void InitializeCameras()
	{
		foreach(CameraController c in cameras)
		{
			c.cameraManager = this;
			c.Initialize();
		}
	}

	private void CameraSwitch(int value)
	{
		bool foundMatch = false;
		
		for (int i = 0; i < cameras.Length; i++)
		{
			if (cameras[i].cameraId == value)
			{
				if (activeCamera!=null)
				{
					activeCamera.isActiveCamera = false;
					if (activeCamera.cameraType == CameraControllerType.AnimationTargetedRoutine || activeCamera.cameraType == CameraControllerType.AnimationFreeAimRoutine)
					{
						activeCamera.SourcePositionAndAimOverride();
					}
					activeCamera.ResetPositionAndAim();
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


	private int CameraSwitchCheck()
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
