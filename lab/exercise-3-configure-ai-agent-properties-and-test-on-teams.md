# Exercise 3: Configure agent properties and test on Teams

Let's configure agentic properties in your Echo Bot to transform it into a Generative AI-powered agent.

## Step 1: Update Project file with new packages

Right-click to **ContosoHRAgent** project and select **Edit Project File**, then add the following packages to in the list of ItemGroup that includes **PackageReference**:

```
<PackageReference Include="Azure.AI.Projects" Version="1.0.0-beta.8" /> 
<PackageReference Include="Azure.Identity" Version="1.14.0-beta.3" /> 
<PackageReference Include="Microsoft.SemanticKernel.Agents.AzureAI" Version="1.47.0-preview" />  
```

The updated Project file will look like below, make sure to save the changes before proceeding:

```
<Project Sdk="Microsoft.NET.Sdk.Web">
  
  <PropertyGroup>
    <TargetFramework>net9.0</TargetFramework>
    <ImplicitUsings>enable</ImplicitUsings>
  </PropertyGroup>
  
  <ItemGroup>
    <PackageReference Include="Microsoft.Agents.Authentication.Msal" Version="1.*" />
    <PackageReference Include="Microsoft.Agents.Hosting.AspNetCore" Version="1.*" />
	<PackageReference Include="Azure.AI.Projects" Version="1.0.0-beta.8" />
	<PackageReference Include="Azure.Identity" Version="1.14.0-beta.3" />
	<PackageReference Include="Microsoft.SemanticKernel.Agents.AzureAI" Version="1.47.0-preview" />
  </ItemGroup>
  
</Project>
```

## Step 2: Add Semantic Kernel in Program.cs

Open **Program.cs** and add the following code snippet right before var app = builder.Build():
    
```
// Configure AzureAIConfiguration 
builder.Services.AddSingleton(serviceProvider =>  
    builder.Configuration.GetSection("AzureAIAgentConfiguration")); 
// Add the Semantic Kernel services 
builder.Services.AddKernel();
```

The updated Program.cs will look like below, make sure to save the changes before proceeding:

```
using MyM365Agent1;
using MyM365Agent1.Bot;
using Microsoft.Agents.Hosting.AspNetCore;
using Microsoft.Agents.Storage;
using Microsoft.Agents.Builder;
  
var builder = WebApplication.CreateBuilder(args);
  
builder.Services.AddControllers();
builder.Services.AddHttpClient("WebClient", client => client.Timeout = TimeSpan.FromSeconds(600));
builder.Services.AddHttpContextAccessor();
builder.Services.AddCloudAdapter();
builder.Logging.AddConsole();
  
// Add AspNet token validation
builder.Services.AddBotAspNetAuthentication(builder.Configuration);
  
// Register IStorage.  For development, MemoryStorage is suitable.
// For production Agents, persisted storage should be used so
// that state survives Agent restarts, and operate correctly
// in a cluster of Agent instances.
builder.Services.AddSingleton<IStorage, MemoryStorage>();
  
// Add AgentApplicationOptions from config.
builder.AddAgentApplicationOptions();
  
// Add the bot (which is transient)
builder.AddAgent<EchoBot>();
builder.Services.AddSingleton<IStorage, MemoryStorage>();
  
// Configure AzureAIConfiguration 
builder.Services.AddSingleton(serviceProvider =>
    builder.Configuration.GetSection("AzureAIAgentConfiguration"));
// Add the Semantic Kernel services 
builder.Services.AddKernel();
  
var app = builder.Build();
  
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
  
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
  
// Map the /api/messages endpoint to the AgentApplication
app.MapPost("/api/messages", async (HttpRequest request, HttpResponse response, IAgentHttpAdapter adapter, IAgent agent, CancellationToken cancellationToken) =>
{
    await adapter.ProcessAsync(request, response, agent, cancellationToken);
});
  
if (app.Environment.IsDevelopment() || app.Environment.EnvironmentName == "Playground")
{
    app.MapGet("/", () => "Echo Agent");
    app.UseDeveloperExceptionPage();
    app.MapControllers().AllowAnonymous();
}
else
{
    app.MapControllers();
}
  
app.Run();
```

## Step 3: Create new classes for agent configuration and contersation state

Right-click to **ContosoHRAgent** project and select **Add > Class** and define your class name as +++AzureAIAgentConfiguration.cs+++. Replace the existing code with the following:

```
namespace ContosoHRAgent
{
    public class AzureAIAgentConfiguration
    {
        public string AgentId { get; set; } = string.Empty;
        public string ConnectionString { get; set; } = string.Empty;
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
