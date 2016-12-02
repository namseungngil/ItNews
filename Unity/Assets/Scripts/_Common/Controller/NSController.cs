using UnityEngine;
using System.Collections;

public class NSController : SSController
{
	public override void Awake ()
	{
		base.Awake ();

		IsCache = false;
	}

	void OnApplicationPause (bool pauseStatus)
	{
		if (!pauseStatus) {
			OffPause ();
		} else {
			OnPause ();
		}
	}
	
	protected virtual void OnPause () {}
	protected virtual void OffPause () {}
}
