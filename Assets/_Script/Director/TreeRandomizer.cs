using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeRandomizer : MonoBehaviour {

	// Use this for initialization
	void Start () {

		transform.localScale = new Vector3(Random.Range(1.0f,1.5f),Random.Range(1.0f,1.5f),Random.Range(1.0f,1.5f));
		transform.rotation = Quaternion.Euler(Random.Range(-3f,3f),Random.Range(0,360),Random.Range(-3f,3f));
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
