using System; 
using System.Collections.Generic;
using Xunit;
using RPGProject;

namespace RPGProjectTests
{
    public class PlayerTests
    {
        [Fact]
        public void EquipItem_ShouldIncreaseDefense_WhenArmorIsEquipped()
        {
            // Arrange
            var player = new Player("TestHero", 100, 50, 50);

            var weapons = new Dictionary<string, Tuple<int, string, string>>();
            var armours = new Dictionary<string, Tuple<int, string, string>>
            {
                { "leather helmet", Tuple.Create(5, "head", "Leather Helmet") }
            };

            // Player must own the armor in inventory (display name)
            player.Inventory.Add("Leather Helmet");

            // Set equipment data like in the game
            player.SetEquipmentData(weapons, armours);

            int oldDefense = player.Defense;

            // Act
            player.EquipItem("leather helmet", weapons, armours);

            // Assert
            Assert.True(player.Defense > oldDefense, "Defense should increase when armor is equipped.");
            Assert.Equal("Leather Helmet", player.EquippedArmour["head"]);
        }

        [Fact]
        public void EquipItem_ShouldIncreaseAttack_WhenWeaponIsEquipped()
        {
            // Arrange
            var player = new Player("TestHero", 100, 50, 50);

            var weapons = new Dictionary<string, Tuple<int, string, string>>
            {
                // key must be lowercase for lookups, display name is Item3
                { "iron sword", Tuple.Create(10, "sword", "Iron Sword") }
            };
            var armours = new Dictionary<string, Tuple<int, string, string>>();

            // Give the player the display name in inventory
            player.Inventory.Add("Iron Sword");

            // Store equipment data (matches how game does it)
            player.SetEquipmentData(weapons, armours);

            int oldAttack = player.Attack;

            // Act
            player.EquipItem("iron sword", weapons, armours);

            // Assert
            Assert.True(player.Attack > oldAttack, "Attack should increase when a weapon is equipped.");
            Assert.Equal("Iron Sword", player.EquippedWeapon);
        }

        [Fact]
        public void EquipItem_ShouldNotThrow_WhenItemDoesNotExist()
        {
            // Arrange
            var player = new Player("TestHero", 100, 10, 5, 0, 0);
            int baseDefense = player.Defense;

            // Act
            var exception = Record.Exception(() =>
                player.EquipItem("NonExistentItem",
                                 new Dictionary<string, Tuple<int, string, string>>(),
                                 new Dictionary<string, Tuple<int, string, string>>())
            );

            // Assert
            Assert.Null(exception);
            Assert.Equal(baseDefense, player.Defense);
        }
    }
}