using System;

namespace Exercise2
{
    public class Fish : Animal
    {
        public string WaterType { get; set; }

        public Fish(string name, int age, string habitat, string diet, int weight, string waterType)
            : base(name, age, habitat, diet, weight)
        {
            WaterType = waterType;
        }

        public override string GetInfo()
        {
            return base.GetInfo() + $", Тип воды: {WaterType}, Тип: Рыба";
        }
    }
}