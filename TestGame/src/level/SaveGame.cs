using System;
using System.IO;
using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Intermediate;
using System.Xml;
using System.Runtime.Serialization.Formatters.Binary;
using System.Diagnostics;

namespace TestGame.src.level
{
    public class SaveGame
    {       
        public static void Serialize<T>(string filename, T data)
        {
            XmlWriterSettings settings = new XmlWriterSettings
            {
                Indent = true
            };
            if (!filename.EndsWith(".xml")) filename += ".xml";
            using (XmlWriter writer = XmlWriter.Create(filename, settings))
            {
                IntermediateSerializer.Serialize<T>(writer, data, null);
            }
        }

        public static void SerializeMap<T>(string filename, T data)
        {
            XmlWriterSettings settings = new XmlWriterSettings
            {
                Indent = true
            };
            if (!filename.EndsWith(".xml")) filename += ".xml";
            filename = "/TestGame/Content/maps/" + filename; 
            filename = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName + filename;
            using (XmlWriter writer = XmlWriter.Create(filename, settings))
            {
                IntermediateSerializer.Serialize<T>(writer, data, null);
            }
        }

        public static T Deserialize<T>(string filename)
        {
            T data;
            if (!filename.EndsWith(".xml")) filename += ".xml";
            using (FileStream stream = new FileStream(filename, FileMode.Open))
            {
                using (XmlReader reader = XmlReader.Create(stream))
                {
                    data = IntermediateSerializer.Deserialize<T>(reader, null);
                }
            }
            return data;
        }
        public static T DeserializeMap<T>(string filename)
        {
            T data;
            if (!filename.EndsWith(".xml")) filename += ".xml";
            filename = "/TestGame/Content/maps/" + filename;
            filename = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName + filename;          
            Debug.WriteLine(filename);
            using (FileStream stream = new FileStream(filename, FileMode.Open))
            {
                using (XmlReader reader = XmlReader.Create(stream))
                {
                    data = IntermediateSerializer.Deserialize<T>(reader, null);
                }
            }
            return data;
        }
    }
        
}

