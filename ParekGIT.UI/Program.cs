using ParekGIT.Core.Data;
using Photino.NET;
using System.Drawing;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ParekGIT.UI
{
    internal class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            // Database
            var dbStore = new LiteDbStore();

            // Window
            var window = new PhotinoWindow()
                .SetTitle("ParekGIT")
                .SetUseOsDefaultSize(false)
                .SetSize(new Size(960, 660))
                .SetMinSize(960, 660)
                .Center()
                .SetContextMenuEnabled(true) // later false
                .SetDevToolsEnabled(true) // later false
                .Load("wwwroot/index.html");

            window.WaitForClose();
        }
    }
}
