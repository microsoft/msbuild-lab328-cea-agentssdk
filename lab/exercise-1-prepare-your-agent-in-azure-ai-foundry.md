# Exercise 1: Prepare your agent in Azure AI Foundry

In this exercise, you'll explore Azure AI Foundry, a platform that enables developers to build, deploy, and scale AI agents with ease. You'll learn how to configure an agent, and test its functionality using the Agents Playground. This hands-on experience will provide insight into the capabilities of Azure AI Agent Service and how it integrates with various AI models and tools. 

## Step 1: Get started with Azure AI Foundry 

Azure AI Foundry is your launchpad for building AI agents. In this step, you’ll log in and locate your preconfigured project so you can explore what’s already set up.

1. Open the browser and navigate to +++https://ai.azure.com/allresources+++ and sign to your Azure account. Use the following credentials to **Sign in**:
    - **Email**: +++@lab.CloudPortalCredential(User1).Username+++
    - **Password**: +++@lab.CloudPortalCredential(User1).Password+++

2. From the Azure AI Foundry homepage, select the pre-polulated project in the list. This will direct you to the Azure AI Foundry project page.

> [!TIP]
> If your project doesn't show up in the list of projects, you can go to +++https://portal.azure.com+++, login with the same credentials, go to **Resource Group > ResourceGroup1** and select **Azure AI project**, then **Launch studio**.
>
> <img width="787" alt="image" src="https://github.com/user-attachments/assets/2483b6b9-8214-4602-aa3d-8b74751176b2" />


3. Once you are in your project, extend the left side bar and select **Agents**. This will open the Agents Playground. Select the OpenAI model from the dropdown list and select **Let's go**.
    <img width="1004" alt="AI Foundry" src="https://github.com/user-attachments/assets/7d17b20b-3ed0-49fa-9795-e917de869074" />

4. Once you are in the **Agents Playground**, you'll recognize there is a pre-populated agent for you in the list. Select the agent and select **Try in playground**.
    <img width="1029" alt="Agents Playground" src="https://github.com/user-attachments/assets/dd481101-c15d-4aed-af62-aeb7d3c8e5ed" />

> [!TIP]
> If you don't see the agent side bar with **Try in playground** option when you click on the agent, extend the blowser size on your screen until it shows up on the right side.

## Step 2: Customize your agent in Agent Playground 

Now that you're inside the Agents Playground, you'll customize your agent's identity and behavior to match a real-world scenario: an internal HR Agent at Contoso.

1. In your agent's **Setup** panel, **Name** your agent as +++Contoso HR Agent+++ and update the **Instructions** as the following:
 
    ```
    You are Contoso HR Agent, an internal assistant for Contoso Electronics. Your role is to help employees find accurate, policy-aligned answers to questions related to:
    - Job role descriptions and responsibilities
    - Performance review process
    - Health and wellness benefits (PerksPlus, Northwind Standard, Northwind Plus)
    - Employee rights and workplace safety
    - Company values and conduct
    
    Always base your responses on the content provided in the official documents such as the Employee Handbook, Role Library, and Benefit Plans. If you are unsure or the information is not covered, suggest the employee contact HR.
    
    Respond in a professional but approachable tone. Keep answers factual and to the point.
    
    Example scenarios you should support:
    - What is the deductible for Northwind Standard?
    - Can I use PerksPlus for spa treatments?
    - What does the CTO at Contoso do?
    - What happens during a performance review?
    ```

2. Finally in the **Knowledge** section, Select **+ Add** and select **Files**, then **Select local files**. Select all the files from the **Desktop > Contoso Documents** folder and hit **Upload and save**. This will create a vector store for our agent.

> When you upload documents, Foundry automatically converts them into vectors, a format that allows the agent to search and retrieve relevant information efficiently.

![Select local files](https://github.com/user-attachments/assets/64bb7392-15f6-458c-9e74-d8ab100ca8fd)

By customizing the instructions and uploading relevant documents, you're teaching the agent how to behave and what knowledge to rely on. This is a simplified form of Retrieval-Augmented Generation (RAG).

## Step 3: Test your agent in the playground

It's time to test your agent. You’ll simulate realistic employee questions to see how well the agent understands and responds based on the documents you uploaded.

In the Agent Playground, interact with your agent by entering prompts and observe the agent's responses, adjust instructions or tools as needed to refine performance. You may use the examples listed below to test the agent’s response:

- +++What’s the difference between Northwind Standard and Health Plus when it comes to emergency and mental health coverage?+++
- +++Can I use PerksPlus to pay for both a rock climbing class and a virtual fitness program?+++
- +++If I hit my out-of-pocket max on Northwind Standard, do I still pay for prescriptions?+++
- +++What exactly happens during a Contoso performance review, and how should I prepare?+++
- +++Is a wellness spa weekend eligible under the PerksPlus reimbursement program?+++
- +++What are the key differences between the roles of COO and CFO at Contoso?+++
- +++How does the split copay work under Northwind Health Plus for office visits?+++
- +++Can I combine yoga class reimbursements from PerksPlus with services covered under my health plan?+++
- +++What values guide behavior and decision-making at Contoso Electronics?+++
- +++I’m seeing a non-participating provider — what costs should I expect under my current plan?+++

> [!TIP]
> Save the **Agent id** that'll be required in the next exercises. You can find your Agent id in the agent’s details panel.
> 
> ![Agents Playground](https://github.com/user-attachments/assets/13421287-d476-41c4-88df-bed1bff2f2f8)

## Next Step

Select **Next >** to go to the next exercise Build your first agent using M365 Agents SDK.
