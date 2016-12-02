using UnityEngine;
using System.Collections;

public class Register
{
	public const string DEFAULT = "0";

	public static void Set (string key, string value)
	{
		PlayerPrefs.SetString (key, Encrypt.Set (key, value));
	}

	public static string Get (string key)
	{
		return Get (key, DEFAULT);
	}

	public static string Get (string key, string defalutValue)
	{
		string value = PlayerPrefs.GetString (key, defalutValue);
		if (value != defalutValue) {
			value = Encrypt.Get (key, value);
		}

		return value;
	}

	public static void DeleteAll ()
	{
		PlayerPrefs.DeleteAll ();
	}
}
