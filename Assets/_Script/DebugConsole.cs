using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugConsole : MonoBehaviour 
{
	public static DebugConsole Instance; 

	public KeyCode visibilityToggle;
	public GUIStyle debugSkin;

	public Action OnRefresh;

	[HideInInspector] public string debug;

	bool _visible = true;

	void Awake()
	{
		Instance = this;
	}
	
	void Update () 
	{
		ManageDebugVisibility();
	}

	void ManageDebugVisibility()
    {
        if(Input.GetKeyDown(visibilityToggle))
        {
            _visible = !_visible;
        } 
    }

	public void OnGUI()
    {
        if(!_visible) return;
		
		debug = "";
		if(OnRefresh != null) OnRefresh.Invoke();
        GUI.skin.label = debugSkin;
        GUILayout.Label(debug);
    }
}
