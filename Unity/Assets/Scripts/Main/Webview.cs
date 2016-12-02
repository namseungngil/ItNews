using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Webview : MonoBehaviour
{
	// component
	private MainController main;
	public UniWebView uniWebView;
	public string url;

	void Start ()
	{
		main = gameObject.GetComponentInParent<MainController> ();
		Init ();
	}

	private void OnLoadBegin (UniWebView webView, string url)
	{
		main.Tab (url);
	}

	private void OnReceivedMessage (UniWebView webView, UniWebViewMessage message)
	{
	}

	private void OnLoadComplete (UniWebView webView, bool success, string errorMessage)
	{
		main.Ad ();
	}

	private bool OnWebViewShouldClose (UniWebView webView)
	{
		Application.Quit ();
		if (uniWebView == webView) {
			uniWebView = null;
			return true;
		}

		return false;
	}

	private void OnEvalJavaScriptFinished (UniWebView webView, string result)
	{
		Debug.Log ("OnEvalJavaScriptFinished : " + result);
	}

	private UniWebViewEdgeInsets InsetsForScreenOreitation (UniWebView webView, UniWebViewOrientation orientation)
	{
		GameObject gObj = GameObject.Find ("Top");
		UI2DSprite uI2DS = gObj.GetComponent<UI2DSprite> ();

		int width = Screen.width;
		int aTop = (int)(Screen.height / 10);

		UIRoot mRoot = NGUITools.FindInParents<UIRoot> (gObj);
		float ratio = (float)mRoot.activeHeight / Screen.height;

		int NGUIwidth = (int)(Mathf.Ceil (width * ratio));
		int NGUIheight = (int)(Mathf.Ceil (aTop * ratio));

		int x = NGUIwidth / 2;
		int y = mRoot.activeHeight / 2 - NGUIheight;

		uI2DS.SetRect (-x, y, NGUIwidth, NGUIheight);

		gObj = GameObject.Find ("Bottom");
		uI2DS = gObj.GetComponent<UI2DSprite> ();

		int aBottom = (int)(Screen.height / 11);

		NGUIheight = (int)(Mathf.Ceil (aBottom * ratio));
		y = mRoot.activeHeight / 2 - NGUIheight;

		uI2DS.SetRect (-x, -y - NGUIheight, NGUIwidth, NGUIheight);

#if UNITY_EDITOR
		return new UniWebViewEdgeInsets (aTop, 0, aBottom, 0);
#elif UNITY_IPHONE
		return new UniWebViewEdgeInsets (aTop / 2, 0, aBottom / 2, 0);
#else
		return new UniWebViewEdgeInsets (aTop, 0, aBottom, 0);
#endif
	}

	public void Init ()
	{
		if (gameObject.GetComponent<UniWebView> () == null) {
			uniWebView = gameObject.AddComponent<UniWebView> ();
		}

		uniWebView.OnReceivedMessage += OnReceivedMessage;
		uniWebView.OnLoadBegin += OnLoadBegin;
		uniWebView.OnLoadComplete += OnLoadComplete;
		uniWebView.OnWebViewShouldClose += OnWebViewShouldClose;
		uniWebView.OnEvalJavaScriptFinished += OnEvalJavaScriptFinished;
		uniWebView.InsetsForScreenOreitation += InsetsForScreenOreitation;
			
		uniWebView.url = Main.mainData [0].url;
		uniWebView.backButtonEnable = true;
		uniWebView.toolBarShow = true;
		uniWebView.SetShowSpinnerWhenLoading (false);
			
		uniWebView.Load ();
		uniWebView.Show ();
	}

	public void Load (string url)
	{
		uniWebView.url = url;
		uniWebView.Load ();
		uniWebView.Show ();
	}

	public void GoBack ()
	{
		uniWebView.GoBack ();
	}

	public void GoForward ()
	{
		uniWebView.GoForward ();
	}
}
