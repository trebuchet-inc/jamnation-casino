using System;
using System.Collections;
using UnityEngine;

public class JoustPhase : GamePhase
{
	public event Action OnJoustGO;
	
	public event Action OnJoustHit;
	
	public override void StartPhase()
	{
		StartCoroutine(WaitForGo());
	} 
    
	public override void EndPhase()
	{
        
	}

	private IEnumerator WaitForGo()
	{
		yield return new WaitForSeconds(3);
		
		if(OnJoustGO != null) OnJoustGO.Invoke();
	}
}
