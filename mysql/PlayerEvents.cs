using AltV.Net;
using AltV.Net.Data;
using AltV.Net.Enums;
using mysql.Database;
using mysql.MyEntitys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace mysql
{
    class PlayerEvents : IScript
    {
        [ScriptEvent(ScriptEventType.PlayerConnect)]
        public void OnPlayerConnect(MyPlayer player, string reason)
        {
            player.Model = (uint)PedModel.FreemodeMale01;
        }

        [ScriptEvent(ScriptEventType.PlayerDisconnect)]
        public void OnPlayerDisconnect(MyPlayer player, string reason)
        {
            if (player.IsLoggedIn) player.Save();
            Alt.Log(player.Name + " saved.");
        }

        [ClientEvent("mysql:loginAttempt")]
        public void OnLoginAttempt(MyPlayer player, string name, string password)
        {
            if (player.IsLoggedIn || name.Length < 4 || password.Length < 4) return;

            Regex regex = new Regex(@"([a-zA-Z]+)_([a-zA-Z]+)");

            if(!regex.IsMatch(name))
            {
                player.Emit("mysql:loginError", 1, "The format is Firstname_Lastname");
                return;
            }

            if(!PlayerDatabase.DoesPlayerNameExists(name))
            {
                player.Emit("mysql:loginError", 1, "That name wasn't found!");
                return;
            }

            if (!PlayerDatabase.CheckLoginDetails(name, password))
            {
                player.LoadPLayer(name);
                player.Spawn(new Position(0, 0, 72), 0);
                player.Emit("mysql:loginSuccess");
                player.SendNotification("Login successful!");
            }
            else
            {
                player.Emit("mysql:loginError", 1, "Login data is wrong!");
                int attemps = 1;
                if(player.HasData("mysql:loginAttempt"))
                {
                    player.GetData("mysql:loginAttempt", out attemps);

                    if (attemps >= 2) player.Kick("To many login tries.");
                    else attemps ++;
                }
                player.SetData("mysql:loginAttempt", attemps);
            }

        }

        [ClientEvent("mysql:registerAttempt")]
        public void OnRegisterAttempt(MyPlayer player, string name, string password)
        {
            if (player.IsLoggedIn || name.Length < 4 || password.Length < 4) return;

            Regex regex = new Regex(@"([a-zA-Z]+)_([a-zA-Z]+)");

            if (!regex.IsMatch(name))
            {
                player.Emit("mysql:loginError", 1, "The format is Firstname_Lastname");
                return;
            }

            if (PlayerDatabase.DoesPlayerNameExists(name))
            {
                player.Emit("mysql:loginError", 2, "Name is already exists!");

            } 
            else
            {
                player.CreatePlayer(name, "null", password);
                player.Spawn(new Position(0, 0, 72), 0);
                player.Emit("mysql:loginSuccess");
                player.SendNotification("Successful registered!");
            }
        }
    }
}
