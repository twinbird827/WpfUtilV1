﻿using Codeplex.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

namespace WpfUtilV1.Common
{
    public class JsonUtil
    {
        public static void Serialize<T>(string filePath, T data, Encoding encoding = null)
        {
            if (encoding == null) encoding = Encoding.UTF8;

            using (var stream = new MemoryStream())
            using (var writer = new StreamWriter(filePath, false, encoding))
            {
                var settings = new DataContractJsonSerializerSettings()
                {
                    UseSimpleDictionaryFormat = true,
                };
                var serializer = new DataContractJsonSerializer(typeof(T), settings);
                serializer.WriteObject(stream, data);
                writer.Write(encoding.GetString(stream.ToArray()));
            }
        }

        public static T Deserialize<T>(string filePath, Encoding encoding = null)
        {
            if (encoding == null) encoding = Encoding.UTF8;

            if (!File.Exists(filePath))
            {
                return default(T);
            }

            var message = File.ReadAllText(filePath);
            using (var stream = new MemoryStream(encoding.GetBytes(message)))
            {
                var settings = new DataContractJsonSerializerSettings()
                {
                    UseSimpleDictionaryFormat = true,
                };
                var serializer = new DataContractJsonSerializer(typeof(T), settings);
                return (T)serializer.ReadObject(stream);
            }
        }

        /// <summary>from JsonSring to DynamicJson</summary>
        public static dynamic Parse(string json)
        {
            return DynamicJson.Parse(json);
        }

        /// <summary>from JsonSring to DynamicJson</summary>
        public static dynamic Parse(string json, Encoding encoding)
        {
            return DynamicJson.Parse(json, encoding);
        }

        /// <summary>from JsonSringStream to DynamicJson</summary>
        public static dynamic Parse(Stream stream)
        {
            return DynamicJson.Parse(stream);
        }

        /// <summary>from JsonSringStream to DynamicJson</summary>
        public static dynamic Parse(Stream stream, Encoding encoding)
        {
            return DynamicJson.Parse(stream, encoding);
        }

        /// <summary>create JsonSring from primitive or IEnumerable or Object({public property name:property value})</summary>
        public static string Serialize(object obj)
        {
            return DynamicJson.Serialize(obj);
        }
    }
}