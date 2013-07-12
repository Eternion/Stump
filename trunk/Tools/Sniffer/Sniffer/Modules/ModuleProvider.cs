using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Sniffer.Modules
{
    public class ModuleProvider<T> where T : BaseModule
    {
        private readonly string _folderUri;
        private readonly Type _interfaceType;
        public readonly List<T> _modules;

        public ModuleProvider(string folderUri)
        {
            _folderUri = folderUri;
            _interfaceType = typeof(T);
            _modules = new List<T>();
        }

        public void LoadModules()
        {
            if (!Directory.Exists(_folderUri))
                return;

            foreach (var file in Directory.GetFiles(_folderUri, "*.dll"))
            {
                //LoadModule(file);
            }
        }


        private void LoadModule(string fileUri)
        {
            var asm = Assembly.LoadFrom(fileUri);

            foreach (var type in asm.GetTypes())
            {
                if (type.BaseType == _interfaceType)
                {
                    AddModule((T)Activator.CreateInstance(type));
                }
            }
        }

        private void AddModule(T module)
        {
            module.Initialize();

            _modules.Add(module);

            Task.Factory.StartNew(module.Run);
        }

        public void RemoveModule(T module)
        {
            _modules.Remove(module);
            module.Stop();
        }

        public void RemoveAllModules()
        {
            var count = _modules.Count;
            for (int i = 0; i < count; i++)
            {
                _modules[i].Stop();
                _modules.RemoveAt(i);
            }
        }

        public T GetModule(string name)
        {
            return _modules.FirstOrDefault(m => m.GetName() == name);
        }

        public IEnumerable<T> GetModules()
        {
            return _modules;
        }

        public void Dispatch(Action<T> action)
        {
            foreach (var module in _modules)
            {
                action(module);
            }
        }

        public void Dispatch(Action<T> action, Predicate<T> predicate)
        {
            foreach (var module in _modules)
            {
                if (predicate(module))
                    action(module);
            }
        }
    }
}
