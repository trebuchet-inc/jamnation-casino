using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

static class Tools 
{
 	public static bool In(this string source, params string[] list)
    {
        if (null == source) throw new ArgumentNullException("source");
        return list.Contains(source, StringComparer.OrdinalIgnoreCase);
    }

    public static void ReplaceMaterial(ref Material[] materials, string materialName, Material newMaterial)
	{
		for(int i = 0; i <  materials.Length; i++)
		{
            Debug.Log(materials[i].name);
			if(materials[i].name.In(materialName, materialName + " (Instance)")) materials[i] = newMaterial;
		}
	}
}
