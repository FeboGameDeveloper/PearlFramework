using System.Collections.Generic;

namespace Pearl.Graph
{
    public class GraphConditional<ContentType, ConditionType>
    {
        #region Private Fields
        // The list of vertices in the graph
        protected Dictionary<ContentType, VertexConditional<ContentType, ConditionType>> vertices = new Dictionary<ContentType, VertexConditional<ContentType, ConditionType>>();
        protected readonly TypeGraph type;
        #endregion

        #region Properties
        public int Size { get { return vertices.Count; } }
        #endregion

        #region Constructors
        public GraphConditional(TypeGraph type)
        {
            this.type = type;
        }
        #endregion

        public VertexConditional<ContentType, ConditionType> GetVertex(in ContentType content)
        {
            return vertices.TryGetValue(content, out VertexConditional<ContentType, ConditionType> vertex) ? vertex : null;
        }

        #region Public Methods
        public void AddVertex(in ContentType content)
        {
            VertexConditional<ContentType, ConditionType> vertex = new VertexConditional<ContentType, ConditionType>(content);
            vertices.Add(content, vertex);
        }

        public void RemoveVertex(in ContentType content)
        {
            if (vertices.TryGetValue(content, out VertexConditional<ContentType, ConditionType> vertex))
            {
                vertex.RemoveAllReference();
                vertices.Remove(content);
            }
        }

        public void AddArch(in ContentType contentSource, in ContentType contentDestination)
        {
            AddArch(contentSource, contentDestination, 1, default);
        }

        public void AddArch(in ContentType contentSource, in ContentType contentDestination, in int weight)
        {
            AddArch(contentSource, contentDestination, weight, default);
        }

        public void AddArch(in ContentType contentSource, in ContentType contentDestination, in ConditionType condition)
        {
            AddArch(contentSource, contentDestination, default, condition);
        }

        public void AddArch(in ContentType contentSource, in ContentType contentDestination, in int weight, in ConditionType condition)
        {
            VertexConditional<ContentType, ConditionType> vertexSource = GetVertex(contentSource);
            VertexConditional<ContentType, ConditionType> vertexDestination = GetVertex(contentDestination);

            AddArchInternal(vertexSource, vertexDestination, weight, condition);

            if (type == TypeGraph.Undirected)
            {
                AddArchInternal(vertexDestination, vertexSource, weight, condition);
            }
        }

        public void SetWeightArch(in ContentType contentSource, in ContentType contentDestination, in int weight)
        {
            VertexConditional<ContentType, ConditionType> vertexSource = GetVertex(contentSource);
            VertexConditional<ContentType, ConditionType> vertexDestination = GetVertex(contentDestination);

            vertexSource.SetWeightArch(vertexDestination, weight);

            if (type == TypeGraph.Undirected)
            {
                vertexDestination.SetWeightArch(vertexSource, weight);
            }
        }


        private void AddArchInternal(in VertexConditional<ContentType, ConditionType> vertexSource, in VertexConditional<ContentType, ConditionType> vertexDestination, in int weight, in ConditionType condition)
        {
            vertexSource.AddArch(vertexDestination, weight, condition);
            vertexDestination.AddReference(vertexSource);
        }

        public void RemoveArch(in ContentType contentSource, in ContentType contentDestination)
        {
            VertexConditional<ContentType, ConditionType> vertexSource = GetVertex(contentSource);
            VertexConditional<ContentType, ConditionType> vertexDestination = GetVertex(contentDestination);

            RemoveArchInternal(vertexSource, vertexDestination);

            if (type == TypeGraph.Undirected)
            {
                RemoveArchInternal(vertexDestination, vertexSource);
            }

        }

        private void RemoveArchInternal(in VertexConditional<ContentType, ConditionType> vertexSource, in VertexConditional<ContentType, ConditionType> vertexDestination)
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
            VertexConditional<ContentType, ConditionType> vertexSource = GetVertex(contentSource);
            VertexConditional<ContentType, ConditionType> vertexDestination = GetVertex(contentDestination);

            return vertexSource != null ? vertexSource.HasArch(vertexDestination) : false;
        }
        #endregion
    }
}
