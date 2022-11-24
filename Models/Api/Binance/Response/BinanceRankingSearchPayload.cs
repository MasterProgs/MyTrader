public class BinanceRankingSearchResponseData
{
    public string encryptedUid { get; set; }
    public string nickName { get; set; }
    public string userPhotoUrl { get; set; }
    public bool positionShared { get; set; }
    public bool deliveryPositionShared { get; set; }
    public int followerCount { get; set; }
    public double pnlValue { get; set; }
    public double roiValue { get; set; }
    public int rank { get; set; }
    public object change { get; set; }
}

public class BinanceRankingSearchResponse
{
    public string code { get; set; }
    public object message { get; set; }
    public object messageDetail { get; set; }
    public List<BinanceRankingSearchResponseData> data { get; set; }
    public bool success { get; set; }
}