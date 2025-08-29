using System.Diagnostics;
using System.Windows.Forms;

namespace Rhythm_Plus_Discord_RPC
{
    public partial class Licences : Form
    {
        public Licences()
        {
            InitializeComponent();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            using (Process.Start("https://github.com/Lachee/discord-rpc-csharp")) { }
        }

        private void linkLabel3_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            using (Process.Start("https://github.com/statianzo/Fleck")) { }
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            using (Process.Start("https://github.com/JamesNK/Newtonsoft.Json")) { }
        }
    }
}
