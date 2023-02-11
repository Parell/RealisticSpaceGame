using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace SaveManager
{
    public enum SaveGamePath { PersistentDataPath, DataPath }

    public static class SaveLoad
    {
        public delegate void SaveHandler(object obj, string identifier, SaveGamePath path);

        public delegate void LoadHandler(object loadedObj, string identifier, SaveGamePath path);

        public static event SaveHandler OnSaving;
        public static event SaveHandler OnSaved;
        public static event LoadHandler OnLoading;
        public static event LoadHandler OnLoaded;

        public static SaveHandler SaveCallback;
        public static LoadHandler LoadCallback;

        private static SaveGamePath _SavePath = SaveGamePath.PersistentDataPath;
        private static List<string> ignoredFiles = new List<string>() { "Player.log", "output_log.txt" };
        private static List<string> ignoredDirectories = new List<string>() { "Analytics" };

        public static SaveGamePath SavePath
        {
            get
            {
                return _SavePath;
            }
            set
            {
                _SavePath = value;
            }
        }

        public static List<string> IgnoredFiles
        {
            get
            {
                return ignoredFiles;
            }
        }

        public static List<string> IgnoredDirectories
        {
            get
            {
                return ignoredDirectories;
            }
        }

        #region Save
        public static void Save<T>(string identifier, T obj)
        {
            Save<T>(identifier, obj, SavePath);
        }

        public static void Save<T>(string identifier, T obj, SaveGamePath path)
        {
            if (string.IsNullOrEmpty(identifier))
            {
                throw new System.ArgumentNullException("identifier");
            }
            if (OnSaving != null)
            {
                OnSaving(obj, identifier, path);
            }

            string filePath = "";
            if (!IsFilePath(identifier))
            {
                switch (path)
                {
                    default:
                    case SaveGamePath.PersistentDataPath:
                        filePath = string.Format("{0}/{1}", Application.persistentDataPath, identifier);
                        break;
                    case SaveGamePath.DataPath:
                        filePath = string.Format("{0}/{1}", Application.dataPath, identifier);
                        break;
                }
            }
            else
            {
                filePath = identifier;
            }
            if (obj == null)
            {
                obj = default(T);
            }

            Directory.CreateDirectory(Path.GetDirectoryName(filePath));

            string data = JsonUtility.ToJson(obj, true); // Todo: add pretty text to parameters
            File.WriteAllText(filePath, data);

            if (SaveCallback != null)
            {
                SaveCallback.Invoke(obj, identifier, path);
            }
            if (OnSaved != null)
            {
                OnSaved(obj, identifier, path);
            }
        }
        #endregion

        #region Load
        public static T Load<T>(string identifier)
        {
            return Load<T>(identifier, default(T), SavePath);
        }

        public static T Load<T>(string identifier, T defaultValue)
        {
            return Load<T>(identifier, defaultValue, SavePath);
        }

        public static T Load<T>(string identifier, SaveGamePath savePath)
        {
            return Load<T>(identifier, default(T), savePath);
        }

        public static T Load<T>(string identifier, T defaultValue, SaveGamePath path)
        {
            if (string.IsNullOrEmpty(identifier))
            {
                throw new System.ArgumentNullException("identifier");
            }
            if (OnLoading != null)
            {
                OnLoading(null, identifier, path);
            }
            if (defaultValue == null)
            {
                defaultValue = default(T);
            }

            T result = defaultValue;

            string filePath = "";
            if (!IsFilePath(identifier))
            {
                switch (path)
                {
                    default:
                    case SaveGamePath.PersistentDataPath:
                        filePath = string.Format("{0}/{1}", Application.persistentDataPath, identifier);
                        break;
                    case SaveGamePath.DataPath:
                        filePath = string.Format("{0}/{1}", Application.dataPath, identifier);
                        break;
                }
            }
            else
            {
                filePath = identifier;
            }
            if (!Exists(filePath, path))
            {
                Debug.LogWarningFormat(
                    "The specified identifier ({1}) does not exists. please use Exists () to check for existent before calling Load.\n" +
                    "returning the default(T) instance.",
                    filePath,
                    identifier);
                return result;
            }

            string data = "";

            data = File.ReadAllText(filePath);
            result = JsonUtility.FromJson<T>(data);

            if (result == null)
            {
                result = defaultValue;
            }
            if (LoadCallback != null)
            {
                LoadCallback.Invoke(
                    result,
                    identifier,
                    path);
            }
            if (OnLoaded != null)
            {
                OnLoaded(
                    result,
                    identifier,
                    path);
            }
            return result;
        }
        #endregion

        public static bool Exists(string identifier)
        {
            return Exists(identifier, SavePath);
        }

        public static bool Exists(string identifier, SaveGamePath path)
        {
            if (string.IsNullOrEmpty(identifier))
            {
                throw new System.ArgumentNullException("identifier");
            }
            string filePath = "";
            if (!IsFilePath(identifier))
            {
                switch (path)
                {
                    default:
                    case SaveGamePath.PersistentDataPath:
                        filePath = string.Format("{0}/{1}", Application.persistentDataPath, identifier);
                        break;
                    case SaveGamePath.DataPath:
                        filePath = string.Format("{0}/{1}", Application.dataPath, identifier);
                        break;
                }
            }
            else
            {
                filePath = identifier;
            }

            bool exists = Directory.Exists(filePath);
            if (!exists)
            {
                exists = File.Exists(filePath);
            }
            return exists;
        }

        public static void Delete(string identifier)
        {
            Delete(identifier, SavePath);
        }

        public static void Delete(string identifier, SaveGamePath path)
        {
            if (string.IsNullOrEmpty(identifier))
            {
                throw new System.ArgumentNullException("identifier");
            }
            string filePath = "";
            if (!IsFilePath(identifier))
            {
                switch (path)
                {
                    default:
                    case SaveGamePath.PersistentDataPath:
                        filePath = string.Format("{0}/{1}", Application.persistentDataPath, identifier);
                        break;
                    case SaveGamePath.DataPath:
                        filePath = string.Format("{0}/{1}", Application.dataPath, identifier);
                        break;
                }
            }
            else
            {
                filePath = identifier;
            }
            if (!Exists(filePath, path))
            {
                return;
            }

            var fileName = Path.GetFileName(filePath);
            if (ignoredFiles.Contains(fileName) || ignoredDirectories.Contains(fileName))
            {
                return;
            }
            if (File.Exists(filePath))
                File.Delete(filePath);
            else if (Directory.Exists(filePath))
                Directory.Delete(filePath, true);
        }

        public static void Clear()
        {
            DeleteAll(SavePath);
        }

        public static void Clear(SaveGamePath path)
        {
            DeleteAll(path);
        }

        public static void DeleteAll()
        {
            DeleteAll(SavePath);
        }

        public static void DeleteAll(SaveGamePath path)
        {
            string dirPath = "";
            switch (path)
            {
                case SaveGamePath.PersistentDataPath:
                    dirPath = Application.persistentDataPath;
                    break;
                case SaveGamePath.DataPath:
                    dirPath = Application.dataPath;
                    break;
            }

            DirectoryInfo info = new DirectoryInfo(dirPath);
            FileInfo[] files = info.GetFiles();
            for (int i = 0; i < files.Length; i++)
            {
                if (ignoredFiles.Contains(files[i].Name))
                {
                    continue;
                }
                files[i].Delete();
            }
            DirectoryInfo[] dirs = info.GetDirectories();
            for (int i = 0; i < dirs.Length; i++)
            {
                if (ignoredDirectories.Contains(dirs[i].Name))
                {
                    continue;
                }
                dirs[i].Delete(true);
            }
        }

        public static FileInfo[] GetFiles()
        {
            return GetFiles(string.Empty, SavePath);
        }

        public static FileInfo[] GetFiles(string identifier)
        {
            return GetFiles(identifier, SavePath);
        }

        public static FileInfo[] GetFiles(string identifier, SaveGamePath path)
        {
            if (string.IsNullOrEmpty(identifier))
            {
                identifier = string.Empty;
            }
            string filePath = "";
            if (!IsFilePath(identifier))
            {
                switch (path)
                {
                    default:
                    case SaveGamePath.PersistentDataPath:
                        filePath = string.Format("{0}/{1}", Application.persistentDataPath, identifier);
                        break;
                    case SaveGamePath.DataPath:
                        filePath = string.Format("{0}/{1}", Application.dataPath, identifier);
                        break;
                }
            }
            else
            {
                filePath = identifier;
            }
            FileInfo[] files = new FileInfo[0];
            if (!Exists(filePath, path))
            {
                return files;
            }
            if (Directory.Exists(filePath))
            {
                DirectoryInfo info = new DirectoryInfo(filePath);
                files = info.GetFiles();
            }
            return files;
        }

        public static DirectoryInfo[] GetDirectories()
        {
            return GetDirectories(string.Empty, SavePath);
        }

        public static DirectoryInfo[] GetDirectories(string identifier)
        {
            return GetDirectories(identifier, SavePath);
        }

        public static DirectoryInfo[] GetDirectories(string identifier, SaveGamePath path)
        {
            if (string.IsNullOrEmpty(identifier))
            {
                identifier = string.Empty;
            }
            string filePath = "";
            if (!IsFilePath(identifier))
            {
                switch (path)
                {
                    default:
                    case SaveGamePath.PersistentDataPath:
                        filePath = string.Format("{0}/{1}", Application.persistentDataPath, identifier);
                        break;
                    case SaveGamePath.DataPath:
                        filePath = string.Format("{0}/{1}", Application.dataPath, identifier);
                        break;
                }
            }
            else
            {
                filePath = identifier;
            }
            DirectoryInfo[] directories = new DirectoryInfo[0];
            if (!Exists(filePath, path))
            {
                return directories;
            }
            if (Directory.Exists(filePath))
            {
                DirectoryInfo info = new DirectoryInfo(filePath);
                directories = info.GetDirectories();
            }
            return directories;
        }

        public static bool IsFilePath(string str)
        {
            bool result = false;
            if (Path.IsPathRooted(str))
            {
                try
                {
                    Path.GetFullPath(str);
                    result = true;
                }
                catch (System.Exception)
                {
                    result = false;
                }
            }
            return result;
        }
    }
}