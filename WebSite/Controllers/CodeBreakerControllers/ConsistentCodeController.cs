﻿using CodeBreakerIntegrationLib;
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
    public class ConsistentCodeController : ApiController
    {
        [HttpPost]
        public HttpResponseMessage ConsistentCodes()
        {
            MemoryStream mStream = new MemoryStream();
            Request.Content.CopyToAsync(mStream).Wait();
            mStream.Seek(0, SeekOrigin.Begin);
            byte[] jsonBytes = new byte[mStream.Length];
            mStream.Read(jsonBytes, 0, Convert.ToInt32(mStream.Length));
            string jsonData = Encoding.UTF8.GetString(jsonBytes);

            ConsistentCodeRequest consistentCodeRequest = (ConsistentCodeRequest)JsonConvert.DeserializeObject(jsonData, typeof(ConsistentCodeRequest));

            FSharpList<FSharpList<int>> consistentCodes = Game.Engine.codesConsistentWithOutcomes(
                consistentCodeRequest.CodeLength,
                consistentCodeRequest.ColorCount,
                SeqModule.ToList(consistentCodeRequest.GameInfo.Select(go => new Game.Engine.GuessOutcome(SeqModule.ToList(go.Guess), new Game.Engine.Outcome(go.BlackCount, go.WhiteCount))))
            );

            ConsistentCodeResponse consistentCodeResponse = new ConsistentCodeResponse
            {
                ConsistentCodes = consistentCodes.Select(cc => cc.ToArray()).ToArray()
            };

            string jsonContent = JsonConvert.SerializeObject(consistentCodeResponse);
            HttpResponseMessage response = HttpResponseMessageFactory.GetResponse(HttpStatusCode.OK);
            response.Content = new StringContent(jsonContent);
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            return response;
        }
    }
}
