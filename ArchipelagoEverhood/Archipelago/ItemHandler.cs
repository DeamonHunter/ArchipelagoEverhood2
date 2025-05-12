using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Archipelago.MultiClient.Net.Models;
using ArchipelagoEverhood.Data;
using ArchipelagoEverhood.Patches;
using Fungus;
using TMPro;

namespace ArchipelagoEverhood.Archipelago
{
    public class ArchipelagoItemHandler
    {
        private HashSet<ScoutedItemInfo> _blockedItems = new();
        private ConcurrentQueue<(global::Archipelago.MultiClient.Net.Models.ItemInfo, bool)> _itemsToAdd = new();
        private bool _inBattle;
        private readonly int _currentSlot;
        private readonly int _currentTeam;

        private Queue<global::Archipelago.MultiClient.Net.Models.ItemInfo> _queuedSays = new();

        public ArchipelagoItemHandler(int currentSlot, int team)
        {
            _currentSlot = currentSlot;
            _currentTeam = team;

            if (!Globals.GameplayRoot)
            {
                Globals.Logging.Error("ItemHandler", "Gameplay root not loaded yet?");
                return;
            }

            //Todo: Do these always work?
            Globals.GameplayRoot!.OnBattleStart += OnBattleStart;
            Globals.GameplayRoot!.OnBattleFinish += OnBattleEnd;
        }

        public void OnDestroy()
        {
            Globals.GameplayRoot!.OnBattleStart += OnBattleStart;
            Globals.GameplayRoot!.OnBattleFinish += OnBattleEnd;
        }

        public void HandleScoutedItem(ScoutedItemInfo info)
        {
            if (!_blockedItems.Add(info))
            {
                Globals.Logging.Error("ItemHandler", $"Scouted Item has been handled twice? {info.LocationId}:{info.ItemId}");
                return;
            }

            if (info.Player.Slot == _currentSlot && info.Player.Team == _currentTeam)
                _itemsToAdd.Enqueue((info, false));
        }

        public void HandleRemoteItem(global::Archipelago.MultiClient.Net.Models.ItemInfo info)
        {
            //Todo: Does this always work?
            if (!_blockedItems.Any(x => x.ItemId == info.ItemId && x.LocationId == info.LocationId && x.Player.Slot == info.Player.Slot))
                return;

            _itemsToAdd.Enqueue((info, true));
        }

        private void OnBattleStart()
        {
            _inBattle = true;
            Globals.Logging.Log("ItemHandler", "Battle Started");
        }

        private void OnBattleEnd()
        {
            _inBattle = true;
            Globals.Logging.Log("ItemHandler", "Battle Ended");
        }

        public void Update()
        {
            //Don't reward items during battle or loading.
            if (!_inBattle || SceneManagerRoot.sceneIsLoading)
                return;

            while (_itemsToAdd.TryDequeue(out var item))
                UnlockItem(item.Item1, item.Item2);

            if (Globals.GameplayRoot!.ActiveBattleRoot || (SayDialog.ActiveSayDialog && (SayDialog.ActiveSayDialog.enabled || SayDialog.ActiveSayDialog.gameObject.activeInHierarchy)))
                return;

            if (!_queuedSays.TryDequeue(out var sayItem))
                return;

            if (sayItem.Player.Slot != _currentSlot && sayItem.Player.Team != _currentTeam)
                SayOnEnterPatch.ForceShowDialogue($"Received {sayItem.ItemDisplayName} from {sayItem.Player.Name}", null);
            else
                SayOnEnterPatch.ForceShowDialogue($"You found your {sayItem.ItemDisplayName}!", null);
        }

        private void UnlockItem(global::Archipelago.MultiClient.Net.Models.ItemInfo itemInfo, bool remote)
        {
            if (_currentSlot == itemInfo.Player.Slot)
            {
                Globals.Logging.Log("ItemHandler", "Tried to Unlock");
                return;
            }

            //Todo: Item Sprites?
            if (ItemData.ItemsById.TryGetValue(itemInfo.ItemId, out var item))
            {
                Globals.ServicesRoot!.GameData.CachedGeneralData.AddCollectedItem(item.ToString(), 1);
                if (remote)
                    _queuedSays.Enqueue(itemInfo);
            }
            else if (ItemData.WeaponsById.TryGetValue(itemInfo.ItemId, out var weapon))
            {
                Globals.ServicesRoot!.GameData.CachedGeneralData.AddWeapon(weapon);
                if (remote)
                    _queuedSays.Enqueue(itemInfo);
            }
            else if (ItemData.ArtifactsById.TryGetValue(itemInfo.ItemId, out var artifact))
            {
                Globals.ServicesRoot!.GameData.CachedGeneralData.AddArtifactItem(artifact.ToString(), 1);
                if (remote)
                    _queuedSays.Enqueue(itemInfo);
            }
            else if (ItemData.XpsById.TryGetValue(itemInfo.ItemId, out var xp))
            {
                if (Globals.GameplayRoot!.ActiveBattleRoot)
                {
                    var victory = (BattleVictoryResult)(typeof(Main_GameplayRoot).GetField("battleVictoryResult", BindingFlags.Instance | BindingFlags.NonPublic)!.GetValue(Globals.GameplayRoot));

                    var xpLevel_player = (GeneralData.XpLevelInfo)(typeof(BattleVictoryResult).GetMethod("AddPlayerXp", BindingFlags.Instance | BindingFlags.NonPublic)
                        !.Invoke(victory, new object[] { Globals.ServicesRoot!, Globals.ServicesRoot!.GameData.GeneralData.xpLevel_player, xp }));

                    var text = (TextMeshProUGUI)(typeof(BattleVictoryResult).GetField("playerXpLeftLabel", BindingFlags.Instance | BindingFlags.NonPublic)!.GetValue(victory));
                    text.text = (xpLevel_player.xpRequiredForNextLevel - xpLevel_player.lastXp).ToString();
                    Globals.ServicesRoot.GameData.GeneralData.playerJustLeveledUp = xpLevel_player.leveledUp;

                    var coRoutine = (IEnumerator)(typeof(BattleVictoryResult).GetMethod("DoPlayerLevelSliderUpdate", BindingFlags.Instance | BindingFlags.NonPublic)
                        !.Invoke(victory, new object[] { xpLevel_player }));
                    victory.StartCoroutine(coRoutine);
                }
                else
                {
                    var xpLevel = Globals.ServicesRoot!.GameData.GeneralData.xpLevel_player;
                    var playerLevelXpRequired = Globals.ServicesRoot!.InfinityProjectExperience.GetPlayerLevelXpRequired(xpLevel.level);
                    Globals.TopdownRoot!.PlayerXpGainUI.GainXP(xpLevel.AddXp(xp, playerLevelXpRequired));
                }
            }
            else if (ItemData.CosmeticsById.TryGetValue(itemInfo.ItemId, out var cosmetic))
            {
                Globals.ServicesRoot!.GameData.CachedGeneralData.AddCosmectic(cosmetic);
                if (remote)
                    _queuedSays.Enqueue(itemInfo);
            }
        }
    }
}