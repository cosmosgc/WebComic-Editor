using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

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
                "   <div style=\"text-align: center;\">\r\n<input type=\"button\"" +
                "value=\"Open Skorpelog\" data-open=\"Open Skorpelog\"" +
                "data-close=\"Close Skorpelog\">\r\n</div>\r\n   " +
                "<div style=\"text-align: left;\">\r\n" +
                "<!--TEXTO AQUI-->\r\n" +
                "</div>\r\n" +
                "</div>\r\n</div>\r\n";

            return text;
        }
        public static string SkorperLogChat(string charName, Color color)
        {
            string text = "<span style=\"color: " +
                "#"+ color.R.ToString("X2") + color.G.ToString("X2") + color.B.ToString("X2") +
                ";\">\r\n" +
                charName + 
                ": " + "<!--TEXTO AQUI-->\r\n" +
                "</span>" +
                "<br>";
            return text;
        }
        public static string AmongUsDiv()
        {
            string text = "<div class=\"among\">\r\n" +
                "<!--Chat Aqui-->" +
                "</div>";
            return text;
        }
        public static string AmongUsChat(string charName)
        {
            string text = "<div class=\"us\" <!--class=\"you\"style=\"--icon: url(ICON HERE)--> \">\r\n" +
                "<name>"+charName+"</name>\r\nTEXT HERE</div>";
            return text;
        }
        public static string Earthbound()
        {
            string text = "<div class=\"earthbstrw\">\r\n" +
                "<div><div class=\"scroll\" style=\"--delay: 0\">\r\n" +
                "<div>} Now in diffrent flavours!</div>\r\n" +
                "</div>\r\n" +
                "<div class=\"scroll\" style=\"--delay: 1.5\">\r\n" +
                "<div>} it doesn't show up on phones but whatever</div>\r\n" +
                "</div>\r\n" +
                "<div class=\"scroll\" style=\"--delay: 3\">\r\n" +
                "<div>} Looks good enough here</div>\r\n" +
                "</div>\r\n" +
                "</div>\r\n" +
                "</div>";
            XElement.Parse(text).ToString();
            return text;
        }


        public static void CopyDirectory(string sourceDir, string destinationDir, bool recursive)
        {
            // Get information about the source directory
            var dir = new DirectoryInfo(sourceDir);

            // Check if the source directory exists
            if (!dir.Exists)
                throw new DirectoryNotFoundException($"Source directory not found: {dir.FullName}");

            // Cache directories before we start copying
            DirectoryInfo[] dirs = dir.GetDirectories();

            // Create the destination directory
            Directory.CreateDirectory(destinationDir);

            // Get the files in the source directory and copy to the destination directory
            foreach (FileInfo file in dir.GetFiles())
            {
                string targetFilePath = Path.Combine(destinationDir, file.Name);
                var file2 = new FileInfo(targetFilePath);
                file2.Delete();
                file.CopyTo(targetFilePath);
            }

            // If recursive and copying subdirectories, recursively call this method
            if (recursive)
            {
                foreach (DirectoryInfo subDir in dirs)
                {
                    string newDestinationDir = Path.Combine(destinationDir, subDir.Name);
                    CopyDirectory(subDir.FullName, newDestinationDir, true);
                }
            }
        }
    }
}
