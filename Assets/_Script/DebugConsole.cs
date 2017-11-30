using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugConsole : MonoBehaviour 
{
	public static DebugConsole Instance; 

	public KeyCode visibilityToggle;
	public Canvas canvas;
	public Text txt;

	public Action OnRefresh;

	[HideInInspector] public string debug
	{
		set
		{
			string formatedText  = value + "\n";
			_debug = formatedText;
		}
		get
		{
			return _debug;
		}
	}

	string _debug;

	bool _visible = true;

	void Awake()
	{
		Instance = this;
	}
	
	void Update () 
	{
		ManageDebugVisibility();
		Draw();
	}

	void ManageDebugVisibility()
    {
        if(Input.GetKeyDown(visibilityToggle))
        {
            _visible = !_visible;
			canvas.gameObject.SetActive(_visible);
        }
    }

	void Draw()
    {
        if(!_visible) return;
		
		_debug = "";
		if(OnRefresh != null) OnRefresh.Invoke();
        txt.text = _debug;
    }
}
