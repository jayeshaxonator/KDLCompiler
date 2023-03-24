using System;
using System.IO;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace KDLCompiler
{
    public class GPTPromptsRunner
    {
        public static void Run(string inputFilePath, string outputFilePath)
        {
            ChatGPTAPI  ai = new ChatGPTAPI();
            
            // Load JSON from file
            string jsonString = File.ReadAllText(inputFilePath);
            JsonDocument document = JsonDocument.Parse(jsonString);

            // Create new JSON array for output
            var outputArray = new JsonArray();

            // Loop through each element in the input array and calculate length
            foreach (JsonElement element in document.RootElement.EnumerateArray())
            {
                // int length = element.GetRawText().Length;
                // outputArray.Add(JsonValue.Create(length));
                string prompt = element.GetString();
                Console.WriteLine("Running prompt: "+ prompt);
                string result = ai.GetResponse(prompt);
                Console.WriteLine("ChatGPT Response: " + result);
                outputArray.Add(JsonValue.Create(result));
                //add delay 
                System.Threading.Thread.Sleep(5000);
            }

            // Write output JSON to file
            File.WriteAllText(outputFilePath, outputArray.ToString());
        }
    }
}
