using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

	public int cameraId;
	public bool isActiveCamera;
	

	[HideInInspector]
	public CameraManager cameraManager;

	private Quaternion _sourceLocalRotation;
	private Vector3 _sourceLocalPosition;
	private float _rotationIntensity;

	// Use this for initialization
	public void Initialize () {
		_rotationIntensity = cameraManager.rotationIntensity;
	}
	
	// Update is called once per frame
	void Update () {
		if (isActiveCamera)
		{
		
		transform.Rotate(Input.GetAxis("Vertical")*_rotationIntensity*Time.deltaTime,
		Input.GetAxis("Horizontal")*_rotationIntensity*Time.deltaTime,
		0, Space.Self);				
			

		}
		
	}

	// void LateUpdate()
	// {
	// 	if (isActiveCamera)
	// 	{
	// 		cameraManager.cameraTransform.position = transform.position;
	// 		cameraManager.cameraTransform.localRotation = transform.localRotation;
	// 	}		
	// }

	public void CutToThisCamera()
	{
		isActiveCamera = true;
		_sourceLocalPosition = transform.localPosition;
		_sourceLocalRotation = transform.localRotation;

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
