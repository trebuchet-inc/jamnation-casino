using UnityEngine;
using UnityEngine.UI;

public class OverlayWeaponStatus : MonoBehaviour
{
	public Image icon;
	public Animator status;

	private void Start()
	{
		icon = GetComponent<Image>();
		status = GetComponentInChildren<Animator>();
	}
}
