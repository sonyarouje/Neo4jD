Neo4jD is a light weight .NET client to access Neo4j graph database. The library is still under development.

*Create a Node*
<pre><code>Node sony=new Node
sony.AddProperty("FirstName", "Sony").SetProperty("LastName", "Arouje").Create();
Node viji= new Node();
viji.AddProperty("FirstName", "Viji").AddProperty("LastName", "P").Create();
Relationship relationship= sony.CreateRelationshipTo(viji, "wife");</code></pre>

*Get a Node*

<pre><code>Node sony=Node.Get(1);</code></pre>

*Add a New property to an existing node*
* The function AddProperty is used to set any property before saving and the properties will persist while Creating the entity.
* The function SetProperty is used to set any new property to a persisted entity as shown below.
<pre><code>Node sony=Node.Get(1);
sony.SetProperty("Profession","Developer");
</code></pre>

*Out flowing Relationship*
<pre><code>IList<Node> outNodes=sony.Out()</code></pre>

*Graph Traversal using REST Api*

<pre><code>
Node node = Node.Get(19);
RestTraversal r = new RestTraversal();
r.Order(OrderType.breadth_first)
   .Filter ( new PropertyFilter().SetPropertyName("FirstName").Contains("marry") )
   .RelationShips(RelationshipDirection.out_direction, "wife")
   .RelationShips(RelationshipDirection.all_direction, "loves")
   .Uniqueness(UniquenessType.node_global)
   .MaxDepth(2);
IList<Node> nodes = node.Filter(r);
     
//you can see the generated query by
r.ToString()
</code></pre>

The Generated Json query will look some thing like below
<pre><code>
{
  "order": "breadth_first",
  "return_filter": {
    "body": "position.endNode().getProperty('FirstName').toLowerCase().contains('sony')",
    "language": "javascript"
  },
  "relationships": [
    {
      "direction": "out",
      "type": "wife"
    },
    {
      "direction": "all",
      "type": "loves"
    }
  ],
  "uniqueness": "node_global",
  "max_depth": 2
}
</code></pre>

*Creating Index and Searching in Index*
An index can be created by using Index object as shown below.
<pre><code>Index fav = Index.Get("Favorite")</code></pre>

Once we have the index object we can easily add a node to it.
<pre><code>
Node sony=Node.Get(1);
fav.Add(sony,"FirstName","sony");
</code></pre>

We can also remove a node from the index as shown below
<pre><code>fav.Remove(sony);</code></pre>

We can also search the index as shown below
<pre><code>
Index fav = Index.Get("favaourites");
IndexQuery qry = new IndexQuery();
qry.GetKey("FirstName").StartsWith("so").OR().GetKey("FirstName").Equals("viji");
IList<Node> nodes= fav.Search(qry);
</code></pre>

*Note*
You will get more insight of the current functionality by going through the NUnit tests.