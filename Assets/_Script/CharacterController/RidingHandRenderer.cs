using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RidingHandRenderer : MonoBehaviour {
	public LineRenderer[] lineRenderers;

	Transform _mountHead;

	void Start () 
	{
		SetHeadTarget();
	}

	void SetHeadTarget()
	{
		MountAgent agent = transform.parent.parent.GetComponent<MountAgent>();
		if(agent != null )
		{
			_mountHead = agent.mountModel.transform.Find("Head");
			return;
		}

		NetworkPlayerComponent npc = transform.parent.parent.GetComponent<NetworkPlayerComponent>();
		if(npc != null)
		{
			_mountHead = npc.mountModel.transform.Find("Head");
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
