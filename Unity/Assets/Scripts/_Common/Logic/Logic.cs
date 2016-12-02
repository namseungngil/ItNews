using UnityEngine;
using System.Collections;

public class Logic : MonoBehaviour
{
	public static GameObject GetChildObject (GameObject gO, string strName)
	{ 
		Transform[] AllData = gO.GetComponentsInChildren<Transform> (); 
		GameObject target = null;
		
		for (int i = 0; i < AllData.Length; i++) {
			if (AllData [i].name == strName) {
				target = AllData [i].gameObject;
				break;
			}
		}
		
		return target;
	}
}
