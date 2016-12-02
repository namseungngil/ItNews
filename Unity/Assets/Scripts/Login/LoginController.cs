using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using NSHttp;

public class LoginController : Login
{
	// varialbe
	string version;

	public override void OnSet (object data)
	{
		base.OnSet (data);

		Main.mainData = new List<MainData> ();
		version = Register.Get (VERSION);

		WWWClient w = new WWWClient (this, url);
		w.OnDone = (WWW www) => {
			string[] line = www.text.Split ("\n" [0]);
			if (line [0] != version) {
				Register.DeleteAll ();
				for (int i = 0; i < line.Length; i++) {
					if (i == 0) {
						Register.Set (VERSION, line [i]);
						continue;
					}

					Register.Set (i.ToString (), line [i]);
				}

				Register.Set (COUNT, line.Length.ToString ());
				for (int i = 1; i < line.Length; i++) {
					SetData (line [i]);
				}
			} else {
				SetData ();
			}

			NSSceneManager.Instance.Screen (Config.SCENE_MAIN);
		};

		w.OnFail = (WWW www) => {
			if (NetworkError ()) {
				SetData ();
			}

			NSSceneManager.Instance.Screen (Config.SCENE_MAIN);
		};

		w.OnDisposed = () => {
			if (NetworkError ()) {
				SetData ();
			}

			NSSceneManager.Instance.Screen (Config.SCENE_MAIN);
		};

		w.Request ();
	}

	private void SetData ()
	{
		int count = int.Parse (Register.Get (COUNT));

		for (int i = 1; i < count; i++) {
			SetData (Register.Get (i.ToString ()));
		}
	}

	private void SetData (string str)
	{
		string[] temp = str.Split (new string[] {DELIMITER}, System.StringSplitOptions.None);
		Main.mainData.Add (new MainData (int.Parse (temp [0]), temp [1], temp [2]));
	}

	private bool NetworkError ()
	{
		if (version != Register.DEFAULT) {
			return true;
		}

		Main.mainData = new List<MainData> ();
		for (int i = 0; i < urls.Count; i++) {
			string[] temp = urls [i].Split (new string[] {DELIMITER}, System.StringSplitOptions.None);
			Main.mainData.Add (new MainData (int.Parse (temp [0]), temp [1], temp [2]));
		}

		return false;
	}
}
