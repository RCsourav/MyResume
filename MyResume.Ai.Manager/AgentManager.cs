using Azure;
using Azure.AI.OpenAI;
using Azure.Core;
using Azure.Identity;
using Microsoft.Extensions.Logging;
using MyResume.Model.AiModels;
using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using System.Text.Json;

namespace MyResume.Ai.Manager
{
    public interface IAgentManager
    {
        Task<string> GetResponse(string promt);
    }
    public class AgentManager : IAgentManager
    {
        private readonly ILogger<AgentManager> _logger;

        public AgentManager(ILogger<AgentManager> logger)
        {
            _logger = logger;
        }
        public async Task<string> GetResponse(string promt)
        {
            _logger.LogInformation("Chat manager started");
            try
            {
                string openAiEndpoint = Environment.GetEnvironmentVariable("AZURE_OPENAI_ENDPOINT")
                    ?? throw new ArgumentNullException("OpenAI endpoint is not provided in config file.")
                    , openAiKey = Environment.GetEnvironmentVariable("AZURE_OPENAI_KEY")
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

                var openAiClient = new OpenAIClient(new Uri(openAiEndpoint)
                    , new AzureKeyCredential(openAiKey));

                var searchClient = new AzureSearchChatExtensionConfiguration()
                {
                    SearchEndpoint = new Uri(searchEndpoint),
                    Authentication = new OnYourDataApiKeyAuthenticationOptions(searchKey),
                    IndexName = searchIndex,

                };

                var extensionOptions = new AzureChatExtensionsOptions()
                {
                    Extensions = { searchClient }
                };


                var option = new ChatCompletionsOptions()
                {
                    Temperature = 0.2f,
                    DeploymentName = chatDeployment
                };

                option.AzureExtensionsOptions = extensionOptions;

                option.Messages.Add(new ChatRequestSystemMessage(@"You are a helpful assistant. Use the data retrieved from the Azure Search index to answer questions.
You are an human like AI assistant that helps people find information about skill, career and experience of Sourav Roy Choudhury. you will respond impersonating as Sourav Roy Choudhury.

Your Azure Search index source contains resume for Sourav. You have Sourav's resume as data. Use that to respond people. Use the data to talk about career and job experience, future vision as lead AI developer and solution architect. Always emphasize how good Sourav is in each of the technology mentioned.

The resume data has educational background of Sourav along with company and job experience and job location and project timeline, when asked, mention all details. When mentioning technical experience always align to resume data.

When ever you are talking about technical skill and experience, never mention those skills which are not mentioned in resume. 

Never mention that you are pulling data from resume or from your general knowledge.

When asked for vision mention about AI vision. When ever responding from resume data, use human like tone first, unless asked for specific type of response. Don't always use bullate points, rather create paragraphs with human like speech. Don't include paragraph headers. Create responses as you are giving technical interview.

At first try to to give short answer and ask if you need to explain. When asked, give more explanation. But always try to respond as a human giving technical interview.

Don't respond 'The requested information is not available in the retrieved data. Please try another query or topic.' rather create response messages such, oops i dont know that. I am sorry i am not aware of that etc."));

                option.Messages.Add(new ChatRequestUserMessage(promt));
                var respons = openAiClient.GetChatCompletions(option);
                var reply = respons.Value.Choices[0].Message.Content;

                return reply;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                throw;
            }
        }
    }
}
