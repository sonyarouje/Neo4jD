using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Net.Graph.Neo4JD;
using Net.Graph.Neo4JD.EntityMapper;
namespace Test.Neo4jClient.EntityMappingTest
{
    public abstract class Person
    {
        [EntityId]
        public int Id { get; set; }
    }

    public class Employee : Person
    {
        public string Name { get; set; }
    }

    public class Manager : Person
    {
        public string OfficeName { get; set; }
    }

    public class MyClass
    {
        [EntityId]
        public int Id { get; set; }
        public virtual Person SelectedPerson { get; set; }
    }

    public class MyClassConfig : EntityConfiguration<MyClass>
    {
        public MyClassConfig()
        {
            this.RelatedTo<Person>(mc => mc.SelectedPerson);
        }
    }

    public class ManagerConfig : EntityConfiguration<Manager>
    {
        public ManagerConfig()
        {
            this.Leaf();
        }
    }

    [TestFixture]
    public class InhiritedEntityTest
    {
        [SetUp]
        public void Initialize()
        {
            GraphEnvironment.SetBaseUri("http://localhost:7474/");
            ModelBuilder.Add(new MyClassConfig());
            ModelBuilder.Add(new ManagerConfig());
            //Add the assemblies to the builder. We should pass Assembly path and Assmebly Name.
            ModelBuilder.AddAssembly((new System.Uri(System.Reflection.Assembly.GetExecutingAssembly().CodeBase)).AbsolutePath);
        }

        [TestFixtureTearDown]
        public void TearDown()
        {
            ModelBuilder.Clear();
        }

        [TestCase]
        public void SaveEntity()
        {
            MyClass claz = new MyClass();
            Manager manager = new Manager { OfficeName = "Neo4jD" };
            claz.SelectedPerson = manager;
            NodeMapper nodeMapper = new NodeMapper();
            MyClass savedClass = nodeMapper.Save<MyClass>(claz);
            Assert.AreNotEqual(0, savedClass.Id);
            Console.WriteLine(savedClass.Id.ToString());
        }

        [TestCase]
        public void Get_MyClassEntity_With_Manager()
        {
            NodeMapper nodeMapper = new NodeMapper();
            MyClass myClaz = nodeMapper.Get<MyClass>(29);
            Manager manager = (Manager)myClaz.SelectedPerson;
            Assert.IsNotNull(myClaz);
            Assert.IsNotNull(manager);
            Assert.AreEqual(29, myClaz.Id);
            Assert.AreEqual("Neo4jD", manager.OfficeName);
        }

        [TestCase]
        public void Get_MyClassEntity_And_Update_Manager()
        {
            NodeMapper nodeMapper = new NodeMapper();
            MyClass myClaz = nodeMapper.Get<MyClass>(29);
            Console.WriteLine("Get_MyClassEntity_And_Update_Manager: " +  myClaz.GetType().ToString());
            Manager manager = (Manager)myClaz.SelectedPerson;
            manager.OfficeName = "Sony Arouje";
            myClaz.SelectedPerson = manager;
            nodeMapper.Save<MyClass>(myClaz);
        }
    }
}
