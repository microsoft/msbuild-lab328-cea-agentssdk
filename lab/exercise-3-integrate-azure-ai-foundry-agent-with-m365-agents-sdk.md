# Exercise 3: Integrate Azure AI Foundry agent with M365 Agents SDK

Let's upgrade the Echo Bot by transforming it into a Generative AI-powered agent.

## Step 1:  Setup and configuration

1. Right-click to **ContosoHRAgent** project and select **Edit Project File**, then add the following packages to in the list of ItemGroup that includes PackageReference:

    ```
    <PackageReference Include="Azure.AI.Projects" Version="1.0.0-beta.8" /> 
    <PackageReference Include="Azure.Identity" Version="1.14.0-beta.3" /> 
    <PackageReference Include="Microsoft.SemanticKernel.Agents.AzureAI" Version="1.47.0-preview" />  
    ```

1. Open **Program.cs** and add the following code snippet right before `var app = builder.Build()`:
    
    ```csharp
    // Configure AzureAIConfiguration 
    builder.Services.AddSingleton(serviceProvider =>  
        builder.Configuration.GetSection("AzureAIAgentConfiguration")); 
    // Add the Semantic Kernel services 
    builder.Services.AddKernel();
    ```

1. Right-click to **ContosoHRAgent** project and select **Add > Class** and define your class name as **AzureAIAgentConfiguration.cs**. Copy the following lines inside your AzureAIAgentConfiguration public class:

    ```csharp
    public string AgentId { get; set; } = string.Empty; 
    public string ConnectionString { get; set; } = string.Empty; 
    ```

1. Right-click to **ContosoHRAgent** project and select **Add > Class** and define your class name as **ConversationStateExtensions.cs**. Replace ConversationStateExtensions class with following:

    ```csharp
    public static class ConversationStateExtensions 
    { 
        public static int MessageCount(this ConversationState state) => state.GetValue<int>("countKey"); 
        public static void MessageCount(this ConversationState state, int value) => state.SetValue("countKey", value); 

        public static int IncrementMessageCount(this ConversationState state) 
        { 
            int count = state.GetValue<int>("countKey"); 
            state.SetValue("countKey", ++count); 
            return count; 
        } 

    } 
    ```

## Step 2: Configure EchoBot with Azure AI Agent

1. In **ContosoHRAgent** project, open **Bot/EchoBot.cs** and add the following lines inside the EchoBot public class: 

    ```csharp
    private readonly AIProjectClient _projectClient; 
    private readonly string _agentId; 
    ```

1. Replace the existing EchoBot constructor with the following: 

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

1. **AzureAIAgent** may cause an error in this step. Right-click to AzureAIAgent and select **Quick Actions and Refactorings > Suppress or configure issues > Configure SKEXP0110 Severity > Silent**. 
1. Replace **OnMessageAsync** method with the following version:

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

1. Open **TeamsApp/m365agents.local.yml** and update **createOrUpdateJsonFile** with the following lines:

    ```yml
    AzureAIAgentConfiguration: 
          AgentId: ${{AZURE_AI_AGENT_ID}} 
          ConnectionString: ${{SECRET_AZURE_AI_PROJECT_CONNECTION_STRING}} 
    ```

1. The final version of **createOrUpdateJsonFile** should look like below:

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

1. Open **TeamsApp/env/.env.local** and add the following line by replacing the **<AzureAIFoundryAgentId>** with your agent Id.

    ```
    AZURE_AI_AGENT_ID=<AzureAIFoundryAgentId> 
    ```

1. Open **TeamsApp/env/.env.local.user** and add the following line by replacing the **<AzureAIFoundryProjectConnectionString>** with your AI Foundry project connection string which can be found in the **Overview** page of the AI Foundry.

    ```
    SECRET_AZURE_AI_PROJECT_CONNECTION_STRING=<AzureAIFoundryProjectConnectionString> 
    ```

## Step 3: Test your agent in Teams

1. **Open Tools > Command Line > Developer Command Prompt** and run:

    ```powershell
    az login 
    ```

1. Sign into your Microsoft account. Use the following credentials to sign in:
    - **Email**: +++@lab.CloudPortalCredential(User1).Username+++
    - **Password**: +++@lab.CloudPortalCredential(User1).Password+++

1. Right click to **M365Agent**, select **Microsoft 365 Agents Toolkit > Microsoft 365 Account**. Login with your Microsoft account:
    - **Email**: +++@lab.CloudPortalCredential(User1).Username+++
    - **Password**: +++@lab.CloudPortalCredential(User1).Password+++

1. Expand **<Multiple Startup Projects>** on top of Visual Studio and Select **Microsoft Teams (browser)**.
1. Expand **Start** and select **Dev Tunnels > Create a Tunnel**.  
    * Select Account and login with Microsoft credentials.
    * Provide a name for your tunnel such as *DevTunnel*.
    * Keep the Tunnel Type **Temporary**.
    * Select Access as **Public** and then **Create**.

Once Dev Tunnel is created, hit **Start** or **F5** to start debugging. Microsoft Teams will launch automatically, and your agent app will pop up on the window. Select Add and start chatting with your agent.  

You can ask one of the following questions to interact with the agent: 
    - What’s the difference between Northwind Standard and Health Plus when it comes to emergency and mental health coverage?
    - Can I use PerksPlus to pay for both a rock climbing class and a virtual fitness program?
    - If I hit my out-of-pocket max on Northwind Standard, do I still pay for prescriptions?

## Next Step

Select **Next >** to go to the next exercise Bring your agent to Copilot Chat.