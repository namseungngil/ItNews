using UnityEngine;
using System.Collections;

public class NSSceneManager : SSSceneManager
{
	private static NSSceneManager instance;
	public static new NSSceneManager Instance {
		get {
			return instance;
		}
	}

	protected override void Awake ()
	{
		base.Awake ();

		if (Application.isPlaying) {
			instance = this;

			m_SolidCamera.tag = "MainCamera";
		}
	}
}
