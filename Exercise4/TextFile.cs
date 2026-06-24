using System;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;

namespace Exercise4
{
    [Serializable]
    public class TextFile
    {
        // Свойства
        public string Name { get; set; }
        public string Content { get; set; }
        public string Path { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }

        // Конструкторы
        public TextFile()
        {
            Name = "Новый файл";
            Content = "";
            Path = "";
            CreatedDate = DateTime.Now;
            ModifiedDate = DateTime.Now;
        }

        public TextFile(string name, string content, string path = "")
        {
            Name = name;
            Content = content;
            Path = path;
            CreatedDate = DateTime.Now;
            ModifiedDate = DateTime.Now;
        }

        // Сохранение в обычный текстовый файл
        public void SaveToTextFile(string filePath)
        {
            File.WriteAllText(filePath, Content);
            ModifiedDate = DateTime.Now;
            Path = filePath;
        }

        // Загрузка из обычного текстового файла
        public void LoadFromTextFile(string filePath)
        {
            Content = File.ReadAllText(filePath);
            Name = System.IO.Path.GetFileName(filePath);
            Path = filePath;
            ModifiedDate = File.GetLastWriteTime(filePath);
            CreatedDate = File.GetCreationTime(filePath);
        }

        // Бинарная сериализация
        public void SaveBinary(string filePath)
        {
            using (FileStream fs = new FileStream(filePath, FileMode.Create))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(fs, this);
            }
        }

        public static TextFile LoadBinary(string filePath)
        {
            using (FileStream fs = new FileStream(filePath, FileMode.Open))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                return (TextFile)formatter.Deserialize(fs);
            }
        }

        // XML сериализация
        public void SaveXml(string filePath)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(TextFile));
            using (FileStream fs = new FileStream(filePath, FileMode.Create))
            {
                serializer.Serialize(fs, this);
            }
        }

        public static TextFile LoadXml(string filePath)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(TextFile));
            using (FileStream fs = new FileStream(filePath, FileMode.Open))
            {
                return (TextFile)serializer.Deserialize(fs);
            }
        }

        // Поиск ключевого слова в содержимом
        public bool ContainsKeyword(string keyword)
        {
            return Content.IndexOf(keyword, StringComparison.OrdinalIgnoreCase) >= 0;
        }

        // Поиск всех ключевых слов
        public bool ContainsAllKeywords(string[] keywords)
        {
            foreach (var keyword in keywords)
            {
                if (!ContainsKeyword(keyword))
                    return false;
            }
            return true;
        }

        public override string ToString()
        {
            return $"Имя: {Name}, Размер: {Content.Length} символов, Изменён: {ModifiedDate:dd.MM.yyyy HH:mm}";
        }
    }
}