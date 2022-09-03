using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebComic_Editor.Class
{
    internal class wcTextBoxes
    {
        public static string deltarune(string selectedText = "TEXT HERE")
        {
            string text = "<div class=\"drune\" style=\"max-width: 400px\"><div>\r\n<!--TEXT HERE-->" + selectedText + "\r\n\r\n</div></div>";
            //XElement.Parse(text).ToString();
            return text;
        }
        public static string acewrap(string charName = "NOME AQUI", string selectedText = "TEXT HERE")
        {
            string text = "<div class=\"acewrap\"><div class=\"acename\">\r\n<!--TEXT HERE-->" + charName + "\r\n\r\n</div><br><div class=\"acea\">" + selectedText + "</div></div>";
            //XElement.Parse(text).ToString();
            return text;
        }
        public static string HollowKnight(string selectedText = "TEXT HERE")
        {
            string text = "<div class=\"hknight\"><div>\r\n<!--TEXT HERE-->" + selectedText + "\r\n\r\n</div></div>";
            //XElement.Parse(text).ToString();
            return text;
        }
        public static string botw(string charName = "Name here", string selectedText = "TEXT HERE")
        {
            string text = "<div class=\"botw\">"+ charName +"<div>\r\n<!--TEXT HERE-->"+ selectedText + "\r\n\r\n</div></div>";
            //XElement.Parse(text).ToString();
            return text;
        }
        public static string picto(Color color, string charName = "NAME", string selectedText = "TEXT HERE")
        {
            string hex = "#" + color.R.ToString("X2") + color.G.ToString("X2") + color.B.ToString("X2");

            string text = "<div class=\"picto\">\r\n\r\n<div class=\"enter\">Now entering: CHAT</div>\r\n\r\n" +
                "<!--MENSAGENS AQUI, JÁ ADICIONAMOS 1, USE O BOTÃO DE CRIAR MAIS MENSAGENS-->" +
                "<div style=\"--col: " + hex + "\"><div>" + charName + "</div> " + selectedText + " </div>\r\n\r\n" +
                "</div>\r\n";
            //XElement.Parse(text).ToString();
            return text;
        }
        public static string pictoChat(Color color, string charName = "NAME", string selectedText = "TEXT HERE")
        {
            string hex = "#" + color.R.ToString("X2") + color.G.ToString("X2") + color.B.ToString("X2");

            string text = "\r\n<div style=\"--col: " + hex + "\"><div>" + charName + "</div> " + selectedText + " </div>\r\n\r\n";
            //XElement.Parse(text).ToString();
            return text;
        }
    }
}
