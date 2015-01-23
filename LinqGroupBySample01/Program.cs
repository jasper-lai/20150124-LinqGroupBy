using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


// ============================================
// http://msdn.microsoft.com/zh-tw/library/bb534304(v=vs.110).aspx
// ============================================

namespace LinqGroupBySample01
{
    class Pet
    {
        public string Name { get; set; }
        public int Age { get; set; }
    }

    class Pet2
    {
        public string Name { get; set; }
        public double Age { get; set; }
    }


    class Program
    {
        // Uses method-based query syntax.
        public static void GroupByEx1()
        {
            // Create a list of pets.
            List<Pet> pets =
                new List<Pet>{ new Pet { Name="Barley", Age=8 },
                                   new Pet { Name="Boots", Age=4 },
                                   new Pet { Name="Whiskers", Age=1 },
                                   new Pet { Name="Daisy", Age=4 } };

            // Group the pets using Age as the key value 
            // and selecting only the pet's Name for each value.

            // Query Syntax
            // -----------------
            //IEnumerable<IGrouping<int, string>> query1 =
            var query1 =
                from pet in pets
                group pet.Name by pet.Age;

            //Method Syntax
            // -----------------
            IEnumerable<IGrouping<int, string>> query2 =
                pets.GroupBy(pet => pet.Age, pet => pet.Name);

            // Iterate over each IGrouping in the collection.
            Console.WriteLine("===== Query Syntax =====");
            foreach (IGrouping<int, string> petGroup in query1)
            {
                // Print the key value of the IGrouping.
                Console.WriteLine(petGroup.Key);
                // Iterate over each value in the 
                // IGrouping and print the value.
                foreach (string name in petGroup)
                    Console.WriteLine("  {0}", name);
            }

            Console.WriteLine("===== Method Syntax =====");
            foreach (IGrouping<int, string> petGroup in query2)
            {
                // Print the key value of the IGrouping.
                Console.WriteLine(petGroup.Key);
                // Iterate over each value in the 
                // IGrouping and print the value.
                foreach (string name in petGroup)
                    Console.WriteLine("  {0}", name);
            }

            //// for LINQPad
            //// ------------------
            //query1.Dump();
            //query2.Dump();

            /*
             This code produces the following output:
            ===== Query Syntax =====
            8
              Barley
            4
              Boots
              Daisy
            1
              Whiskers
            ===== Method Syntax =====
            8
              Barley
            4
              Boots
              Daisy
            1
              Whiskers
            */
        }


        public static void GroupByEx2()
        {
            // Create a list of pets.
            List<Pet> pets =
                new List<Pet>{ new Pet { Name="Barley", Age=8 },
                                   new Pet { Name="Boots", Age=4 },
                                   new Pet { Name="Whiskers", Age=1 },
                                   new Pet { Name="Daisy", Age=4 } };

            // Group the pets using Age as the key value 
            // and selecting only the pet's Name for each value.

            // Query Syntax
            // -----------------
            //IEnumerable<IGrouping<int, Pet>> query1 =
            var query1 =
                from pet in pets
                group pet by pet.Age;

            //Method Syntax
            // -----------------
            IEnumerable<IGrouping<int, Pet>> query2 =
                pets.GroupBy(pet => pet.Age, pet => pet);

            // Iterate over each IGrouping in the collection.
            Console.WriteLine("===== Query Syntax =====");
            foreach (IGrouping<int, Pet> petGroup in query1)
            {
                // Print the key value of the IGrouping.
                Console.WriteLine(petGroup.Key);
                // Iterate over each value in the 
                // IGrouping and print the value.
                foreach (var item in petGroup)
                    Console.WriteLine("  {0} : {1}", item.Name, item.Age ) ;
            }

            Console.WriteLine("===== Method Syntax =====");
            foreach (IGrouping<int, Pet> petGroup in query2)
            {
                // Print the key value of the IGrouping.
                Console.WriteLine(petGroup.Key);
                // Iterate over each value in the 
                // IGrouping and print the value.
                foreach (var item in petGroup)
                    Console.WriteLine("  {0} : {1}", item.Name, item.Age);
            }

            //// for LINQPad
            //// ------------------
            //query1.Dump();
            //query2.Dump();

            /*
             This code produces the following output:
            ===== Query Syntax =====
            8
              Barley : 8
            4
              Boots : 4
              Daisy : 4
            1
              Whiskers : 1
            ===== Method Syntax =====
            8
              Barley : 8
            4
              Boots : 4
              Daisy : 4
            1
              Whiskers : 1
            */
        }

