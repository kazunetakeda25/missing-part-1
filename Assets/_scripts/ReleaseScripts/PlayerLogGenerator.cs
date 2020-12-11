using UnityEngine;
using System;
using System.IO;

namespace MissingComplete
{
    public static class PlayerLogGenerator
    {
        private const string LOG_PATH = "Logs";
        private const string PLAYER_LOG_EXTENSTION = ".log";

        public static void SavePlayerLog(SaveGameManager.SaveGame save)
        {
            string newLine = Environment.NewLine;

            string outputString = "Player Log: " + save.profileName + newLine;
            outputString += "Progress Complete: " + save.GetPercentageComplete() + newLine;

            if (save.playTime > 5.0f)
            {
                TimeSpan time = TimeSpan.FromSeconds(save.playTime);
                string timePlayed = string.Format("{0:D2}h:{1:D2}m:{2:D2}s", time.Hours, time.Minutes, time.Seconds);
                outputString += "Time Played: " + timePlayed + newLine;
            }
            Debug.Log("Writing!");
            if (save.gameComplete == true)
            {
                Debug.Log("Writing!");
                outputString += "Date Completed: " + save.dateCompleted.Month + " / " + save.dateCompleted.Day + " / " + save.dateCompleted.Year + newLine;
            }

            outputString += "=================================" + newLine;


#if UNITY_STANDALONE_WIN
            if (Directory.Exists(LOG_PATH) == false)
            {
                Directory.CreateDirectory(LOG_PATH);
            }

            string filePath = LOG_PATH + "/" + save.profileName + PLAYER_LOG_EXTENSTION;
#else
						if(Directory.Exists(Application.persistentDataPath + "/" + LOG_PATH) == false) {
				Directory.CreateDirectory(Application.persistentDataPath + "/" + LOG_PATH);
			}

			string filePath = Application.persistentDataPath + "/" + LOG_PATH + "/" + save.profileName + PLAYER_LOG_EXTENSTION;
#endif

            File.WriteAllText(filePath, outputString);
        }
    }
}
