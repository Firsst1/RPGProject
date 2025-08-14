public class Player : Character
{
    public List<string> Inventory { get; private set; }
    public string EquippedWeapon { get; private set; }
    public Dictionary<string, string> EquippedArmour { get; private set; }
    private Dictionary<string, Tuple<int, string, string>> _weapons;
    private Dictionary<string, Tuple<int, string, string>> _armours;

    public Player(string name, int maxHP, int maxMana, int maxStamina, int baseAttack = 6, int baseDefense = 0)
        : base(name, maxHP, maxMana, maxStamina, baseAttack, baseDefense)
    {
        Inventory = new List<string> { "Console", "Iron Sword", "Leather Helmet", "Iron Chestplate", "Health Potion" };

        EquippedWeapon = null;
        EquippedArmour = new Dictionary<string, string>()
        {
            { "head", null },
            { "chest", null },
            { "legs", null },
            { "hands", null }
        };

        RecalculateStats();
    }

    public void SetEquipmentData(Dictionary<string, Tuple<int, string, string>> weapons,
                             Dictionary<string, Tuple<int, string, string>> armours)
    {
        _weapons = weapons;
        _armours = armours;
        RecalculateStats();
    }

    public void RecalculateStats(Dictionary<string, Tuple<int, string, string>> weapons = null,
                                 Dictionary<string, Tuple<int, string, string>> armours = null)
    {
        Attack = BaseAttack;
        Defense = BaseDefense;

        if (weapons != null && EquippedWeapon != null)
        {
            string key = EquippedWeapon.ToLower();
            if (weapons.ContainsKey(key))
                Attack += weapons[key].Item1; // weapon bonus
        }

        if (armours != null)
        {
            foreach (var slot in EquippedArmour.Keys)
            {
                string item = EquippedArmour[slot];
                if (item != null && armours.ContainsKey(item.ToLower()))
                {
                    Defense += armours[item.ToLower()].Item1; // armour bonus
                }
            }
        }
    }

    public void ShowInventory()
    {
        if (Inventory.Count == 0)
            Console.WriteLine("\nYour inventory is empty.");
        else
        {
            Console.WriteLine("\nYou have these items:");
            foreach (var item in Inventory)
                Console.WriteLine($"- {item}");
        }
    }

    public void ShowEquippedGear()
    {
        Console.WriteLine("\nEquipped Gear:");
        Console.WriteLine($"Weapon: {(EquippedWeapon ?? "None")}");
        foreach (var slot in EquippedArmour.Keys)
            Console.WriteLine($"{slot.ToUpper()}: {(EquippedArmour[slot] ?? "None")}");
    }

    public void EquipItemPrompt(Dictionary<string, Tuple<int, string, string>> weapons,
                                Dictionary<string, Tuple<int, string, string>> armours)
    {
        var inventoryLower = Inventory.Select(i => i.ToLower()).ToList();

        var equipableItems = inventoryLower
            .Where(item => weapons.ContainsKey(item) || armours.ContainsKey(item))
            .Distinct()
            .ToList();

        if (equipableItems.Count == 0)
        {
            Console.WriteLine("You have no equipable items in your inventory.");
            return;
        }

        Console.WriteLine("\nEquipable items in your inventory:");
        foreach (var item in equipableItems)
        {
            if (weapons.ContainsKey(item))
            {
                var weapon = weapons[item];
                Console.WriteLine($"- Weapon: {weapon.Item3} (Damage: {weapon.Item1})");
            }
            else if (armours.ContainsKey(item))
            {
                var armour = armours[item];
                Console.WriteLine($"- Armour: {armour.Item3} (Slot: {armour.Item2}, Defense: {armour.Item1})");
            }
        }

        Console.WriteLine("\nEnter the name of the item you want to equip:");
        string input = Console.ReadLine();
        EquipItem(input, weapons, armours);
    }

    public void EquipItem(string itemName,
        Dictionary<string, Tuple<int, string, string>> weapons,
        Dictionary<string, Tuple<int, string, string>> armours)
    {
        string itemKey = itemName.ToLower().Trim();
        var inventoryLower = Inventory.Select(i => i.ToLower()).ToList();

        if (!inventoryLower.Contains(itemKey))
        {
            Console.WriteLine("You don't have that item in your inventory.");
            return;
        }

        if (weapons.ContainsKey(itemKey))
        {
            var weapon = weapons[itemKey];
            EquippedWeapon = weapon.Item3;
            Console.WriteLine($"You equipped the weapon: {weapon.Item3}");
        }
        else if (armours.ContainsKey(itemKey))
        {
            var armour = armours[itemKey];
            string slot = armour.Item2;
            if (EquippedArmour.ContainsKey(slot) && EquippedArmour[slot] != null)
                Console.WriteLine($"Replacing {EquippedArmour[slot]} on your {slot}.");

            EquippedArmour[slot] = armour.Item3;
            Console.WriteLine($"You equipped {armour.Item3} on your {slot}.");
        }
        else
        {
            Console.WriteLine("That item cannot be equipped.");
        }

        // Update Attack and Defense after equipping
        RecalculateStats(weapons, armours);
    }

    public void UsePotionPrompt(Dictionary<string, Tuple<int, int, int, string>> potions)
    {
        var inventoryLower = Inventory.Select(item => item.ToLower()).ToList();

        var availablePotions = potions.Keys
            .Where(potionName => inventoryLower.Contains(potionName.ToLower()))
            .ToList();

        if (availablePotions.Count == 0)
        {
            Console.WriteLine("You don't have any potions to use.");
            return;
        }

        Console.WriteLine("You have the following potions:");
        foreach (var pot in availablePotions)
        {
            Console.WriteLine($"- {pot}");
        }

        Console.WriteLine("Which potion would you like to use?");
        string selected = Console.ReadLine();
        UsePotion(selected, potions);
    }

    public void UsePotion(string potionName, Dictionary<string, Tuple<int, int, int, string>> potions)
    {
        string key = potionName.ToLower();
        if (!potions.ContainsKey(key) || !Inventory.Contains(potions[key].Item4))
        {
            Console.WriteLine($"You don't have any {potionName}s.");
            return;
        }

        var potion = potions[key];
        int statIndex = potion.Item1;
        int restoreAmount = potion.Item2;
        int maxValue = potion.Item3;
        string displayName = potion.Item4;

        Console.WriteLine($"\nWould you like to use a {displayName}? y/n");
        string answer = Console.ReadLine()?.ToLower().Trim();

        if (!string.IsNullOrEmpty(answer) && answer[0] == 'y')
        {
            Stats[statIndex] += restoreAmount;
            if (Stats[statIndex] > maxValue)
                Stats[statIndex] = maxValue;

            Inventory.Remove(displayName);
            Console.WriteLine($"You used a {displayName} and restored {restoreAmount} points.");
        }
        else
        {
            Console.WriteLine($"{displayName} not used.");
        }
    }
}