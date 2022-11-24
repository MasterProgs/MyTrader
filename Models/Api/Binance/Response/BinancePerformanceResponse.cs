    public class PerformanceResponse
    {
        public string periodType { get; set; }
        public string statisticsType { get; set; }
        public double value { get; set; }
        public int rank { get; set; }
    }

    public class BinancePerformanceResponse
    {
        public string code { get; set; }
        public object message { get; set; }
        public object messageDetail { get; set; }
        public List<PerformanceResponse> data { get; set; }
        public bool success { get; set; }
    }