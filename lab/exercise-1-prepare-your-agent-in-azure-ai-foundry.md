# Exercise 1: Prepare your agent in Azure AI Foundry

In this exercise, you'll explore Azure AI Foundry—a platform that enables developers to build, deploy, and scale AI agents with ease. You'll learn how to create a new project, configure an agent, and test its functionality using the Agent Playground. This hands-on experience will provide insight into the capabilities of Azure AI Agent Service and how it integrates with various AI models and tools. 

## Step 1: Setup Azure AI Foundry 

1. Open the browser and navigate to +++https://ai.azure.com+++ and sign to your Azure account. Use the following credentials to **Sign in**:
    - **Email**: +++@lab.CloudPortalCredential(User1).Username+++
    - **Password**: +++@lab.CloudPortalCredential(User1).Password+++

2. From the Azure AI Foundry homepage, select **Create a project**. Keep all the settings suggested and select **Create**. (It can take about 2-3 minutes to get all the resources created for the project.)
3. Within your project, extend the left side panel and navigate to the **Agents** section under "Build and customize".
4. Select your Azure OpenAI resource and select **Let’s go**.
5. In the Deploy a model window, search and select **gpt-4o**, then **Confirm**. Keep the deployment details as suggested and select **Deploy**.

## Step 2: Create your agent in Agent Playground 

1. Open the **Agent Playground** to prototype and test your agent—no coding required.
2. You'll see an auto-created agent in the list. Select it and click **Try in playground**.
3. In the Setup panel, **Name** your agent as +++Contoso HR Agent+++ and update the **Instructions** as the following:
 
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

4. Finally in the **Knowledge** section, Select **+ Add** and select **Files**, then **Select local files**. Select all the files from the **Desktop > Contoso Documents** folder and hit **Upload and save**. This will create a vector store for our agent.

![Select local files](https://github.com/user-attachments/assets/64bb7392-15f6-458c-9e74-d8ab100ca8fd)


## Step 3: Test your agent in the playground

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
> Save the **Agent id** that'll be required in the next exercises.
> 
> ![Agents Playground](https://github.com/user-attachments/assets/13421287-d476-41c4-88df-bed1bff2f2f8)

## Next Step

Select **Next >** to go to the next exercise Build your first agent using M365 Agents SDK.
