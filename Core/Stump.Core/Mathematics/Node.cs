using System;
using System.Collections.Generic;
using System.Linq;

namespace Stump.Core.Mathematics
{
    public class Node<T>
    {
        private readonly List<Node<T>> m_children;
        private Node<T> m_parent;

        /// <summary>
        ///   Initializes a new instance of the <see cref = "Node&lt;T&gt;" /> class.
        /// </summary>
        public Node()
        {
            m_children = new List<Node<T>>();
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref = "Node&lt;T&gt;" /> class.
        /// </summary>
        /// <param name = "value">The value.</param>
        public Node(T value)
        {
            m_children = new List<Node<T>>();
            Value = value;
        }

        /// <summary>
        ///   Gets the parent.
        /// </summary>
        /// <value>The parent.</value>
        public Node<T> Parent
        {
            get { return m_parent; }
        }

        /// <summary>
        ///   Gets the children.
        /// </summary>
        /// <value>The children.</value>
        public IEnumerable<Node<T>> Nodes
        {
            get { return m_children; }
        }

        /// <summary>
        ///   Gets the first.
        /// </summary>
        /// <value>The first.</value>
        public Node<T> FirstNode
        {
            get { return m_children.First(); }
        }

        /// <summary>
        ///   Gets the last.
        /// </summary>
        /// <value>The last.</value>
        public Node<T> LastNode
        {
            get { return m_children.Last(); }
        }

        /// <summary>
        ///   Gets the prev node.
        /// </summary>
        /// <value>The prev node.</value>
        public Node<T> PrevNode
        {
            get { return m_parent.m_children[m_parent.m_children.IndexOf(this) - 1]; }
        }

        /// <summary>
        ///   Gets the next node.
        /// </summary>
        /// <value>The next node.</value>
        public Node<T> NextNode
        {
            get { return m_parent.m_children[m_parent.m_children.IndexOf(this) + 1]; }
        }

        /// <summary>
        ///   Gets the depth.
        /// </summary>
        /// <value>The depth.</value>
        public int Depth
        {
            get
            {
                int depth = 0;
                Node<T> currentNode = this;

                while (currentNode != null)
                {
                    currentNode = currentNode.m_parent;
                    depth++;
                }
                return depth - 1;
            }
        }

        /// <summary>
        ///   Gets the branch.
        /// </summary>
        /// <value>The branch.</value>
        public int Branches
        {
            get { return m_children.Count; }
        }

        /// <summary>
        ///   Gets or sets the value.
        /// </summary>
        /// <value>The value.</value>
        public T Value
        {
            get;
            set;
        }

        /// <summary>
        ///   Adds the specified child.
        /// </summary>
        /// <param name = "child">The child.</param>
        public void Add(Node<T> child)
        {
            m_children.Add(child);
            child.SetParent(this);
        }

        /// <summary>
        ///   Adds the specifieds nodes.
        /// </summary>
        /// <param name = "values">The nodes.</param>
        public void Add(params Node<T>[] nodes)
        {
            foreach (var node in nodes)
            {
                m_children.Add(node);
                node.SetParent(this);
            }
        }

        /// <summary>
        ///   Adds the specified value.
        /// </summary>
        /// <param name = "value">The value.</param>
        public Node<T> Add(T value)
        {
            var newNode = new Node<T>(value);
            m_children.Add(newNode);
            newNode.SetParent(this);
            return newNode;
        }

        /// <summary>
        ///   Adds the specifieds values.
        /// </summary>
        /// <param name = "values">The values.</param>
        /// <returns></returns>
        public List<Node<T>> Add(params T[] values)
        {
            var nodes = new List<Node<T>>(values.Length);
            foreach (T value in values)
            {
                var newNode = new Node<T>(value);
                m_children.Add(newNode);
                newNode.SetParent(this);
                nodes.Add(newNode);
            }
            return nodes;
        }

        /// <summary>
        ///   Removes the specified child.
        /// </summary>
        /// <param name = "child">The child.</param>
        public void Remove(Node<T> child)
        {
            child.SetParent(null);
            m_children.Remove(child);
        }

        /// <summary>
        ///   Removes at.
        /// </summary>
        /// <param name = "index">The index.</param>
        public void RemoveAt(int index)
        {
            Node<T> node = m_children[index];

            if (node != null)
            {
                node.SetParent(null);
                m_children.RemoveAt(index);
            }
        }

        /// <summary>
        ///   Removes all.
        /// </summary>
        /// <param name = "index">The index.</param>
        public void RemoveAll(int index)
        {
            for (int i = 0; i < m_children.Count; i++)
            {
                m_children[i].SetParent(null);
                m_children.RemoveAt(i);
            }
        }

        /// <summary>
        ///   Sets the parent.
        /// </summary>
        /// <param name = "parent">The parent.</param>
        private void SetParent(Node<T> parent)
        {
            m_parent = parent;
        }

        /// <summary>
        ///   Gets the node count.
        /// </summary>
        /// <param name = "includeSubNodes">if set to <c>true</c> [include sub nodes].</param>
        /// <returns></returns>
        public int GetNodeCount(bool includeSubNodes)
        {
            if (!includeSubNodes)
                return m_children.Count;

            int count = m_children.Count;

            foreach (var node in m_children)
                count += node.GetNodeCount(true);

            return count;
        }

        /// <summary>
        ///   Gets the leaf nodes.
        /// </summary>
        /// <returns></returns>
        public List<Node<T>> GetLeafNodes()
        {
            var nodes = new List<Node<T>>();

            foreach (var node in m_children)
            {
                if (node.GetNodeCount(false) == 0)
                    nodes.Add(node);
                else
                    nodes.AddRange(node.GetLeafNodes());
            }
            return nodes;
        }

        /// <summary>
        ///   Gets the leaf roads.
        /// </summary>
        /// <returns></returns>
        public List<List<Node<T>>> GetLeafRoads(bool includeThis, bool includeLeaf)
        {
            var roads = new List<List<Node<T>>>();
            List<Node<T>> leafNodes = GetLeafNodes();

            foreach (var leafNode in leafNodes)
            {
                var road = new List<Node<T>>();
                Node<T> currentNode;

                if (includeLeaf)
                    currentNode = leafNode;
                else
                    currentNode = leafNode.m_parent;

                if (includeThis)
                    while ((currentNode != null && currentNode.m_parent != null) || currentNode == this)
                    {
                        road.Add(currentNode);
                        currentNode = currentNode.m_parent;
                    }
                else
                    while (currentNode != null && currentNode.m_parent != null && currentNode != this)
                    {
                        road.Add(currentNode);
                        currentNode = currentNode.m_parent;
                    }
                road.Reverse();
                roads.Add(road);
            }
            return roads;
        }

        /// <summary>
        /// Get the best road with the select and cast it thanks to converter
        /// </summary>
        /// <typeparam name="U">Type</typeparam>
        /// <param name="selector">Selector</param>
        /// <param name="converter">Convertor</param>
        /// <returns></returns>
        public List<U> GetCastedBestRoad<U>(Func<Node<T>, decimal?> selector, Converter<Node<T>, U> converter)
        {
            List<List<Node<T>>> roads = GetLeafRoads(false, true);
            List<Node<T>> bestRoad = roads.OrderBy(road => road.Sum(selector)).Last();
            return bestRoad.ConvertAll(converter);
        }

        /// <summary>
        /// Get the best road corresponding with the sum of the selector
        /// </summary>
        /// <param name="selector">The selector.</param>
        /// <returns></returns>
        public List<Node<T>> GetBestRoad(Func<Node<T>, decimal?> selector)
        {
            List<List<Node<T>>> roads = GetLeafRoads(false, true);

            List<Node<T>> bestRoad = roads.OrderBy(road => road.Sum(selector)).Last();
            return bestRoad;
        }

        /// <summary>
        /// Get the best child with the selector
        /// </summary>
        /// <param name="selector">The selector.</param>
        /// <returns></returns>
        public Node<T> GetBestChild(Func<Node<T>, decimal?> selector)
        {
            return m_children.OrderBy(selector).Last();
        }

        /// <summary>
        ///   Gets the root road.
        /// </summary>
        /// <param name = "includeThis">if set to <c>true</c> [include this].</param>
        /// <param name = "includeRoot">if set to <c>true</c> [include root].</param>
        /// <returns></returns>
        public List<Node<T>> GetRootRoad(bool includeThis, bool includeRoot)
        {
            var road = new List<Node<T>>();
            Node<T> currentNode;

            if (includeThis)
                currentNode = this;
            else
                currentNode = m_parent;

            if (includeRoot)
                while (currentNode != null)
                {
                    road.Add(currentNode);
                    currentNode = currentNode.m_parent;
                }
            else
                while (currentNode != null && currentNode.m_parent != null)
                {
                    road.Add(currentNode);
                    currentNode = currentNode.m_parent;
                }

            return road;
        }
    }
}