        public static void GroupByEx3()
        {
            // Create a list of pets.
            List<Pet2> petsList =
                new List<Pet2>{ new Pet2 { Name="Barley", Age=8.3 },
                                   new Pet2 { Name="Boots", Age=4.9 },
                                   new Pet2 { Name="Whiskers", Age=1.5 },
                                   new Pet2 { Name="Daisy", Age=4.3 } };

            // Group Pet objects by the Math.Floor of their age.
            // Then project an anonymous type from each group
            // that consists of the key, the count of the group's
            // elements, and the minimum and maximum age in the group.

            // Query Syntax
            // -----------------
            var query1 = from p in petsList
                        group p by Math.Floor(p.Age)
                            into pets
                            select new
                            {
                                Key = pets.Key,
                                Count = pets.Count(),
                                Min = pets.Min(pet => pet.Age),
                                Max = pets.Max(pet => pet.Age),
                                DetailList = new List<Pet2>(pets)
                            };

            // Method Syntax
            // -----------------
            var query2 = petsList.GroupBy(
                pet => Math.Floor(pet.Age),
                (age, pets) => new
                {
                    Key = age,
                    Count = pets.Count(),
                    Min = pets.Min(pet => pet.Age),
                    Max = pets.Max(pet => pet.Age),
                    DetailList = new List<Pet2>(pets)
                });


            // Iterate over each anonymous type.
            Console.WriteLine("");
            Console.WriteLine("===== Query Syntax =====");
            foreach (var result in query1)
            {
                Console.WriteLine("Age group: " + result.Key);
                Console.WriteLine("Number of pets in this age group: " + result.Count);
                Console.WriteLine("Minimum age: " + result.Min);
                Console.WriteLine("Maximum age: " + result.Max);
                foreach (var item in result.DetailList)
                {
                    Console.WriteLine("    Members: " + item.Name + item.Age);
                }
            }

            Console.WriteLine("===== Method Syntax =====");
            foreach (var result in query2)
            {
                Console.WriteLine("Age group: " + result.Key);
                Console.WriteLine("Number of pets in this age group: " + result.Count);
                Console.WriteLine("Minimum age: " + result.Min);
                Console.WriteLine("Maximum age: " + result.Max);
                foreach (var item in result.DetailList)
                {
                    Console.WriteLine("    Members: " + item.Name + item.Age);
                }
            }

            //// for LINQPad
            //// ------------------
            //query1.Dump();
            //query2.Dump();

            /*  This code produces the following output:

            ===== Query Syntax =====
            Age group: 8
            Number of pets in this age group: 1
            Minimum age: 8.3
            Maximum age: 8.3
                Members: Barley8.3
            Age group: 4
            Number of pets in this age group: 2
            Minimum age: 4.3
            Maximum age: 4.9
                Members: Boots4.9
                Members: Daisy4.3
            Age group: 1
            Number of pets in this age group: 1
            Minimum age: 1.5
            Maximum age: 1.5
                Members: Whiskers1.5
            ===== Method Syntax =====
            Age group: 8
            Number of pets in this age group: 1
            Minimum age: 8.3
            Maximum age: 8.3
                Members: Barley8.3
            Age group: 4
            Number of pets in this age group: 2
            Minimum age: 4.3
            Maximum age: 4.9
                Members: Boots4.9
                Members: Daisy4.3
            Age group: 1
            Number of pets in this age group: 1
            Minimum age: 1.5
            Maximum age: 1.5
                Members: Whiskers1.5
            */
        }


        public static void GroupByEx4()
        {
            // Create a list of pets.
            List<Pet2> petsList =
                new List<Pet2>{ new Pet2 { Name="Barley", Age=8.3 },
                                   new Pet2 { Name="Boots", Age=4.9 },
                                   new Pet2 { Name="Whiskers", Age=1.5 },
                                   new Pet2 { Name="Daisy", Age=4.3 } };

            // Group Pet.Age values by the Math.Floor of the age.
            // Then project an anonymous type from each group
            // that consists of the key, the count of the group's
            // elements, and the minimum and maximum age in the group.
            var query = petsList.GroupBy(
                pet => Math.Floor(pet.Age),
                pet => pet.Age,
                (baseAge, ages) => new
                {
                    Key = baseAge,
                    Count = ages.Count(),
                    Min = ages.Min(),
                    Max = ages.Max()
                });

            // Iterate over each anonymous type.
            foreach (var result in query)
            {
                Console.WriteLine("\nAge group: " + result.Key);
                Console.WriteLine("Number of pets in this age group: " + result.Count);
                Console.WriteLine("Minimum age: " + result.Min);
                Console.WriteLine("Maximum age: " + result.Max);
            }

            /*  This code produces the following output:

                Age group: 8
                Number of pets in this age group: 1
                Minimum age: 8.3
                Maximum age: 8.3

                Age group: 4
                Number of pets in this age group: 2
                Minimum age: 4.3
                Maximum age: 4.9

                Age group: 1
                Number of pets in this age group: 1
                Minimum age: 1.5
                Maximum age: 1.5
            */
        }


        static void Main(string[] args)
        {
            GroupByEx1();
            GroupByEx2();
            GroupByEx3();
            GroupByEx4();

            Console.ReadLine();

        }
    }

}
