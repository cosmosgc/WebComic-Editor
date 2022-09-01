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
using MaterialSkin;
using MaterialSkin.Controls;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Web.UI.WebControls;

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
            toolTip1.SetToolTip(htmlPasteImageBtn, "Cola uma imagem no html");
            toolTip1.SetToolTip(htmlAddImageBtn, "Add tag imagem no html");
            
        }

        private void FastColoredTextBox1_TextChangedDelayed(object sender, FastColoredTextBoxNS.TextChangedEventArgs e)
        {
            htmlPreview.DocumentText = fastColoredTextBox1.Text;
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

        private void WebComicGrid_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            if(WebComic.Count > e.RowIndex)
            {
                Log("Selecionou a Row: " + e.RowIndex);
                mspa selectedPage = WebComic[e.RowIndex];
                PagePreview.DocumentText = selectedPage.content;
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

        private void htmlImage_Click(object sender, EventArgs e)
        {

            
        }

        private void htmlImageBtn_Click(object sender, EventArgs e)
        {
            int cursorPos = fastColoredTextBox1.SelectionStart;
            fastColoredTextBox1.InsertText("<img src=\"\">");
        }

        private void CreateChatSystemBtn_Click(object sender, EventArgs e)
        {
            string text = "";
            Log(ChatSystemDropDown.Text);
            if(ChatSystemDropDown.Text == "SkerperLog")
            {
                text = Wcutils.SkorperLogDiv();
            }
            fastColoredTextBox1.InsertText(text);
        }
    }
}
