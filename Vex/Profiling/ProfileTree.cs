using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Vex.Profiling
{
    /// <summary>
    /// A tree which encapsulates profile elements
    /// </summary>
    public class ProfileTree
    {
        
        public ProfileTree(string title)
        {
            m_Title = title;
            m_InvokeCount = 1;
            m_ElapsedTime = 0;
            m_SubTrees = new List<ProfileTree>();
        }

        /// <summary>
        /// Returns the sub trees
        /// </summary>
        public List<ProfileTree> SubTrees
        {
            get
            {
                return m_SubTrees;
            }
        }

        /// <summary>
        /// Set&&Get self profiler entry
        /// </summary>
        internal ProfileEntry SelfEntry
        {
            get
            {
                return m_SelfEntry;
            }
            set
            {
                m_SelfEntry = value;
            }
        }

        /// <summary>
        /// Returns the marker title
        /// </summary>
        public string Title
        {
            get
            {
                return m_Title;
            }
        }

        /// <summary>
        /// Returns how many times this marker invoked
        /// </summary>
        public uint InvokeCount
        {
            get
            {
                return m_InvokeCount;
            }
        }

        /// <summary>
        /// Set&&Get time between first marker and the last one
        /// </summary>
        public long ElapsedTime
        {
            get
            {
                return m_ElapsedTime;
            }
            internal set
            {
                m_ElapsedTime = value;
            }
        }

        /// <summary>
        /// Util function for debugging the tree
        /// </summary>
        /// <param name="gap"></param>
        internal void Debug(uint gap)
        {
            /*
             * Create gap
             */
            for(int i=0;i<gap;i++)
            {
                Console.Write(" ");
            }

            /*
             * Debug self
             */
            Console.WriteLine($"{m_Title}[{m_InvokeCount}] took {m_ElapsedTime} ms");

            /*
             * Debug sub trees
             */
            gap++;
            foreach (ProfileTree subTreeIt in m_SubTrees)
                subTreeIt.Debug(gap);
            

        }

        /// <summary>
        /// Increments the invoke count of this profile tree
        /// </summary>
        internal void IncrementInvoke()
        {
            m_InvokeCount++;
        }

        /// <summary>
        /// Registers a sub tree
        /// </summary>
        /// <param name="tree"></param>
        internal void RegisterSubTree(ProfileTree tree)
        {
            /*
             * Check if there is a tree title match
             */
            bool alreadHas = false;
            foreach(ProfileTree treeIt in m_SubTrees)
            {
                if(treeIt.Title == tree.Title)
                {
                    treeIt.IncrementInvoke();
                    alreadHas = true;
                    break;
                }
            }    

            /*
             * Add new if no match found
             */
            if(!alreadHas)
                m_SubTrees.Add(tree);
        }

        private List<ProfileTree> m_SubTrees;
        private ProfileEntry m_SelfEntry;
        private string m_Title;
        private uint m_InvokeCount;
        private long m_ElapsedTime;
       
    }

   
}
