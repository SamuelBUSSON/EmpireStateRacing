using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Twitch
{
    public class Interface : MonoBehaviour
    {
        public static Interface instance;
        public CommandLog last;
        [SerializeField] private CommandLog _commandLogPrefab;

        public static void Log(string command, string pseudo, string[] parameters)
        {
            instance.InstanceLog(command, pseudo, parameters);
        }

        // Start is called before the first frame update
        void Start()
        {
            instance = this;
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void InstanceLog(string command, string pseudo, string[] parameters)
        {
            CommandLog commandLog = Instantiate<CommandLog>(_commandLogPrefab, transform);
            commandLog.Init($"<color=#f33>{pseudo} : </color> {command}");
            if (last != null)
            {
                last.transform.parent = commandLog.transform;
                last.transform.position = last.transform.position + Vector3.up * 30;
            }

            last = commandLog;
        }
    }
}