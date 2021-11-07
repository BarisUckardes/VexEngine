using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Vex.Profiling
{
  
    public class ProfileEntry
    {
        [DllImport("Kernel32.dll", CallingConvention = CallingConvention.Winapi)]
        private static extern void GetSystemTimePreciseAsFileTime(out long filetime);
        public ProfileEntry()
        {
            /*
             * Generate entry title
             */
            GenerateEntryTitle();

        }

        public string Title
        {
            get
            {
                return m_ClassName +"::"+m_FunctionName;
            }
        }
        public long ElapsedTime
        {
            get
            {
                return m_ElapsedTime;
            }
        }
        internal ProfileTree SelfTree
        {
            get
            {
                return m_Tree;
            }
           
        }
       
      
        /// <summary>
        /// Starts recording
        /// </summary>
        internal void Start()
        {
            /*
             * Get start time
             */
            GetSystemTimePreciseAsFileTime(out m_StartTime);
        }

        /// <summary>
        /// Ends recording
        /// </summary>
        /// <returns></returns>
        internal void End()
        {
            /*
             * Get end system time
             */
            GetSystemTimePreciseAsFileTime(out m_EndTime);

            /*
             * Get elapsed time
             */
            m_ElapsedTime = DateTime.FromFileTimeUtc(m_EndTime).Millisecond - DateTime.FromFileTimeUtc(m_StartTime).Millisecond;
        }

        internal void Bind(ProfileTree tree)
        {
            m_Tree = tree;
            tree.SelfEntry = this;
        }
       
        [MethodImpl(MethodImplOptions.NoInlining)]
        private void GenerateEntryTitle()
        {
            StackTrace stackTrace = new StackTrace();
            StackFrame stackFrame = stackTrace.GetFrame(3);

            m_ClassName = stackFrame.GetMethod().ReflectedType.Name;
            m_FunctionName = stackFrame.GetMethod().Name;
        }

        private ProfileTree m_Tree;
        private string m_FunctionName;
        private string m_ClassName;
        private long m_ElapsedTime;
        private long m_StartTime;
        private long m_EndTime;
        
    }
}
