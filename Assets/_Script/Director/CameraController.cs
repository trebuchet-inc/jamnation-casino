using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

	public int cameraId;
	public bool isActiveCamera;
	private Quaternion sourceLocalRotation;
	private Vector3 sourceLocalPosition;

	[HideInInspector]
	public CameraManager cameraManager;

	// Use this for initialization
	public void Initialize () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (isActiveCamera)
		{
			
			{
				transform.rotation.eulerAngles = ;				
			}

		}
		
	}

	void LateUpdate()
	{
		if (isActiveCamera)
		{
			cameraManager.cameraTransform.position = transform.position;
			cameraManager.cameraTransform.localRotation = transform.localRotation;
		}		
	}

	public void CutToThisCamera()
	{
		isActiveCamera = true;
		sourceLocalPosition = transform.localPosition;
		sourceLocalRotation = transform.localRotation;
		cameraManager.cameraTransform.position = transform.position;
		cameraManager.cameraTransform.localRotation = transform.localRotation;

	}

	public void ResetPosition()
	{
		transform.localRotation = sourceLocalRotation;
		transform.localPosition = sourceLocalPosition;
	}
}
