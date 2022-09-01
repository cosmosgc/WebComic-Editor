using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WebComic_Editor.Class
{
    internal class Wcutils
    {
        public static string ShowDialog(string text, string caption)
        {
            Form prompt = new Form()
            {
                Width = 500,
                Height = 150,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                Text = caption,
                StartPosition = FormStartPosition.CenterScreen
            };
            Label textLabel = new Label() { Left = 50, Top = 20, Text = text };
            TextBox textBox = new TextBox() { Left = 50, Top = 50, Width = 400 };
            Button confirmation = new Button() { Text = "Ok", Left = 350, Width = 100, Top = 70, DialogResult = DialogResult.OK };
            confirmation.Click += (sender, e) => { prompt.Close(); };
            prompt.Controls.Add(textBox);
            prompt.Controls.Add(confirmation);
            prompt.Controls.Add(textLabel);
            prompt.AcceptButton = confirmation;

            return prompt.ShowDialog() == DialogResult.OK ? textBox.Text : "";
        }
        public static string SkorperLogDiv()
        {
            string text = "<div class=\"spoiler closed\">\r\n" +
                "   <div style=\"text-align: center;\"><input type=\"button\" " +
                "value=\"Open Skorpelog\" data-open=\"Open Skorpelog\" " +
                "data-close=\"Close Skorpelog\"></div>\r\n   " +
                "</div>\r\n</div>";

            return text;
        }
        public static string SkorperLogChat(Color color)
        {
            string text = "<span style=\"color: " +
                "#"+ color.R.ToString("X2") + color.G.ToString("X2") + color.B.ToString("X2") +
                ";\">WA: Man-</span>" +
                "<br>";
            return text;
        }
    }
}
