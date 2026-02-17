using Azure.AI.Projects;
using Azure.AI.Projects.OpenAI;
using Azure.Identity;
using Microsoft.Extensions.Logging;
using OpenAI;
using OpenAI.Responses;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace MyResume.Ai.Manager
{
    public interface IAgenticManager
    {
        public Task<string> GetResponse(string promt);
    }
    public class AgenticManager : IAgenticManager
    {
        private readonly ILogger<AgenticManager> _logger;

        public AgenticManager(ILogger<AgenticManager> logger)
        {
            _logger = logger;
        }

        public async Task<string> GetResponse(string promt)
        {
#pragma warning disable OPENAI001 // Type is for evaluation purposes only and is subject to change or removal in future updates. Suppress this diagnostic to proceed.
#pragma warning disable CA2252 // This API requires opting into preview features
            string projectEndpoint = Environment.GetEnvironmentVariable("AZURE_PROJECT_ENDPOINT")
                ?? throw new InvalidOperationException("Environment variable 'AZURE_PROJECT_ENDPOINT' is not set or empty.")
                , tenantId = Environment.GetEnvironmentVariable("AZURE_TANENT_ID")
                 ?? throw new InvalidOperationException("Environment variable 'AZURE_TANENT_ID' is not set or empty.")
                , clientId = Environment.GetEnvironmentVariable("AZURE_CLIENT_ID")
                ?? throw new InvalidOperationException("Environment variable 'AZURE_CLIENT_ID' is not set or empty.")
                , clientSecret = Environment.GetEnvironmentVariable("AZURE_CLINET_SECRET")
                 ?? throw new InvalidOperationException("Environment variable 'AZURE_CLINET_SECRET' is not set or empty.")
                 , agentName = Environment.GetEnvironmentVariable("AZURE_AGENT_NAME")
                ?? throw new InvalidOperationException("Environment variable 'AZURE_AGENT_NAME' is not set or empty.");

            var credential = new ClientSecretCredential(tenantId, clientId, clientSecret);
            Uri uri = new Uri(projectEndpoint)
                ?? throw new InvalidOperationException("Project uri can not be null.");

            AIProjectClient projectClient = new(uri, credential);

            AgentRecord agentRecord = projectClient.Agents.GetAgent(agentName);

            ProjectResponsesClient responseClient = projectClient.OpenAI.GetProjectResponsesClientForAgent(agentRecord);

            ResponseResult response = responseClient.CreateResponse(promt);

            return response.GetOutputText();
        }
    }
}
