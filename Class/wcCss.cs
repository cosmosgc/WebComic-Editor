using MaterialSkin;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace WebComic_Editor.Class
{
    internal class wcCss
    {
        public static string AmongUsChat(string charName, string selectedText = "TEXT HERE",bool toRight = false)
        {
            string DirectionClass = "class=\"us\"";
            if (toRight)
                DirectionClass = "class=\"you\"";
            string text = "<div "+DirectionClass+" style=\"--icon: url(ICON HERE) \"><name>" + charName + "</name><br>\r\n"+selectedText+"\r\n\r\n</div>";
            //XElement.Parse(text).ToString();
            return text;
        }
        //<div class="combo" style="color: HEXCOLORHERE ; --shadow: HEXCOLORHERE">TEXT HERE</div>
        public static string COMBOBOX(string selectedText, Color color)
        {
            if (selectedText == null)
                selectedText = "TEXT HERE";
            string hex = "#" + color.R.ToString("X2") + color.G.ToString("X2") + color.B.ToString("X2");
            Color shadow = color.Darken(0.3f);
            string shadowHex = "#" + shadow.R.ToString("X2") + shadow.G.ToString("X2") + shadow.B.ToString("X2");
            string text = "<div class=\"combo\" style=\"color: "+hex+" ; --shadow: "+ shadowHex + "\">"+ selectedText + "</div>";
            //XElement.Parse(text).ToString();
            return text;
        }
        public static string glitch(string selectedText)
        {
            if (selectedText == null)
                selectedText = "TEXT HERE";
            string text = "";
            text += "<spam class=\"glitch\">" + selectedText + "</spam>";
            //XElement.Parse(text).ToString();
            return text;
        }
        public static string shake(string selectedText)
        {
            if (selectedText == null)
                selectedText = "TEXT HERE";
            string text = "";
            text += "<spam class=\"shake\">" + selectedText + "</spam>";
            //XElement.Parse(text).ToString();
            return text;
        }
        public static string gimpact(string charName)
        {
            string text = "";
            text += "<div class=\"gimpact\"><h1>" + charName + "</h1><h2>Title</h2> \r\nText here\r\n\r\n</div>";
            //XElement.Parse(text).ToString();
            return text;
        }
        public static string aol()
        {
            string text = "";
            text += "<div class=\"aol\" style='--app: \"Messenger\"'>\r\n<div> \r\nTEXT HERE\r\n\r\n </div>\r\n</div>";
            //XElement.Parse(text).ToString();
            return text;
        }

        public static string dcord2()
        {
            string text = "";
            text = "<div class=\"dcord2\"><br>" +
                "\r\n\r\n<!--TEXT HERE-->\r\n\r\n" +
                "<input placeholder=\"Message @chat\"><br></div>";
            //XElement.Parse(text).ToString();
            return text;
        }
        public static string dcord2Chat(string charName, Color color)
        {
            string hex = "#" + color.R.ToString("X2") + color.G.ToString("X2") + color.B.ToString("X2");
            string text = "";
            text += "\r\n<div>\r\n<user style=\"--icon: url(https://file.garden/X1htvgJ0DEp_tp-Z/9b.png); color: " + hex +";\">"+charName+ "</user><br>\r\n\r\n Text Here <at>@mençãoaqui</at>!\r\n\r\n</div><br>\r\n\r\n";
            //XElement.Parse(text).ToString();
            return text;
        }
        public static string RAINBOW(string selectedText)
        {
            if (selectedText == null)
                selectedText = "TEXT HERE";
            string text = "";
            text += "<div class=\"textgrad\">" + selectedText + "</div>";
            //XElement.Parse(text).ToString();
            return text;
        }
        public static string TF2()
        {
            string text = "<div class=\"tf2chat\">\r\n<div> Text here </div>\r\n\r\n<!--TEXT HERE-->\r\n\r\n</div>";
            //XElement.Parse(text).ToString();
            return text;
        }
        public static string Diary(string selectedText = "TEXT HERE")
        {
            string text = "<div class=\"wimp\">\r\n\r\n" + selectedText + "\r\n\r\n</div>";
            //XElement.Parse(text).ToString();
            return text;
        }
        public static string mcBtn(string selectedText)
        {
            string text = "<button class=\"mcBtn\">"+selectedText+"</button>";
            //XElement.Parse(text).ToString();
            return text;
        }
        public static string PCTERMINAL()
        {
            string text = "<div class=\"realTerminal\">\r\n\r\nText Here\r\n\r\n</div>";
            //XElement.Parse(text).ToString();
            return text;
        }
        public static string ytcom(string charName)
        {
            string text = "\r\n<div class=\"ytcom\" style=\"--icon: url(IMAGEHERE)\">\r\n" +
                "<h1>" + charName + "</h1> 14/04/2944<br>\r\n\r\nTEXTHERE\r\n <p> LIKES\r\n</div>\r\n\r\n";
            //XElement.Parse(text).ToString();
            return text;
        }
        public static string falloutvgnorm(string selectedText = "TEXT HERE")
        {
            string text = "<div class=\"falloutvgnorm\"> \r\n\r\n" + selectedText+ "\r\n\r\n</div>";
            //XElement.Parse(text).ToString();
            return text;
        }
        public static string hylics2(string selectedText = "TEXT HERE")
        {
            string text = "<div class=\"hylics2\">\r\n<!--TEXT HERE-->\r\n " + selectedText + " \r\n\r\n</div>";
            //XElement.Parse(text).ToString();
            return text;
        }
        public static string jazztronauts(string charName, string selectedText = "TEXT HERE", bool ToRight = false)
        {
            string Direction = "";
            if (ToRight)
                Direction = "R";
            string text = "<div class=\"jazztronauts"+ Direction + "\"> "+charName+ " <div> \r\n<!--TEXT HERE-->\r\n" + selectedText + " \r\n\r\n</div></div>";
            //XElement.Parse(text).ToString();
            return text;
        }
        public static string phone(string charName, Color color)
        {
            string hex = "#" + color.R.ToString("X2") + color.G.ToString("X2") + color.B.ToString("X2");

            string text = "\r\n<div class=\"phone\" style=\"--color1: " + hex+";\">\r\n<div class=\"phhead\"> [IMAGE] "+ charName + " </div>\r\n\r\n" +
                "<!--TEXT HERE USE O BOTÃO DE CRIAR MSG NO LADO DO SELECIONAR NOME--> \r\n\r\n</div>\r\n";
            //XElement.Parse(text).ToString();
            return text;
        }
        public static string phoneChat(string selectedText, bool ToRight)
        {
            string DivSyntax = "<div class=\"phother\">";
            if (selectedText == null)
                selectedText = "TEXT HERE";
            if (ToRight)
                DivSyntax = "\r\n<div class=\"phself\">";
                
            string text = DivSyntax+ "\r\n<p>\r\n<!--TEXT HERE, use <br> para aumentar o balão, use <p> para criar um novo balão adequadamente-->\r\n " + selectedText+ "\r\n\r\n</div>\r\n";
            //XElement.Parse(text).ToString();
            return text;
        }
        public static string mcchat(string selectedText = "TEXT HERE")
        {
            string text = "<div class=\"mcchat\"><div>\r\n<!--TEXT HERE-->\r\n" + selectedText + "\r\n\r\n</div></div>\r\n";
            //XElement.Parse(text).ToString();
            return text;
        }
        public static string mcsign(string selectedText = "TEXT HERE")
        {
            string text = "<div class=\"mcsign\"><div>\r\n<!--TEXT HERE-->" + selectedText + "</div></div>\r\n";
            //XElement.Parse(text).ToString();
            return text;
        }
        public static string TextAppearingSequentially(string selectedText = "TEXT HERE")
        {
            string text = "<div class=\"scroll\" style=\"--delay: 0; text-align:left;\"><div><!--TEXT HERE-->" + selectedText + "</div></div>";
            //XElement.Parse(text).ToString();
            return text;
        }
        public static string dthuman(string selectedText = "TEXT HERE")
        {
            string text = "<div class=\"dthuman\"><div>\r\n<div><p> LINKHERE </div>\r\n<div><p> LINKHERE </div>\r\n<div><p> LINKHERE </div>\r\n<div><p> LINKHERE </div>\r\n</div></div>";
            //XElement.Parse(text).ToString();
            return text;
        }
        public static string UTmenu3()
        {
            string text = "<div class=\"UTmenu3\">\r\n\r\n<div class=\"UTmenu3stats\">NAME\r\nLV 1\r\nHP 20/20\r\nG  10\r\n</div>\r\n\r\n<div class=\"UTmenu3items\">" +
                "\r\n\r\n<!-- ITENS AQUI Aperte o botão de chat para criar 1 item-->\r\n\r\n<br></div></div>";
            //XElement.Parse(text).ToString();
            return text;
        }
        public static string UTmenu3Item()
        {
            string text = "<div>\r\n<input type=\"radio\" id=\"mitem\" name=\"UTmenu3\">\r\n<label for=\"mitem\">ITEM</label>\r\n<div class=\"UTmenu3info\">* This is the Item menu\r\n</div>\r\n</div>";
            //XElement.Parse(text).ToString();
            return text;
        }
        public static string roblox(Color color)
        {
            string hex = "#" + color.R.ToString("X2") + color.G.ToString("X2") + color.B.ToString("X2");

            string text = "<div class=\"roblox\"><table>\r\n\r\n<tr>\r\n<th>People</th>\r\n<th>KOs</th>\r\n<th>Wipeouts</th>\r\n</tr>\r\n\r\n<tr id=\"head\" style=\"--color1: "+hex+"\">\r\n<td>Jogador1</td>\r\n<td>1</td>\r\n<td>0</td>\r\n</tr>\r\n\r\n<tr>\r\n<td>Jogador2</td>\r\n<td>1</td>\r\n<td>0</td>\r\n</tr>\r\n\r\n</table></div>";
            //XElement.Parse(text).ToString();
            return text;
        }
        public static string DocScratch(Color color, string selectedText = "TEXT HERE")
        {
            string hex = "#" + color.R.ToString("X2") + color.G.ToString("X2") + color.B.ToString("X2");

            string text = "<div class=\"docyell\" style=\"--color: "+hex+"\">"+ selectedText + "</div>";
            //XElement.Parse(text).ToString();
            return text;
        }
    }
}
