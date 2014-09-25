﻿//******************************************************************************************************
//  SeekFilterUniverse.cs - Gbtc
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
//  11/9/2013 - Steven E. Chisholm
//       Generated original version of source code. 
//     
//******************************************************************************************************

using System;
using GSF.IO;
using GSF.SortedTreeStore.Tree;

namespace GSF.SortedTreeStore.Filters
{
    /// <summary>
    /// Represents no filter
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    public class SeekFilterUniverse<TKey>
        : SeekFilterBase<TKey>
        where TKey : SortedTreeTypeBase<TKey>, new()
    {
        private bool m_isEndReached;

        public SeekFilterUniverse()
        {
            StartOfFrame = new TKey();
            EndOfFrame = new TKey();
            StartOfRange = StartOfFrame;
            EndOfRange = EndOfFrame;
            Reset();
        }

        public override Guid FilterType
        {
            get
            {
                return Guid.Empty;
            }
        }

        public override void Save(BinaryStreamBase stream)
        {
            throw new NotSupportedException();
        }

        public override void Reset()
        {
            m_isEndReached = false;
            StartOfRange.SetMin();
            EndOfRange.SetMax();
        }

        public override bool NextWindow()
        {
            if (m_isEndReached)
            {
                return false;
            }
            StartOfRange.SetMin();
            EndOfRange.SetMax();
            m_isEndReached = true;
            return true;
        }
    }
}
