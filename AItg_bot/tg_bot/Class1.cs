using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TelegramBotExperiments;

namespace TelegramBotExperiments
{
     public class Class1
    {
       public static List<string> matches = new List<string>();
        public async Task Second(string arg)
        {
            
            arg = arg.Replace("\n", "");
            using (var client = new HttpClient())
            {
                var request = new HttpRequestMessage(HttpMethod.Post, "http://192.168.43.109:5000/generate_text");
                //var myText = "top vegan food, eco-bags and best eco-food. Green solution for your buissness";
                //request.Content = new StringContent("{\"input\": \"  \" }", Encoding.UTF8, "application/json");
                request.Content = new StringContent($"{{\"input\": \"{arg}\"}}", Encoding.UTF8, "application/json");
                using (var response = await client.SendAsync(request))
                {
                    response.EnsureSuccessStatusCode();
                    var responseContent = await response.Content.ReadAsStringAsync();
                    string myString = responseContent;
                     Console.WriteLine("qqqqqqqq:  " + myString);
                    string pattern = @"www\.\w+";

                   // List<string> matches = new List<string>();
                    foreach (Match match in Regex.Matches(myString, pattern))
                    {
                        matches.Add(match.Value);
                    }

                    Console.WriteLine(string.Join(", ", matches));
                    //  Console.WriteLine("after cut: " + result);

                }
            }
            
        }
    }
}
