namespace vintage_garage_web.Models.Login
{
    public class LoginRes
    {
        public string Username { get; set; }
        public string Role { get; set; }
        public string AccessToken { get; set; }
        public double Expires { get; set; }
    }
}
