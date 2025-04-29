# Exercise 4: Bring your agent to Copilot Chat

Go to M365Agent/AppPackage/manifest.json, update the manifest schema and version as following: 

    ```json 
    "$schema": "https://developer.microsoft.com/en-us/json-schemas/teams/v1.20/MicrosoftTeams.schema.json", 
    "manifestVersion": "1.20", 
    ```

Replace bots section with the following that will also add copilotAgents in the manifest:

    ```json   
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
                  "title": "Contoso performance review", 
                  "description": "What exactly happens during a Contoso performance review, and how should I prepare?" 
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

Hit **Start** or **F5** to start debugging. Microsoft Teams will launch automatically, and your agent app will pop up on the window. This time, you’ll also have Copilot icon in the launch. Select **Add** and select **Open with Copilot** to start chatting with your agent on Copilot. You can select one of the conversation starters to chat with your agent.

## Resources

- [Copilot Developer Camp](https://aka.ms/copilotdevcamp)