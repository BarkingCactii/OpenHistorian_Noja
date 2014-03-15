﻿//******************************************************************************************************
//  StateMachine.cs - Gbtc
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
//  1/13/2013 - Steven E. Chisholm
//       Generated original version of source code. 
//  2/14/2014 - Steven E. Chisholm
//       Migrated code to GSF.Core from the openHistorian project.
//       
//
//******************************************************************************************************

using System;
using System.Threading;

namespace GSF.Threading
{
    /// <summary>
    /// Helps facilitate a multithreaded state machine.
    /// </summary>
    /// <remarks>
    /// State machine variables must be integers since <see cref="Interlocked"/> methods require premitive types.
    /// </remarks>
    public class StateMachine
    {
        /// <summary>
        /// The internal state of the state machine.
        /// </summary>
        private volatile int m_state;
        private volatile int m_lockVersion;
        int m_pendingState;

        /// <summary>
        /// Creates a new <see cref="StateMachine"/>
        /// </summary>
        /// <param name="initialState">the initial state of the machine</param>
        /// <param name="pendingState">the value that designates a pending state has been entered. 
        /// Typically this state is used to lock the state machine while completing a series of tasks.</param>
        public StateMachine(int initialState, int pendingState)
        {
            m_state = initialState;
            m_pendingState = pendingState;
            m_lockVersion = 1;
        }

        /// <summary>
        /// Acquires a lock on the state variable, swaping its current state with the pending state.
        /// </summary>
        /// <returns>A disposable object that can be used to configure the state.</returns>
        public LockState Lock()
        {
            SpinWait wait = default(SpinWait);
            while (true)
            {
                int state = m_state;
                if (state != m_pendingState)
                    if (Interlocked.CompareExchange(ref m_state, m_pendingState, state) == state)
                    {
                        m_lockVersion++;
                        return new LockState(this, m_lockVersion, state);
                    }
                wait.SpinOnce();
            }
        }

        /// <summary>
        /// Attempts to release the lock on the state machine. 
        /// </summary>
        /// <param name="state">the state to set the lock to</param>
        /// <param name="version">the verion number associated with this lock. Prevents duplicate calls.</param>
        /// <returns></returns>
        bool TryRelease(int state, int version)
        {
            //Prevents duplicate calls.
            if (m_lockVersion != version)
                return false;
            m_lockVersion++;
            m_state = state;
            return true;
        }

        /// <summary>
        /// A Disposable lock that will release the lock on a state machine.
        /// </summary>
        public struct LockState : IDisposable
        {

            StateMachine m_stateMachine;

            /// <summary>
            /// to prevent duplicate calls to Release();
            /// </summary>
            int m_version;

            int m_state;

            /// <summary>
            /// Gets the state of the machine before the lock was acquired.
            /// </summary>
            public int State
            {
                get
                {
                    return m_state;
                }
            }

            /// <summary>
            /// Creates a lock around the state machine.
            /// </summary>
            /// <param name="machine"></param>
            /// <param name="version"></param>
            /// <param name="state"></param>
            internal LockState(StateMachine machine, int version, int state)
            {
                m_stateMachine = machine;
                m_version = version;
                m_state = state;
            }

            /// <summary>
            /// Same as <see cref="Release()"/>
            /// Note, duplicate calls to this function will be ignored.
            /// </summary>
            public void Dispose()
            {
                m_stateMachine.TryRelease(m_state, m_version);
                m_state = int.MinValue;
            }

            /// <summary>
            /// Releases the lock on the state machine. Setting it back to it's previous state.
            /// </summary>
            public void Release()
            {
                if (!m_stateMachine.TryRelease(m_state, m_version))
                    throw new Exception("Duplicate lock release on the state machine.");
                m_state = int.MinValue;
            }

            /// <summary>
            /// Releases the lock on the state machine. Setting its state to the provided variable.
            /// Note, duplicate calls to this function will throw an exception.
            /// </summary>
            /// <param name="state">the state to set the machine to.</param>
            public void Release(int state)
            {
                if (!m_stateMachine.TryRelease(state, m_version))
                    throw new Exception("Duplicate lock release on the state machine.");
                m_state = int.MinValue;
            }

            /// <summary>
            /// Reacquires an existing lock that has been released. Note, the state variables might be different.
            /// </summary>
            public void Reacquire()
            {
                if (m_version == m_stateMachine.m_lockVersion) 
                    throw new Exception("Lock has not yet been released. Release the lock before reacquring one.");
                this = m_stateMachine.Lock();
            }

            /// <summary>
            /// Gets the state of the state machine at the time of the lock.
            /// </summary>
            /// <param name="state"></param>
            /// <returns></returns>
            public static implicit operator int(LockState state)
            {
                return state.m_state;
            }
        }


    }
}