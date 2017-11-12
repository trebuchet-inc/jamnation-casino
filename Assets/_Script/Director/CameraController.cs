using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.Rendering.PostProcessing;

public enum CameraControllerType
{
	Tripod,
	AimLocked,
	// ToggleState,
	AnimationTargetedRoutine,
	AnimationFreeAimRoutine
}

public class CameraController : MonoBehaviour {


	public CameraControllerType cameraType;
	public int cameraId;
	public bool isActiveCamera;
	

	[HideInInspector]
	public CameraManager cameraManager;
	[HideInInspector]
	public CameraThrottler throttleGuider;
	[HideInInspector]
	public float apertureIntense;
	[HideInInspector]
	public Transform cameraParent;


	// private PostProcessProfile _postFxs;
	// private DepthOfField _DoF;
	// private float _apertureDefault;
	// private float _focusDistanceDefault;

	private Quaternion _sourceLocalRotation;
	private Vector3 _sourceLocalPosition;
	private float _rotationIntensity;
	private float _translateIntensity;


	//CameraAimLocked && Routine
	private Transform[] _mainTargets;
	private int _currentTarget;

	//Camera Routine Target




	// Use this for initialization
	public void Initialize () {
		_rotationIntensity = cameraManager.rotationIntensity;
		_translateIntensity = cameraManager.translationIntensity;
		cameraParent = transform.GetChild(0);
		// apertureIntense = cameraManager.apertureIntense;
		throttleGuider = transform.GetComponentInChildren<CameraThrottler>();
		throttleGuider.translationIntensity = _translateIntensity;
		throttleGuider.cameraBrother = this;
		throttleGuider.Initialize();
		
		switch (cameraType)
		{
			case CameraControllerType.Tripod:
			break;

			case CameraControllerType.AimLocked:
			InitializeTargets();
			break;

			case CameraControllerType.AnimationTargetedRoutine:
			InitializeTargets();
			throttleGuider.activated=true;
			break;
			case CameraControllerType.AnimationFreeAimRoutine:
			throttleGuider.activated=true;
			break;
			default:
			break;
		}
		
		// _postFxs  = FindObjectOfType<PostProcessVolume>().profile;
		// _DoF =;
		// _postFxs.
		// _focusDistanceDefault = _DoF.focusDistance;
		// _apertureDefault = _DoF.aperture;


	}
	
	// Update is called once per frame
	void Update () {
		if (isActiveCamera)
		{
			switch (cameraType)
			{
				case CameraControllerType.Tripod:
					cameraParent.Rotate(
					-Input.GetAxis("Vertical")*_rotationIntensity*Time.deltaTime,
					Input.GetAxis("Horizontal")*_rotationIntensity*Time.deltaTime,
					-Input.GetAxis("Yaw")*_rotationIntensity*Time.deltaTime,
					Space.Self);	
				break;

				case CameraControllerType.AimLocked:
					cameraParent.Translate(Input.GetAxis("Horizontal")*_translateIntensity*Time.deltaTime,
					Input.GetAxis("Yaw")*_translateIntensity*Time.deltaTime,
					Input.GetAxis("Vertical")*_translateIntensity*Time.deltaTime
					);
					if (Input.GetKeyDown(KeyCode.RightControl))
					{
						_currentTarget = (_currentTarget+1)%_mainTargets.Length;
					}
				break;

				default:
				break;
			}
		}

		switch (cameraType)
		{
			case CameraControllerType.AnimationTargetedRoutine:
			cameraParent.position = Vector3.Lerp(cameraParent.position, throttleGuider.transform.position,1*Time.deltaTime/2);
			if (isActiveCamera)
			{
				if (Input.GetKeyDown(KeyCode.RightControl))
				{
					_currentTarget = (_currentTarget+1)%_mainTargets.Length;
				}
			}		
			break;

			case CameraControllerType.AnimationFreeAimRoutine:
			cameraParent.position = Vector3.Lerp(cameraParent.position, throttleGuider.transform.position,1*Time.deltaTime/2);
			break;
			
			default:
			break;
		}		
	}

	void LateUpdate()
	{

		switch (cameraType)
			{
				case CameraControllerType.Tripod:
				break;

				case CameraControllerType.AimLocked:
				if (isActiveCamera)
				{
					AimAdjustment(_mainTargets[_currentTarget].position);
					// _DoF.focusDistance.value = (_mainTargets[_currentTarget].position-transform.position).magnitude;
				}
				
				break;

				case CameraControllerType.AnimationTargetedRoutine:
				if (isActiveCamera)
				{
					AimAdjustment(_mainTargets[_currentTarget].position);
					// _DoF.focusDistance.value = (_mainTargets[_currentTarget].position-transform.position).magnitude;
				}
				break;

				case CameraControllerType.AnimationFreeAimRoutine:
				AimAdjustment(throttleGuider.transform.position,0.3f);
				break;
				
				default:
				break;
			}	
	}

	public void CutToThisCamera()
	{
		isActiveCamera = true;
		Input.ResetInputAxes();
		// _DoF.aperture.value = _apertureDefault;
		// _DoF.focusDistance.value = _focusDistanceDefault;
		

		switch (cameraType)
			{
				case CameraControllerType.Tripod:
				break;

				case CameraControllerType.AimLocked:
				cameraParent.rotation = Quaternion.LookRotation(_mainTargets[_currentTarget].position-cameraParent.position);
				// _DoF.aperture.value = apertureIntense;
				break;

				case CameraControllerType.AnimationTargetedRoutine:
			cameraParent.rotation = Quaternion.LookRotation(_mainTargets[_currentTarget].position-cameraParent.position);
				// _DoF.aperture.value = apertureIntense;
				break;

				case CameraControllerType.AnimationFreeAimRoutine:
				break;
				
				default:
				break;
			}	
		SourcePositionAndAimOverride();

		cameraManager.cameraTransform.SetParent(cameraParent);
		cameraManager.cameraTransform.localPosition = Vector3.zero;
		cameraManager.cameraTransform.localRotation = Quaternion.identity;
		

	}

	public void ResetPositionAndAim()
	{
		cameraParent.localRotation = _sourceLocalRotation;
		cameraParent.localPosition = _sourceLocalPosition;
	}

	

	public void SourcePositionAndAimOverride()
	{
		_sourceLocalRotation = cameraParent.localRotation;
		_sourceLocalPosition = cameraParent.localPosition;
	}

	private void AimAdjustment(Vector3 target, float multiplier = 1)
	{
		Quaternion aim = Quaternion.LookRotation(target-cameraParent.position);
		cameraParent.rotation = Quaternion.Lerp(cameraParent.rotation,aim,2*Time.deltaTime*multiplier);
	}

	private void InitializeTargets()
	{
		foreach(AimCameraSpeadsheet a in cameraManager.aimCameraTargets)
				{
					if (a.cameraId == cameraId)
					{
						_mainTargets = a.targets;
					}
				}
				_currentTarget = 0;
	}
}
