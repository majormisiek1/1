using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using DiscordBot.Core;
using DiscordBot.Util;
using MySql.Data.MySqlClient;

namespace DiscordBot.Commands
{
    static class CommandInitializer
    {
        public static void init(DiscordClient _client)
        {
            JoinCommand.createCommand(_client);
            LeaveCommand.createCommand(_client);
            StartGameCommand.createCommand(_client);
            VoteTallyCommand.createCommand(_client);
            PlayerListCommand.createCommand(_client);
            StopGameCommand.createCommand(_client);
            TestCommand.createCommand(_client);
            SetupCommand.createCommand(_client);

            _client.GetService<CommandService>().CreateGroup("debug", debug => 
            {
                CookieCommand.createCommand(debug);
                PingCommand.createCommand(debug);

                debug.CreateCommand("connection").Do(async e => 
                {
                    MySqlConnection conn = new MySqlConnection(Sneaky.connectionString);
                    using (conn)
                    {
                        Message loadingMessage = await e.Channel.SendMessage("Establishing Connection to the Database... :clock2:");
                        try
                        {
                            await conn.OpenAsync();
                            await loadingMessage.Edit("Connection Established. :white_check_mark:");
                        }
                        catch (Exception exc)
                        {
                            await loadingMessage.Edit("Error connecting to the database. See log for more details. :x:");
                            Console.WriteLine(exc);
                        }
                    }
                });
            });

            _client.GetService<CommandService>().CreateCommand("howtoplay").Do(async e => 
            {
                await e.Channel.SendMessage(
                    "**How to play quickplay mafia games, with ME**\n\nYou can join a game by typing `!join` in the chat, once you're ready you'll have to type `!ready`.\nThe game won't start before __5__ people have joined!\nEveryone must be ready before the game starts.\n\nOnce the game starts you get your role PM. Any actions will be explained to you in your role PM.\nIn the game chat you can vote by typing `VOTE: @playername`\nFor a list of commands type `!help`."
                    );
            });
            _client.GetService<CommandService>().CreateCommand("bug").Do(async e =>
            {
                await e.Channel.SendMessage(
                    "Found a bug? Or wanna recommend something be added/removed/changed?\nSend SoaringDylan a PM about it,\nPost it on our github https://github.com/dylanpiera/Mafia-Quickplay-DiscordBot \nor join our development server https://discord.gg/Tu82eWU"
                    );
            });

            //Bot Invite Link Storage
            _client.GetService<CommandService>().CreateCommand("inviteLink").Hide().Do(async e => { if(e.User.Id == 221314609255153675 || e.User.Id == 135735651059499008 || e.User.Id == 221620985684557826 || e.User.Id == 117878923370430464) await e.Channel.SendMessage(Sneaky.botInvite); } );
        }
    }
}
