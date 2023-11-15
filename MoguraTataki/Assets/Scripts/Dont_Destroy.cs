using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dont_Destroy : MonoBehaviour
{
	// ƒVƒ“ƒOƒ‹ƒgƒ“
	private static Dont_Destroy instance;
	public static Dont_Destroy Instance
	{
		get
		{
			if (null == instance)
			{
				instance = (Dont_Destroy)FindObjectOfType(typeof(Dont_Destroy));
				if (null == instance) Debug.Log(" Dont_Destroy Instance Error ");
			}
			return instance;
		}
	}

	void Awake()
	{
		GameObject[] obj = GameObject.FindGameObjectsWithTag("BGM");
		if (1 < obj.Length) Destroy(gameObject);
		else DontDestroyOnLoad(gameObject);
	}
}
