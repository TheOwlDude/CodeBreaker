using CodeBreakerIntegrationLib;
using Microsoft.FSharp.Collections;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace WebSite.Controllers.CodeBreakerControllers
{
    public class GuessController : ApiController
    {

        [HttpPost]
        public HttpResponseMessage Guess()
        {
            MemoryStream mStream = new MemoryStream();
            Request.Content.CopyToAsync(mStream).Wait();
            mStream.Seek(0, SeekOrigin.Begin);
            byte[] jsonBytes = new byte[mStream.Length];
            mStream.Read(jsonBytes, 0, Convert.ToInt32(mStream.Length));
            string jsonData = Encoding.UTF8.GetString(jsonBytes);

            CodeGuess guess = (CodeGuess)JsonConvert.DeserializeObject(jsonData, typeof(CodeGuess));

            Game.Engine.Outcome outcome = Game.Engine.calculateGuessResult(SeqModule.ToList(guess.Guess), SeqModule.ToList(guess.Code));

            GuessOutcome result = new GuessOutcome
            {
                CodeLength = guess.CodeLength,
                ColorCount = guess.ColorCount,
                Code = guess.Code,
                Guess = guess.Guess,
                BlackCount = outcome.blackCount,
                WhiteCount = outcome.whiteCount
            };

            string jsonContent = JsonConvert.SerializeObject(result);
            HttpResponseMessage response = HttpResponseMessageFactory.GetResponse(HttpStatusCode.OK);
            response.Content = new StringContent(jsonContent);
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            return response;
        }
    }
}
