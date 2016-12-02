using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MainData
{
	public int id;
	public string name;
	public string url;

	public MainData (int id, string name, string url) {
		this.id = id;
		this.name = name;
		this.url = url;
	}
}

public class Main : NSController
{
	protected Color32[] color = new Color32[] {
		new Color32 (217, 70, 64, 255),
		new Color32 (230, 146, 36, 255),
		new Color32 (62, 198, 109, 255),
		new Color32 (64, 142, 208, 255),
		new Color32 (145, 82, 171, 255)
	};

	public static List<MainData> mainData;
	public static int id;
}
