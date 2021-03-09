## Overview

This is a Bot Framework Composer(BFC) project created with version: `1.3.0`.

Bot has two functionalities implemented:
- Display currently playing track of the authenticated user.
- Start playback for authenticated user of a random top track by a given artist.

## Setup guide

To run the project, the following setup is required.

Requirements:
- Azure Account - create an account at https://portal.azure.com/. A free trail is enough.
- Spotify Application - create an application at https://developer.spotify.com/dashboard/login.
- Bot Framework Composer - download and install https://github.com/microsoft/BotFramework-Composer.
- Bot Framework Emulator - download and install https://github.com/microsoft/BotFramework-Emulator


### Setup Bot Framework Composer project

```
This guide shows the resource provisioning through BFC built-in Publish function. To run the bot locally you only need Bot Channel Registration and LUIS Authoring resource. You can provision them separately and just fill in the necessary Project Settings info.
```

1. Clone the project and open it with Bot Framework Composer.
2. Navigate to Project Settings.
3. Add new publish profile.
   1. Set profile name e.g. DevEnv.
   2. Select publish target to: Publish bot to Azure Web App.
   3. Log in to your Azure account.
   4. Create new Azure resources.
   5. Select subscription that you want to use.
   6. Select name for resource group e.g. oop-spotify-bot.
   7. Select location to West Europe or your preferred one.
   8. This project only needs LUIS. From the Optional resources select: LUIS Authoring and LUIS Prediction.
   9. Finish the setup.
   10. In case you get errors during creation make sure that you have all the necessary providers registered: `Microsoft.CognitiveServices, Microsoft.Web, Microsoft.Search, Microsoft.Storage`. You can register providers through Azure CLI e.g. ```az provider register --namespace Microsoft.Web``` .
4.  Enable custom runtime. Set location to `runtime` and start command to `dotnet run --project azurewebapp`.
5.  Fill in the project settings based on provisioning result. Select Advanced Settings View to see the json and look at `publishTargets.configuration` field. Find the provision result string and fill the following fields: `Microsoft App Id, Microsoft App Password, LUIS authoring key, LUIS region`.
6.  Navigate to Spotify Developer Dashboard and create an app. Write down Client ID and Client Secret.
    1.  Edit Settings and add add Redirect URI: `https://token.botframework.com/.auth/web/redirect`
7.  Navigate to Azure Portal and find your newly created Bot Channel Registration. 
8.  Open Settings.
9.  Find OAuth Connection Settings and click Add Setting.
    1.  Name your connection: Spotify.
    2.  Service Provider: Generic Oauth 2.
    3.  Fill Client id with Spotify Client Id.
    4.  Fill Secret with Spotify Client Secret.
    5.  Fill Authorization URL with: https://accounts.spotify.com/authorize
    6.  Fill Token URL with: https://accounts.spotify.com/api/token
    7.  Fill Refresh URL with: https://accounts.spotify.com/api/token
    8.  Fill Scopes with: `user-read-playback-state,user-modify-playback-state`
    9.  Save and test your connection.
10. Go back to BFC and run the bot. You can run it inside the Emulator or Publish it to Azure thourgh the Publish Profile.