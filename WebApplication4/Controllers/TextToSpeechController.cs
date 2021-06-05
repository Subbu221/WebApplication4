using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Speech.Synthesis;
using System.Globalization;

namespace WebApplication4.Controllers
{
    public class TextToSpeechController : ApiController
    {
        [System.Web.Http.Route("GetVoice/{Text}")]
        [HttpGet]
        public HttpResponseMessage GetAsync(string Text)
        {
            try
            {
                HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
                var audioStream = GetAudio(Text);
                audioStream.Position = 0;
                response.Content = new StreamContent(audioStream);
                response.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("audio/wav");
               // return Request.CreateResponse(HttpStatusCode.OK, response);
                return response;
            }
            catch (Exception ex)
            {
                var res = new HttpResponseMessage();
                res.StatusCode = HttpStatusCode.BadRequest;
                return res;
            }

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

    }
}
