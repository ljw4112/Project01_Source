using System.Text.RegularExpressions;
using UnityEngine;
using HtmlAgilityPack;


public class UpdateChecker : MonoBehaviour
{
    private HtmlWeb web;
    private HtmlDocument doc;
    private string marketVersion;
    private const string url = "https://play.google.com/store/apps/details?id=com.DJS.InChoice";
    public bool CheckVersion()
    {
        UnsafeSecurityPolicy.Instate();

        this.web = new HtmlWeb();
        this.doc = web.Load(url);

        foreach(HtmlNode node in doc.DocumentNode.SelectNodes("//span[@class='htlgb']"))
        {
            this.marketVersion = node.InnerText.Trim();
            if (marketVersion != null)
            {
                if (Regex.IsMatch(this.marketVersion, @"^\d{1}\.\d{1}\.\d{1}$"))
                {
                    string myVer = Application.version;
                    string marketVer = this.marketVersion;

                    if(myVer != marketVer)
                    {
                        return false;
                    }
                }
            }
        }
        return true;
    }
}
