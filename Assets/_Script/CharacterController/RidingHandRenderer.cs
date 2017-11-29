using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RidingHandRenderer : MonoBehaviour {
	public LineRenderer[] lineRenderers;

	PupetPart _part;
	Transform _mountHead;

	void Start () 
	{
		_part = GetComponent<PupetPart>();
		SetHeadTarget();
		_part.OnKilled += OnHit;
		_part.OnRevived += OnReset;
	}

	void OnHit()
	{
		foreach(LineRenderer line in lineRenderers)
		{
			line.enabled = false;
		}
	}

	void OnReset()
	{
		foreach(LineRenderer line in lineRenderers)
		{
			line.enabled = true;
		}
	}

	void SetHeadTarget()
	{
		MountAgent agent = transform.parent.parent.GetComponent<MountAgent>();
		if(agent != null )
		{
			_mountHead = agent.mountModel.transform.Find("model").Find("Head");
			return;
		}

		NetworkPlayerComponent npc = transform.parent.parent.GetComponent<NetworkPlayerComponent>();
		if(npc != null)
		{
			_mountHead = npc.mountModel.transform.Find("model").Find("Head");
			return;
		}
	}
	
	void Update () {
		for(int i = 0; i < lineRenderers.Length; i++)
		{
			lineRenderers[i].SetPosition(0, lineRenderers[i].transform.position);
			lineRenderers[i].SetPosition(1, _mountHead.position - (_mountHead.right * 0.05f) + (_mountHead.right * 0.1f * i));
		}
	}
}
