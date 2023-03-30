using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Casual.Abstracts;
using UnityEngine;

namespace Casual
{
    public static class ItemFactory
    {
        private static Dictionary<string, Type> itemsByName;
        private static bool IsInitialized => itemsByName != null;
        
        private static void InitializeFactory()
        {
            if(IsInitialized) return;

            itemsByName = new Dictionary<string, Type>();
            
            var itemTypes = Assembly.GetAssembly(typeof(Item)).GetTypes()
                .Where(type => type.IsClass && !type.IsAbstract && type.IsSubclassOf(typeof(Item)));
            
            foreach (var type in itemTypes)
            {
                itemsByName.Add(type.Name, type);
            }
        }

        public static Item GetItem(string itemType)
        {
            InitializeFactory();
            
            if (itemsByName.ContainsKey(itemType))
            {
                Type type = itemsByName[itemType];
                var item = Activator.CreateInstance(type) as Item;
                return item;
            }

            return null;
        }
    }
}
