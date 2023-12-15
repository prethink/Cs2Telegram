using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cs2Telegram.Models
{
    public class CustomMenu
    {
        public string ButtonName { get; set; } = "Custom Menu";
        public string Message { get; set; } = "Custom Message";
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
