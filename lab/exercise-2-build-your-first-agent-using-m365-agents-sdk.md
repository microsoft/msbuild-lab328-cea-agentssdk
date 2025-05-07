# Exercise 2: Build your first agent using M365 Agents SDK

This exercise introduces you to the Microsoft 365 Agents SDK and Microsoft 365 Agents Toolkit for building enterprise-grade, scalable, multi-channel agents. You'll learn how to create a new agent project with Visual Studio and test it within Test Tool. This experience will demonstrate how to integrate agent capabilities into Microsoft 365 apps and Copilot Chat effectively.

## Step 1: Create your echo agent using Visual Studio

1. Open Visual Studio 2022 and select **Create a new project**.
1. Search and select **Microsoft 365 Agents**.
1. Provide a name for your agent as `ContosoHRAgent` and select **Create**.  
1. From the list of templates, select **Echo Bot** and select **Create**.
1. When the project template is scaffolded, go to Solution Explorer on the right-side panel and explore the agent template.
    - Open **Program.cs**, this code configures and runs the web server that hosts your agent. It sets up required services like authentication, routing, storage and registers the **EchoBot** and injects memory-based state handling.
    - Open **Bot > EchoBot.cs** and observe that this sample sets up a basic AI agent using the **Microsoft.Agents.Builder**. It sends a welcome message when a user joins the chat and listens for any message and echoes it back with a running message count.

## Step 2: Test your agent in Test Tool

To test your echo agent, hit **Start** or **F5**. This will launch Test Tool automatically in localhost where you can interact with your agent.

Type anything such as “Hi”, “Hello”. Observe that the agent echoes everything back.

![Echo bot](https://github.com/user-attachments/assets/4562052d-856b-44d5-b2dd-27623d9bed11)

## Next Step

Select **Next >** to go to the next exercise Integrate Azure AI Foundry agent with M365 Agents SDK.
