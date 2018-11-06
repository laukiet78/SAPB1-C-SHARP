﻿// <copyright filename="InstanceFactory.cs" project="Framework">
//   This file is licensed to you under the MIT License.
//   Full license in the project root.
// </copyright>

namespace B1PP.Data
{
    using System;
    using System.Collections.Generic;
    using System.Dynamic;
    using System.Linq;
    using System.Reflection;

    using Extensions.Types;

    using SAPbobsCOM;

    internal static class InstanceFactory
    {
        private const BindingFlags MapableProperties =
            BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;

        /// <summary>
        /// Creates an instance of a {T} type.
        /// </summary>
        /// <typeparam name="T">Type of the instance to create.</typeparam>
        /// <param name="reader">The reader that contains the data to populate the instance.</param>
        /// <returns>
        /// An instance of {T} populated with the data.
        /// </returns>
        public static T AutoCreateInstance<T>(IRecordsetReader reader) where T : class
        {
            var instance = (T) Activator.CreateInstance(typeof(T));
            var columns = reader.Columns.ToList();

            var properties = GetMatchingProperties(instance, columns);

            foreach (var property in properties)
            {
                if (!property.CanWrite)
                {
                    continue;
                }

                SetPropertyValue(reader, property, instance);
            }

            return instance;
        }

        public static dynamic CreateDynamicObject(IRecordsetReader reader)
        {
            var expando = new ExpandoObject();
            var data = (IDictionary<string, object>) expando;
            foreach (var column in reader.Columns)
            {
                if (data.ContainsKey(column.Name))
                {
                    string message = $@"The column '{column.Name}' appears twice or more times. " +
                                     @"Please use different aliases for each column.";
                    throw new ArgumentException(message);
                }

                switch (column.Type)
                {
                    case BoFieldTypes.db_Alpha:
                    case BoFieldTypes.db_Memo:
                        data.Add(column.Name, reader.GetString(column.Name));
                        break;
                    case BoFieldTypes.db_Numeric:
                        data.Add(column.Name, reader.GetInt(column.Name));
                        break;
                    case BoFieldTypes.db_Date:
                        data.Add(column.Name, reader.GetDateTime(column.Name));
                        break;
                    case BoFieldTypes.db_Float:
                        data.Add(column.Name, reader.GetDouble(column.Name));
                        break;
                    default:
                        throw new ArgumentOutOfRangeException($@"Unknown column type: {column.Type}");
                }
            }

            return expando;
        }

        private static IEnumerable<PropertyInfo> GetMatchingProperties<T>(T instance, List<IColumn> columns)
            where T : class
        {
            return instance.GetType()
                .GetProperties(MapableProperties)
                .Where(NamesMatch(columns));
        }

        private static Func<PropertyInfo, bool> NamesMatch(List<IColumn> columns)
        {
            return p => columns.Exists(c => c.Name.Equals(p.Name));
        }

        private static void SetPropertyValue<T>(IRecordsetReader reader, PropertyInfo property, T instance)
            where T : class
        {
            var propertyType = property.PropertyType;

            if (propertyType == typeof(string))
            {
                property.SetValue(instance, reader.GetString(property.Name));
            }
            else if (propertyType == typeof(DateTime))
            {
                property.SetValue(instance, reader.GetDateTimeOrDefault(property.Name));
            }
            else if (propertyType == typeof(int))
            {
                property.SetValue(instance, reader.GetIntOrDefault(property.Name));
            }
            else if (propertyType == typeof(double))
            {
                property.SetValue(instance, reader.GetDoubleOrDefault(property.Name));
            }
            else if (propertyType == typeof(bool))
            {
                property.SetValue(instance, reader.GetBoolOrDefault(property.Name));
            }
            else if (propertyType == typeof(Id))
            {
                string value = reader.GetString(property.Name);
                property.SetValue(instance, new Id(value));
            }
        }
    }
}