using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using ICities;
using ColossalFramework;
using ColossalFramework.Packaging;
using ColossalFramework.Plugins;

namespace AutoContinueMod
{
    public class AutoContinue : IUserMod
    {
        public void Continue()
        {
            DebugOutputPanel.AddMessage(PluginManager.MessageType.Message, "Loading most recent savegame..");
            SaveGameMetaData latestSaveGame = SaveHelper.GetLatestSaveGame();
            if (latestSaveGame != null)
            {
                SimulationMetaData simulationMetaData = new SimulationMetaData
                {
                    m_CityName = latestSaveGame.cityName,
                    m_updateMode = SimulationManager.UpdateMode.LoadGame
                };
                if (Singleton<PluginManager>.instance.enabledModCount > 0)
                {
                    simulationMetaData.m_disableAchievements = SimulationMetaData.MetaBool.True;
                }
                LoadingManager.instance.m_currentlyLoading = false; //This might cause problems
                Singleton<LoadingManager>.instance.LoadLevel(latestSaveGame.assetRef, "Game", "InGame", simulationMetaData);
            }
        }

        public string Name
        {
            get
            {
                Thread loadingThread = new Thread(new ThreadStart(Continue));
                loadingThread.Start();
                return "Auto Continue";
            }
        }

        public string Description
        {
            get { return "When the game starts up, it will automatically continue your most recent game."; }
        }
    }
}
