using System;

namespace Exercise2
{
    public class Reptile : Animal
    {
        public bool IsVenomous { get; set; }

        public Reptile(string name, int age, string habitat, string diet, int weight, bool isVenomous)
            : base(name, age, habitat, diet, weight)
        {
            IsVenomous = isVenomous;
        }

        public override string GetInfo()
        {
            return base.GetInfo() + $", Ядовитость: {(IsVenomous ? "ядовитое" : "неядовитое")}, Тип: Пресмыкающееся";
        }
    }
}
