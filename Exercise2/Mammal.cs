using System;

namespace Exercise2
{
    public class Mammal : Animal
    {
        public bool HasFur { get; set; }

        public Mammal(string name, int age, string habitat, string diet, int weight, bool hasFur)
            : base(name, age, habitat, diet, weight)
        {
            HasFur = hasFur;
        }

        public override string GetInfo()
        {
            return base.GetInfo() + $", Шерсть: {(HasFur ? "есть" : "нет")}, Тип: Млекопитающее";
        }
    }
}