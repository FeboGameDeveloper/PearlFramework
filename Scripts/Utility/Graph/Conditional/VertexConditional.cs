using System.Collections.Generic;
using UnityEngine;

namespace Pearl.Graph
{
    public class VertexConditional<ContentType, ConditionType>
    {

        #region Private Fields
        protected readonly Dictionary<VertexConditional<ContentType, ConditionType>, ArchConditional<ConditionType>> neighbors = new Dictionary<VertexConditional<ContentType, ConditionType>, ArchConditional<ConditionType>>();
        private List<VertexConditional<ContentType, ConditionType>> references = new List<VertexConditional<ContentType, ConditionType>>();
        #endregion

        #region Constructors
        public VertexConditional(ContentType content)
        {
            Content = content;
        }
        #endregion

        #region Properties
        public ContentType Content { get; set; }
        public IEnumerable<KeyValuePair<VertexConditional<ContentType, ConditionType>, ArchConditional<ConditionType>>> Neighbors { get { return neighbors; } }
        #endregion

        #region Public methods
        public void AddReference(in VertexConditional<ContentType, ConditionType> vertexSourceReference)
        {
            references.Add(vertexSourceReference);
        }

        public void RemoveReference(in VertexConditional<ContentType, ConditionType> vertexSourceReference)
        {
            references.Remove(vertexSourceReference);
        }

        public void RemoveAllReference()
        {
            foreach (var vertexReference in references)
            {
                vertexReference.RemoveArch(this);
            }
            references.Clear();
        }

        public VertexConditional<ContentType, ConditionType> GetLightestNeighbor()
        {
            return GetLightestVertex(neighbors.Keys);
        }

        private VertexConditional<ContentType, ConditionType> GetLightestVertex(in IEnumerable<VertexConditional<ContentType, ConditionType>> vertices)
        {
            VertexConditional<ContentType, ConditionType> lightestVertex = null;
            float minWeight = Mathf.Infinity;
            foreach (var result in vertices)
            {
                int weight = GetWeightArch(result);
                if (weight < minWeight)
                {
                    minWeight = weight;
                    lightestVertex = result;
                }
            }

            return lightestVertex;
        }

        public void AddArch(in VertexConditional<ContentType, ConditionType> vertexDestination)
        {
            AddArch(vertexDestination, 1, default);
        }

        public void AddArch(in VertexConditional<ContentType, ConditionType> vertexDestination, in ConditionType condition)
        {
            AddArch(vertexDestination, 1, condition);
        }

        public void AddArch(in VertexConditional<ContentType, ConditionType> vertexDestination, in int weight)
        {
            AddArch(vertexDestination, weight, default);
        }

        public void AddArch(in VertexConditional<ContentType, ConditionType> vertexDestination, in int weight, in ConditionType condition)
        {
            ArchConditional<ConditionType> arch = new ArchConditional<ConditionType>(weight, condition);
            neighbors.Update(vertexDestination, arch);
        }

        public void SetWeightArch(in VertexConditional<ContentType, ConditionType> vertexDestination, in int newWeight)
        {
            if (neighbors.TryGetValue(vertexDestination, out ArchConditional<ConditionType> arch))
            {
                arch.weight = newWeight;
            }
        }

        public int GetWeightArch(in VertexConditional<ContentType, ConditionType> vertexDestination)
        {
            return neighbors.TryGetValue(vertexDestination, out ArchConditional<ConditionType> arch) ? arch.weight : -1;
        }

        public void SetConditionArch(in VertexConditional<ContentType, ConditionType> vertexDestination, in ConditionType newCondition)
        {
            if (vertexDestination != null && newCondition != null)
            {
                if (neighbors.TryGetValue(vertexDestination, out ArchConditional<ConditionType> arch))
                {
                    arch.SetNewCondition(newCondition);
                }
            }
        }

        public void RemoveArch(in VertexConditional<ContentType, ConditionType> vertex)
        {
            neighbors.Remove(vertex);
        }

        public bool HasArch(in VertexConditional<ContentType, ConditionType> vertex)
        {
            return neighbors.ContainsKey(vertex);
        }

        public VertexConditional<ContentType, ConditionType> GetNeighborIdeal(in ConditionType currentCondition)
        {
            var results = GetNeighborsWithRespectCondition(in currentCondition);
            return GetLightestVertex(results);
        }

        private List<VertexConditional<ContentType, ConditionType>> GetNeighborsWithRespectCondition(in ConditionType currentCondition)
        {
            List<VertexConditional<ContentType, ConditionType>> result = new List<VertexConditional<ContentType, ConditionType>>();

            foreach (var neighbor in neighbors)
            {
                if (neighbor.Value.IsRespectCondition(currentCondition))
                {
                    result.Add(neighbor.Key);
                }
            }
            return result;
        }

        public override bool Equals(object obj)
        {
            return obj is Vertex<ContentType> vertex && Content.Equals(vertex.Content);
        }

        public override int GetHashCode()
        {
            return -1584136870 + EqualityComparer<ContentType>.Default.GetHashCode(Content);
        }
        #endregion
    }
}
