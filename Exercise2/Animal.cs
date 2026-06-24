using System;

namespace Exercise2
{
    public class Animal
    {
        public string Name { get; set; }
        public int Age { get; set; }
        public string Habitat { get; set; }
        public string Diet { get; set; }
        public int Weight { get; set; }

        public Animal(string name, int age, string habitat, string diet, int weight)
        {
            Name = name;
            Age = age;
            Habitat = habitat;
            Diet = diet;
            Weight = weight;
        }

        public virtual string GetInfo()
        {
            return $"Кличка: {Name}, Возраст: {Age}, Среда: {Habitat}, Питание: {Diet}, Вес: {Weight} кг";
        }
    }
}
