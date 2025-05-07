using Azure.AI.Projects;
using Azure.Identity;
using Microsoft.Agents.Builder;
using Microsoft.Agents.Builder.App;
using Microsoft.Agents.Builder.State;
using Microsoft.Agents.Core.Models;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Agents.AzureAI;
using Microsoft.SemanticKernel.ChatCompletion;
using System.Configuration;
using System.Threading;

namespace ContosoHRAgent.Bot;

public class EchoBot : AgentApplication
{
    private readonly AIProjectClient _projectClient;

    private readonly string _agentId;
    public EchoBot(AgentApplicationOptions options, IConfiguration configuration) : base(options)
    {
        OnConversationUpdate(ConversationUpdateEvents.MembersAdded, WelcomeMessageAsync);

        // Listen for ANY message to be received. MUST BE AFTER ANY OTHER MESSAGE HANDLERS
        OnActivity(ActivityTypes.Message, OnMessageAsync);
        var agentConfig = configuration.GetSection("AzureAIAgentConfiguration").Get<AzureAIAgentConfiguration>();
        _projectClient = AzureAIAgent.CreateAzureAIClient(agentConfig.ConnectionString, new AzureCliCredential());
        _agentId = agentConfig.AgentId;


    }

    protected async Task WelcomeMessageAsync(ITurnContext turnContext, ITurnState turnState, CancellationToken cancellationToken)
    {
        foreach (ChannelAccount member in turnContext.Activity.MembersAdded)
        {
            if (member.Id != turnContext.Activity.Recipient.Id)
            {
                await turnContext.SendActivityAsync(MessageFactory.Text("Hello and Welcome!"), cancellationToken);
            }
        }
    }

    protected async Task OnMessageAsync(ITurnContext turnContext, ITurnState turnState, CancellationToken cancellationToken)
    {
        // get the Azure AI Agent
        var agentClient = _projectClient.GetAgentsClient();
        var agentModel = await agentClient.GetAgentAsync(_agentId, cancellationToken);
        var agent = new AzureAIAgent(agentModel, agentClient);


        try
        {
            //send the initial message
            await turnContext.StreamingResponse.QueueInformativeUpdateAsync("Working on it...", cancellationToken);

            //increment the message count in state
            int count = turnState.Conversation.IncrementMessageCount();
            turnContext.StreamingResponse.QueueTextChunk($"[{count}] ");

            //create the chat message
            var message = new ChatMessageContent(AuthorRole.User, turnContext.Activity.Text);


            await foreach (StreamingChatMessageContent response in agent.InvokeStreamingAsync(message, cancellationToken: cancellationToken))
            {
                var content = string.IsNullOrEmpty(response.Content) ? "" : response.Content.ToString();
                turnContext.StreamingResponse.QueueTextChunk(content);
            }

        }
        finally
        {
            await turnContext.StreamingResponse.EndStreamAsync(cancellationToken);
        }
    }


}
