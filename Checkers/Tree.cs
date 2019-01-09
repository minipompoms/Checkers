using System;
using System.Collections.Generic;

namespace Checkers
{
    class Tree<T>
    {
       
            private List<Tree<T>> children;
            private T value;

            public Tree(T value)
            {
                this.value = value;
                this.children = new List<Tree<T>>();
            }

            public Tree<T> AddChild(T value)
            {
                Tree<T> child = new Tree<T>(value);
                child.Parent = this;
                this.children.Add(child);
                return child;
            }

            public void Traverse(Action<T> visitor)
            {
                this.traverse(visitor);
            }

            protected void traverse(Action<T> visitor)
            {
                visitor(this.value);
                foreach (Tree<T> child in this.children)
                    child.traverse(visitor);
            }

            public Tree<T> Parent
            {
                get;
                private set;
            }

            public List<Tree<T>> Children
            {
                get { return this.children; }
            }

            public T Value
            {
                get { return this.value; }
            }

            public float Score;

            public List<float> AllScores
            {
                get
                {
                    var scores = new List<float>();
                    if (Children == null || Children.Count == 0)
                    {
                        scores.Add(Score);
                    }
                    else
                    {
                        foreach (var node in Children)
                        {
                            scores.AddRange(node.AllScores);
                        }
                    }
                
                    return scores;
                }
            }


    }
}