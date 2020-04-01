using System;
using System.Collections.Generic;
using System.Linq;

namespace Acklann.Picmin
{
    public static class CommandFactory
    {
        static CommandFactory()
        {
            var knownTypes = from t in typeof(CommandFactory).Assembly.GetTypes()
                             where typeof(ICommand).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract
                             select t;

            _knownCommands = new Dictionary<string, Type>();
            foreach (Type item in knownTypes)
            {
                _knownCommands.Add(item.Name, item);
                _knownCommands.Add((item.Name + "Options"), item);
                _knownCommands.Add((item.Name + "Settings"), item);
            }
        }

        public static ICommand CreateInstance(object options)
        {
            if (options == null) throw new ArgumentNullException(nameof(options));

            Type type = options.GetType();
            return (ICommand)Activator.CreateInstance(_knownCommands[type.Name], new object[] { options });
        }

        #region Backing Members

        private static readonly IDictionary<string, Type> _knownCommands;

        #endregion Backing Members
    }
}