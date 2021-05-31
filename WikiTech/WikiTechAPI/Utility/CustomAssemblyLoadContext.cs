//Auteur    : Pancini Marco
//Date      : 30.05.2021
//Fichier   : CustomAssemblyLoadContext.cs
//Description : cette classe permet de fournir au runtime la localisation des dépendances,
//              plus précisement la dll qui me permet de générer des pdf depuis du html
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using System.Threading.Tasks;

namespace WikiTechAPI.Utility
{
    internal class CustomAssemblyLoadContext : AssemblyLoadContext
    {
        public IntPtr LoadUnmanagedLibrary(string absolutePath)
        {
            return LoadUnmanagedDll(absolutePath);
        }
        protected override IntPtr LoadUnmanagedDll(String unmanagedDllName)
        {
            return LoadUnmanagedDllFromPath(unmanagedDllName);
        }
        protected override Assembly Load(AssemblyName assemblyName)
        {
            throw new NotImplementedException();
        }
    }
}
