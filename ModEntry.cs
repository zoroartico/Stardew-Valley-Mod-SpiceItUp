
using System;
using Microsoft.Xna.Framework;
using SpiceItUpMod.Services;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewModdingAPI.Utilities;
using StardewValley.GameData.Objects;
using StardewValley.GameData.BigCraftables;
using StardewValley;
using StardewValley.Menus;

using SpiceItUpMod.Menus;

namespace SpiceItUpMod
{
    internal sealed class ModEntry : Mod
    {
        private SpiceManager spiceManager = null!;

        public override void Entry(IModHelper helper)
        {
            this.spiceManager = new SpiceManager(
                this.ModManifest.UniqueID
            );
            helper.Events.Input.ButtonPressed += this.OnButtonPressed;
            helper.Events.Content.AssetRequested += this.OnAssetRequested;
        }


        private const string GrinderId =
            "(BC)zoroartico.SpiceItUpMod_SpiceGrinder";

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
            if (placedObject.QualifiedItemId != GrinderId)
                return;

            // Prevent Stardew from processing the same interaction.
            this.Helper.Input.Suppress(e.Button);

            // Temporary interaction confirmation.
            Game1.activeClickableMenu = new SpiceGrinderMenu(
                this.spiceManager
            );
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

                    const string grinderId =
                        "zoroartico.SpiceItUpMod_SpiceGrinder";

                    machines[grinderId] = new BigCraftableData
                    {
                        Name = grinderId,
                        DisplayName = "Spice Grinder",
                        Description = "A machine for grinding and blending spices.",
                        Price = 500,
                        Texture = "TileSheets/Craftables",
                        SpriteIndex = 0,
                        CanBePlacedIndoors = true,
                        CanBePlacedOutdoors = true,
                        Fragility = 0
                    };
                });
            }
        }

    }
}
