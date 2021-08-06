using System.Collections.Generic;

public class PurchaseData
{
    public string id;
    public List<string> chapterName;

    public PurchaseData()
    {
        this.chapterName = new List<string>();
    }
}