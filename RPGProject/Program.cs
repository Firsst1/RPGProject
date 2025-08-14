namespace RPGProject
{
    class Program
    {
        static void Main(string[] args)
        {
            string myName = GetPlayerName();
            Player player = new Player(myName, 125, 100, 100);

            bool running = true;
            Random rng = new Random();

            var weapons = new Dictionary<string, Tuple<int, string, string>>()
            {
                { "iron sword", Tuple.Create(10, "sword", "Iron Sword") },
                { "wooden axe", Tuple.Create(6, "axe", "Wooden Axe") }
            };

            var armours = new Dictionary<string, Tuple<int, string, string>>()
            {
                { "leather helmet", Tuple.Create(3, "head", "Leather Helmet") },
                { "iron chestplate", Tuple.Create(6, "chest", "Iron Chestplate") },
                { "cloth gloves", Tuple.Create(2, "hands", "Cloth Gloves") }
            };

            var potions = new Dictionary<string, Tuple<int, int, int, string>>()
            {
                { "health potion", Tuple.Create(0, 50, player.MaxHP, "Health Potion") },
                { "mana potion", Tuple.Create(1, 40, player.MaxMana, "Mana Potion") },
                { "stamina potion", Tuple.Create(2, 30, player.MaxStamina, "Stamina Potion") }
            };

            player.SetEquipmentData(weapons, armours);

            var enemies = new List<Enemy>
            {
                new Enemy("Goblin", 30, 5, 0),
                new Enemy("Wolf", 40, 12, 2),
                new Enemy("Bandit", 50, 15, 3)
            };

            while (running)
            {
                Console.WriteLine("\nChoose an action:");
                Console.WriteLine("  * View Inventory");
                Console.WriteLine("  * View Stats");
                Console.WriteLine("  * View Gear");
                Console.WriteLine("  * Equip Item");
                Console.WriteLine("  * Use Potion");
                Console.WriteLine("  * Take Damage");
                Console.WriteLine("  * Exit Game");

                string input = Console.ReadLine()?.ToLower();

                switch (input)
                {
                    case "view inventory":
                        player.ShowInventory();
                        break;

                    case "view stats":
                        player.ShowStats();
                        break;

                    case "view gear":
                        player.ShowEquippedGear();
                        break;

                    case "equip item":
                        player.EquipItemPrompt(weapons, armours);
                        break;

                    case "use potion":
                        player.UsePotionPrompt(potions);
                        break;

                    case "take damage":
                        Console.WriteLine("Enter damage amount:");
                        if (int.TryParse(Console.ReadLine(), out int dmg))
                            player.TakeDamage(dmg, ref running);
                        else
                            Console.WriteLine("Invalid number.");
                        break;

                    case "exit game":
                        Console.WriteLine("Exiting the game...");
                        running = false;
                        break;

                    default:
                        Console.WriteLine("Invalid option. Try again.");
                        break;
                }

                if (running && TryEncounterEnemy(rng, out Enemy enemy, enemies))
                {
                    enemy.Fight(player, ref running);
                }
            }
        }

        static string GetPlayerName()
        {
            Console.WriteLine("Welcome Traveller. What is your name?");
            string name = Console.ReadLine()?.Trim();

            if (string.IsNullOrWhiteSpace(name))
            {
                Console.WriteLine("Oops! You didn't tell me your name.");
                return GetPlayerName();
            }

            Console.WriteLine($"Greetings {name}, that is a very unique name!");
            return name;
        }
        static bool TryEncounterEnemy(Random rng, out Enemy enemy, List<Enemy> enemies)
        {
            enemy = null;
            if (rng.Next(100) < 20)
            {
                Enemy template = enemies[rng.Next(enemies.Count)];
                // Create a new Enemy instance so it's fresh
                enemy = new Enemy(template.Name, template.MaxHP, template.BaseAttack, template.BaseDefense);
                return true;
            }
            return false;
        }
    }
}