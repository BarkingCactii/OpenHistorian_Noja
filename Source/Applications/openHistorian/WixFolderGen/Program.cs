﻿//******************************************************************************************************
//  Program.cs - Gbtc
//
//  Copyright © 2017, Grid Protection Alliance.  All Rights Reserved.
//
//  Licensed to the Grid Protection Alliance (GPA) under one or more contributor license agreements. See
//  the NOTICE file distributed with this work for additional information regarding copyright ownership.
//  The GPA licenses this file to you under the MIT License (MIT), the "License"; you may not use this
//  file except in compliance with the License. You may obtain a copy of the License at:
//
//      http://opensource.org/licenses/MIT
//
//  Unless agreed to in writing, the subject software distributed under the License is distributed on an
//  "AS-IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. Refer to the
//  License for the specific language governing permissions and limitations.
//
//  Code Modification History:
//  ----------------------------------------------------------------------------------------------------
//  12/07/2017 - J. Ritchie Carroll
//       Generated original version of source code.
//
//******************************************************************************************************

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using GSF;
using GSF.IO.Checksums;

namespace WiXFolderGen
{
    class Program
    {
        private static readonly HashSet<string> ExistingIdentifiers = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        private static readonly HashSet<string> NewIdentifiers = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        private static int Generation;

        static void Main()
        {
            // Generate WiX setup include files for "wwwroot" folder
            NewIdentifiers.Clear();
            Generation = 1;
            GenWWWRootIncludes.Execute();

            // Generate WiX setup include files for "Grafana" folder
            ExistingIdentifiers.UnionWith(NewIdentifiers);
            NewIdentifiers.Clear();
            Generation = 2;
            GenGrafanaIncludes.Execute();
        }

        public static string GetCleanID(string path, string prefix, string suffix, int limit,
            bool replaceDot = false,
            bool removeDot = false,
            bool replaceDirectorySeparatorChar = false,
            bool removeDirectorySeparatorChar = false,
            bool replaceSpaces = false,
            bool removeSpaces = false,
            bool removeUnderscores = false)
        {
            if (replaceDot)
                path = path.Replace('.', '_');

            if (removeDot)
                path = path.Replace(".", "");

            if (replaceDirectorySeparatorChar)
                path = path.Replace(Path.DirectorySeparatorChar, '_');

            if (removeDirectorySeparatorChar)
                path = path.Replace(Path.DirectorySeparatorChar.ToString(), "");

            if (replaceSpaces)
                path = path.ReplaceWhiteSpace('_');

            if (removeSpaces || !replaceSpaces)
                path = path.RemoveWhiteSpace();

            path = new Regex("[^a-zA-Z0-9_.]").Replace(path, "_");

            path = path.RemoveDuplicates("_").TrimEnd('_').TrimStart('_');

            if (removeUnderscores)
                path = path.Replace("_", "");

            if (path.Length > limit)
            {
                byte[] nameBytes = Encoding.Default.GetBytes(path);
                uint crc = nameBytes.Crc32Checksum(0, nameBytes.Length);
                string crcSuffix = crc.ToString();
                path = path.Substring(0, limit - crcSuffix.Length) + crcSuffix;
            }

            path = $"{prefix}{path}{suffix}";

            // Make sure identifiers are unique per generation
            if (ExistingIdentifiers.Contains(path))
                path = $"{path}{Generation}";

            NewIdentifiers.Add(path);

            return path;
        }
    }
}
