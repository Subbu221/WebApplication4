﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Speech.Synthesis;
using System.IO;
using System.Globalization;

namespace WebApplication4.Controllers
{
    public class SpeechController : ApiController
    {
        // GET: api/Speech
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }
        [HttpGet]
        // GET: api/Speech/5
        public string Get(string text)
        {

            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
            var audioStream = GetAudio(text);
            audioStream.Position = 0;
            response.Content = new StreamContent(audioStream);
            byte[] bytes = audioStream.ToArray();

            //response.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("audio/wav");
            string base64Text = "data:audio/wav;base64,";
            var te = Convert.ToBase64String(bytes);
            base64Text = base64Text + te;
            return base64Text;
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

        // POST: api/Speech
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/Speech/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Speech/5
        public void Delete(int id)
        {
        }
    }
}