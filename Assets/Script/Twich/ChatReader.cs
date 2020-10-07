using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Twitch
{
    public class ChatReader : MonoBehaviour
    {
        public int cooldown = 10;
        public List<string> commandsCode;
        private List<string> cooldownPseudo;   
        public void Start()
        {
            cooldownPseudo = new List<string>();
            TwitchChatClient.instance.onChatMessageReceived += ShowMessage;
        }

        void ShowMessage(TwitchChatMessage chatMessage)
        {
            string command = chatMessage.command.Remove(0, 1);
            //string chatMessageText = $"Command: '{chatMessage.command}' - Sender: {chatMessage.sender.Split('!')[0]} - Parameters: {string.Join(" - ", chatMessage.parameters)}";

            string reply;
            bool valid = ExecuteCommand(command, chatMessage.sender.Split('!')[0], chatMessage.parameters, out reply);
            if(!valid) TwitchChatClient.instance.SendTwitchChatMessage(reply);
        }
        
        private bool ExecuteCommand(string command, string pseudo, string[] parameters, out string reply)
        {
            reply = "";
            
            // Invalid parameters
            if (parameters.Length == 0 || commandsCode.Exists(x => x == parameters[0]))
            {
                reply = "Invalid command parameters.";
                return false;
            }
            // Player cooldown
            if (cooldownPseudo.Exists(x => x == pseudo))
            {
                reply = "Already acted less than "+cooldown+" seconds ago.";
                return false;
            }
            // Invalid command
            if(command.ToLower() != "tesla" && command.ToLower() != "edison"){
                reply = "Invalid command type (valid type : tesla|edison).";
                return false;
            }
            // Register cooldown
            StartCoroutine(Cooldown(pseudo));
            // Log command
            Interface.Log(command, pseudo, parameters);
            // Paw
            int paw = commandsCode.FindIndex(x=> x== parameters[0]);
            
            // Command
            if (command.ToLower() == "tesla") return Robot.tesla.Command(paw);
            return Robot.edison.Command(paw);
        
        }

        private IEnumerator Cooldown(string pseudo)
        {
            cooldownPseudo.Add(pseudo);
            yield return new WaitForSeconds(cooldown);
            cooldownPseudo.Remove(pseudo);
        }
    }
}