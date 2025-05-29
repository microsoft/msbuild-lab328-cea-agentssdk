# Exercise 3: Configure agent properties and test on Teams

Now that you’ve created a basic bot, it’s time to enhance it with generative AI capabilities and upgrade it to an AI agent. In this exercise, you’ll install key libraries such as Semantic Kernel and prepare your agent to reason and respond more intelligently, ready for Teams or Copilot Chat.

## Step 1: Update Project file with new packages

The packages you'll add in this step will provide support for Azure AI integration. Right-click to **ContosoHRAgent** project and select **Edit Project File**, replace the ItemGroup that includes **PackageReference** with the following:

```
  <ItemGroup>
    <PackageReference Include="Microsoft.Agents.Authentication.Msal" Version="1.1.91-beta" />
    <PackageReference Include="Microsoft.Agents.Hosting.AspNetCore" Version="1.1.91-beta" />
    <PackageReference Include="Microsoft.SemanticKernel.Agents.AzureAI" Version="1.52.1-preview" />
  </ItemGroup>
```

## Step 2: Add Semantic Kernel in Program.cs

Open **Program.cs** and add the following code snippet right before var app = builder.Build():

```
builder.Services.AddKernel();
```

This registers the Semantic Kernel, a core component that allows your agent to interact with generative AI models.

## Step 3: Add custom classes for document citations and message tracking

Right-click to **ContosoHRAgent** project and select **Add > Class** and define your class name as +++FileReference.cs+++. Replace the existing code with the following:

> This class defines the structure used when referencing specific documents in responses—useful when your agent cites content from uploaded files.

```
using Microsoft.Agents.Core.Models;
  
namespace ContosoHRAgent
{
    public class FileReference(string fileId, string fileName, string quote, Citation citation)
    {
        public string FileId { get; set; } = fileId;
        public string FileName { get; set; } = fileName;
        public string Quote { get; set; } = quote;
        public Citation Citation { get; set; } = citation;
    }
}
```

Right-click to **ContosoHRAgent** project and select **Add > Class** and define your class name as +++ConversationStateExtensions.cs+++. Replace existing the code with following:

> This class adds helper methods to manage and track the number of user messages—demonstrating how state is stored and modified during an ongoing conversation.

```
using Microsoft.Agents.Builder.State;
  
namespace ContosoHRAgent
{
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
}
```

## Next Step

Select **Next >** to go to the next exercise Integrate Azure AI Foundry Agent with M365 Agents SDK.
