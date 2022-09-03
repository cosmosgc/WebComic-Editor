using Microsoft.Web.WebView2.Core;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Policy;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using WebComic_Editor.Class;
using static System.Net.Mime.MediaTypeNames;
using static System.Net.WebRequestMethods;
using File = System.IO.File;
using System.Net.Http;
using System.Web;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Web.UI.WebControls;
using Cyotek.Windows.Forms;
using System.Diagnostics;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ToolBar;
using MaterialSkin.Controls;

namespace WebComic_Editor
{
    public partial class Form1 : Form
    {
        public string projectName = "Sem Nome";
        public string sourceURL;
        private List<mspa> WebComic = new List<mspa>();
        private int currentPage = 0;
        public Uri url;
        private int pageSelected = 0;

        public Form1()
        {
            InitializeComponent();
            string path = Path.Combine(Environment.CurrentDirectory, @"www\", "template.html");
            PagePreview.Source = new Uri(path);
            //htmlPreview.Navigate(path);
            htmlPreview.Source = new Uri(path);
        }

        private async void FastColoredTextBox1_TextChangedDelayed(object sender, FastColoredTextBoxNS.TextChangedEventArgs e)
        {
            //htmlPreview.Document.GetElementById("content").InnerHtml = fastColoredTextBox1.Text;
            //Log("Atualizando Preview");
            await htmlPreview.EnsureCoreWebView2Async();
            string js = "document.getElementById(\"content\").innerHTML = `" + fastColoredTextBox1.Text + "`;";
            await htmlPreview.CoreWebView2.ExecuteScriptAsync(js);

            //Skorperlog funciona após ser iniciado por essa função
            await htmlPreview.CoreWebView2.ExecuteScriptAsync("initSpoiler()");
            if (WebComic.Count > 0)
            {
                WebComic[pageSelected].content = fastColoredTextBox1.Text;
            }
        }



        private void button1_Click(object sender, EventArgs e)
        {
            currentPage = (int)downloadNumberStart.Value;
            if (sourceSelect.Text == "Vast Error")
            {
                sourceURL = "https://www.deconreconstruction.com/vasterror/";
            }
            if(sourceSelect.Text == "mspfa")
            {
                sourceURL = "https://mspfa.com/" + ComicRef.Text;
            }
            if(sourceSelect.Text == "custom")
            {
                sourceURL = ComicRef.Text;
            }
            Uri url = new Uri(sourceURL + currentPage);
            WebsiteDownloadTextBox.Text = url.ToString();
            webView21.Source = url;
        }

        public void DownloadWebComicPage(int page)
        {
            HtmlElement htmlContent = htmlDownloadPreview.Document.GetElementById("content");
            HtmlElement htmlCommand = htmlDownloadPreview.Document.GetElementById("command");
            
            if (htmlContent == null)
            {
                return;
            }

            string content = htmlContent.InnerHtml;
            string command = htmlCommand.InnerHtml;
            fastColoredTextBox2.Text = content;
            htmlDownloadPreview.DocumentText = command + "<br>" + content;

            mspa newPage = new mspa()
            {
                page = page,
                command = command,
                content = content
            };
            WebComic.Add(newPage);
            
            if (currentPage < downloadNumberFinish.Value)
            {
                currentPage++;
                url = new Uri(sourceURL + currentPage);
                WebsiteDownloadTextBox.Text = url.ToString();
                //httpRequestHTML(url);
                webView21.Source = url;
                return;
            }
            else
            {
                updateComicData();
            }
        }

        private void updateComicData()
        {

            //WebComicGrid.DataSource = null;
            var bindingList = new BindingList<mspa>(WebComic);
            var source = new BindingSource(bindingList, null);
            WebComicGrid.DataSource = source;
            //WebComicGrid.DataSource = WebComic;
            WebComicGrid.Update();
            WebComicGrid.Refresh();


            Form1.ActiveForm.Text = projectName;
        }

        private async void webView21_NavigationCompleted(object sender, Microsoft.Web.WebView2.Core.CoreWebView2NavigationCompletedEventArgs e)
        {
            await Task.Delay(1000);
            string htmlEncoded = await webView21.CoreWebView2.ExecuteScriptAsync("document.documentElement.outerHTML");
            await Task.Delay(300);
            string htmlDecoded = Regex.Unescape(htmlEncoded);

            htmlDownloadPreview.DocumentText = htmlDecoded;
        }

        private void htmlDownloadPreview_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            DownloadWebComicPage(currentPage);
        }

        private void htmlDownload_Navigating(object sender, WebBrowserNavigatingEventArgs e)
        {
            WebsiteDownloadTextBox.Text = url.ToString();
        }
        
