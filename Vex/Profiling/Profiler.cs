using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vex.Profiling
{
    /// <summary>
    /// A static class for profiler utility
    /// </summary>
    public static class Profiler
    {
        /// <summary>
        /// Returns whether a profile session is currently ongoing
        /// </summary>
        public static bool IsProfileSessionRunning
        {
            get
            {
                return s_ProfileSession;
            }
        }

        /// <summary>
        /// Starts a new profiler session
        /// </summary>
        public static void StartProfileSession()
        {
            /*
             * Clear old profile elements
             */
            s_LastSesionResult = s_Result;
            s_Result = null;
            s_ProfileSession = true;
            s_CurrentStack?.Clear();
            s_Entries?.Clear();
            s_CurrentStack = null;

            /*
             * Start new profile session
             */
            s_CurrentStack = new Stack<ProfileTree>();
            s_Entries = new List<ProfileEntry>();
            s_Current = new ProfileTree("Profiler");
            s_CurrentStack.Push(s_Current);

        }

        /// <summary>
        /// Ends the profile session
        /// </summary>
        public static void EndProfileSession()
        {
            /*
             * Make current profile tree accessable
             */
            s_Result = s_Current;
            s_Current = null;
            s_ProfileSession = false;
        }

        /// <summary>
        /// Starts a profile marker
        /// </summary>
        public static void StartProfile()
        {
            /*
             * Create new profile entry
             */
            ProfileEntry newEntry = new ProfileEntry();

            /*
             * Validate if this title is a unique title
             */
            ProfileEntry searchResult = null;
            foreach(ProfileEntry entry in s_Entries)
            {
                if(entry.Title == newEntry.Title)
                {
                    searchResult = entry;
                    break;
                }
            }

            if(searchResult !=null) // find the existing one and increment invoke count
            {
                /*
                 * Register it to the entries
                 */
                s_Entries.Add(newEntry);

                /*
                 * Start entry
                 */
                newEntry.Start();

                /*
                 * Push and register new tree to current stack
                 */
                if (s_CurrentStack.Count > 0)
                    s_CurrentStack.Peek().RegisterSubTree(searchResult.SelfTree);

                s_CurrentStack.Push(searchResult.SelfTree);
            }
            else // push new one
            {
                /*
                 * Register it to the entries
                 */
                s_Entries.Add(newEntry);

                /*
                 * Create new profile tree and bind it to the entry
                 */
                ProfileTree tree = new ProfileTree(newEntry.Title);
                newEntry.Bind(tree);

                /*
                 * Start entry
                 */
                newEntry.Start();

                /*
                 * Push and register new tree to current stack
                 */
                if(s_CurrentStack.Count > 0)
                    s_CurrentStack.Peek().RegisterSubTree(tree);

                s_CurrentStack.Push(tree);
            }

        }

        /// <summary>
        /// Starts a profile marker with a custom name
        /// </summary>
        /// <param name="name"></param>
        public static void StartProfile(string name)
        {
            /*
             * Create new profile entry
             */
            ProfileEntry newEntry = new ProfileEntry(name);

            /*
             * Validate if this title is a unique title
             */
            ProfileEntry searchResult = null;
            foreach (ProfileEntry entry in s_Entries)
            {
                if (entry.Title == newEntry.Title)
                {
                    searchResult = entry;
                    break;
                }
            }

            if (searchResult != null) // find the existing one and increment invoke count
            {
                /*
                 * Register it to the entries
                 */
                s_Entries.Add(newEntry);

                /*
                 * Start entry
                 */
                newEntry.Start();

                /*
                 * Push and register new tree to current stack
                 */
                if (s_CurrentStack.Count > 0)
                    s_CurrentStack.Peek().RegisterSubTree(searchResult.SelfTree);

                s_CurrentStack.Push(searchResult.SelfTree);
            }
            else // push new one
            {
                /*
                 * Register it to the entries
                 */
                s_Entries.Add(newEntry);

                /*
                 * Create new profile tree and bind it to the entry
                 */
                ProfileTree tree = new ProfileTree(newEntry.Title);
                newEntry.Bind(tree);

                /*
                 * Start entry
                 */
                newEntry.Start();

                /*
                 * Push and register new tree to current stack
                 */
                if (s_CurrentStack.Count > 0)
                    s_CurrentStack.Peek().RegisterSubTree(tree);

                s_CurrentStack.Push(tree);
            }

        }

        /// <summary>
        /// Ends a profile marker
        /// </summary>
        public static void EndProfile()
        {
            /*
             * Finalize this profile title
             */
            ProfileTree lastTree = s_CurrentStack.Pop();

            /*
             * Get last tree entry
             */
            ProfileEntry entry = null;
            foreach(ProfileEntry entryIt in s_Entries)
            {
                if (entryIt.Title == lastTree.Title)
                    entry = entryIt;
            }

            /*
             * End entry
             */
            entry.End();

            /*
             * Update tree properties
             */
            lastTree.ElapsedTime+= entry.ElapsedTime;
        }

        /// <summary>
        /// Returns the result of the current profile session
        /// </summary>
        /// <returns></returns>
        public static ProfileTree GetResultTree()
        {
            return s_Result;
        }

        /// <summary>
        /// Returns the result of the last profile session
        /// </summary>
        /// <returns></returns>
        public static ProfileTree GetLastSessionResult()
        {
            return s_LastSesionResult;
        }

        private static List<ProfileEntry> s_Entries;
        private static ProfileTree s_Current;
        private static Stack<ProfileTree> s_CurrentStack;
        private static ProfileTree s_Result;
        private static ProfileTree s_LastSesionResult;
        private static bool s_ProfileSession;
    }
}
