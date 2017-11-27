using UnityEngine;
using System;
using System.Collections;

[Serializable]
public class AimCameraSpeadsheet
{
	public string cameraId;
	public Transform[] targets;
}

public class CameraManager : FeedbackManager 
{
	[HideInInspector]
	public CameraController[] cameras;
	public CameraController activeCamera;
	public Transform cameraTransform;
	public float rotationIntensity;
	public float translationIntensity;
	public float apertureIntense;
	public AimCameraSpeadsheet[] aimCameraTargets;

	private int loopIndex = 0;

	Camera cam;

	private void Awake()
	{
		cameras = FindObjectsOfType<CameraController>();
		
		foreach(CameraController c in cameras)
		{
			Destroy(c.transform.GetComponent<Camera>());
		}
		cam = cameraTransform.GetComponentInChildren<Camera>();
	}

	protected override void Start () 
	{
		base.Start();
		InitializeCameras();
		CameraSwitch("1");
	}
	
	private void Update () 
	{
		int a = CameraSwitchCheck();
		
		if (a != 0)
		{
			CameraSwitch(a.ToString());			
		}

		if(Input.GetKeyDown(KeyCode.P))
		{
			SwitchToFirstPerson();
		}
	}
	
	//
	// Event handlers
	//
	
	protected override void OnPhaseStartedHandler(Phases phases)
	{
		StopCoroutine("CameraLoop");
		
		switch (phases) 
		{
			case Phases.WeaponSelection:
				StartCoroutine(CameraLoop(new string[] {"3", "7"}, 5));
				break;
				
			case Phases.Parade:
				CameraSwitch("4");
				break;
				
			case Phases.Joust:
				CameraSwitch("4");
				break;	
				
			case Phases.Intermission:
				break;
				
			case Phases.End:
				CameraSwitch("4");
				break;
		}
	}
	
	//
	// Camera functions
	//

	private IEnumerator CameraLoop(string[] cameraIDs, float interval)
	{
		CameraSwitch(cameraIDs[loopIndex]);

		if (loopIndex == cameraIDs.Length -1)
		{
			loopIndex = 0;
		}
		else
		{
			loopIndex++;
		}
		
		yield return new WaitForSeconds(interval);

		StartCoroutine(CameraLoop(cameraIDs, interval));
	}

	private void InitializeCameras()
	{
		foreach(CameraController c in cameras)
		{
			c.cameraManager = this;
			c.Initialize();
		}
	}

	private void SwitchToFirstPerson()
	{
		cam.depth *= -1;
	}

	private void CameraSwitch(string id)
	{
		bool foundMatch = false;
		
		for (int i = 0; i < cameras.Length; i++)
		{
			if (cameras[i].cameraId == id)
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
