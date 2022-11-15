using System.Collections.Generic;

namespace Pearl.Graph
{
    public class Graph<ContentType>
    {
        #region private Fields
        // The list of vertices in the graph
        protected Dictionary<ContentType, Vertex<ContentType>> vertices = new Dictionary<ContentType, Vertex<ContentType>>();
        protected readonly TypeGraph type;
        #endregion

        #region Properties
        public int Size { get { return vertices.Count; } }
        #endregion

        #region Constructors
        public Graph(TypeGraph type)
        {
            this.type = type;
        }
        #endregion

        public Vertex<ContentType> GetVertex(in ContentType content)
        {
            return vertices.TryGetValue(content, out Vertex<ContentType> vertex) ? vertex : null;
        }

        #region Public Methods
        public void AddVertex(in ContentType content)
        {
            Vertex<ContentType> vertex = new Vertex<ContentType>(content);
            vertices.Add(content, vertex);
        }

        public void RemoveVertex(in ContentType content)
        {
            if (vertices.TryGetValue(content, out Vertex<ContentType> vertex))
            {
                vertex.RemoveAllReference();
                vertices.Remove(content);
            }
        }

        public void AddArch(in ContentType contentSource, in ContentType contentDestination)
        {
            AddArch(contentSource, contentDestination, 1);
        }

        public void AddArch(in ContentType contentSource, in ContentType contentDestination, in int weight)
        {
            Vertex<ContentType> vertexSource = GetVertex(contentSource);
            Vertex<ContentType> vertexDestination = GetVertex(contentDestination);

            AddArchInternal(vertexSource, vertexDestination, weight);

            if (type == TypeGraph.Undirected)
            {
                AddArchInternal(vertexDestination, vertexSource, weight);
            }
        }

        public void SetWeightArch(in ContentType contentSource, in ContentType contentDestination, in int weight)
        {
            Vertex<ContentType> vertexSource = GetVertex(contentSource);
            Vertex<ContentType> vertexDestination = GetVertex(contentDestination);

            vertexSource.SetWeightArch(vertexDestination, weight);

            if (type == TypeGraph.Undirected)
            {
                vertexDestination.SetWeightArch(vertexSource, weight);
            }
        }


        private void AddArchInternal(in Vertex<ContentType> vertexSource, in Vertex<ContentType> vertexDestination, in int weight)
        {
            vertexSource.AddArch(vertexDestination, weight);
            vertexDestination.AddReference(vertexSource);
        }

        public void RemoveArch(in ContentType contentSource, in ContentType contentDestination)
        {
            Vertex<ContentType> vertexSource = GetVertex(contentSource);
            Vertex<ContentType> vertexDestination = GetVertex(contentDestination);

            RemoveArchInternal(vertexSource, vertexDestination);

            if (type == TypeGraph.Undirected)
            {
                RemoveArchInternal(vertexDestination, vertexSource);
            }

        }

        private void RemoveArchInternal(in Vertex<ContentType> vertexSource, in Vertex<ContentType> vertexDestination)
        {
            vertexSource.RemoveArch(vertexDestination);
            vertexDestination.RemoveReference(vertexSource);
        }

        public bool HasVertex(in ContentType vertex)
        {
            return vertices.ContainsKey(vertex);
        }

        public bool HasHarc(in ContentType contentSource, in ContentType contentDestination)
        {
            Vertex<ContentType> vertexSource = GetVertex(contentSource);
            Vertex<ContentType> vertexDestination = GetVertex(contentDestination);


            return vertexSource != null ? vertexSource.HasArch(vertexDestination) : false;
        }
        #endregion
    }
}
