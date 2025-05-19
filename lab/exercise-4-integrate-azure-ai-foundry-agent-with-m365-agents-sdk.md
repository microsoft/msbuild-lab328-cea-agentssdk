# Exercise 4: Integrate Azure AI Foundry Agent with M365 Agents SDK

You’ve built an agent using the M365 Agents SDK and configured it with generative AI capabilities. Now, you’ll connect this local agent to the Azure AI Foundry agent you created earlier. This enables your agent to respond using enterprise data and instructions stored in the Foundry project, bringing everything full circle.

## Step 1: Configure EchoBot.cs to Connect with Azure AI Foundry Agent

In this step, you’ll connect to the Azure AI Foundry agent by adding a client to fetch and invoke your Foundry-hosted model inside the EchoBot.cs.

In **ContosoHRAgent** project, open **Bot/EchoBot.cs** and add the following lines inside the EchoBot public class: 

  ```
  private readonly AIProjectClient _projectClient; 
  private readonly string _agentId; 
  ```

Replace the existing EchoBot constructor with the following: 

  ```
  public EchoBot(AgentApplicationOptions options, IConfiguration configuration) : base(options)
  {

      OnConversationUpdate(ConversationUpdateEvents.MembersAdded, WelcomeMessageAsync);

      // Listen for ANY message to be received. MUST BE AFTER ANY OTHER MESSAGE HANDLERS 
      OnActivity(ActivityTypes.Message, OnMessageAsync);

      // Azure AI Foundry Project ConnectionString
      string connectionString = configuration["AIServices:AzureAIFoundryProjectConnectionString"];
      if (string.IsNullOrEmpty(connectionString))
      {
          throw new InvalidOperationException("AzureAIFoundryProjectConnectionString is not configured.");
      }
      _projectClient = AzureAIAgent.CreateAzureAIClient(connectionString, new AzureCliCredential());

      // Azure AI Foundry Agent Id
      _agentId = configuration["AIServices:AgentID"];
      if (string.IsNullOrEmpty(_agentId))
      {
          throw new InvalidOperationException("AgentID is not configured.");
      }

  }
  ```

