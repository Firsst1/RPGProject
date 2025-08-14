public class Enemy : Character
{
    public Enemy(string name, int maxHP, int attack, int defense)
        : base(
            string.IsNullOrWhiteSpace(name) ? throw new ArgumentException("Enemy name cannot be empty.", nameof(name)) : name,
            maxHP > 0 ? maxHP : throw new ArgumentOutOfRangeException(nameof(maxHP), "Max HP must be greater than zero."),
            0,
            0,
            attack >= 0 ? attack : throw new ArgumentOutOfRangeException(nameof(attack), "Attack must be non-negative."),
            defense >= 0 ? defense : throw new ArgumentOutOfRangeException(nameof(defense), "Defense must be non-negative.")
        )
    {
    }

    public void Fight(Player player, ref bool running)
    {
        if (player == null)
            throw new ArgumentNullException(nameof(player), "Player cannot be null when starting a fight.");

        if (Stats[0] <= 0)
        {
            Console.WriteLine($"{Name} is already defeated and cannot fight.");
            return;
        }

        Console.WriteLine($"\nA wild {Name} appears!");

        while (Stats[0] > 0 && player.Stats[0] > 0)
        {
            Console.WriteLine($"\nYour HP: {player.Stats[0]} | {Name} HP: {Stats[0]}");
            Console.WriteLine("Choose action: [attack] or [flee]");
            string action = Console.ReadLine()?.ToLower().Trim();

            if (string.IsNullOrWhiteSpace(action))
            {
                Console.WriteLine("Invalid action. Please type 'attack' or 'flee'.");
                continue;
            }

            if (action == "attack")
            {
                Console.WriteLine("Choose your attack type: [quick] or [heavy]");
                string attackType = Console.ReadLine()?.ToLower().Trim();

                if (string.IsNullOrWhiteSpace(attackType) || 
                    (attackType != "quick" && attackType != "heavy"))
                {
                    Console.WriteLine("Invalid attack type! You lose your turn.");
                    attackType = null; // skip player attack
                }

                int damageToEnemy = 0;
                Random rng = new Random();

                if (attackType == "quick")
                {
                    damageToEnemy = Math.Max(0, player.Attack - Defense - 2);
                    Console.WriteLine("You perform a quick strike!");
                }
                else if (attackType == "heavy")
                {
                    if (rng.Next(100) < 70)
                    {
                        damageToEnemy = Math.Max(0, (int)(player.Attack + 4 + (player.Attack * 0.3)) - Defense);
                        Console.WriteLine("You swing a heavy blow!");
                    }
                    else
                    {
                        Console.WriteLine("Your heavy attack missed!");
                    }
                }

                // Apply damage if any
                if (damageToEnemy > 0)
                {
                    Stats[0] -= damageToEnemy;
                    Console.WriteLine($"You deal {damageToEnemy} damage to the {Name}.");
                }

                if (Stats[0] <= 0)
                {
                    Console.WriteLine($"You defeated the {Name}!");
                    break;
                }

                // Enemy attacks player
                Console.WriteLine($"DEBUG: Enemy Attack={Attack}, Player Defense={player.Defense}");
                int damageToPlayer = Attack;
                Console.WriteLine($"DEBUG: Damage calculated = {damageToPlayer}");
                player.TakeDamage(damageToPlayer, ref running);
            }
            else if (action == "flee")
            {
                Console.WriteLine("You fled the battle!");
                break;
            }
            else
            {
                Console.WriteLine("Invalid action.");
            }
        }
    }
}
