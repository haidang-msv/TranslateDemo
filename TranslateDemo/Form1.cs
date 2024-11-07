using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net.Http; /*add reference System.Net.Http.dll*/
using System.Web.Script.Serialization; /*add reference System.Web.Extensions.dll*/
using System.Windows.Forms;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections;
using DarrenLee.Translator;

namespace TranslateDemo
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }





        public string bingTranslateText(string inputText, string fromLangISOcode, string toLangISOcode)
        {
            string translatedText = "";// mình sẽ hướng dẫn các bạn viết một ứng dụng dịch ngôn ngữ
            //try
            //{
            //    TranslatorService.LanguageServiceClient client = new TranslatorService.LanguageServiceClient();
            //    translatedText = client.Translate("your App ID", inputText, "vi", "en");
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.Message);
            //}

            //translatedText = Translator.Translate(inputText, "vi", "ja"); // japanese
            //translatedText = Translator.Translate(inputText, "vi", "my"); // burmese
            //translatedText = Translator.Translate(inputText, "vi", "th"); // thailand
            if (inputText.Length > 256)
                inputText = inputText.Substring(0, 256); //maxlength=30720
            translatedText = Translator.Translate(inputText, fromLangISOcode, toLangISOcode);
            return translatedText;
        }

        private void textBox1_Validating(object sender, CancelEventArgs e)
        {
            string s = textBox1.Text.Trim();
            if (s == "") return;
            ////textBox2.Text = TranslateText(textBox1.Text);
            if (textBox2.Text.Trim().Length == 0)
                textBox2.Text = bingTranslateText(s, "vi", "en");
            if (textBox3.Text.Trim().Length == 0)
                textBox3.Text = bingTranslateText(s, "vi", "ja");
        }

        private void textBox2_Validating(object sender, CancelEventArgs e)
        {
            string s = textBox2.Text.Trim();
            if (s == "") return;
            if (textBox1.Text.Trim().Length == 0)
                textBox1.Text = bingTranslateText(s, "en", "vi");
            if (textBox3.Text.Trim().Length == 0)
                textBox3.Text = bingTranslateText(s, "en", "ja");
        }

        private void textBox3_Validating(object sender, CancelEventArgs e)
        {
            string s = textBox3.Text.Trim();
            if (s == "") return;
            if (textBox1.Text.Trim().Length == 0)
                textBox1.Text = bingTranslateText(s, "ja", "vi");
            if (textBox2.Text.Trim().Length == 0)
                textBox2.Text = bingTranslateText(s, "ja", "en");
        }



        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            string s = textBox1.Text.Trim();
            if (s == "")
            {
                stsCountWords.Text = "0";
                return;
            }
            string[] arr = s.Split(' ');
            stsCountWords.Text = arr.Length.ToString();
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            string s = textBox2.Text.Trim();
            if (s == "")
            {
                stsCountWords.Text = "0";
                return;
            }
            string[] arr = s.Split(' ');
            stsCountWords.Text = arr.Length.ToString();
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            string s = textBox3.Text.Trim();
            if (s == "")
            {
                stsCountWords.Text = "0";
                return;
            }
            string[] arr = s.Split(' ');
            stsCountWords.Text = arr.Length.ToString();
        }

        private void textBox1_Enter(object sender, EventArgs e)
        {
            textBox1_TextChanged(null, null);
        }

        private void textBox2_Enter(object sender, EventArgs e)
        {
            textBox2_TextChanged(null, null);
        }

        private void textBox3_Enter(object sender, EventArgs e)
        {
            textBox3_TextChanged(null, null);
        }

        /**********************************************************************/
        private void textBox4_Validating(object sender, CancelEventArgs e)
        {
            string s = textBox4.Text.Trim();
            if (s == "") return;
            textBox5.Text = TranslateText(s);
        }

        private void textBox5_Validating(object sender, CancelEventArgs e)
        {

        }


        public string TranslateText(string input)
        {
            //return "";
            // Set the language from/to in the url (or pass it into this function)
            string url = string.Format("https://translate.googleapis.com/translate_a/single?client=gtx&sl={0}&tl={1}&dt=t&q={2}", "vi", "en", Uri.EscapeUriString(input));
            HttpClient httpClient = new HttpClient(); /*thay = WebClient thử*/
            string result = httpClient.GetStringAsync(url).Result;

            // Get all json data
            //var jsonData = new JavaScriptSerializer().Deserialize<List<dynamic>>(result);
            List<object> jsonData = JsonConvert.DeserializeObject<List<object>>(result);

            JArray ArrJson = JArray.Parse(jsonData[0].ToString());


            JArray jArr = JArray.Parse(result);
            JArray jArr1 = (JArray)(jArr[0]);
            JArray jArr2 = (JArray)(jArr1[0]);
            //Newtonsoft.Json.Linq.JEnumerable<Newtonsoft.Json.Linq.JToken> jTokens = jArr[0].Children();
            Console.WriteLine("");
            foreach (JToken item in jArr.Children())
            {
                Console.WriteLine("print detail:");
                var itemPro = item.Children<JProperty>();
                //you could do a foreach or a linq here depending on what you need to do exactly with the value
                //var myElement = itemProperties.FirstOrDefault(x => x.Name == "url");
                //var myElementValue = myElement.Value; ////This is a JValue type
                Console.WriteLine("print detail:");
            }

            //// Extract just the first array element (This is the only data we are interested in)
            //var translationItems = jsonData[0];

            // Translation Data
            string translation = "";

            //// Loop through the collection extracting the translated objects
            //foreach (object item in translationItems)
            //{
            //    // Convert the item array to IEnumerable
            //    IEnumerable translationLineObject = item as IEnumerable;

            //    // Convert the IEnumerable translationLineObject to a IEnumerator
            //    IEnumerator translationLineString = translationLineObject.GetEnumerator();

            //    // Get first object in IEnumerator
            //    translationLineString.MoveNext();

            //    // Save its value (translated text)
            //    translation += string.Format(" {0}", Convert.ToString(translationLineString.Current));
            //}

            // Remove first blank character
            if (translation.Length > 1) { translation = translation.Substring(1); };

            // Return translation
            return translation;
        }

        public string ggTranslateText(string input)
        {
            return string.Empty;
            //string url = string.Format("https://translate.googleapis.com/translate_a/single?client=gtx&sl={0}&tl={1}&dt=t&q={2}", "vi", "en", Uri.EscapeUriString(input));
            //HttpClient httpClient = new HttpClient();
            //string result = httpClient.GetStringAsync(url).Result;
            //var jsonData = new JavaScriptSerializer().Deserialize<List<dynamic>>(result);
            //var translationItems = jsonData[0];
            //string translation = "";
            //foreach (object item in translationItems)
            //{
            //    IEnumerable translationLineObject = item as IEnumerable;
            //    IEnumerator translationLineString = translationLineObject.GetEnumerator();
            //    translationLineString.MoveNext();
            //    translation += string.Format(" {0}", Convert.ToString(translationLineString.Current));
            //}
            //if (translation.Length > 1) { translation = translation.Substring(1); };
            //return translation;
        }














    }
}
