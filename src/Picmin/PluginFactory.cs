using System;
using System.Collections.Generic;
using System.Linq;

namespace Acklann.Picmin
{
    public static class PluginFactory
    {
        static PluginFactory()
        {
            var plugins = from t in typeof(PluginFactory).Assembly.GetTypes()
                          where typeof(IPlugin).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract
                          select t;

            _knownPlugins = new Dictionary<string, Type>();
            foreach (Type item in plugins) _knownPlugins.Add(item.Name, item);
        }

        public static IPlugin CreateInstance(object options)
        {
            if (options == null) return new NullPlugin();

            Type type = options.GetType();
            if (_knownPlugins.ContainsKey(type.Name))
                return (IPlugin)Activator.CreateInstance(type, new object[] { options });
            else
                return new NullPlugin();
        }

        #region Backing Members

        private static readonly IDictionary<string, Type> _knownPlugins;

        #endregion Backing Members
    }
}