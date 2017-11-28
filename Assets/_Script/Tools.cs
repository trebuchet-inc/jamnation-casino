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
			if(materials[i].name.In(materialName, materialName + " (Instance)")) materials[i] = newMaterial;
		}
	}

	public static string GetNumberInput()
    {
       if(Input.GetKeyDown(KeyCode.Keypad1))
       {
		   return "1";
       }
       if(Input.GetKeyDown(KeyCode.Keypad2))
       {
           return "2";
       }
       if(Input.GetKeyDown(KeyCode.Keypad3))
       {
           return "3";
       }
       if(Input.GetKeyDown(KeyCode.Keypad4))
       {
           return "4";
       }
       if(Input.GetKeyDown(KeyCode.Keypad5))
       {
           return "5";
       }
       if(Input.GetKeyDown(KeyCode.Keypad6))
       {
           return "6";
       }
       if(Input.GetKeyDown(KeyCode.Keypad7))
       {
           return "7";
       }
       if(Input.GetKeyDown(KeyCode.Keypad8))
       {
           return "8";
       }
       if(Input.GetKeyDown(KeyCode.Keypad9))
       {
           return "9";
       }
       if(Input.GetKeyDown(KeyCode.Keypad0))
       {
           return "0";
       }
       if(Input.GetKeyDown(KeyCode.Period))
       {
           return ".";
       }

	   return "";
    }
}
