﻿//******************************************************************************************************
//  KeyMatchFilterUniverse.cs - Gbtc
//
//  Copyright © 2013, Grid Protection Alliance.  All Rights Reserved.
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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GSF.IO;

namespace GSF.SortedTreeStore.Filters
{
    /// <summary>
    /// Represents no filter
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    public class KeyMatchFilterUniverse<TKey>
        : KeyMatchFilterBase<TKey>
    {
        public override Guid FilterType
        {
            get
            {
                return Guid.Empty;
            }
        }

        public override void Load(BinaryStreamBase stream)
        {
            
        }

        public override void Save(BinaryStreamBase stream)
        {
            
        }

        public override bool Contains(TKey value)
        {
            return true;
        }
     
    }
}
