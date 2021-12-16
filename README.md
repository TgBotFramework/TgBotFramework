# TgBotFramework
[![package](https://img.shields.io/nuget/v/TgBotFramework)](https://www.nuget.org/packages/TgBotFramework/)
[![licence](https://img.shields.io/github/license/TgBotFramework/TgBotFramework)]()
![Lines of code](https://img.shields.io/tokei/lines/github/TgBotFramework/TgBotFramework)

Make [Telegram.Bot.Framework](https://github.com/TelegramBots/Telegram.Bot.Framework) great again!

read [Wiki](https://github.com/TgBotFramework/TgBotFramework/wiki) for more details

# Early state
there is nothing that is guarantied to work, but something definitely works ;)

# Whats new?

This project targets .NET 6.0+ and there won`t be any support for Framework. So keep your stack updated =)

In this implementation you can get (at least) same pipeline experience as in [Telegram.Bot.Framework](https://github.com/TelegramBots/Telegram.Bot.Framework) but enchanted with:


#Short-term goals
- [x] Same pipeline processing for Longpolling, Webhook and testing.
- [x] Longpolling
- [x] **Middlewares** - special concept to separate update handling from updateContext configuration.
- [x] **Attributes** and some reflection magic to handle states and command handlers
- [x] Webhook
- [x] **DB integration** EF and MongoDB examples
- [x] **Stages** (stage and step) to describe user`s state. (yeah, state machine out of the box, at least in plans)
- [x] **Roles** - it is always needed to separate bot owner from others, right?
- [ ] Localization out of the box
- [ ] Auto add class to DI with scope from attribute

  
#Long-term goals
- [ ] **Dashboard** (un plans: update rate, main exceptions, ability to add custom stats, for example number of updates by type/run time)
- [ ] Better **Logging**
- [ ] Fully tested (I wouldn't count on that one 😭) 
- [ ] Possibility to run update processing in parallel mode (and smart parallel mode) 
- [x] Better EF support
- [x] Caching state/user info. (with EF)
- [ ] Comments and refactorings
