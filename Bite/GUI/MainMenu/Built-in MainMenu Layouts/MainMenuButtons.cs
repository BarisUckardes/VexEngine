using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bite.GUI;
namespace Bite
{
    public static class MainMenuButtons
    {
        [MainMenuItem("Windows/World Observer")]
        public static void CreteWorldObserver()
        {
            GUIWindow.CreateWindow(typeof(WorldObserverGUIWindow));
        }

        [MainMenuItem("Windows/Object Observer")]
        public static void CreateObjectObserver()
        {
            GUIWindow.CreateWindow(typeof(ObjectObserverGUIWindow));
        }

        [MainMenuItem("Windows/Domain Observer")]
        public static void CreateDomainObserver()
        {
            GUIWindow.CreateWindow(typeof(DomainObserverGUIWindow));
        }

        [MainMenuItem("Windows/Game Observer")]
        public static void CreateGameObserver()
        {
            GUIWindow.CreateWindow(typeof(GameObserverGUIWindow));
        }

        [MainMenuItem("Windows/Performance Observer")]
        public static void CreatePerformanceObserver()
        {
            GUIWindow.CreateWindow(typeof(PerformanceObserverGUIWindow));
        }


    }
}
