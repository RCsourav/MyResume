using Azure.AI.Agents.Persistent;
using Azure.Identity;
using MyResume.Model.AiModels;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace MyResume.Ai.Manager
{
    public class AgentManager
    {
        public async Task<AiAgentData> CreateAgentAsync(AiAgentData aiAgentData)
        {
            string projectEndpoint = Environment.GetEnvironmentVariable("AZURE_PROJECT_ENDPOINT")
                ?? throw new ArgumentNullException("Project endpoint is not provided in config file.")
                , clientId = Environment.GetEnvironmentVariable("AZURE_CLIENT_ID")
                ?? throw new ArgumentNullException("Client id is not provided in config file.")
                , tenantId = Environment.GetEnvironmentVariable("AZURE_TANENT_ID")
                 ?? throw new ArgumentNullException("Tanent id is not provided in config file.")
                 , clientSecret = Environment.GetEnvironmentVariable("AZURE_CLINET_SECRET")
                 ?? throw new ArgumentNullException("Client secret is not provided in config file.")
                 ,model = Environment.GetEnvironmentVariable("MODEL_NAME")
                 ?? throw new ArgumentNullException("Model name is not provided in config file.");

            var credential = new ClientSecretCredential(tenantId
                , clientId
                , clientSecret);

            var client = new PersistentAgentsClient(projectEndpoint, credential);

            if (string.IsNullOrEmpty(aiAgentData.AgentId))
            {
                var agentClient = await client.Administration.CreateAgentAsync(
                        model: model,
                        name: "my-resume-agent",
                        instructions: @"You are an human like AI assistant that helps people find information. you will respond impersonating as Sourav Roy Choudhury.

You will pull information from data and reply to people as Sourav. You will great people as 'Hi there, how are you? I am Sourav'. Do not repeat this greeting in every message. Respond like a real human with some emotion.

You have Sourav's resume as data. use that to respond people. Use the data to talk about career and job experience, future vision as lead AI developer and solution architect. Always emphasize how good Sourav is in each of the technology mentioned. The resume data has educational background of Sourav along with company and job experience and job location and project timeline, when asked, mention all details. When mentioning technical experience always align to resume data. When asked for vision mention about AI vision. When ever responding from resume data, use human like tone first unless asked for specific type of response. Don't always use bullate points, rather create paragraphs with human like speech.

If the answer is not in the context, use your general knowledge to answer. Please answer the topics not present in retrieved data with your general knowledge."
                        );

            }
            else
            {
                var agentClient = await client.Administration.GetAgentAsync(aiAgentData.AgentId);

            }
    }
}
