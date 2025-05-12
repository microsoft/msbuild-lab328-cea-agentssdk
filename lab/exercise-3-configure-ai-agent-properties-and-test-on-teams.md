# Exercise 3: Configure agent properties and test on Teams

Let's configure agentic properties in your Echo Bot to transform it into a Generative AI-powered agent.

## Step 1: Update Project file with new packages

Right-click to **ContosoHRAgent** project and select **Edit Project File**, replace the ItemGroup that includes **PackageReference** with the following:

```
  <ItemGroup>
	<PackageReference Include="Microsoft.Agents.Authentication.Msal" Version="1.1.40-beta" />
	<PackageReference Include="Microsoft.Agents.Hosting.AspNetCore" Version="1.1.40-beta" />
	<PackageReference Include="Azure.AI.Projects" Version="1.0.0-beta.8" />
	<PackageReference Include="Azure.Identity" Version="1.14.0-beta.3" />
	<PackageReference Include="Microsoft.SemanticKernel.Agents.AzureAI" Version="1.49.0-preview" />
  </ItemGroup>
```

## Step 2: Add Semantic Kernel in Program.cs

Open **Program.cs** and add the following code snippet right before var app = builder.Build():
    
```
builder.Services.AddKernel();
```

## Step 3: Create new classes for agent configuration and contersation state

Right-click to **ContosoHRAgent** project and select **Add > Class** and define your class name as +++FileReference.cs+++. Replace the existing code with the following:

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

Select **Next >** to go to the next exercise Bring your agent to Copilot Chat.
