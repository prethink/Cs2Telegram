namespace Cs2Telegram.Models
{
    public class CustomCommands
    {
        public List<CustomCommand> Commands { get; set; } = new() { new CustomCommand() };
    }

    public class CustomCommand
    {
        public string ButtonName { get; set; } = "Custom Menu";
        public string Message { get; set; } = "Custom Message";
        public bool AddInMainMenu { get; set; } = false;
        public int Column { get; set; } = 1;
        public List<WebMenuItem> WebMenuItems { get; set; } = new() { new WebMenuItem {Name = "Google" , Url = "Https://google.com" } };
        public bool IsValid()
        {
            return !string.IsNullOrWhiteSpace(ButtonName) && !string.IsNullOrWhiteSpace(Message);
        }
    }

    public class WebMenuItem
    {
        public string Name { get; set; }
        public string Url { get; set; }
    }
}
