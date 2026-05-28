namespace App.Backend.Models
{
    public class SettingsRequest
    {
        public int SessionDuration { get; set; }
        public int SessionsAmount { get; set; }
    }

    public class SettingsResponse
    {
        public int SessionDuration { get; set; }
        public int SessionsAmount { get; set; }
    }
}