> **⚠️ Note:** You might see a warning (SKEXP0110) because this feature is still in preview. You can safely suppress this warning for now by right-clicking on AzureAIAgent, selecting **Quick Actions and Refactorings > Suppress or configure issues > Configure SKEXP0110 Severity > Silent**.
> 
> ![Warning](https://github.com/user-attachments/assets/3dc267c0-c3b6-4436-9dc6-09157f9a8b5b)

Replace **OnMessageAsync** method with the following:

```
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
       turnContext.StreamingResponse.QueueTextChunk($"({count}) ");

       var fileReferences = new List<FileReference>();
       var citations = new List<Citation>();
       var quote = string.Empty;

       // create the chat message to send to the agent
       var message = new ChatMessageContent(AuthorRole.User, turnContext.Activity.Text);

       // stream the response from the agent to the user
       await foreach (StreamingChatMessageContent chunk in agent.InvokeStreamingAsync(message, cancellationToken: cancellationToken))
       {
           // get the annotation content from the message chunk items, if there are any
           var annotations = chunk.Items.OfType<StreamingAnnotationContent>();
           foreach (StreamingAnnotationContent annotation in annotations)
           {
               // check if the file reference already exists in the list and skip it if it does
               if (fileReferences.Any(fr => fr.Quote == annotation.Quote)) { continue; }

               var agentFile = await agent.Client.GetFileAsync(annotation.FileId, cancellationToken);
               var citation = new Citation(string.Empty, agentFile.Value.Filename, "https://m365.cloud.microsoft/chat");

               var fileReference = new FileReference(agentFile.Value.Id, agentFile.Value.Filename, annotation.Quote, citation);
               fileReferences.Add(fileReference);
           }

           // if the message chunk content is empty, we can skip it
           // this happens when the chunk contains StreamingAnnotationContent items
           if (chunk.Content == null) { continue; }

           // if the previous message chunk contained the citation quote, we can process it now
           if (quote != string.Empty)
           {
               var fileReferenceIndex = fileReferences.FindIndex(fr => fr.Quote == quote);
               turnContext.StreamingResponse.QueueTextChunk($" [{fileReferenceIndex + 1}] ");

               // reset the quote to empty string to avoid processing it again
               quote = string.Empty;
               continue;
           }

           // if the message chunk contains an annotation quote 【4:0†source】
           // store the value for the next message chunk so we can process it
           // we don't want to send it to the user yet
           if (chunk.Content.Contains('【'))
           {
               quote = chunk.Content;
               continue;
           }
           else
           {
               // just a regular message chunk, we can send it to the user
               turnContext.StreamingResponse.QueueTextChunk(chunk.Content);
           }
       }

       // enable generated by AI label
       turnContext.StreamingResponse.EnableGeneratedByAILabel = true;

       // add sensitivity label
       turnContext.StreamingResponse.SensitivityLabel = new SensitivityUsageInfo()
       {
           Name = "General",
           Description = "Business data which is NOT meant for public consumption. This can be shared with internal employees, business guests and external partners as needed."
       };

       // add citations
       foreach (var fileReference in fileReferences)
       {
           citations.Add(fileReference.Citation);
       }
       turnContext.StreamingResponse.AddCitations(citations);
   }
   finally
   {
       await turnContext.StreamingResponse.EndStreamAsync(cancellationToken);
   }
}
```

> **Summary:** What happens in OnMessageAsync?
> 
> The *OnMessageAsync* method is the heart of your agent’s response logic. By replacing the default echo behavior, you’ve enabled your agent to:
> * Send the user’s message to your Azure AI Foundry agent
> * Stream the response back to the user in real time
> * Track and attach citations and file references for transparency
> * Add sensitivity and AI-generated labels for security and traceability

## Step 2: Configure Azure AI Agent Service Keys

Add your Foundry connection details to appsettings.json, these values connect your M365 agent to the correct Foundry project and agent. In **ContosoHRAgent** project, open **appsettings.json** and add the following lines at the bottom of the appsettings list:

```
,
  "AIServices": {
    "AzureAIFoundryProjectConnectionString": "<AzureAIFoundryProjectConnectionString>",
    "AgentID": "<AzureAIFoundryAgentId>"
  }
```

> You can find these values in the **Overview** and **Agents Playground** sections of Azure AI Foundry.

Replace the **<AzureAIFoundryAgentId>** with your **Agent id** which can be found in **Agents Playground**.

![Agents Playground](https://github.com/user-attachments/assets/13421287-d476-41c4-88df-bed1bff2f2f8)

Replace **<AzureAIFoundryProjectConnectionString>** with your AI Foundry project connection string which can be found in the **Overview** page of the AI Foundry, under the Project details.

![Connection String](https://github.com/user-attachments/assets/d2e59830-11bd-48ae-9bfc-fff2999cf5f2)

Final version of the **appsettings.json** will look like below:

```
{
  "AgentApplicationOptions": {
    "StartTypingTimer": false,
    "RemoveRecipientMention": false,
    "NormalizeMentions": false
  },
  
  "TokenValidation": {
    "Audiences": [
      "{{ClientId}}" // this is the Client ID used for the Azure Bot
    ]
  },
  
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning",
      "Microsoft.Agents": "Warning",
      "Microsoft.Hosting.Lifetime": "Information"
    }
  },
  "AllowedHosts": "*",
  "Connections": {
    "BotServiceConnection": {
      "Settings": {
        "AuthType": "UserManagedIdentity", // this is the AuthType for the connection, valid values can be found in Microsoft.Agents.Authentication.Msal.Model.AuthTypes.
        "ClientId": "{{BOT_ID}}", // this is the Client ID used for the connection.
        "TenantId": "{{BOT_TENANT_ID}}",
        "Scopes": [
          "https://api.botframework.com/.default"
        ]
      }
    }
  },
  "ConnectionsMap": [
    {
      "ServiceUrl": "*",
      "Connection": "BotServiceConnection"
    }
  ],
  "AIServices": {
    "AzureAIFoundryProjectConnectionString": "<AzureAIFoundryProjectConnectionString>",
    "AgentID": "<AzureAIFoundryAgentId>"
  }
}
```

## Step 3: Test your agent on Teams

Open **Tools > Command Line > Developer Command Prompt** and run:

  ```
  az login 
  ```

A window will pop up on your browser and you'll need to sign into your Microsoft account to successfully complete az login. Use the following credentials to sign in:
* **Email**: +++@lab.CloudPortalCredential(User1).Username+++
* **Password**: +++@lab.CloudPortalCredential(User1).Password+++

Expand **Start** and select **Dev Tunnels > Create a Tunnel**.  
  * Select **Sign in** and **Work or school account**. Login with the same credentials mentioned above.
  * Provide a name for your tunnel such as +++DevTunnel+++.
  * Keep the Tunnel Type **Temporary**.
  * Select Access as **Public** and then **Create**.

![Dev Tunnel](https://github.com/user-attachments/assets/146fb3d4-256d-48b3-95a1-9e285f6bbc08)

Right click to **M365Agent** project, select **Microsoft 365 Agents Toolkit > Microsoft 365 Account**.

![M365 Agents Toolkit](https://github.com/user-attachments/assets/6981343d-8668-4b33-b36f-63b12739fc9d)

If your account doesn't show up automatically, select **Sign in** and **Work or school account**. Login with the same credentials, then **Continue**:
* **Email**: +++@lab.CloudPortalCredential(User1).Username+++
* **Password**: +++@lab.CloudPortalCredential(User1).Password+++
  
Expand **<Multiple Startup Projects>** on top of Visual Studio and Select **Microsoft Teams (browser)**.

![Teams Browser](https://github.com/user-attachments/assets/0f564f0a-0394-49de-a679-6be59761b4fb)

You're now ready to run your integrated agent and test it live in Microsoft Teams. Make sure your dev tunnel is created and your account is authenticated.

Once Dev Tunnel is created, hit **Start** or **F5** to start debugging. Microsoft Teams will launch automatically, and your agent app will pop up on the window. Select **Add** and **Open** to start chatting with your agent.  

You can ask one of the following questions to interact with the agent:

* +++What’s the difference between Northwind Standard and Health Plus when it comes to emergency and mental health coverage?+++
* +++Can I use PerksPlus to pay for both a rock climbing class and a virtual fitness program?+++
* +++What values guide behavior and decision-making at Contoso Electronics?+++

You should observe that you are getting similar responses with the agent you've created on Azure AI Foundry.

![Agent on Teams](https://github.com/user-attachments/assets/73ef491f-eaff-4743-bb2d-79a52a9ae301)


## Next Step

Select **Next >** to go to the next exercise Bring your agent to Copilot Chat.
