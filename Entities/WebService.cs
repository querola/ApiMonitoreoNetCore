namespace monitoreoApiNetCore.Entities{
    public class WebService{
        public WebService(System.DateTime? date, bool delay)
        {
            Date = date;
            Delay = delay;
        }
        public System.DateTime? Date { get; set; }
        public bool Delay { get; set; }
    }
}