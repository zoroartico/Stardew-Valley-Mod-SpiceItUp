
using HarmonyLib;
using SpiceItUpMod.Menus;
using SpiceItUpMod.Services;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using StardewValley.GameData.BigCraftables;
using StardewValley.GameData.Shops;

namespace SpiceItUpMod
{
    internal sealed class ModEntry : Mod
    {
        private SpiceManager spiceManager = null!;
        internal static string ModId { get; private set; } = string.Empty;

        public override void Entry(IModHelper helper)
        {
            ModId = this.ModManifest.UniqueID;

            var harmony = new Harmony(this.ModManifest.UniqueID);
            harmony.PatchAll();

            this.spiceManager = new SpiceManager(ModId);
            
            helper.Events.Input.ButtonPressed += this.OnButtonPressed;
            helper.Events.Content.AssetRequested += this.OnAssetRequested;
            helper.Events.GameLoop.DayStarted += this.OnDayStarted;
        }


        private string GrinderId =>
            $"(BC){this.ModManifest.UniqueID}_SpiceGrinder";
        private string FoodPrepStationId =>
            $"(BC){this.ModManifest.UniqueID}_FoodPrepStation";

        private void OnButtonPressed(object? sender, ButtonPressedEventArgs e)
        {
            if (!Context.IsWorldReady)
                return;

            // Only handle the normal action button.
            if (!e.Button.IsActionButton())
                return;

            //disables interference with existing menus.
            if (Game1.activeClickableMenu != null)
                return;

            // Get the tile the player is attempting to interact with.
            var tile = e.Cursor.GrabTile;

            if (!Game1.currentLocation.objects.TryGetValue(tile, out StardewValley.Object placedObject))
                return;

            // Check whether the object is Spice Grinder.
            if (placedObject.QualifiedItemId == GrinderId)
            {
                this.Helper.Input.Suppress(e.Button);
                Game1.activeClickableMenu = new SpiceGrinderMenu();
                return;
            }

            // Check whether the object is Food Prep Station.
            if (placedObject.QualifiedItemId == FoodPrepStationId)
            {
                this.Helper.Input.Suppress(e.Button);
                Game1.activeClickableMenu = new FoodPrepMenu(placedObject);
                return;
            }
        }

        private void OnAssetRequested(
            object? sender,
            AssetRequestedEventArgs e)
        {
            if (e.NameWithoutLocale.IsEquivalentTo("Data/Objects"))
            {
                e.Edit(asset =>
                {
                    var objects =
                        asset.AsDictionary<string, StardewValley.GameData.Objects.ObjectData>().Data;

                    this.spiceManager.AddToObjectData(objects);
                });
            }
            else if (e.NameWithoutLocale.IsEquivalentTo("Data/BigCraftables"))
            {
                e.Edit(asset =>
                {
                    var machines =
                        asset.AsDictionary<string, BigCraftableData>().Data;

                    string grinderId =
                        $"{this.ModManifest.UniqueID}_SpiceGrinder";

                    machines[grinderId] = new BigCraftableData
                    {
                        Name = grinderId,
                        DisplayName = "Spice Grinder",
                        Description = "A machine for grinding and blending spices.",
                        Price = 500,
                        Texture = this.Helper.ModContent.GetInternalAssetName(
                            "Assets/BigCraftables.png"
                        ).Name,
                        SpriteIndex = 0,
                        CanBePlacedIndoors = true,
                        CanBePlacedOutdoors = true,
                        Fragility = 0
                    };

                    string prepStationId =
                        $"{this.ModManifest.UniqueID}_FoodPrepStation";

                    machines[prepStationId] = new BigCraftableData
                    {
                        Name = prepStationId,
                        DisplayName = "Food Prep Station",
                        Description = "A station for preparing and seasoning cooked food.",
                        Price = 1000,
                        Texture = this.Helper.ModContent.GetInternalAssetName(
                            "Assets/BigCraftables.png"
                        ).Name,
                        SpriteIndex = 1,
                        CanBePlacedIndoors = true,
                        CanBePlacedOutdoors = true,
                        Fragility = 0
                    };
                });
            }
            else if (e.NameWithoutLocale.IsEquivalentTo("Data/CraftingRecipes"))
            {
                e.Edit(asset =>
                {
                    var recipes = asset.AsDictionary<string, string>().Data;

                    this.spiceManager.AddToCraftingRecipes(recipes);
                });
            }
            else if (e.NameWithoutLocale.IsEquivalentTo("Data/Shops"))
            {
                e.Edit(asset =>
                {
                    var shops = asset.AsDictionary<string, ShopData>().Data;

                    if (!shops.TryGetValue("Saloon", out ShopData? saloon) || saloon == null)
                        return;

                    saloon.Items.Add(new ShopItemData
                    {
                        Id = $"{this.ModManifest.UniqueID}_SpiceGrinder",
                        ItemId = GrinderId,
                        Price = 1000,
                        Condition = "PLAYER_FRIENDSHIP_POINTS Current Gus 1000"
                    });
                    saloon.Items.Add(new ShopItemData
                    {
                        Id = $"{this.ModManifest.UniqueID}_FoodPrepStation",
                        ItemId = FoodPrepStationId,
                        Price = 2000,
                        Condition = "PLAYER_FRIENDSHIP_POINTS Current Gus 1000"
                    });
                });
            }
            else if (e.NameWithoutLocale.IsEquivalentTo("Data/mail"))
            {
                e.Edit(asset =>
                {
                    var mail = asset.AsDictionary<string, string>().Data;

                    mail[$"{ModId}_GusStationLetter"] =
                        "Hey there @!^^"
                        + "I think it's time I share a little food secret with you. "
                        + "The secret to a good meal isn't just the ingredients, "
                        + "but the perfect blend of spices too!^^"
                        + "I'm now selling a 'Spice Grinder' and a^"
                        + "'Food Prep Station'.^^"
                        + "- Gus[#]A Note from Gus";
                });
            }
        }

        private void OnDayStarted(object? sender, DayStartedEventArgs e)
        {
            string mailId = $"{ModId}_GusStationLetter";
            int hearts = Game1.player.getFriendshipHeartLevelForNPC("Gus");
            bool alreadyQueued = Game1.player.hasOrWillReceiveMail(mailId);

            if (hearts < 4 || alreadyQueued)
                return;

            Game1.addMailForTomorrow(mailId);

        }
    }
}