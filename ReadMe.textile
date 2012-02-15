	<p>Neo4jD is a light weight .NET client to access Neo4j graph database. The library is still under development.</p>

	<p><strong>Create a Node</strong>
<pre><code>Node sony=new Node
sony.AddProperty("FirstName", "Sony").SetProperty("LastName", "Arouje").Create();
Node viji= new Node();
viji.AddProperty("FirstName", "Viji").AddProperty("LastName", "P").Create();
Relationship relationship= sony.CreateRelationshipTo(viji, "wife");</code></pre></p>

	<p><strong>Get a Node</strong></p>

<pre><code>Node sony=Node.Get(1);</code></pre>

	<p><strong>Add a New property to an existing node</strong></p>
	<ul>
		<li>The function AddProperty is used to set any property before saving and the properties will persist while Creating the entity.
		<li>The function SetProperty is used to set any new property to a persisted entity as shown below.
<pre><code>Node sony=Node.Get(1);
sony.SetProperty("Profession","Developer");
</code></pre>

	<p><strong>Out flowing Relationship</strong>
<pre><code>IList&lt;Node&gt; outNodes=sony.Out()</code></pre></p>

	<p><strong>Graph Traversal using REST Api</strong></p>

<pre><code>
Node node = Node.Get(19);
RestTraversal r = new RestTraversal();
r.Order(OrderType.breadth_first)
   .Filter ( new PropertyFilter().SetPropertyName("FirstName").Contains("marry") )
   .RelationShips(RelationshipDirection.out_direction, "wife")
   .RelationShips(RelationshipDirection.all_direction, "loves")
   .Uniqueness(UniquenessType.node_global)
   .MaxDepth(2);
IList&lt;Node&gt; nodes = node.Filter(r);

	<p>//you can see the generated query by
r.ToString()
</code></pre></p>

	<p>The Generated Json query will look some thing like below
<pre><code>
{</p>
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

	<p><strong>Creating Index and Searching in Index</strong>
An index can be created by using Index object as shown below.
<pre><code>Index fav = Index.Get("Favorite")</code></pre></p>

	<p>Once we have the index object we can easily add a node to it.
<pre><code>
Node sony=Node.Get(1);
fav.Add(sony,"FirstName","sony");
</code></pre></p>

	<p>We can also remove a node from the index as shown below
<pre><code>fav.Remove(sony);</code></pre></p>

	<p>We can also search the index as shown below
<pre><code>
Index fav = Index.Get("favaourites");
IndexQuery qry = new IndexQuery();
qry.GetKey("FirstName").StartsWith("so").OR().GetKey("FirstName").Equals("viji");
IList&lt;Node&gt; nodes= fav.Search(qry);
</code></pre></p>

	<p><strong>Note</strong><br />
You will get more insight of the current functionality by going through the NUnit tests.</p>


 