namespace campaign_hub.Models
{
    public class Campaign
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public string? img { get; set; }
        public DateTime? start_date { get; set; }
        public DateTime? end_date { get; set; }
    }
}
