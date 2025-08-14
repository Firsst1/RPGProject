public class Character : Entity
{
    public int[] Stats { get; protected set; } // [0] = HP, [1] = Mana, [2] = Stamina
    public int MaxHP { get; protected set; }
    public int MaxMana { get; protected set; }
    public int MaxStamina { get; protected set; }
    public int BaseAttack { get; protected set; }
    public int BaseDefense { get; protected set; }
    public int Attack { get; protected set; }
    public int Defense { get; protected set; }

    public Character(string name, int maxHP, int maxMana, int maxStamina, int baseAttack = 6, int baseDefense = 0)
        : base(name)
    {
        MaxHP = maxHP;
        MaxMana = maxMana;
        MaxStamina = maxStamina;
        Stats = new int[] { maxHP, maxMana, maxStamina };

        BaseAttack = baseAttack;
        BaseDefense = baseDefense;

        Attack = baseAttack;
        Defense = baseDefense;
    }

    public void ShowStats()
    {
        Console.WriteLine($"\nStats for {Name}:\nHP: {Stats[0]} \nMana: {Stats[1]} \nStamina: {Stats[2]}\nAttack: {Attack}\nDefense: {Defense}");
    }

    public void TakeDamage(int amount, ref bool running)
    {
        int reducedDamage = Math.Max(amount - Defense, 0); // reduce by defense
        Stats[0] -= reducedDamage; // defense is applied here
        Console.WriteLine($"{Name} takes {reducedDamage} damage (raw {amount})!");

        if (Stats[0] <= 0)
        {
            Stats[0] = 0;
            Console.WriteLine($"{Name} has fallen!");
            running = false;
        }
    }
}