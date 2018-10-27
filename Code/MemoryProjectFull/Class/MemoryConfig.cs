﻿using System;
using System.Collections.Generic;

namespace MemoryProjectFull
{
    
    public class DataEntry
    {

        public DataEntry(string name, int value)
        {
            this.name = name;
            this.value = value;
        }

        public DataEntry(string name, string value)
        {
            this.name = name;
            this.value = value;
        }

        public string GetName()
        {
            return name;
        }

        public Type GetValueType()
        {
            return value.GetType();
        }

        public ValueType GetValue<ValueType>()
        {
            return (ValueType) value;
        }

        public void SetValue(int newValue)
        {
            value = newValue;
        }

        public void SetValue(string newValue)
        {
            value = newValue;
        }

        private readonly string name;
        private object value;

    }

    public class DataGroup
    {

        public DataGroup(string name)
        {
            this.name = name;
            this.entries = new Dictionary<string, DataEntry>();
        }

        public string GetName()
        {
            return name;
        }

        public bool HasEntry(string name)
        {
            return entries.ContainsKey(name);
        }

        public DataGroup AddEntry(string name, int value)
        {
            try
            {
                entries.Add(name, new DataEntry(name, value));
                return this;
            }
            catch
            {
                throw;
            }
        }

        public DataGroup AddEntry(string name, string value)
        {
            try
            {
                entries.Add(name, new DataEntry(name, value));
                return this;
            }
            catch
            {
                throw;
            }
        }

        public DataEntry GetEntry(string name)
        {
            try
            { return entries[name]; }
            catch
            { return null; }
        }

        public bool RemoveEntry(string name)
        {
            return entries.Remove(name);
        }

        public IEnumerable<DataEntry> Entries
        {
            get
            {
                foreach (var entry in entries) yield return entry.Value;
            }
        }

        private readonly string name;
        private Dictionary<string, DataEntry> entries;

    }

    public partial class MemoryConfig
    {

        public MemoryConfig()
        {
            this.groups = new Dictionary<string, DataGroup>();
        }

        public static MemoryConfig LoadFromFile(string filePath)
        {
            MemoryConfig config = new MemoryConfig();

            try
            {
                FileHandler.DeserializeConfig(config, filePath);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                config = null;
            }

            return config;
        }

        public bool SaveToFile(string filePath)
        {
            bool success = true;

            try
            {
                FileHandler.SerializeConfig(this, filePath);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                success = false;
            }

            return success;
        }

        public bool HasGroup(string name)
        {
            return groups.ContainsKey(name);
        }

        public MemoryConfig AddGroup(string name)
        {
            try
            {
                groups.Add(name, new DataGroup(name));
                return this;
            }
            catch
            {
                throw;
            }
        }

        public DataGroup GetGroup(string name)
        {
            try
            { return groups[name]; }
            catch
            { return null; }
        }

        public bool RemoveGroup(string name)
        {
            return groups.Remove(name);
        }

        public IEnumerable<DataGroup> Groups
        {
            get
            {
                foreach (var group in groups) yield return group.Value;
            }
        }

        private Dictionary<string, DataGroup> groups;

    }

}
