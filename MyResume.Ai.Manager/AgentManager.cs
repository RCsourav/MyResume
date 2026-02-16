using Azure;
using Azure.AI.OpenAI;
using Azure.AI.OpenAI.Chat;
using Azure.Core;
using MyResume.Model.AiModels;
using OpenAI.Chat;
using Azure.Identity;
using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using System.Text.Json;

namespace MyResume.Ai.Manager
{
    public class AgentManager
    {
        public async Task<string> GetResponse( string promt)
        {
            try
            {
                string openAiEndpoint = Environment.GetEnvironmentVariable("AZURE_OPENAI_ENDPOINT")
                    ?? throw new ArgumentNullException("OpenAI endpoint is not provided in config file.")
                    ,openAiKey= Environment.GetEnvironmentVariable("AZURE_OPENAI_KEY")
                    ?? throw new ArgumentNullException("OpenAI key is not provided in config file.")
                    , embeddingDeployment = Environment.GetEnvironmentVariable("EMBEDDING_MODEL")
                    ?? throw new ArgumentNullException("OpenAI key is not provided in config file.")
                    , chatDeployment = Environment.GetEnvironmentVariable("DEPLOYMENT_MODEL")
                    ?? throw new ArgumentNullException("OpenAI key is not provided in config file.")
                    , searchEndpoint = Environment.GetEnvironmentVariable("AI_SEARCH_ENDPOINT")
                    ?? throw new ArgumentNullException("OpenAI key is not provided in config file.")
                    , searchKey = Environment.GetEnvironmentVariable("AI_SEARCH_KEY")
                    ?? throw new ArgumentNullException("OpenAI key is not provided in config file.")
                    , searchIndex = Environment.GetEnvironmentVariable("AI_SEARCH_INDEX")
                    ?? throw new ArgumentNullException("OpenAI key is not provided in config file.");

                var openAiClient = new AzureOpenAIClient(new Uri(openAiEndpoint)
                    , new AzureKeyCredential(openAiKey));
                var chatClient=openAiClient.GetChatClient(chatDeployment);
#pragma warning disable AOAI001 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
                var auth = DataSourceAuthentication.FromApiKey(searchKey);
#pragma warning restore AOAI001 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
#pragma warning disable AOAI001 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
                var searchClient = new AzureSearchChatDataSource()
                {
                    Endpoint = new Uri(searchEndpoint),
                    Authentication= auth,
                    IndexName = searchIndex,
                };
#pragma warning restore AOAI001 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.

                var option=new ChatCompletionOptions()
                {
                    Temperature = 0.7f,
                    TopP = 0.9f,
                    FrequencyPenalty = 0.0f,
                    PresencePenalty = 0.0f
                };

#pragma warning disable AOAI001 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
                option.AddDataSource(searchClient);
#pragma warning restore AOAI001 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.

                var messages =new[]{(ChatMessage)( new SystemChatMessage("")) ,  new UserChatMessage(promt) };

                var respons= chatClient.CompleteChat(messages,option);
                var reply = respons.Value.Content[0].Text;

                return reply;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
