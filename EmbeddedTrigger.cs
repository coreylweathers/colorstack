using System;
using System.Collections.Generic;
using System.Net;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Twilio.TwiML;
using Twilio.TwiML.Messaging;

namespace oiliwt.Functions
{
    public static class EmbeddedTrigger
    {
        [Function("EmbeddedTriggerGet")]
        public static HttpResponseData Run([HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequestData req, FunctionContext executionContext)
        {
            var logger = executionContext.GetLogger("EmbeddedTrigger");
            logger.LogInformation("C# HTTP trigger function processed a request.");


            var msgResponse = new MessagingResponse();
            var msg = new Message();
            msg.Body("Here's a random fact about Corey!");
            msg.Body(GetAFact());

            msgResponse.Append(msg);

            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "text/xml; charset=utf-8");

            response.WriteString(msgResponse.ToString());

            return response;
        }

        [Function("EmbeddedTriggerPost")]
        public static HttpResponseData Post([HttpTrigger(AuthorizationLevel.Anonymous, "post")]HttpRequestData requestData, FunctionContext executionContext)
        {
            var voiceResponse = new VoiceResponse();
            
            voiceResponse.Say("All operators are busy at this time. Your call is important to us. Please hold and the next available operator will be on the line shortly.", voice: "woman");
            voiceResponse.Pause(2);
            voiceResponse.Play(new Uri("https://demo.twilio.com/docs/classic.mp3"));
            voiceResponse.Say("Thanks for waiting. Please use this promo code for $15 on Twilio. Thank you for calling and have a great day!");
            voiceResponse.Hangup();
            
            var response = requestData.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "text/xml; charset=utf-8");
            response.WriteString(voiceResponse.ToString());

            return response; 
        }

        private static string GetAFact()
        {
            var facts = new List<string>
            {
                "Corey doesn't share food (unless it's a 🥗) https://kliptok.com/clip/ImportantEagerGalagoFailFish",
                "Corey is forgetful (beyond belief 🤦🏾‍♂️)",
                "Corey doesn't share food but apparently you can steal it from his 🍽 https://www.twitch.tv/twilio/clip/CuteImpartialReindeerUnSane",
                "Corey is on Kliptok https://kliptok.com/cldubya (shout out to Jeff Fritz 😉)",
                "Corey is his Twitch channel's (twitch.tv/cldubya) personal ASMR????",
                "Corey draws the most AMAZING crumb dots! https://www.twitch.tv/twilio/clip/ObliviousArtsyHorseradishRalpherZ",
                "Corey remains the Twilio Swag King (despite all naysayers including that one guy in the UK who thinks otherwise)",
                "Corey is also affectionately known as CLI Dubya but you can pronounce that as C.L. DuBois 🤵🏾🙌🏾",
                "Corey does not love VIM (unless you're talking about the sneaker store)",
                "Corey only writes production JS on live streams (which means you obviously shouldn't trust it) twitch.tv/cldubya",
                "Corey is horrible at spelling when coding live",
                "Corey knows Notepad++ SAVES LIVES!!!",
                "Corey and RJ Pearson have very different interpretations of production JS",
                "Corey loves to drink water & water based drinks",
                "Corey is 64 years old 😉",
                "Corey works for a communication company but still forgets to text people back",
                "Corey loves to send spicy tweets while live on Twitch",
                "Corey loves celebrating loved ones of the world 😊 (everyone else??? not so much 😒) https://clips.twitch.tv/SmokyDreamyBeanPipeHype-AWdseXuqKG9CGZQM",
                "Corey is neither living nor dead. But some have spotted him around the world. https://twitter.com/heccbrent/status/1428853049087377411?s=20"
            };

            var selected = new Random().Next(facts.Count);
            return facts[selected];

        }
    }
}
