using OpenAI_API;
using OpenAI_API.Completions;
using OpenAI_API.Models;

namespace KDLCompiler
{
    internal class OpenAI
    {
        private const string ApiKey = "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";

        public OpenAI()
        {
        }
        internal string GetResponse(string prompt)
        {
            //  var api = new OpenAIAPI(ApiKey);
            //string result = await api.Completions.GetCompletion(prompt);
            string answer = string.Empty;
            var openai = new OpenAIAPI(ApiKey);
            CompletionRequest completion = new CompletionRequest();
            completion.Prompt = prompt;
            completion.Model = Model.ChatGPTTurbo0301;
            completion.MaxTokens = 4000;
            
            var result = openai.Completions.CreateCompletionAsync(completion);
            if (result != null)
            {
                foreach (var item in result.Result.Completions)
                {
                    answer = item.Text;
                }
                return answer;
            }
            else
            {
                return "Not found";
            }
        }
    }
}