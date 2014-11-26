﻿//******************************************************************************************************
//  ConvertArchiveFile.cs - Gbtc
//
//  Copyright © 2014, Grid Protection Alliance.  All Rights Reserved.
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
//  12/12/2012 - Steven E. Chisholm
//       Generated original version of source code. 
//  11/24/2014 - J. Ritchie Carroll
//       Updated to support simplified file format.
//
//******************************************************************************************************

using System;
using System.IO;
using GSF.IO;
using GSF.Snap;
using GSF.Snap.Collection;
using GSF.Snap.Storage;
using openHistorian.Snap;

namespace openHistorian.Utility
{
    public static class ConvertArchiveFile
    {
        public static unsafe long ConvertVersion1File(string oldFileName, string newFileName, EncodingDefinition compressionMethod, out long readTime, out long sortTime, out long writeTime)
        {
            long startTime;
            long count = 0;

            if (!File.Exists(oldFileName))
                throw new ArgumentException("Old file does not exist", "oldFileName");

            if (File.Exists(newFileName))
                throw new ArgumentException("New file already exists", "newFileName");

            HistorianKey key = new HistorianKey();
            HistorianValue value = new HistorianValue();
            OldHistorianReader hist = new OldHistorianReader(oldFileName);

            // TODO: Derive a new class that will increment EntryNumber instead of removing duplicates
            SortedPointBuffer<HistorianKey, HistorianValue> points = null;

            Func<OldHistorianReader.Points, bool> fillInMemoryTree = (p) =>
            {
                count++;

                if (count > long.MaxValue)
                    return false;

                key.Timestamp = (ulong)p.Time.Ticks;
                key.PointID = (ulong)p.PointID;

                value.Value3 = (ulong)p.Flags;
                value.Value1 = *(uint*)&p.Value;

                if (!points.TryEnqueue(key, value))
                    count--;

                return true;
            };

            startTime = DateTime.UtcNow.Ticks;
            hist.Read(fillInMemoryTree, out points);
            readTime = DateTime.UtcNow.Ticks - startTime;

            startTime = DateTime.UtcNow.Ticks;
            points.IsReadingMode = true;
            sortTime = DateTime.UtcNow.Ticks - startTime;

            startTime = DateTime.UtcNow.Ticks;
            SortedTreeFileSimpleWriter<HistorianKey, HistorianValue>.Create(Path.Combine(FilePath.GetDirectoryName(newFileName), FilePath.GetFileName(newFileName) + ".~d2i"), newFileName, 4096, null, compressionMethod, points);
            writeTime = DateTime.UtcNow.Ticks - startTime;

            return count;
        }
    }
}