        private async void WebComicGrid_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            if(WebComic.Count > e.RowIndex)
            {
                Log("Selecionou a Row: " + e.RowIndex);
                mspa selectedPage = WebComic[e.RowIndex];
                await PagePreview.EnsureCoreWebView2Async();
                string js = "document.getElementById(\"content\").innerHTML = `" + selectedPage.content + "`;";
                await PagePreview.CoreWebView2.ExecuteScriptAsync(js);
                await PagePreview.CoreWebView2.ExecuteScriptAsync("console.log('content atualizado');");
                //htmlPreview.Document.GetElementById("content").InnerHtml = selectedPage.content;
                fastColoredTextBox1.Text = selectedPage.content;
                pageSelected = e.RowIndex;
            }
        }

        private void salvarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string save = JsonConvert.SerializeObject(WebComic, Formatting.Indented);
            string fileName = projectName + ".json";
            string path = Path.Combine(Environment.CurrentDirectory, @"Save\", fileName);
            if (!Directory.Exists(path))
            {
                // Try to create the directory.
                string FolderPath = Path.Combine(Environment.CurrentDirectory, @"Save\");
                DirectoryInfo di = Directory.CreateDirectory(FolderPath);
            }
            File.WriteAllText(path, save);
        }

        private void carregarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory = "c:\\";
                openFileDialog.Filter = "json files (*.json)|*.json|All files (*.*)|*.*";
                openFileDialog.FilterIndex = 2;
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    //Get the path of specified file
                    string filePath = openFileDialog.FileName;

                    //Read the contents of the file into a stream
                    var fileStream = openFileDialog.OpenFile();

                    projectName = Path.GetFileNameWithoutExtension(filePath);

                    using (StreamReader reader = new StreamReader(fileStream))
                    {
                        string jsonString = reader.ReadToEnd();
                        WebComic = JsonConvert.DeserializeObject<List<mspa>>(jsonString);
                        updateComicData();
                    }
                }
            }
        }
        private void inglesPortuguesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string text = fastColoredTextBox1.Text;
            //fastColoredTextBox1.Text = wcutils.googleTranslate(text);
        }

        

        private void recentesToolStripMenuItem_MouseEnter(object sender, EventArgs e)
        {
            recentesToolStripMenuItem.DropDownItems.Clear();
            string path = Path.Combine(Environment.CurrentDirectory, @"Save\");
            if (Directory.Exists(path))
            {
                var files = Directory.GetFiles(path);
                foreach (string file in files)
                {
                    if (File.Exists(file))
                    {
                        // This path is a file
                        ToolStripMenuItem fileRecent = new ToolStripMenuItem(file, null, loadRecentFile);
                        recentesToolStripMenuItem.DropDownItems.Add(fileRecent);
                    }
                }
            }
        }

        private void loadRecentFile(object sender, EventArgs e)
        {
            string jsonString = File.ReadAllText(sender.ToString());
            WebComic = JsonConvert.DeserializeObject<List<mspa>>(jsonString);
            projectName = Path.GetFileNameWithoutExtension(sender.ToString());
            updateComicData();
        }

        private void novoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string promptValue = Wcutils.ShowDialog("Nome do projeto", "Novo Projeto");
            projectName = promptValue;

            newProject();
        }

        private void newProject()
        {
            WebComic.Clear();
            updateComicData();
            DebugLog.Text += "Nova WebComic preparada";
        }

        

        private void renomearProjetoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string novoNome = Wcutils.ShowDialog("Renomear projeto", "Novo nome");
            projectName = novoNome;
            Form1.ActiveForm.Text = novoNome;
        }

        private void htmlAddImageBtn_Click(object sender, EventArgs e)
        {
            int cursorPos = fastColoredTextBox1.SelectionStart;
            fastColoredTextBox1.InsertText("<img src=\"\">");
            fastColoredTextBox1.Paste();
        }

        private void pasteImageBtn_Click(object sender, EventArgs e)
        {
            int cursorPos = fastColoredTextBox1.SelectionStart;
            fastColoredTextBox1.InsertText("<img src=\"" + Clipboard.GetText() + "\">");
            
        }

        private void htmlPasteImageBtn_Click(object sender, EventArgs e)
        {
            string text = "";
            if (Clipboard.ContainsText())
            {
                text = Clipboard.GetText();
            }
            else
            {
                text = Clipboard.GetText();
            }
            fastColoredTextBox1.InsertText("<img src=\""+ text + "\">");
        }

        private void WebComicGrid_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (WebComic.Count < e.RowIndex)
            {
                mspa selectedPage = WebComic[e.RowIndex];
                selectedPage.command = WebComicGrid[e.ColumnIndex, e.RowIndex].Value.ToString();
                fastColoredTextBox1.Text = selectedPage.command;
                pageSelected = e.RowIndex;
            }
        }

        private void createNewPageBtn_Click(object sender, EventArgs e)
        {
            mspa newPage = new mspa()
            {
                page = WebComic.Count+1,
                command = "",
                content = ""
            };
            WebComic.Add(newPage);
            Log("Nova pagina: " + newPage.page + " | " + newPage);
            updateComicData();
            WebComicGrid.ClearSelection();
            WebComicGrid.FirstDisplayedScrollingRowIndex = WebComicGrid.RowCount - 1;
            WebComicGrid.Rows[WebComicGrid.RowCount-1].Selected = true;
        }
        public void Log(string toLog)
        {
            DebugLog.Text += toLog + "\n";
            DebugLog.SelectionStart = DebugLog.Text.Length;
            DebugLog.ScrollToCaret();
        }

        private void htmlImageBtn_Click(object sender, EventArgs e)
        {
            int cursorPos = fastColoredTextBox1.SelectionStart;
            fastColoredTextBox1.InsertText("<img src=\"\">");
        }


        private void ColorPickerBtn_Click(object sender, EventArgs e)
        {
            ShowColorPicker();
        }

        
        private void colorPreviewPanel_DoubleClick(object sender, EventArgs e)
        {
            ShowColorPicker();
        }
        public void ShowColorPicker()
        {
            using (ColorPickerDialog dialog = new ColorPickerDialog
            {
                Color = colorPreviewPanel.BackColor,
            })
            {
                //dialog.CustomColorsLoading += this.DialogCustomColorsLoadingHandler;
                //dialog.PreviewColorChanged += this.DialogColorChangedHandler;

                if (dialog.ShowDialog(this) == DialogResult.OK)
                {
                    colorPreviewPanel.BackColor = dialog.Color;
                }

                //dialog.PreviewColorChanged -= this.DialogColorChangedHandler;
            }
        }

        private void publicarProjetoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string sourcePath = Path.Combine(Environment.CurrentDirectory, @"www\");
            string DestinyPath = Path.Combine(Environment.CurrentDirectory, @"Publish\");
            Wcutils.CopyDirectory(sourcePath, DestinyPath, true);
            string save = JsonConvert.SerializeObject(WebComic, Formatting.Indented);
            string fileName = "webcomic.json";
            string path = Path.Combine(DestinyPath, fileName);
            File.WriteAllText(path, save);
            Process.Start("explorer.exe", DestinyPath);
        }
        private void CreateChatSystemBtn_Click(object sender, EventArgs e)
        {
            string text = "";
            string chatSystem = ChatSystemDropDown.Text;
            bool toRight = false;
            string chatName = ChatCharName.Text;
            string selectedText = fastColoredTextBox1.SelectedText;
            Color selectedColor = colorPreviewPanel.BackColor;

            Log(ChatSystemDropDown.Text);
            if (chatSystem == "SkerperLog")
            {
                text = Wcutils.SkorperLogDiv();
            }
            if (chatSystem == "AmongUs")
            {
                text = Wcutils.AmongUsDiv();
            }
            if (chatSystem == "Earthbound")
            {
                text = Wcutils.Earthbound();
            }
            if (chatSystem == "gimpact")
            {
                text = wcCss.gimpact(chatName);
            }
            if (chatSystem == "dcord2")
            {
                text = wcCss.dcord2();
            }
            if (chatSystem == "aol")
            {
                text = wcCss.aol();
            }
            if (chatSystem == "TF2")
            {
                text = wcCss.TF2();
            }
            if (chatSystem == "PCTERMINAL")
            {
                text = wcCss.PCTERMINAL();
            }
            if (chatSystem == "YBComment")
            {
                text = wcCss.ytcom(chatName);
            }
            if (chatSystem == "falloutvgnorm")
            {
                text = wcCss.falloutvgnorm(selectedText);
            }
            if (chatSystem == "hylics2")
            {
                text = wcCss.hylics2(selectedText);
            }
            if (chatSystem == "jazztronauts")
            {
                text = wcCss.jazztronauts(chatName, selectedText, toRight);
            }
            if (chatSystem == "phone")
            {
                text = wcCss.phone(chatName, selectedColor);
            }
            if (chatSystem == "minecraft")
            {
                text = wcCss.mcchat(selectedText);
            }
            if (chatSystem == "Detroit")
            {
                text = wcCss.dthuman(selectedText);
            }
            if (chatSystem == "Undertale Menu")
            {
                text = wcCss.UTmenu3();
            }
            if (chatSystem == "robloxScore")
            {
                text = wcCss.roblox(selectedColor);
            }
            if (chatSystem == "deltarune")
            {
                text = wcTextBoxes.deltarune(selectedText);
            }
            if (chatSystem == "Ace Atorney")
            {
                text = wcTextBoxes.acewrap(chatName, selectedText);
            }
            if (chatSystem == "HollowKnight")
            {
                text = wcTextBoxes.HollowKnight(selectedText);
            }
            if (chatSystem == "botw")
            {
                text = wcTextBoxes.botw(chatName, selectedText);
            }
            if (chatSystem == "picto")
            {
                text = wcTextBoxes.picto(selectedColor, chatName, selectedText);
            }
            fastColoredTextBox1.InsertText(text);
        }
        private void createChatTextBtn_Click(object sender, EventArgs e)
        {
            string text = "";
            string chatSystem = ChatSystemDropDown.Text;
            bool toRight = false;
            string chatName = ChatCharName.Text;
            string selectedText = fastColoredTextBox1.SelectedText;
            Color selectedColor = colorPreviewPanel.BackColor;
            if (chatSystem == "SkerperLog")
            {
                text = Wcutils.SkorperLogChat(chatName, selectedColor);
            }
            if (chatSystem == "AmongUs")
            {
                text = Wcutils.AmongUsChat(chatName);
            }
            if (chatSystem == "Earthbound")
            {
                text = Wcutils.Earthbound();
            }
            if (chatSystem == "dcord2")
            {
                text = wcCss.dcord2Chat(chatName, selectedColor);
            }
            if (chatSystem == "phone")
            {
                text = wcCss.phoneChat(selectedText, toRight);
            }
            if (chatSystem == "Undertale Menu")
            {
                text = wcCss.UTmenu3Item();
            }
            if (chatSystem == "picto")
            {
                text = wcTextBoxes.pictoChat(selectedColor, chatName, selectedText);
            }

            fastColoredTextBox1.InsertText(text);
        }

        private void EffectBtn_Click(object sender, EventArgs e)
        {
            string text = "";
            string selectedText = fastColoredTextBox1.SelectedText;
            string chatSystem = cssEffectSelector.Text;
            string chatName = ChatCharName.Text;
            Color selectedColor = colorPreviewPanel.BackColor;
            if (chatSystem == "glitch")
            {
                text = wcCss.glitch(selectedText);
            }
            if (chatSystem == "COMBOBOX")
            {
                text = wcCss.COMBOBOX(selectedText, selectedColor);
            }
            if (chatSystem == "shake")
            {
                text = wcCss.shake(selectedText);
            }
            if (chatSystem == "RAINBOW")
            {
                text = wcCss.RAINBOW(selectedText);
            }
            if (chatSystem == "Diary")
            {
                text = wcCss.Diary(selectedText);
            }
            if (chatSystem == "Minecraft Button")
            {
                text = wcCss.mcBtn(selectedText);
            }
            if (chatSystem == "Minecraft Placa")
            {
                text = wcCss.mcsign(selectedText);
            }
            if (chatSystem == "Animar texto sequencial")
            {
                text = wcCss.TextAppearingSequentially(selectedText);
            }
            if (chatSystem == "Doc Scratch")
            {
                text = wcCss.DocScratch(selectedColor, selectedText);
            }

            fastColoredTextBox1.InsertText(text);
        }

        private void bonitificarHTMLToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string text = fastColoredTextBox1.Text;
            //XElement.Parse(text).ToString();
            fastColoredTextBox1.Text = text;
        }

        private void fastColoredTextBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (!e.Alt && e.KeyCode == Keys.Return)
            {
                fastColoredTextBox1.InsertText("<br>");
            }
            if (e.Control && e.KeyCode == Keys.E)
            {
                EffectBtn_Click(sender, e);
            }
            if (e.Control && e.KeyCode == Keys.Q)
            {
                CreateChatSystemBtn_Click(sender, e);
            }
            if (e.Control && e.KeyCode == Keys.W)
            {
                createChatTextBtn_Click(sender, e);
            }
        }

        private void logToolStripMenuItem_Click(object sender, EventArgs e)
        {
            splitContainer7.Panel2Collapsed = !splitContainer7.Panel2Collapsed;
            if (splitContainer7.Panel2Collapsed)
            {
                splitContainer7.Panel2.Hide();
            }
            else
            {
                splitContainer7.Panel2.Show();
            }
        }

    }
}
