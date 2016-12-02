using UnityEngine;
//using System;
using System.Collections;
using System.Collections.Generic;

public class MainController : Main
{
	private const string PANEL_1000 = "Panel:1000";
	private List<GameObject> listGObj;
	private GoogleMobileAdsManager googleMobileAdsManager;
	private Webview webview;
	private Vector3 normalS;
	private Vector3 bigS;
	private string url = "";
	private float down;
	private float up;
	private int count;
	private bool touchStarted;
	
	public override void OnSet (object data)
	{
		base.OnSet (data);

		count = 0;

		GameObject temp = GameObject.Find (Config.GAMEOBJECT_TEMP);
		GameObject gridList = GameObject.Find (Config.GAMEOBJECT_GRIDLIST);

		listGObj = new List<GameObject> ();
		for (int i = 0; i < mainData.Count; i++) {
			GameObject gObj = Instantiate (temp, new Vector3 (0, 0, 0), Quaternion.identity) as GameObject;
			gObj.transform.parent = gridList.transform;
			gObj.transform.localScale = new Vector3 (1f, 1f, 1f);
			gObj.transform.localPosition = new Vector3 (i * 100, 0, 0);

			gObj.name = mainData [i].id.ToString ();
			UIButton uIButton = gObj.GetComponent<UIButton> ();
			uIButton.defaultColor = color [i % color.Length];
			uIButton.hover = color [i % color.Length];
			Logic.GetChildObject (gObj, Config.GAMEOBJECT_LABEL).GetComponent<UILabel> ().text = mainData [i].name;

			listGObj.Add (gObj);
		}

		normalS = new Vector3 (1f, 1f, 1f);
		bigS = new Vector3 (1f, 1.2f, 1f);

		listGObj [0].transform.localScale = bigS;
		temp.SetActive (false);
		gridList.GetComponent<UIGrid> ().Reposition ();

		googleMobileAdsManager = gameObject.GetComponentInParent<GoogleMobileAdsManager> ();
		webview = GameObject.Find (PANEL_1000).GetComponent<Webview> ();

#if UNITY_IOS
		googleMobileAdsManager.bannerView.Hide ();
		StartCoroutine (TimeAd ());
#endif
	}

	private IEnumerator TimeAd ()
	{
		googleMobileAdsManager.SetAd ();
		yield return new WaitForSeconds (150);

		googleMobileAdsManager.GetAd ();
		StartCoroutine (TimeAd ());
	}

	public override void OnKeyBack ()
	{
		base.OnKeyBack ();
		Application.Quit ();
	}

	public void Tab (string url)
	{
		string[] temp = url.Split (new string[] {Config.SLASH + Config.SLASH}, System.StringSplitOptions.None);
		temp = temp [1].Split (new string[] {Config.SLASH}, System.StringSplitOptions.None);

		bool check = true;
		for (int i = 0; i < mainData.Count; i++) {
			if (mainData [i].url.Contains (temp [0])) {
				this.url = url;
				if (mainData [i].url.IndexOf (url) == 0) {
					check = false;
#if UNITY_IOS
#else
					googleMobileAdsManager.bannerView.Show ();
#endif
				}

				listGObj [i].transform.localScale = bigS;
				for (int j = 0; j < mainData.Count; j++) {
					if (i != j) {
						listGObj [j].transform.localScale = normalS;
					}
				}

				break;
			}
		}

		if (check) {
#if UNITY_IOS
#else
			googleMobileAdsManager.bannerView.Hide ();
#endif
		}
	}

	public void Ad (string url = "")
	{
#if UNITY_IOS
#else
		count++;
		if (count >= 10) {
			count = 0;
			googleMobileAdsManager.GetAd ();
		} else if (count == 1) {
			googleMobileAdsManager.SetAd ();
		}
#endif
	}

	public void OnClickButton ()
	{
		int id = int.Parse (UIButton.current.name);

		for (int i = 0; i < mainData.Count; i++) {
			listGObj [i].transform.localScale = normalS;
			if (mainData [i].id == id) {
				listGObj [i].transform.localScale = bigS;
				webview.Load (mainData [i].url);
			}
		}
	}

	public void OnClickLeft ()
	{
		webview.GoBack ();
	}

	public void OnClickRight ()
	{
		webview.GoForward ();
	}

	public void OnClickBrowser ()
	{
		if (url != "") {
			Application.OpenURL (url);
		}
	}
}
