# Cs2Telegram

Tested on Windows, not tested on Linux yet.

### Bot Functionality
- View server status
- View players on the server
- Notifications to the administrator if the server is shutting down

### Bot Commands:
 - Status
 - Players
 - Menu

### Startup Instructions
1. Create a new bot on BotFather and obtain the token. [Create Telegram Chatbot](https://sendpulse.com/knowledge-base/chatbot/telegram/create-telegram-chatbot)
2. Move the "Cs2Telegram" folder into the "plugins" folder.
3. Open file telegramconfig.json
4. In the "Token" field, enter the token obtained through BotFather.
5. Restart server

If you want the bot to be used only by you, add your Telegram user ID to the "WhiteListUsers" field. [Get my id](https://t.me/getmyid_bot) To add an administrator, use the "Admins" field.

#### telegramconfig.json

{       
  "Token": "",       
  "Admins": [],       
  "WhiteListUsers": [],       
  "ClearUpdatesOnStart": true,       
  "BotId": 0       
}    

Token - Key for interacting with the bot.       
Admins - List of administrators.       
WhiteListUsers - List of users who can use the bot. If the value is empty, all users can use the bot.       
ClearUpdatesOnStart - Clears commands that were invoked when the bot was not running.       
BotId - Unique identifier of the bot. May be required if multiple bots are used in the same application.          
   
# The result of the bot's work

![BotResult](/doc/BotResult.png)
