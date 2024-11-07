using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TranslationWS;

namespace TranslateDemo
{
    public partial class Form2 : Form
    {
        string viLang = "VI", enLang = "EN", jpLang = "JP";
        public Form2()
        {
            InitializeComponent();
            //
            InitBgwTranslating();
            //
            cboSource.Items.Clear();
            cboSource.Items.Add(enLang);
            cboSource.Items.Add(viLang);
            cboSource.Items.Add(jpLang);
            //
            cboTarget.Items.Clear();
            cboTarget.Items.Add(enLang);
            cboTarget.Items.Add(viLang);
            cboTarget.Items.Add(jpLang);
            //
            btnClear_Click(null, null);
        }

        private void btnTranslate_Click(object sender, EventArgs e)
        {
            stsCounter.Text = "";
            stsMessage.Text = "Translating...";
            TargetText = txtTarget.Text;
            btnClear.Enabled = btnTranslate.Enabled = false;
            txtSource.ReadOnly = txtTarget.ReadOnly = true;
            string sourceLang = cboSource.Text;
            string targetLang = cboTarget.Text;
            string sourceText = txtSource.Text.Trim();
            string times = txtTimes.Text.Trim();
            string[] arr = new string[] { sourceText, sourceLang, targetLang, times };
            bgwTranslating.RunWorkerAsync(arr);
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            stsCounter.Text = "";
            cboSource.Text = enLang;
            cboTarget.Text = viLang;
            txtSource.Text = txtTarget.Text = "";
        }
        
        #region bgw
        BackgroundWorker bgwTranslating;
        void InitBgwTranslating()
        {
            bgwTranslating = new BackgroundWorker();
            bgwTranslating.WorkerReportsProgress = true;
            bgwTranslating.WorkerSupportsCancellation = true;

            bgwTranslating.DoWork += new DoWorkEventHandler(bgwTranslating_DoWork);/* công việc mà thread sẽ làm */
            bgwTranslating.ProgressChanged += new ProgressChangedEventHandler(bgwTranslating_ProgressChanged);/* cập nhật tiến độ thread */
            bgwTranslating.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bgwTranslating_RunWorkerCompleted);/* khi thread hoàn thành công việc */
        }
        void bgwTranslating_DoWork(object sender, DoWorkEventArgs e)
        {
            /* công việc mà thread sẽ làm */
            string[] arr = (string[])e.Argument;
            //string sourceText = txtSource.Text.Trim();
            //string sourceLang = cboSource.Text;
            //string targetLang = cboTarget.Text;
            string sourceText = arr[0];
            string sourceLang = arr[1];
            string targetLang = arr[2];
            int times = 0;int.TryParse(arr[3], out times);
            if (times <= 0) times = 1;
            if (sourceLang == "" || targetLang == "" || sourceText == "") return;
            if (sourceLang == targetLang)
            {
                TranslatingProgressMessage = new KeyValuePair<int, string>(0, string.Empty);
                bgwTranslating.ReportProgress(1);
                return;
            }
            TranslationServices.LanguageCodes s = TranslationServices.LanguageCodes.VI;
            if (sourceLang == enLang) s = TranslationServices.LanguageCodes.EN;
            else if (sourceLang == viLang) s = TranslationServices.LanguageCodes.VI;
            else if (sourceLang == jpLang) s = TranslationServices.LanguageCodes.JA;

            TranslationServices.LanguageCodes t = TranslationServices.LanguageCodes.VI;
            if (targetLang == enLang) t = TranslationServices.LanguageCodes.EN;
            else if (targetLang == viLang) t = TranslationServices.LanguageCodes.VI;
            else if (targetLang == jpLang) t = TranslationServices.LanguageCodes.JA;

            if (s == t)
            {
                TranslatingProgressMessage = new KeyValuePair<int, string>(0, string.Empty);
                bgwTranslating.ReportProgress(1);
                return;
            }
            for (int i = 0; i < times; i++)
            {
             string   TransText = "";
                string user = "haidang@mankichi.net";
                object token = string.Concat(user, DateTime.Today.ToString("yyyyMMdd"));
                TranslationServices ts = new TranslationServices(token, user);
                int status;
                string msg = "";
                TranslationServices.TranslationResult tr = ts.TranslateText(sourceText, s, t, out status, out msg, 3000);
                if (TargetText.Trim().Length > 0) TransText += "\n";
                TransText += tr.TranslatedText;
                int c = 0; int.TryParse(stsCounter.Text, out c);
                //
                TranslatingProgressMessage = new KeyValuePair<int, string>(++c, TransText);
                bgwTranslating.ReportProgress(1);
            }
        }
        void bgwTranslating_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            /* cập nhật tiến độ thread */
            if (!bgwTranslating.CancellationPending)
            {
                string TransText = TranslatingProgressMessage.Value;
                if (TransText != string.Empty) txtTarget.AppendText(TransText);
                stsCounter.Text = TranslatingProgressMessage.Key.ToString();
                TargetText = txtTarget.Text;
            }
        }
        void bgwTranslating_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            /* khi thread hoàn thành công việc */
            //mnuStopChecking.Visible = mnuStopChecking.Enabled = false;
            //mnuExport.Enabled = mnuReset.Enabled = true;
            btnClear.Enabled = btnTranslate.Enabled = true;
            if (e.Cancelled)
            {
                /* Process was cancelled. */
                stsCounter.Text = "";
                stsMessage.Text = "Translate canceled!";
                return;
            }
            else if (e.Error != null)
            {
                /* There was an error running process. The thread aborted. */
                stsCounter.Text = "";
                stsMessage.Text = "Error occurred while importing! The thread aborted.";
                return;
            }
            else
            {
                /* Process was completed. */
                //setClipBoard("Completed");
                btnClear.Enabled = btnTranslate.Enabled = true;
                txtSource.ReadOnly = txtTarget.ReadOnly = false;
                stsMessage.Text = "Translate done.";
                MessageBox.Show("Translate done.", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
        }
        KeyValuePair<int, string> TranslatingProgressMessage = new KeyValuePair<int, string>();
        //string TransText = "";
        string TargetText = "";
        #endregion

        private void SetEnable(bool Enabled) { }

    }

}
