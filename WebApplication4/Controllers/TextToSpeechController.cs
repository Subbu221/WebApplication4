using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Speech.Synthesis;
using System.Globalization;
using System.Net.Http.Headers;
using System.Web;
using System.Text;

namespace WebApplication4.Controllers
{
    public class ResponseObject
    {
        public string Response { get; set; }
        public string Error { get; set; }


    }
    public class TextToSpeechController : ApiController
    {
        
        [HttpGet]
        public ResponseObject Get(string Text)
        {
            var rsponseObject = new ResponseObject();
            try
            {
                HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
                var audioStream = GetAudio(Text);
                audioStream.Position = 0;
                response.Content = new StreamContent(audioStream);
                byte[] bytes = audioStream.ToArray();
                
                //response.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("audio/wav");
                string base64Text = "data:audio/wav;base64,";
                var te = Convert.ToBase64String(bytes);
                base64Text = base64Text + te;
                //return response;
                //return base64Text;
                //return "Subbu";
                //return bytes;

                rsponseObject.Response = base64Text;
            }
            catch (Exception ex)
            {
                var res = new HttpResponseMessage();
                res.StatusCode = HttpStatusCode.BadRequest;
                //byte[] re1s = GetAudio(Text).ToArray();
                //return re1s;
                // return res.StatusCode.ToString(); 

                rsponseObject.Error = ex.Message;

            }
            return rsponseObject;
        }
        private static MemoryStream GetAudio(string input)
        {
            MemoryStream audioStream = new MemoryStream();

            var t = new System.Threading.Thread(new System.Threading.ThreadStart(() =>
            {
                SpeechSynthesizer synthesizer = new SpeechSynthesizer();
                synthesizer.SetOutputToDefaultAudioDevice();
                synthesizer.SelectVoiceByHints(VoiceGender.Female, VoiceAge.Teen, 0, CultureInfo.GetCultureInfo("en-gb"));
               
                synthesizer.SetOutputToWaveStream(audioStream);

                //add a space between all characters to spell it out.
                //string val = String.Join<char>(" ", input);
                synthesizer.Speak(input);

            }));

            t.Start();
            t.Join();

            return audioStream;
        }

        [HttpGet]
        public string Text()
        {
            return "abc";
        }
        [HttpGet]
        public HttpResponseMessage Generate()
        {
            var stream = new MemoryStream();
            // processing the stream.
            var result = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new ByteArrayContent(stream.ToArray())
            };
            result.Content.Headers.ContentDisposition =
                new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment")
                {
                    FileName = "CertificationCard.pdf"
                };
            result.Content.Headers.ContentType =
                new MediaTypeHeaderValue("application/octet-stream");
            return result;
        }
    }
}
