# Lab BMA2 - Build your first agent using M365 Agents SDK

This exercise introduces you to the Microsoft 365 Agents SDK and Microsoft 365 Agents Toolkit for building enterprise-grade, scalable, multi-channel agents. You'll learn how to create a new agent project with Visual Studio and test it within Test Tool. This experience will demonstrate how to integrate agent capabilities into Microsoft 365 apps and Copilot Chat effectively.

## Exercise 1: Build your first agent using M365 Agents SDK

### Step 1: Create an echo bot using Visual Studio

Now that you’ve seen how to build an agent using Azure AI Foundry, let’s switch gears and explore how to build your own agent locally using the Microsoft 365 Agents SDK. This SDK lets you build multi-channel, production-ready agents that can run in Microsoft Teams, Microsoft 365 Copilot, and other preferred channels.

1. Open Visual Studio 2022 and select **Sign in** and **Work or school account**, use the following credentials:
    - **Email**: +++@lab.CloudPortalCredential(User1).Username+++
    - **Password**: +++@lab.CloudPortalCredential(User1).Password+++
1. During the sign in process, there will be a pop-up on the screen asking *Automatically sign in to all desktop apps and websites on this device?*, select **No, this app only**.
    ![Sign in to app devices pop up](https://github.com/user-attachments/assets/c5cb1ab0-92c7-4dec-bbc1-166919bfbeab)

1. When sign in is complete, select **Close** and then select **Create a new project**.
1. Search and select **Microsoft 365 Agents** templatem then **Next**.
1. Provide a name for your agent as `ContosoHRAgent` and select **Create**.  
1. From the list of templates, select **Echo Bot** and select **Create**.
1. When the project template is scaffolded, go to Solution Explorer on the right-side panel and explore the agent template. Expand the **ContosoHRAgent** project.
    - Open **Program.cs**, this code configures and runs the web server that hosts your agent. It sets up required services like authentication, routing, storage and registers the **EchoBot** and injects memory-based state handling.
    - Open **Bot > EchoBot.cs** and observe that this sample sets up a basic AI agent using the **Microsoft.Agents.Builder**. It sends a welcome message when a user joins the chat and listens for any message and echoes it back with a running message count.

You've started with an **Echo Bot**, a simple bot that repeats back any message a user sends. It’s a useful way to verify your setup and understand how conversations are handled behind the scenes.

### Step 2: Test your agent in Test Tool

To test your echo agent, hit **Start** or **F5**. You'll see a pop-up *Do you want to allow public and private networks to access this app?*, select **Allow**.

![Playground pop-up](https://github.com/user-attachments/assets/37ee9ce3-eebe-4e14-b0a6-47ca42549d23)

This will launch Test Tool automatically in localhost where you can interact with your agent. In case if Visual Studio asks you to confirm the creation of a self-issued SSL certificate to test the application locally, confirm and proceed.

Type anything such as “Hi”, “Hello” in the chat playground. Observe that the agent echoes everything back.

![The local Microsoft 365 Agents Playground when testing locally the Echo Bot. On the left side of the screen there is an emulated chat, while on the right side of the screen there is a panel with the history of the interaction between the user and the agent.](https://github.com/user-attachments/assets/4562052d-856b-44d5-b2dd-27623d9bed11)

>[!TIP]
>Stop the debugging session on Visual Studio before moving to the next exercise.

## Next Step

This simple agent forms the base for more powerful experiences. In the next step, you'll combine this with your Azure AI Foundry agent to enable richer, context-aware answers.

Select **Next >** to go to the next exercise Integrate Azure AI Foundry agent with M365 Agents SDK.
