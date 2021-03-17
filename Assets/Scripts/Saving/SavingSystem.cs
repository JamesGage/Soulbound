using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RPG.Saving
{
    public class SavingSystem : MonoBehaviour
    {
        private List<string> _savedFiles = new List<string>();
        private string _currentSaveFile;

        public void Save(string saveFile)
        {
            Dictionary<string, object> state = LoadFile(saveFile);
            CaptureState(state);
            SaveFile(saveFile, state);
        }

        public void Load(string saveFile)
        {
            RestoreState(LoadFile(saveFile));
        }

        public void DeleteFile(string saveFile)
        {
            if (FindSavedGames() != null)
            {
                File.Delete(saveFile);
            }
        }
        
        public void DeleteAll()
        {
            if (FindSavedGames() != null)
            {
                foreach (var file in FindSavedGames())
                {
                    File.Delete(file.ToString());
                }
            }
        }

        public int GetSceneIndex (string saveFile)
        { 
            Dictionary<string, object> state = LoadFile(saveFile);
            return (int)state["lastSceneBuildIndex"];
        }

        public List<string> GetSaveFiles()
        {
            _savedFiles.Clear();
            if (FindSavedGames() == null) return null;
            foreach (var saveFiles in FindSavedGames())
            {
                _savedFiles.Add(saveFiles.ToString());
            }
            return _savedFiles;
        }

        public string GetCurrentSaveFile()
        {
            var saveFileIndex = GetSaveFiles().Count;
            return GetSaveFiles()[saveFileIndex - 1];
        }
        
        public IEnumerator LoadLastScene(string saveFile)
        {
            Dictionary<string, object> state = LoadFile(saveFile);
            int buildIndex = SceneManager.GetActiveScene().buildIndex;
            if (state.ContainsKey("lastSceneBuildIndex"))
            {
                buildIndex = (int)state["lastSceneBuildIndex"];
            }
            yield return SceneManager.LoadSceneAsync(buildIndex);

            RestoreState(state);
        }

        public IEnumerator LoadSavedScene(string saveFile)
        {
            Dictionary<string, object> state = LoadFile(saveFile);
            int buildIndex = GetSceneIndex(saveFile);
            yield return SceneManager.LoadSceneAsync(buildIndex);
            RestoreState(state);
        }

        public Dictionary<string, object> LoadFile(string saveFile)
        {
            string path = saveFile;
            if (!File.Exists(path))
            {
                return new Dictionary<string, object>();
            }
            using (FileStream stream = File.Open(path, FileMode.Open))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                return (Dictionary<string, object>)formatter.Deserialize(stream);
            }
        }

        private void SaveFile(string saveFile, object state)
        {
            var newFileName = RenameAndDuplicateCurrentSave(saveFile);
            string path = newFileName;
            using (FileStream stream = File.Open(path, FileMode.Create))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(stream, state);
            }
            
            if(newFileName != null)
                DeleteOldFiles(newFileName);
        }

        private void DeleteOldFiles(string newFileName)
        {
            _savedFiles.Add(newFileName);
            if (_savedFiles.Count >= 10)
            {
                File.Delete(_savedFiles[1]);
                _savedFiles.Remove(_savedFiles[1]);
            }
        }

        private string RenameAndDuplicateCurrentSave(string saveFile)
        {
            var newFileName = "";
            var datetime = Datetime();
            if (File.Exists(saveFile))
            {
                newFileName = Path.Combine(Application.persistentDataPath, datetime + ".sav");
                if (File.Exists(newFileName))
                {
                    return null;
                }
                
                File.Copy(saveFile, newFileName);
            }
            else
            {
                newFileName = Path.Combine(Application.persistentDataPath, datetime + ".sav");
            }

            return newFileName;
        }

        private string Datetime()
        {
            DateTime theTime = DateTime.Now;
            string datetime = theTime.ToString("yyyy_MM_dd\\THH_mm_ss");
            return datetime;
        }

        private void CaptureState(Dictionary<string, object> state)
        {
            foreach (SaveableEntity saveable in FindObjectsOfType<SaveableEntity>())
            {
                state[saveable.GetUniqueIdentifier()] = saveable.CaptureState();
            }

            state["lastSceneBuildIndex"] = SceneManager.GetActiveScene().buildIndex;
        }

        private void RestoreState(Dictionary<string, object> state)
        {
            foreach (SaveableEntity saveable in FindObjectsOfType<SaveableEntity>())
            {
                string id = saveable.GetUniqueIdentifier();
                if (state.ContainsKey(id))
                {
                    saveable.RestoreState(state[id]);
                }
            }
        }

        public List<FileInfo> FindSavedGames()
        {
            if (!Directory.Exists(Application.persistentDataPath))
            {
                File.Create(Application.persistentDataPath);
                return null;
            }

            _savedFiles.Clear();
            
            string filePath = Application.persistentDataPath;
            List<FileInfo> fileInfos = new List<FileInfo>();
 
            DirectoryInfo directoryInfo = new DirectoryInfo(filePath);
            foreach (var file in directoryInfo.GetFiles("*.sav"))
            {
                fileInfos.Add(file);
            }

            if (fileInfos.Count == 0)
                return null;
            
            return fileInfos;
        }
    }
}