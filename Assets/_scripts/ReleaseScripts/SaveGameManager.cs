using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace MissingComplete
{
    public class SaveGameManager : MonoBehaviour
    {
        [System.Serializable]
        public class SaveGame
        {
            public SaveGame()
            {
                Debug.Log("Creating Saved Game!");
            }

            public const int TOTAL_CHECKPOINTS = 90;

            public string profileName = null;
            public float playTime = 0.0f;
            public int checkPoint = 0;
            public int aarCheckpoint = 0;
            public string subjectDataJSON;
            public bool male = false;

            public bool gameComplete = false;
            public System.DateTime dateCompleted;
            //public string sessionDataJSON = "";
            //public string badgeTrackerJSON = "";

            //			public void UpdateSessionData(string json)
            //			{
            //				this.sessionDataJSON = json;
            //			}

            public void UpdateTime(float timeToUpdate)
            {
                playTime += timeToUpdate;
            }

            public int GetPercentageComplete()
            {
                if (checkPoint == TOTAL_CHECKPOINTS)
                    return 100;

                float percentage = ((float)(checkPoint - 1) / (float)TOTAL_CHECKPOINTS) * 100.0f;
                return Mathf.RoundToInt(percentage);
            }
        }

        private static SaveGameManager instance;
        public static SaveGameManager Instance
        {
            get
            {
                return instance;
            }
        }

        public const int NUMBER_OF_SAVE_GAMES = 30;
        private const string PROFILE_KEY_NAME = "PNAME";
        private const string PROFILE_KEY_TIME = "PTIME";
        private const string PROFILE_KEY_CHECKPOINT = "PCHECKPOINT";
        private const string PROFILE_SESSION_DATA = "PSESSIONDATA";
        private const string PROFILE_BADGETRACKER = "PBADGETRACKER";

        private const string SAVE_PATH = "Saves";
        private const string SAVE_FILE_NAME = "Missing.sav";

        [SerializeField] private int loadedSaveGame = -1;

        [SerializeField] SaveGame[] saveGames = new SaveGame[NUMBER_OF_SAVE_GAMES];
        [SerializeField] bool deleteSaveFile;

        public SaveGame GetCurrentSaveGame()
        {
            if (loadedSaveGame == -1)
            {
                Debug.LogWarning("No Saved Game Loaded!!");
                return null;
            }

            return saveGames[loadedSaveGame];
        }

        public void CreateSaveGame(int index, string profileName, bool male)
        {
            Debug.Log("Create Save Game: " + index);

            if (CheckForDuplicateName(profileName))
            {
                MenuNavigator.Instance.OnErrorPopUp();
                return;
            }

            SaveGame game = new SaveGame();
            game.profileName = profileName;
            game.checkPoint = 1;
            game.male = male;
            //game.sessionDataJSON = SessionManager.Instance.GetGameDataJSON();
            saveGames[index] = game;
            SaveGameInSlot(index, game);
            LoadSaveGame(index);
        }

        public void UnloadSavedGame()
        {
            SessionManager.GetSessionManager().currentSubject = null;
            loadedSaveGame = -1;
        }

        public void SaveCurrentGame()
        {
            if (loadedSaveGame == -1)
            {
                Debug.LogWarning("No Save Game Loaded..");
                return;
            }

            //Retrieve JSON
            //GetCurrentSaveGame().sessionDataJSON = SessionManager.Instance.GetGameDataJSON();
            //GetCurrentSaveGame().badgeTrackerJSON = BadgeTracker.Instance.ExportBadgeScores();

            if (SessionManager.Instance != null)
            {
                GetCurrentSaveGame().subjectDataJSON = JsonConvert.SerializeObject(SessionManager.Instance.currentSubject);
            }

            PlayerLogGenerator.SavePlayerLog(GetCurrentSaveGame());

            SaveGameInSlot(loadedSaveGame, GetCurrentSaveGame());
        }

        public SubjectData GetSubjectData()
        {
            if (GetCurrentSaveGame().subjectDataJSON == null)
            {
                return null;
            }

            return JsonConvert.DeserializeObject<SubjectData>(GetCurrentSaveGame().subjectDataJSON);
        }

        public void SaveGameInSlot(int index, SaveGame save)
        {
            Debug.Log("Saving Game - " + save.profileName + " - in slot: " + index);
            WriteSaveGames();
        }

        public void LoadSaveGame(int index)
        {
            Debug.Log("Loading: " + index);
            loadedSaveGame = index;

            //Kickstart New GameLoad
            GameLoader.Instance.Load(saveGames[index].checkPoint);
            if (SessionManager.Instance != null)
            {
                SessionManager.Instance.Init(saveGames[index].profileName);
            }
            //SessionManager.Instance.SetGameDataJSON(saveGames[index].sessionDataJSON);
            //BadgeTracker.Instance.LoadBadgeScores(saveGames[index].badgeTrackerJSON);
            //
        }

        public void DeleteSaveGame(int index)
        {
            if (loadedSaveGame == index)
            {
                loadedSaveGame = -1;
            }

            saveGames[index] = null;
            WriteSaveGames();
            RefreshGUI();
        }

        public SaveGame GetSaveGameData(int index)
        {

            return saveGames[index];
        }

        private void LoadSaveFile()
        {
#if UNITY_STANDALONE_WIN
            string filePath = SAVE_PATH + "/" + SAVE_FILE_NAME;
#else
			string filePath = Application.persistentDataPath + "/" + SAVE_PATH + "/" + SAVE_FILE_NAME;
#endif
            Debug.Log("Checking If Save File Exists: " + filePath);
            if (File.Exists(filePath))
            {
                if (deleteSaveFile)
                {
                    DeleteSaveFile(filePath);
                    return;
                }
                LoadFile(filePath);
            }
            else
            {
                CreateNewFile(filePath);
            }
        }

        private void DeleteSaveFile(string path)
        {
            Debug.Log("Resetting Save File");
            File.Delete(path);
            CreateNewFile(path);
        }

        private void LoadFile(string path)
        {
            string json = File.ReadAllText(path);
            Debug.Log("File Found: " + File.ReadAllText(path));
            saveGames = JsonConvert.DeserializeObject<SaveGame[]>(json);
        }

        private void CreateNewFile(string path)
        {
            Debug.Log("No Save File Found, creating new File...");
#if UNITY_STANDALONE_WIN
            Directory.CreateDirectory(SAVE_PATH);
#else
			Directory.CreateDirectory(Application.persistentDataPath + "/" + SAVE_PATH);
#endif
            saveGames = new SaveGame[NUMBER_OF_SAVE_GAMES];
            WriteSaveGames();
        }

        private void WriteSaveGames()
        {
            Debug.Log("Writing Save Games...");
#if UNITY_STANDALONE_WIN
            string filePath = SAVE_PATH + "/" + SAVE_FILE_NAME;
#else
			string filePath = Application.persistentDataPath + "/" +SAVE_PATH + "/" + SAVE_FILE_NAME;
#endif
            string json = JsonConvert.SerializeObject(saveGames);
            File.WriteAllText(filePath, json);
        }

        private void RefreshGUI()
        {
            if (ProfilePopulator.Instance != null)
            {
                ProfilePopulator.Instance.PopulateSavedGames();
            }
        }

        private void OnDestroy()
        {
            Debug.Log("Getting Destroyed!!");
        }

        private void Awake()
        {
            if (instance != null)
            {
                GameObject.Destroy(instance.gameObject);
            }

            GameObject.DontDestroyOnLoad(this.gameObject);
            instance = this;
        }

        private void Start()
        {
            LoadSaveFile();
            RefreshGUI();
        }

        private bool CheckForDuplicateName(string nameToCheck)
        {
            foreach (SaveGame save in saveGames)
            {
                if (save == null)
                    continue;

                if (save.profileName == null)
                    continue;

                if (String.Equals(save.profileName, nameToCheck, StringComparison.InvariantCultureIgnoreCase))
                    return true;
            }

            return false;
        }

        private void Update()
        {

            if (loadedSaveGame == -1)
                return;

            saveGames[loadedSaveGame].UpdateTime(Time.deltaTime);
        }
    }
}
