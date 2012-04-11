using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Net.Graph.Neo4JD;
using Net.Graph.Neo4JD.Exceptions;
using Net.Graph.Neo4JD.EntityMapper;
using NUnit.Framework;
namespace Test.Neo4jClient
{
    public class Person
    {
        [EntityId]
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public Address Address { get; set; }
    }

    public class Address
    {
        [EntityId]
        public int AddressId { get; set; }
        public string Address1 { get; set; }
        public string City { get; set; }
    }
    //public class Person
    //{
    //    [EntityId]
    //    public int Id { get; set; }
    //    public string Name { get; set; }
    //    public string Address { get; set; }
    //    public int Pin { get; set; }
    //    public Address DestAddress { get; set; }
    //}


    [TestFixture]
    public class SimpleEntityMapperTest
    {
        [SetUp]
        public void Initialize()
        {
            GraphEnvironment.SetBaseUri("http://localhost:7474/");
        }

        [TestCase]
        public Person SaveTest()
        {
            Person person = new Person { FirstName = "Sony", LastName="Arouje" };
            NodeMapper mapper = new NodeMapper();
            person=mapper.Save<Person>(person);
            Console.WriteLine("Generated Id: " + person.Id.ToString());
            Assert.AreNotEqual(0, person.Id);
            return person;
        }

        [TestCase]
        public void GetEntityTest()
        {
            NodeMapper mapper = new NodeMapper();
            Person person = mapper.Get<Person>(7);
            Console.WriteLine("Generated Id: " + person.Id.ToString());
            Assert.AreNotEqual(0, person.Id);
            Assert.AreEqual("Sony", person.FirstName);
        }

        [TestCase]
        [ExpectedException(typeof(NodeNotFoundException))]
        public void DeleteEntityTest()
        {
            Person person = this.SaveTest();
            NodeMapper mapper = new NodeMapper();
            mapper.Delete<Person>(person);

            Person tryGetDeletedPerson = mapper.Get<Person>(person.Id);
            Assert.AreEqual(person.Id, tryGetDeletedPerson.Id);
        }

        [TestCase]
        public void CanReadNodeToDifferentEntity()
        {
            NodeMapper mapper = new NodeMapper();
            Address address = mapper.Get<Address>(7);
        }

        ///Methods are obselete now and removed.
        //[TestCase]
        //public void CanCreateRelationships()
        //{
        //    Person person = new Person { FirstName = "Sony Relation", LastName="Arouje" };
        //    person.Address = new Address();
        //    person.Address.Address1 = "Hosur";
        //    person.Address.City = "Bangalore";
        //    NodeMapper mapper = new NodeMapper();
        //    mapper.CreateRelationshipTo<Person, Address>(person,person.Address);
        //    Console.WriteLine(person.Id.ToString());
        //}

        //[TestCase]
        //public void CanGetRelatedNodes()
        //{
        //    NodeMapper mapper = new NodeMapper();
        //    Person person = mapper.Get<Person>(12);
        //    IList<Address> address = mapper.GetRelatedEntities<Person, Address>(person, typeof(Address));
        //    Assert.AreEqual(1, address.Count);
        //    Assert.AreEqual("Hosur", address[0].Address1);
        //}

        //[TestCase]
        //public void EntityConfigurationTest()
        //{
        //    EntityConfiguration config = new EntityConfiguration();
        //    config.Include<Person,Address>(p => p.Address);
        //}
    }
}
