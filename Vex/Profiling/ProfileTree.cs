using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Vex.Profiling
{
    public class ProfileTree
    {
        
        public ProfileTree(string title)
        {
            m_Title = title;
            m_InvokeCount = 1;
            m_ElapsedTime = 0;
            m_SubTrees = new List<ProfileTree>();
        }

        public List<ProfileTree> SubTrees
        {
            get
            {
                return m_SubTrees;
            }
        }
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
        public string Title
        {
            get
            {
                return m_Title;
            }
        }
        public uint InvokeCount
        {
            get
            {
                return m_InvokeCount;
            }
        }
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
        internal void IncrementInvoke()
        {
            m_InvokeCount++;
        }
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
