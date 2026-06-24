using System;

namespace Exercise2
{
    public class Amphibian : Animal
    {
        public string SkinMoisture { get; set; }

        public Amphibian(string name, int age, string habitat, string diet, int weight, string skinMoisture)
            : base(name, age, habitat, diet, weight)
        {
            SkinMoisture = skinMoisture;
        }

        public override string GetInfo()
        {
            return base.GetInfo() + $", Влажность кожи: {SkinMoisture}, Тип: Земноводное";
        }
    }
}
