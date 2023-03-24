using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace KDLCompiler
{
    public class GPTPromptsGenerator
    {
        private const string InputFilePath = @"E:\JK\GPTJsonPageGenFiles\GPTInput.jsonc";
        private const string OutputFilePath = @"E:\JK\GPTJsonPageGenFiles\GPTPageContent.jsonc";

        public void Generate()
        {
            var inputJson = File.ReadAllText(InputFilePath);
            inputJson = Regex.Replace(inputJson, @"^\s*//.*$", "", RegexOptions.Multiline);
            var input = JsonConvert.DeserializeObject<Input>(inputJson);
            var prompts = input.Prompts;
            var promptStrings = new List<string>();
            foreach (var prompt in prompts)
            {
                var promptString = GetPromptString(prompt, input);
                promptStrings.Add(promptString);
            }
            var outputJson = JsonConvert.SerializeObject(promptStrings, Formatting.Indented);
            File.WriteAllText(OutputFilePath, outputJson);
        }

        private string GetPromptString(Prompt prompt, Input input)
        {
            string numberString = prompt.Number.ToString();
            string keywords = String.Join(", ", input.Keywords.Select(x => x.ToString()).ToArray());

            string expected_once_list = string.Empty;
            string expected_once_json = string.Empty;
            string json_format = string.Empty;
            if (prompt.ExpectedOnce != null)
            {
                expected_once_list += String.Join(", ", prompt.ExpectedOnce.Select(x => x.ToString()).ToArray());
                expected_once_json += String.Join(", ", prompt.ExpectedOnce.Select(x => $"'{x}': {x}").ToArray());
                json_format +=  $"'Header': {{{expected_once_json}}}";
            }
            string expected_repeat_list = string.Empty;
            string expected_repeat_json = string.Empty;
            if (prompt.ExpectedRepeat != null)
            {
                expected_repeat_list += String.Join(", ", prompt.ExpectedRepeat.Select(x => x.ToString()).ToArray());
                expected_repeat_json += String.Join(", ", prompt.ExpectedRepeat.Select(x => $"'{x}': {x}").ToArray());
                json_format += $", 'Points': '[{{{expected_repeat_json}}},]'";
            }
            if (json_format != string.Empty)
            {
                json_format = $"The response should be in a json format described here: {{{json_format}}}.";
            }
            string additionalNotes = "Note only give JSON and not extra text. All titles, subtitle and descriptions must be less than 45 words";
            var promptString = string.Empty;
            if (prompt.Type == "SinglePointPrompt")
                promptString = $"Give {prompt.What} for the app: {input.AppName}. Here is what the app does: {input.AppDescription}. {prompt.AdditionalPrompt}. Use the keywords '{keywords}' as much as possible in your response. {json_format}. {additionalNotes}";
            else if (prompt.Type == "MultiPointPrompt")
                promptString = $"Give {numberString} {prompt.What} for the app: {input.AppName}. Here is what the app does: {input.AppDescription}. {prompt.AdditionalPrompt}. Provide {expected_once_list}. And for each point provide: {expected_repeat_list}. Use the keywords '{keywords}' as much as possible in your response. Maximum length for headlines and descriptions should be 50 words. {json_format} {additionalNotes}";
            
            return promptString;
        }

    }

    public class Input
    {
        public List<string> Keywords { get; set; }
        public string BackgroundContent { get; set; }
        public string AppName { get; set; }
        public string AppDescription { get; set; }
        public List<Prompt> Prompts { get; set; }
    }

    public class Prompt
    {
        public string Type { get; set; }
        public int Number { get; set; }
        public string What { get; set; }
        public string AdditionalPrompt { get; set; }
        public List<string> ExpectedOnce { get; set; }
        public List<string> ExpectedRepeat { get; set; }
    }
}
