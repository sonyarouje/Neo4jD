using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Net.Graph.Neo4JD;
using Net.Graph.Neo4JD.Exceptions;
using Net.Graph.Neo4JD.EntityMapper;
using NUnit.Framework;
using NUnit.Core;
namespace Test.Neo4jClient
{
    public class Person
    {
        [EntityId]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public int Pin { get; set; }

        public Address DestAddress { get; set; }
    }

    public class Address
    {
        [EntityId]
        public int AddressId { get; set; }
        public string Address1 { get; set; }
        public string City { get; set; }
    }
    [TestFixture]
    public class EntityMapperTest
    {
        [SetUp]
        public void Initialize()
        {
            GraphEnvironment.SetBaseUri("http://localhost:7474/");
        }

        [TestCase]
        public Person SaveTest()
        {
            Person person = new Person { Name = "sony", Address = "Bangalore", Pin=560068 };
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
            Assert.AreEqual(560068, person.Pin);
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

        [TestCase]
        public void CanCreateRelationships()
        {
            Person person = new Person { Name = "sony Relation", Address = "Bangalore", Pin = 560068 };
            person.DestAddress = new Address();
            person.DestAddress.Address1 = "Hosur";
            person.DestAddress.City = "Bangalore";
            NodeMapper mapper = new NodeMapper();
            mapper.CreateRelationshipTo<Person, Address>(person,person.DestAddress);
            Console.WriteLine(person.Id.ToString());
        }

        [TestCase]
        public void CanGetRelatedNodes()
        {
            NodeMapper mapper = new NodeMapper();
            Person person = mapper.Get<Person>(12);
            IList<Address> address = mapper.GetRelatedEntities<Person, Address>(person, typeof(Address));
            Assert.AreEqual(1, address.Count);
            Assert.AreEqual("Hosur", address[0].Address1);
        }
    }
}
