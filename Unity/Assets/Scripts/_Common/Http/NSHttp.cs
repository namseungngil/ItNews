using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace NSHttp
{
	public class WWWClient
	{
		public delegate void FinishedDelegate (WWW www);

		public delegate void DisposedDelegate ();

		private float defaultTimeOut = 10f;
		private MonoBehaviour monoBehaviour;
		private string url;
		private WWW wWW;
		private WWWForm wWWFrom;
		private Dictionary<string, string> mHeaders;
		private float timeOut;
		private FinishedDelegate mOnDone;
		private FinishedDelegate mOnFail;
		private DisposedDelegate mOnDisposed;
		private bool mDisposed;

		private IEnumerator RequestCoroutine ()
		{
			if (wWWFrom.data.Length > 0) {
				foreach (var entry in wWWFrom.headers) {
					mHeaders [System.Convert.ToString (entry.Key)] = System.Convert.ToString (entry.Value);
				}
				
				// POST request
				wWW = new WWW (url, wWWFrom.data, mHeaders);
			} else {
				// GET request
				wWW = new WWW (url, null, mHeaders);
			}
			
			yield return monoBehaviour.StartCoroutine (CheckTimeout ());
			
			if (mDisposed) {
				if (mOnDisposed != null) {
					mOnDisposed ();
				}
			} else if (System.String.IsNullOrEmpty (wWW.error)) {
				if (mOnDone != null) {
					mOnDone (wWW);
				}
			} else {
				if (mOnFail != null) {
					mOnFail (wWW);
				}
			}
		}
		
		private IEnumerator CheckTimeout ()
		{
			float startTime = Time.time;
			
			while (!mDisposed && !wWW.isDone) {
				if (timeOut > 0 && (Time.time - startTime) >= timeOut) {
					Dispose ();
					break;
				} else {
					yield return null;
				}
			}
			
			yield return null;
		}

		public Dictionary<string, string> Headers {
			set { mHeaders = value; }
			get { return mHeaders; }
		}
		
		public float Timeout {
			set { timeOut = value; }
			get { return timeOut; }
		}
		
		public FinishedDelegate OnDone {
			set { mOnDone = value; }
		}
		
		public FinishedDelegate OnFail {
			set { mOnFail = value; }
		}
		
		public DisposedDelegate OnDisposed {
			set { mOnDisposed = value; }
		}
		
		public WWWClient (MonoBehaviour mB, string u)
		{
			monoBehaviour = mB;
			url = u;
			mHeaders = new Dictionary<string, string> ();
			wWWFrom = new WWWForm ();
			timeOut = defaultTimeOut;
			mDisposed = false;
		}

		public WWWClient (MonoBehaviour mB, string u, string param)
		{
			monoBehaviour = mB;
			url = u + param;
			mHeaders = new Dictionary<string, string> ();
			wWWFrom = new WWWForm ();
			timeOut = defaultTimeOut;
			mDisposed = false;
		}

		public WWWClient (MonoBehaviour mB, string u, float time)
		{
			monoBehaviour = mB;
			url = u;
			mHeaders = new Dictionary<string, string> ();
			wWWFrom = new WWWForm ();
			timeOut = time;
			mDisposed = false;
		}

		public WWWClient (MonoBehaviour mB, string u, Dictionary<string, string> param)
		{
			monoBehaviour = mB;
			url = u;

			int count = 0;
			var enumerator = param.GetEnumerator ();
			while (enumerator.MoveNext ()) {
				if (count == 0) {
					url += "?" + enumerator.Current.Key + "=" + enumerator.Current.Value;
				} else {
					url += "&" + enumerator.Current.Key + "=" + enumerator.Current.Value;
				}

				count++;
			}

			mHeaders = new Dictionary<string, string> ();
			wWWFrom = new WWWForm ();
			timeOut = defaultTimeOut;
			mDisposed = false;
		}
		
		public void AddHeader (string headerName, string value)
		{
			mHeaders.Add (headerName, value);
		}
		
		public void AddData (string fieldName, string value)
		{
			wWWFrom.AddField (fieldName, value);
		}
		
		public void AddBinaryData (string fieldName, byte[] contents)
		{
			wWWFrom.AddBinaryData (fieldName, contents);
		}
		
		public void AddBinaryData (string fieldName, byte[] contents, string fileName)
		{
			wWWFrom.AddBinaryData (fieldName, contents, fileName);
		}
		
		public void AddBinaryData (string fieldName, byte[] contents, string fileName, string mimeType)
		{
			wWWFrom.AddBinaryData (fieldName, contents, fileName, mimeType);
		}
		
		public void Request ()
		{
			monoBehaviour.StartCoroutine (RequestCoroutine ());
		}
		
		public void Dispose ()
		{
			if (wWW != null && !mDisposed) {
				wWW.Dispose ();
				mDisposed = true;
			}
		}

		/*
		* StartCoroutine(checkInternetConnection((isConnected)=>{
		* // handle connection status here
		* }));
		* */
		public IEnumerator CheckConnection (string url, Action<bool> action)
		{
			WWW wWW = new WWW (url);

			yield return wWW;

			if (wWW.error != null) {
				action (false);
			} else {
				action (true);
			}
		}

		public static String CreateURL (List<string> u, string extension = null, Dictionary<string, string> param = null)
		{
			System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder ();
			for (int i = 0; i < u.Count; i++) {
				if (i == (u.Count - 1)) {
					stringBuilder.Append (u [i]);
				} else {
					stringBuilder.Append (u [i] + "/");
				}
			}

			if (extension != null) {
				stringBuilder.Append ("." + extension);
			}

			if (param != null) {
				int count = 0;
				foreach (KeyValuePair<string, string> keyValuePair in param) {
					if (count == 0) {
						stringBuilder.Append ("?" + keyValuePair.Key + "=" + keyValuePair.Value);
					} else {
						stringBuilder.Append ("&" + keyValuePair.Key + "=" + keyValuePair.Value);
					}
					count++;
				}
			}

			return stringBuilder.ToString ();
		}
	}
}