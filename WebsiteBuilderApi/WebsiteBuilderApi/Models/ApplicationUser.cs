namespace WebsiteBuilderApi.Models
{
    public class ApplicationUser
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Provider { get; set; }
        public string OAuthId { get; set; }
    }
}
