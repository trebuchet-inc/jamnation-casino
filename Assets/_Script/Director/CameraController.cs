using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CameraControllerType
{
	Tripod,
	AimLocked,
	ToggleState,
	AnimationRoutine
}

public class CameraController : MonoBehaviour {


	public CameraControllerType cameraType;
	public int cameraId;
	public bool isActiveCamera;
	

	[HideInInspector]
	public CameraManager cameraManager;

	private Quaternion _sourceLocalRotation;
	private Vector3 _sourceLocalPosition;
	private float _rotationIntensity;
	private float _translateIntensity;


	//CameraAimLocked && Routine
	private Transform[] _mainTargets;
	private int _currentTarget;





	// Use this for initialization
	public void Initialize () {
		_rotationIntensity = cameraManager.rotationIntensity;
		_translateIntensity = cameraManager.translationIntensity;
		switch (cameraType)
			{
				case CameraControllerType.Tripod:
				break;

				case CameraControllerType.AimLocked:
				foreach(AimCameraSpeadsheet a in cameraManager.aimCameraTargets)
				{
					if (a.cameraId == cameraId)
					{
						_mainTargets = a.targets;
					}
				}
				_currentTarget = 0;
				
				break;

				case CameraControllerType.ToggleState:
				break;

				// case 
				
				default:
				break;
			}

	}
	
	// Update is called once per frame
	void Update () {
		if (isActiveCamera)
		{
			switch (cameraType)
			{
				case CameraControllerType.Tripod:
					transform.Rotate(-Input.GetAxis("Vertical")*_rotationIntensity*Time.deltaTime,
					Input.GetAxis("Horizontal")*_rotationIntensity*Time.deltaTime,
					0, Space.Self);	
				break;

				case CameraControllerType.AimLocked:
					transform.Translate(Input.GetAxis("Horizontal")*_translateIntensity*Time.deltaTime,
					Input.GetAxis("Vertical")*_translateIntensity*Time.deltaTime,
					0);
					if (Input.GetKeyDown(KeyCode.RightControl))
					{
						_currentTarget = (_currentTarget+1)%_mainTargets.Length;
					}

				break;

				case CameraControllerType.ToggleState:
				break;
				
				default:
				break;
			}
		
				
			

		}
		
	}

	void LateUpdate()
	{
		switch (cameraType)
			{
				case CameraControllerType.Tripod:
				break;

				case CameraControllerType.AimLocked:
				Quaternion aim = Quaternion.LookRotation(_mainTargets[_currentTarget].position-transform.position);
				transform.rotation = Quaternion.Lerp(transform.rotation,aim,3*Time.deltaTime);

				break;

				case CameraControllerType.ToggleState:
				break;
				
				default:
				break;
			}	
	}

	public void CutToThisCamera()
	{
		isActiveCamera = true;
		Input.ResetInputAxes();

		switch (cameraType)
			{
				case CameraControllerType.Tripod:
				break;

				case CameraControllerType.AimLocked:
				transform.rotation = Quaternion.LookRotation(_mainTargets[_currentTarget].position-transform.position);
				break;

				case CameraControllerType.ToggleState:
				break;
				
				default:
				break;
			}	
		_sourceLocalRotation = transform.localRotation;
		_sourceLocalPosition = transform.localPosition;

		cameraManager.cameraTransform.SetParent(transform);
		cameraManager.cameraTransform.localPosition = Vector3.zero;
		cameraManager.cameraTransform.localRotation = Quaternion.identity;
		

	}

	public void ResetPosition()
	{
		transform.localRotation = _sourceLocalRotation;
		transform.localPosition = _sourceLocalPosition;
	}
}
