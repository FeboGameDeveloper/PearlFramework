using System.Collections.Generic;
using UnityEngine;

namespace Pearl.Graph
{
    public class Vertex<ContentType>
    {
        #region Private Fields
        protected readonly Dictionary<Vertex<ContentType>, Arch> neighbors = new Dictionary<Vertex<ContentType>, Arch>();
        private List<Vertex<ContentType>> references = new List<Vertex<ContentType>>();
        #endregion

        #region Constructors
        public Vertex(ContentType content)
        {
            Content = content;
        }
        #endregion

        #region Properties
        public ContentType Content { get; set; }
        public IEnumerable<KeyValuePair<Vertex<ContentType>, Arch>> Neighbors { get { return neighbors; } }
        #endregion

        #region Public methods
        public void AddReference(in Vertex<ContentType> vertexSourceReference)
        {
            references.Add(vertexSourceReference);
        }

        public void RemoveReference(in Vertex<ContentType> vertexSourceReference)
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

        public Vertex<ContentType> GetLightestNeighbor()
        {
            Vertex<ContentType> lightestVertex = null;
            float minWeight = Mathf.Infinity;
            foreach (var result in neighbors.Keys)
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


        public void AddArch(in Vertex<ContentType> vertexDestination, in int weight)
        {
            Arch arch = new Arch(weight);
            neighbors.Update(vertexDestination, arch);
        }

        public void SetWeightArch(in Vertex<ContentType> vertexDestination, in int newWeight)
        {
            if (neighbors.TryGetValue(vertexDestination, out Arch arch))
            {
                arch.weight = newWeight;
            }
        }

        public int GetWeightArch(in Vertex<ContentType> vertexDestination)
        {
            return neighbors.TryGetValue(vertexDestination, out Arch arch) ? arch.weight : -1;
        }

        public void RemoveArch(in Vertex<ContentType> vertex)
        {
            neighbors.Remove(vertex);
        }

        public bool HasArch(in Vertex<ContentType> vertex)
        {
            return neighbors.ContainsKey(vertex);
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