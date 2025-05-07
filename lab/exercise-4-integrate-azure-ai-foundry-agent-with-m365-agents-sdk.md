# Exercise 4: Integrate Azure AI Foundry Agent with M365 Agents SDK

In this exercise, you'll integrate your agent with the Azure AI Foundry agent that you've created in previous exercises.

## Step 1: Configure EchoBot.cs with Azure AI Agent

In **ContosoHRAgent** project, open **Bot/EchoBot.cs** and add the following lines inside the EchoBot public class: 

  ```csharp
  private readonly AIProjectClient _projectClient; 
  private readonly string _agentId; 
  ```

Replace the existing EchoBot constructor with the following: 

  ```csharp
  public EchoBot(AgentApplicationOptions options, IConfiguration configuration) : base(options) 
  { 

      OnConversationUpdate(ConversationUpdateEvents.MembersAdded, WelcomeMessageAsync); 

      // Listen for ANY message to be received. MUST BE AFTER ANY OTHER MESSAGE HANDLERS 
      OnActivity(ActivityTypes.Message, OnMessageAsync); 
      var agentConfig = configuration.GetSection("AzureAIAgentConfiguration").Get<AzureAIAgentConfiguration>(); 
      _projectClient = AzureAIAgent.CreateAzureAIClient(agentConfig.ConnectionString, new AzureCliCredential()); 
      _agentId = agentConfig.AgentId; 

  } 
  ```

**AzureAIAgent** may cause an error in this step. Right-click to AzureAIAgent and select **Quick Actions and Refactorings > Suppress or configure issues > Configure SKEXP0110 Severity > Silent**.

![image](https://github.com/user-attachments/assets/3dc267c0-c3b6-4436-9dc6-09157f9a8b5b)

Replace **OnMessageAsync** method with the following:

  ```csharp
  protected async Task OnMessageAsync(ITurnContext turnContext, ITurnState turnState, CancellationToken cancellationToken) 
  { 
      // get the Azure AI Agent 
      var agentClient = _projectClient.GetAgentsClient(); 
      var agentModel = await agentClient.GetAgentAsync(_agentId, cancellationToken); 
      var agent = new AzureAIAgent(agentModel, agentClient); 

      try 
      { 
          // send the initial message 
          await turnContext.StreamingResponse.QueueInformativeUpdateAsync("Working on it...", cancellationToken); 
          // increment the message count in state 
          int count = turnState.Conversation.IncrementMessageCount(); 
          turnContext.StreamingResponse.QueueTextChunk($"[{count}] "); 
          // create the chat message 
          var message = new ChatMessageContent(AuthorRole.User, turnContext.Activity.Text); 
          // stream the chat response 
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
  ```

## Step 2: Configure Azure AI Agent properties

Open **M365Agent/m365agents.local.yml** and locate **createOrUpdateJsonFile** and replace it with the following:

```yml
  # Generate runtime appsettings to JSON file
  - uses: file/createOrUpdateJsonFile
    with:
      target: ../ContosoHRAgent/appsettings.Development.json
      content:
        TokenValidation:
          Audiences:
            ClientId: ${{BOT_ID}}
        Connections:
          BotServiceConnection:
            Settings:
              AuthType: "ClientSecret"
              AuthorityEndpoint: "https://login.microsoftonline.com/botframework.com"
              ClientId: ${{BOT_ID}}
              ClientSecret: ${{SECRET_BOT_PASSWORD}}
        AzureAIAgentConfiguration:
          AgentId: ${{AZURE_AI_AGENT_ID}}
          ConnectionString: ${{SECRET_AZURE_AI_PROJECT_CONNECTION_STRING}}
```

Open **M365Agent/env/.env.local**, add the following line:

  ```
  AZURE_AI_AGENT_ID=<AzureAIFoundryAgentId> 
  ```

Replace the **<AzureAIFoundryAgentId>** with your Agent id that you saved in the previous exercises.


Open **TeamsApp/env/.env.local.user** and add the following line:

  ```
  SECRET_AZURE_AI_PROJECT_CONNECTION_STRING=<AzureAIFoundryProjectConnectionString> 
  ```

Replace **<AzureAIFoundryProjectConnectionString>** with your AI Foundry project connection string which can be found in the **Overview** page of the AI Foundry.

![Connection String](https://github.com/user-attachments/assets/d2e59830-11bd-48ae-9bfc-fff2999cf5f2)


## Step 3: Test your agent on Teams

**Open Tools > Command Line > Developer Command Prompt** and run:

  ```powershell
  az login 
  ```

A window will pop up on your browser and you'll need to sign into your Microsoft account to successfully complete az login. Use the following credentials to sign in:
    - **Email**: +++@lab.CloudPortalCredential(User1).Username+++
    - **Password**: +++@lab.CloudPortalCredential(User1).Password+++

Right click to **M365Agent** project, select **Microsoft 365 Agents Toolkit > Microsoft 365 Account**. Select **Sign in** and **Work or school account**. Login with the same credentials as above, then **Continue**.

![M365 Agents Toolkit](https://github.com/user-attachments/assets/6981343d-8668-4b33-b36f-63b12739fc9d)

Expand **<Multiple Startup Projects>** on top of Visual Studio and Select **Microsoft Teams (browser)**.

![Teams Browser](https://github.com/user-attachments/assets/0f564f0a-0394-49de-a679-6be59761b4fb)

Expand **Start** and select **Dev Tunnels > Create a Tunnel**.  
  * Select Account and login with Microsoft credentials.
  * Provide a name for your tunnel such as *DevTunnel*.
  * Keep the Tunnel Type **Temporary**.
  * Select Access as **Public** and then **Create**.

![Dev Tunnel](https://github.com/user-attachments/assets/146fb3d4-256d-48b3-95a1-9e285f6bbc08)


Once Dev Tunnel is created, hit **Start** or **F5** to start debugging. Microsoft Teams will launch automatically, and your agent app will pop up on the window. Select Add and start chatting with your agent.  

You can ask one of the following questions to interact with the agent:

* +++What’s the difference between Northwind Standard and Health Plus when it comes to emergency and mental health coverage?+++
* +++Can I use PerksPlus to pay for both a rock climbing class and a virtual fitness program?+++
* +++If I hit my out-of-pocket max on Northwind Standard, do I still pay for prescriptions?+++

## Next Step

Select **Next >** to go to the next exercise Bring your agent to Copilot Chat.
