using System;
using System.Collections.Generic;

namespace Task6._8
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Arena arena = new Arena();
            arena.Work();
        }
    }

    class Arena
    {
        public void Work()
        {
            int defaultWidth = Console.WindowWidth;
            bool isWork = true;
            Console.WriteLine(GetRandomPhrase());

            while (isWork)
            {
                List<Fighter> fighters = new List<Fighter>
                {
                    new Wizard("Волшебник", 100, 2, 2),
                    new Knight("Рыцарь", 100, 25, 5),
                    new Robber("Разбойник", 100, 15, 5),
                    new Ninja("Ниндзя", 100, 25, 5),
                    new Hunter("Охотник", 100, 20, 2)
                };
                Console.WriteLine("1.Показать бойцов. \n2.Показать умения бойцов. \n3.Начать бой. \n4.Выход.");
                string userInput = Console.ReadLine();
                Console.Clear();

                switch (userInput)
                {
                    case "1":
                        ShowStats(fighters);
                        break;
                    case "2":
                        ShowSkills(fighters);
                        break;
                    case "3":
                        Fight(fighters);
                        break;
                    case "4":
                        isWork = false;
                        break;
                }

                Console.WriteLine(" \nДля продолжения нажмите любую клавишу:");
                Console.ReadKey();
                Console.Clear();
                fighters.Clear();
                Console.WindowWidth = defaultWidth;
            }
        }

        private void ShowStats(List<Fighter> fighters)
        {
            Console.WindowWidth = 155;

            for (int i = 0; i < fighters.Count; i++)
            {
                Console.Write((i + 1) + ".");
                fighters[i].ShowStats();
            }
        }

        private void ShowSkills(List<Fighter> fighters)
        {
            for (int i = 0; i < fighters.Count; i++)
            {
                Console.Write((i + 1) + ".");
                fighters[i].ShowSkills();
            }
        }

        private void Fight(List<Fighter> fighters)
        {
            ShowStats(fighters);
            Fighter firstFighter = fighters[ChooseFighterIndex("первого", fighters.Count) - 1];
            Fighter secondFighter = fighters[ChooseFighterIndex("второго", fighters.Count) - 1];

            if (firstFighter != secondFighter)
            {
                Console.Clear();
                while (secondFighter.IsLive() && firstFighter.IsLive())
                {
                    secondFighter.MakeDamage(firstFighter.Damage);
                    firstFighter.MakeDamage(secondFighter.Damage);
                    firstFighter.ShowStats();
                    secondFighter.ShowStats();
                    Console.WriteLine("---------------------------------------------------------------------------------------------------");
                }

                WriteWinner(firstFighter, secondFighter);
            }
            else
            {
                Console.WriteLine("Бойцы не могут сражаться против себя!");
            }
        }

        private int ChooseFighterIndex(string text, int countOfFighters)
        {
            Console.WriteLine($"Выберите {text} бойца:");
            int fighterIndex = 0;
            bool isRepeating = true;

            while (isRepeating)
            {
                string userInput = Console.ReadLine();

                if (int.TryParse(userInput, out fighterIndex) == false || countOfFighters < fighterIndex)
                {
                    Console.WriteLine("Данные некорректны! Попробуйте снова:");
                }
                else
                {
                    isRepeating = false;
                }
            }

            return fighterIndex;
        }

        private void WriteWinner(Fighter firstFighter, Fighter secondFighter)
        {
            if (firstFighter.IsLive())
            {
                Console.WriteLine($"\nВыйграл: {firstFighter.Name}");
            }
            else
            {
                Console.WriteLine($"\nВыйграл: {secondFighter.Name}");
            }
        }

        private string GetRandomPhrase()
        {
            Random random = new Random();
            string[] phrases = new string[5]
            {
                "Добро пожаловать на арену. Кто же сегодня у нас сразится?",
                $"Делаем ставки! Делаем стаки! Только сегодня! Cтавка на {GetRandomWord()} 2х!",
                "Нужен «сувенирчик» на память?",
                "Что тебя сюда привело?",
                "Тебе что-то нужно от меня?",
            };
            string randomPhrase = phrases[random.Next(0, phrases.Length)];
            return randomPhrase;
        }
        
        private string GetRandomWord()
        {
            Random random = new Random();
            string[] words = new string[4]
            {
                "волшебника",
                "рыцаря",
                "разбойника",
                "охотника",
            };
            string word = words[random.Next(0, words.Length)];
            return word;
        }
    }

    class Wizard : Fighter
    {
        private int _mana = 10;

        public Wizard(string name, int health, int damage, int armor) : base(name, health, damage, armor) { }

        public override void MakeDamage(int damage)
        {
            CastSpell();
            base.MakeDamage(damage);
        }

        public override void ShowSkills(string text)
        {
            text = "Увеличивает урон и броню, пока есть мана.";
            base.ShowSkills(text);
        }

        private void CastSpell()
        {
            int manaCost = 2;
            int addDamage = 2;
            int addArmor = 3;

            if (_mana >= manaCost)
            {
                _mana -= manaCost;
                Damage *= addDamage;
                Armor += addArmor;
                Console.WriteLine($"{Name}: Урон и броня улучшены.");
            }
        }
    }

    class Knight : Fighter
    {
        public Knight(string name, int health, int damage, int armor) : base(name, health, damage, armor) { }

        public override void MakeDamage(int damage)
        {
            Heal();
            base.MakeDamage(damage);
        }

        public override void ShowSkills(string text)
        {
            text = "Может исцелить свое здоровье.";
            base.ShowSkills(text);
        }

        private void Heal()
        {
            int maxChance = 100;
            int chanceForHealing = 50;
            int amountOfHealing = Health / 10;
            int chance = Random.Next(maxChance);

            if (chance <= chanceForHealing)
            {
                Health += amountOfHealing;
                Console.WriteLine($"{Name}: исцелен на {amountOfHealing} хп.");
            }
        }
    }

    class Robber : Fighter
    {
        private int _countOfHits = 0;

        public Robber(string name, int health, int damage, int armor) : base(name, health, damage, armor) { }

        public override void MakeDamage(int damage)
        {
            IncreaseDamage();
            base.MakeDamage(damage);
        }

        public override void ShowSkills(string text)
        {
            text = "Увеличивает урон после серии ударов.";
            base.ShowSkills(text);
        }

        private void IncreaseDamage()
        {
            int addDamage = 2;

            if (_countOfHits >= 2)
            {
                _countOfHits = 0;
                Damage *= addDamage;
                Console.WriteLine($"{Name}: Урон улучшен.");
            }

            _countOfHits++;
        }
    }

    class Ninja : Fighter
    {
        public Ninja(string name, int health, int damage, int armor) : base(name, health, damage, armor)
        { }

        public override void MakeDamage(int damage)
        {
            if (IsMissHit() == false)
                base.MakeDamage(damage);
        }

        public override void ShowSkills(string text)
        {
            text = "Имеет шанс промаха.";
            base.ShowSkills(text);
        }

        private bool IsMissHit()
        {
            bool isMiss = false;
            int chanceForMiss = 25;
            int maxValue = 100;
            int chance = Random.Next(maxValue);

            if (chance <= chanceForMiss)
            {
                isMiss = true;
                Console.WriteLine($"{Name}: урон был пропущен.");
            }

            return isMiss;
        }
    }

    class Hunter : Fighter
    {
        public Hunter(string name, int health, int damage, int armor) : base(name, health, damage, armor) { }

        public override void MakeDamage(int damage)
        {
            HitExtra();
            base.MakeDamage(damage);
        }

        public override void ShowSkills(string text)
        {
            text = "Улучшает урон и броню за счет петов и стрел.";
            base.ShowSkills(text);
        }

        private void HitExtra()
        {
            int minValue = 1;
            int maxValue = 5;
            int damageByPoison = 4;
            int damageByTiger = 3;
            int damageByEagle = 3;
            int armorByTurtleSpirit = 5;
            int numberExtraHit = Random.Next(minValue, maxValue);

            switch (numberExtraHit)
            {
                case 1:
                    AddDamage(damageByPoison, "ядовитых стрел");
                    break;
                case 2:
                    AddDamage(damageByTiger, "призыва тигра");
                    break;
                case 3:
                    AddDamage(damageByEagle, "призыва орла");
                    break;
                case 4:
                    AddArmor(armorByTurtleSpirit, "призыва духа черепахи");
                    break;
            }
        }

        private void AddDamage(int damageBy, string text)
        {
            Damage += damageBy;
            Console.WriteLine($"{Name}: урон увеличен от {text}.");
        }

        private void AddArmor(int armorBy, string text)
        {
            Armor += armorBy;
            Console.WriteLine($"{Name}: броня увеличена от {text}.");
        }
    }

    class Fighter
    {
        protected int Health;
        protected int Armor;
        protected Random Random = new Random();

        public int Damage { get; protected set; }
        public string Name { get; protected set; }

        protected Fighter(string name, int health, int damage, int armor)
        {
            Name = name;
            Health = health;
            Damage = damage;
            Armor = armor;
        }

        public virtual void MakeDamage(int damage)
        {
            if (damage > Armor)
                Health -= damage - Armor;
        }

        public void ShowStats()
        {
            Console.WriteLine($"{DrawHealthBar()} {Name}. Урон: {Damage}. Броня: {Armor}.");
        }

        public virtual void ShowSkills(string text = "")
        {
            Console.WriteLine($"Имя: {Name}. {text}");
        }

        public bool IsLive()
        {
            bool isLive = true;

            if (Health <= 0)
            {
                isLive = false;
            }

            return isLive;
        }

        private string DrawHealthBar()
        {
            ConsoleColor defaultColor = Console.BackgroundColor;
            string bar = "";
            char symbol = '_';
            Console.Write('[');

            for (int i = 0; i < Health; i++)
            {
                bar += symbol;
            }

            Console.BackgroundColor = ConsoleColor.Red;
            Console.Write(bar);
            Console.BackgroundColor = defaultColor;
            bar = "";
            Console.Write(']');
            return bar;
        }
    }
}
