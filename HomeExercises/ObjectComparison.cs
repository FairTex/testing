﻿using System;
using FluentAssertions;
using FluentAssertions.Equivalency;
using NUnit.Framework;

namespace HomeExercises
{
	public class ObjectComparison
	{
		[Test]
		[Description("Проверка текущего царя")]
		[Category("ToRefactor")]
		public void CheckCurrentTsar()
		{
			var actualTsar = TsarRegistry.GetCurrentTsar();

			var expectedTsar = new Person("Ivan IV The Terrible", 54, 170, 70,
				new Person("Vasili III of Russia", 28, 170, 60, null));

            // Перепишите код на использование Fluent Assertions.

		    actualTsar.Name.Should().BeEquivalentTo(expectedTsar.Name);
		    actualTsar.Age.Should().Be(expectedTsar.Age);
		    actualTsar.Height.Should().Be(expectedTsar.Height);
		    actualTsar.Weight.Should().Be(expectedTsar.Weight);

		    actualTsar.Parent.Name.Should().BeEquivalentTo(expectedTsar.Parent.Name);
		    actualTsar.Parent.Age.Should().Be(expectedTsar.Parent.Age);
		    actualTsar.Parent.Height.Should().Be(expectedTsar.Parent.Height);
		    actualTsar.Parent.Weight.Should().Be(expectedTsar.Parent.Weight);
		    actualTsar.Parent.Parent.ShouldBeEquivalentTo(expectedTsar.Parent.Parent);


            Assert.AreEqual(actualTsar.Name, expectedTsar.Name);
            Assert.AreEqual(actualTsar.Age, expectedTsar.Age);
            Assert.AreEqual(actualTsar.Height, expectedTsar.Height);
            Assert.AreEqual(actualTsar.Weight, expectedTsar.Weight);

            Assert.AreEqual(expectedTsar.Parent.Name, actualTsar.Parent.Name);
            Assert.AreEqual(expectedTsar.Parent.Age, actualTsar.Parent.Age);
            Assert.AreEqual(expectedTsar.Parent.Height, actualTsar.Parent.Height);
            Assert.AreEqual(expectedTsar.Parent.Parent, actualTsar.Parent.Parent);
        }

		[Test]
		[Description("Альтернативное решение. Какие у него недостатки?")]
		public void CheckCurrentTsar_WithCustomEquality()
		{
			var actualTsar = TsarRegistry.GetCurrentTsar();
			var expectedTsar = new Person("Ivan IV The Terrible", 54, 170, 70,
			    new Person("Vasili III of Russia", 28, 170, 60, null));
            
            // Какие недостатки у такого подхода? 
            // Assert.True(AreEqual(actualTsar, expectedTsar));
            // При добавлении новых полей, нужно постоянно поддерживать тест

            actualTsar.ShouldBeEquivalentTo(expectedTsar, 
                o => o.Excluding(p => p.SelectedMemberInfo.Name == nameof(Person.Id) 
                    && p.SelectedMemberInfo.DeclaringType == typeof(Person)));
		}

		private bool AreEqual(Person actual, Person expected)
		{
			if (actual == expected) return true;
			if (actual == null || expected == null) return false;
			return
			    actual.Name == expected.Name
			    && actual.Age == expected.Age
			    && actual.Height == expected.Height
			    && actual.Weight == expected.Weight
			    && AreEqual(actual.Parent, expected.Parent);
		}
	}

	public class TsarRegistry
	{
		public static Person GetCurrentTsar()
		{
			return new Person(
				"Ivan IV The Terrible", 54, 170, 70,
				new Person("Vasili III of Russia", 28, 170, 60, null));
		}
	}

	public class Person
	{
		public static int IdCounter = 0;
		public int Age, Height, Weight;
		public string Name;
		public Person Parent;
		public int Id;

		public Person(string name, int age, int height, int weight, Person parent)
		{
			Id = IdCounter++;
			Name = name;
			Age = age;
			Height = height;
			Weight = weight;
			Parent = parent;
		}
	}
}
