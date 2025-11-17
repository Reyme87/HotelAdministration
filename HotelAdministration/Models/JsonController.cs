using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;

namespace PasswordManager.Models
{
    internal static class JsonController<T>
    {
        static public ObservableCollection<T> GetInfo(string fileName)
        {
            ObservableCollection<T> collection = new ObservableCollection<T>();
            using (FileStream fs = new FileStream(fileName, FileMode.OpenOrCreate))
            {
                FileInfo fileInfo = new FileInfo(fileName);
                if (fileInfo.Length != 0)
                {
                    try
                    {
                        collection = System.Text.Json.JsonSerializer.Deserialize<ObservableCollection<T>>(fs);
                    }
                    catch
                    {
                        MessageBox.Show("Ошибка чтения данных!");
                    }
                }
            }
            return collection;
        }

        static public T GetInfo<T>(string fileName) where T : class
        {
            using (FileStream fs = new FileStream(fileName, FileMode.OpenOrCreate))
            {
                FileInfo fileInfo = new FileInfo(fileName);
                if (fileInfo.Length != 0)
                {
                    try
                    {
                        return System.Text.Json.JsonSerializer.Deserialize<T>(fs);
                    }
                    catch
                    {
                        MessageBox.Show("Ошибка чтения данных!");
                    }
                }
            }

            return null;
        }

        static public async void LoadInfoAsync<T>(ObservableCollection<T> collection, string fileName)
        {
            string json = JsonConvert.SerializeObject(collection, Formatting.Indented);
            await File.WriteAllTextAsync(fileName, json);
        }

        static public async void LoadInfoAsync<T>(T targetObject, string fileName)
        {
            string json = JsonConvert.SerializeObject(targetObject, Formatting.Indented);
            await File.WriteAllTextAsync(fileName, json);
        }
    }
}
