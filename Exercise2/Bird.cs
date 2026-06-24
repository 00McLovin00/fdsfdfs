using System;

namespace Exercise2
{
    public class Bird : Animal
    {
        public float WingSpan { get; set; }

        public Bird(string name, int age, string habitat, string diet, int weight, float wingSpan)
            : base(name, age, habitat, diet, weight)
        {
            WingSpan = wingSpan;
        }

        public override string GetInfo()
        {
            return base.GetInfo() + $", Размах крыльев: {WingSpan} м, Тип: Птица";
        }
    }
}
