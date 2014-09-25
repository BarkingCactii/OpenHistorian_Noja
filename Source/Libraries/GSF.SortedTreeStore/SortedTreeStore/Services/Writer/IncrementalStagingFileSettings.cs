//******************************************************************************************************
//  IncrementalStagingFile`2.cs - Gbtc
//
//  Copyright � 2014, Grid Protection Alliance.  All Rights Reserved.
//
//  Licensed to the Grid Protection Alliance (GPA) under one or more contributor license agreements. See
//  the NOTICE file distributed with this work for additional information regarding copyright ownership.
//  The GPA licenses this file to you under the Eclipse Public License -v 1.0 (the "License"); you may
//  not use this file except in compliance with the License. You may obtain a copy of the License at:
//
//      http://www.opensource.org/licenses/eclipse-1.0.php
//
//  Unless agreed to in writing, the subject software distributed under the License is distributed on an
//  "AS-IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. Refer to the
//  License for the specific language governing permissions and limitations.
//
//  Code Modification History:
//  ----------------------------------------------------------------------------------------------------
//  2/16/2014 - Steven E. Chisholm
//       Generated original version of source code. 
//       
//
//******************************************************************************************************

using GSF.SortedTreeStore.Tree;

namespace GSF.SortedTreeStore.Services.Writer
{
    /// <summary>
    /// The settings for an <see cref="IncrementalStagingFile{TKey,TValue}"/>
    /// </summary>
    public class IncrementalStagingFileSettings
    {
        /// <summary>
        /// Determines if the archive is a Memory Archive
        /// </summary>
        public bool IsMemoryArchive = true;
        /// <summary>
        /// Gets the encoding method to write final files in.
        /// </summary>
        public EncodingDefinition Encoding = SortedTree.FixedSizeNode;
        /// <summary>
        /// The save path to write final archive files in.
        /// </summary>
        public string SavePath = string.Empty;

        /// <summary>
        /// Sets the file extension that will be used. Should appear with a leading period only.
        /// </summary>
        public string FileExtension = ".d2";
    }
}