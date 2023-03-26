﻿using System.Reflection;
using System.Resources;

// General Information
[assembly: AssemblyTitle("Mod Api")]
[assembly: AssemblyDescription("Mod API is an application programming interface designed for the My Summer Car Modding Community.")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("Tommo J. Productions")]
[assembly: AssemblyProduct("Mod Api")]
[assembly: AssemblyCopyright("Tommo J. Productions Copyright © 2022")]
[assembly: AssemblyTrademark("Azine")]
[assembly: NeutralResourcesLanguage("en-AU")]

// Version information
[assembly: AssemblyVersion("1.0.342.1")]
[assembly: AssemblyFileVersion("1.0.342.1")]

namespace TommoJProductions.ModApi
{

    public class VersionInfo
    {
	    public const string lastestRelease = "10.12.2022 09:32 AM";
	    public const string version = "1.0.342.1";

        /// <summary>
        /// Represents if the mod has been complied for x64
        /// </summary>
        #if x64
            internal const bool IS_64_BIT = true;
        #else
            internal const bool IS_64_BIT = false;
        #endif
        /// <summary>
        /// Represents if the mod has been complied in Debug mode
        /// </summary>
        #if DEBUG
            internal const bool IS_DEBUG_CONFIG = true;
        #else
            internal const bool IS_DEBUG_CONFIG = false;
        #endif
    }
}
