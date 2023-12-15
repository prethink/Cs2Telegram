# Cs2Telegram

Tested on Windows server 2022 and ubuntu 22.

# ⚠️ WARNING
*For proper operation, it is required to disable the hibernate mode on the server. To do this, specify __sv_hibernate_when_empty 0__ in the server config.*

## Requirements
 - [CounterStrikeSharp](https://github.com/roflmuffin/CounterStrikeSharp/tree/main)
 - [PRTelegramBot](https://github.com/prethink/PRTelegramBot)
 - [Telegram.Bot](https://github.com/TelegramBots/Telegram.Bot)
 - [Newtonsoft.Json](https://github.com/JamesNK/Newtonsoft.Json)       


### Bot Functionality
- WhiteList
- Administrators
- View server status
- View players on the server
- View players info and actions *(Only for admins)
- Send server command *(Only for admins)
- Send server message *(Only for admins)
- Notify admin if player connected to server (setting NotifyAdminOnConnectUser in Cs2Telegram.json)
- Create Custom menu
- Player can send a message to the administrators of the server on Telegram.


### Bot Commands:
- Status
- Players
- Menu
- Admin menu *(Only for admins)
- Server command *(Only for admins)
   * Generate inline menu favorite commands from config Cs2Telegram.json - ServerCommandsMenuItems
- Server message *(Only for admins)
-  Players info *(Only for admins)
   * Actions with players

### Commands in server
!tgreport [message] - Send a message to the administrators of the server on Telegram.

### Startup Instructions
1. Create a new bot on BotFather and obtain the token. [Create Telegram Chatbot](https://sendpulse.com/knowledge-base/chatbot/telegram/create-telegram-chatbot)
2. Move the "Cs2Telegram" folder into the "plugins" folder.
3. Open file telegramconfig.json
4. In the "Token" field, enter the token obtained through BotFather.
5. Restart server

If you want the bot to be used only by you, add your Telegram user ID to the "WhiteListUsers" field. [Get my id](https://t.me/getmyid_bot) To add an administrator, use the "Admins" field.

#### Configuration
path - ..\csgo\addons\counterstrikesharp\configs\plugins\Cs2Telegram\Cs2Telegram.json     

Token - Key for interacting with the bot.       
Admins - List of administrators.       
WhiteListUsers - List of users who can use the bot. If the value is empty, all users can use the bot.       
ClearUpdatesOnStart - Clears commands that were invoked when the bot was not running.       
BotId - Unique identifier of the bot. May be required if multiple bots are used in the same application.  
ServerCommandsMenuItems - List command for inline buttons        
   
# The result of the bot's work

![BotResult](/doc/BotResult.png)
