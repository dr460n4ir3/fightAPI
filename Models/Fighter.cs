using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace fightAPI.Models
{
    public class Fighter
    {
        public int Id { get; set; }
        public string Name { get; set; } = "Fighter";
        public int Health { get; set; } = 100;
        //public int Damage { get; set; } = 10;
        public int Strength { get; set; } = 10;
        public int Defense { get; set; } = 10;
        //public int Speed { get; set; } = 10;
        public int Intelligence { get; set; } = 10;
        public BattleClass Class { get; set; } = BattleClass.Tank;
        public User? User { get; set; }
    }
}