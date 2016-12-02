using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Login : NSController
{
	// const
	protected const string url = "http://namseungngil.cafe24.com/itnews/itnews.json";
	protected const string DELIMITER = "::";
	protected const string VERSION = "version";
	protected const string COUNT = "count";
	// array
	protected List<string> urls;

	public override void OnSet (object data)
	{
		base.OnSet (data);

		urls = new List<string> ();
		urls.Add ("1" + DELIMITER + "ZDNet" + DELIMITER + "http://m.zdnet.co.kr/");
		urls.Add ("2" + DELIMITER + "Bloter" + DELIMITER + "http://www.bloter.net/");
		urls.Add ("3" + DELIMITER + "Platum" + DELIMITER + "http://platum.kr/");
		urls.Add ("4" + DELIMITER + "techNeedle" + DELIMITER + "http://techneedle.com/"); 
		urls.Add ("5" + DELIMITER + "Naver" + DELIMITER + "http://m.news.naver.com/main.nhn?mode=LSD&sid1=105");
		urls.Add ("6" + DELIMITER + "Daum" + DELIMITER + "http://m.media.daum.net/m/media/digital/");
		urls.Add ("7" + DELIMITER + "Nate" + DELIMITER + "http://m.news.nate.com/section?mid=m05&sq=480601");
		urls.Add ("8" + DELIMITER + "ITnews" + DELIMITER + "http://www.itnews.or.kr/category/all-news/");
		urls.Add ("9" + DELIMITER + "ITwrold" + DELIMITER + "http://www.itworld.co.kr/");
		urls.Add ("10" + DELIMITER + "Betanews" + DELIMITER + "http://m.betanews.net/");
		urls.Add ("11" + DELIMITER + "Gametoc" + DELIMITER + "http://m.gametoc.hankyung.com/");
	}
}
