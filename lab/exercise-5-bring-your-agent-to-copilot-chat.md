# Exercise 5: Bring your agent to Copilot Chat

> [!TIP]
> Close the previous debugging session before proceeding with this exercise.

Go to **M365Agent/AppPackage/manifest.json**, update the manifest schema and version as following: 

``` 
"$schema": "https://developer.microsoft.com/en-us/json-schemas/teams/v1.20/MicrosoftTeams.schema.json", 
"manifestVersion": "1.20", 
```

Replace bots section with the following that will also add copilotAgents in the manifest:

```   
  "bots": [ 
    { 
      "botId": "${{BOT_ID}}", 
      "scopes": [ 
        "personal", 
        "team", 
        "groupChat" 
      ], 
      "supportsFiles": false, 
      "isNotificationOnly": false, 
      "commandLists": [ 
        { 
          "scopes": [ "personal", "team", "groupChat" ], 
          "commands": [ 
            { 
              "title": "Emergency and Mental Health",
              "description": "What’s the difference between Northwind Standard and Health Plus when it comes to emergency and mental health coverage?" 
            }, 
            { 
              "title": "PerksPlus Details", 
              "description": "Can I use PerksPlus to pay for both a rock climbing class and a virtual fitness program?" 
            }, 
            { 
              "title": "Contoso Electronics Values", 
              "description": "What values guide behavior and decision making at Contoso Electronics?" 
            } 
          ] 
        } 
      ] 
    } 
  ], 
  "copilotAgents": { 
    "customEngineAgents": [ 
      { 
        "id": "${{BOT_ID}}", 
        "type": "bot" 
      } 
    ] 
  }, 
```

Delete **build** folder under **M365Agent > AppPackage**, that will allow you to create a new app package with the new manifest changes.

Hit **Start** or **F5** to start debugging. Microsoft Teams will launch automatically. 

> [!TIP]
> When Microsoft Teams open in your browser, ignore the app pop-up and select **Apps > Manage your apps > Upload an app**. In your files go to +++C:\Users\LabUser\source\repos\ContosoHRAgent\M365Agent\appPackage\build+++ select **appPackage.local**.
> 
>![Upload app](https://github.com/user-attachments/assets/5fad723f-b087-4481-8c8c-d5ad87c1bead)

Your app will pop-up on Teams again, select **Add**. This time there will be an option to **Open with Copilot**, select **Open with Copilot** to test your agent on Copilot.

![Copilot option](https://github.com/user-attachments/assets/97f9d9fd-bd90-48b5-983b-b1fea3f85721)

You can select one of the conversation starters to chat with your agent.

![agent on Copilot](https://github.com/user-attachments/assets/2aab299c-23ff-4369-a42c-bd74c66f854d)

Observe that your agent responds back with a similar behavior on Copilot Chat.

![image](https://github.com/user-attachments/assets/4211f43d-8aef-4262-95e3-1efac7dba495)

## Next Step

Select **Next >** to go to the next exercise Bring your agent to Copilot Chat.
