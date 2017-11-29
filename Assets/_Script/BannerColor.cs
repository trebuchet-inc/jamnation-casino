using UnityEngine;

public class BannerColor : MonoBehaviour
{
	public Material InitialMaterial;
	public Material Blue;
	public Material Red;

	private SkinnedMeshRenderer _renderer;

	private void Start()
	{
		_renderer = GetComponent<SkinnedMeshRenderer>();
		InitialMaterial = _renderer.material;
	}

	public void SetBlue()
	{
		_renderer.material = Blue;
	}

	public void SetRed()
	{
		_renderer.material = Red;
	}

	public void SetInitial()
	{
		_renderer.material = InitialMaterial;
	}
}
