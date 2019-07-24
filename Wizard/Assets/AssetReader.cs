using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Wizard.Assets
{
    public class AssetReader
    {
        private static List<IAsset> _components;
        private static Dictionary<Type, ObjectPathAttribute> _assetsWithObjPath;
        private static Dictionary<Type, IList<(PropertyInfo prop, PropertyPathAttribute propPath)>> _assetsWithPropPaths;

        public static List<IAsset> Components
        {
            get
            {
                if (_components == null)
                {
                    _components = typeof(AssetReader).Assembly.GetTypes()
                        .Where(t => typeof(IAsset).IsAssignableFrom(t))
                        .Select(t => Activator.CreateInstance(t) as IAsset)
                        .ToList();
                }

                return _components;
            }
        }

        public static Dictionary<Type, ObjectPathAttribute> AssetsWithObjPath
        {
            get
            {
                if (_assetsWithObjPath == null)
                {
                    _assetsWithObjPath = typeof(AssetReader).Assembly.GetTypes()
                        .Where(t =>
                            typeof(IAsset).IsAssignableFrom(t) &&
                            t.GetCustomAttribute<ObjectPathAttribute>() != null)
                        .ToDictionary(t => t, t => t.GetCustomAttribute<ObjectPathAttribute>());
                }

                return _assetsWithObjPath;
            }
        }

        public static Dictionary<Type, IList<(PropertyInfo prop, PropertyPathAttribute propPath)>> AssetsWithPropPaths
        {
            get
            {
                if (_assetsWithPropPaths == null)
                {
                    _assetsWithPropPaths = new Dictionary<Type, IList<(PropertyInfo prop, PropertyPathAttribute propPath)>>();
                    foreach (var component in Components)
                    {
                        var props = component.GetType().GetProperties()
                            .Where(p => p.GetCustomAttribute<PropertyPathAttribute>() != null)
                            .Select(p => (p, p.GetCustomAttribute<PropertyPathAttribute>()))
                            .ToList();
                        _assetsWithPropPaths.Add(component.GetType(), props);
                    }
                }

                return _assetsWithPropPaths;
            }
        }

        public static IList<IAsset> Read(string manifestJsonFile)
        {
            var jtoken = JToken.Parse(File.ReadAllText(manifestJsonFile));
            List<IAsset> instances = new List<IAsset>();

            foreach (var component in Components)
            {
                if (AssetsWithObjPath.ContainsKey(component.GetType()))
                {
                    var objPath = AssetsWithObjPath[component.GetType()];
                    if (objPath.AllowMultiple)
                    {
                        var tokens = jtoken.SelectTokens(objPath.JPath);
                        if (tokens?.Any() == true)
                        {
                            foreach (var token in tokens)
                            {
                                if (JsonConvert.DeserializeObject(token.ToString(), component.GetType()) is IAsset instance)
                                {
                                    instances.Add(instance);
                                }
                            }
                        }
                    }
                    else
                    {
                        var token = jtoken.SelectToken(objPath.JPath);
                        if (token != null)
                        {
                            if (JsonConvert.DeserializeObject(token.ToString(), component.GetType()) is IAsset instance)
                            {
                                instances.Add(instance);
                            }
                        }
                    }
                }
                else if (AssetsWithPropPaths.ContainsKey(component.GetType()))
                {
                    var propPaths = AssetsWithPropPaths[component.GetType()];
                    var instance = Activator.CreateInstance(component.GetType()) as IAsset;
                    foreach (var tuple in propPaths)
                    {
                        if (tuple.propPath.IsArray)
                        {
                            var tokens = jtoken.SelectTokens(tuple.propPath.JPath);
                            if (tokens?.Any() == true)
                            {
                                ArrayList array = new ArrayList();
                                foreach (var token in tokens)
                                {
                                    var propValue =
                                        JsonConvert.DeserializeObject(token.ToString(), tuple.prop.PropertyType);
                                    if (propValue != null)
                                    {
                                        array.Add(propValue);
                                    }
                                }
                                tuple.prop.SetValue(instance, array.ToArray());
                            }
                        }
                        else
                        {
                            var token = jtoken.SelectToken(tuple.propPath.JPath);
                            var propValue =
                                JsonConvert.DeserializeObject(token.ToString(), tuple.prop.PropertyType);
                            if (propValue != null)
                            {
                                tuple.prop.SetValue(instance, propValue);
                            }
                        }
                    }

                    instances.Add(instance);
                }
            }

            return instances;
        }
    }
}