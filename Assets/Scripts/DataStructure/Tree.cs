using UnityEngine;
using System.Collections.Generic;

public class Tree
{
    public Node root;
    public string name;
    public Tree(Dictionary<int, Prologue> root, int num)
    {
        this.root = new Node(root, num);
    }

    public Node AddChild(char direction, Dictionary<int, Prologue> parent, Dictionary<int, Prologue> data, int num)
    {
        Node tmp = new Node(data, num);

        Node findNode = this.FindData(root, parent);
        //Debug.Log(findNode.num + " " + num);
        if (findNode == null)
        {
            return null;
        }

        //트리에 같은 데이터가 있으면?
        Node duplicate = this.FindData(root, data);
        if(duplicate != null)
        {
            duplicate.isVisited = false;
            if (direction == 'L')
            {
                findNode.left = duplicate;
            }
            else if (direction == 'R')
            {
                findNode.right = duplicate;
            }
        }
        else
        {
            tmp.isVisited = false;
            if (direction == 'L')
            {
                findNode.left = tmp;
            }
            else if (direction == 'R')
            {
                findNode.right = tmp;
            }
        }
        return tmp;
    }

    //public Node FindData(Node discover, Dictionary<int, Prologue> data)
    //{
    //    if (discover.isVisited)
    //    {
    //        return null;
    //    }

    //    discover.isVisited = true;
    //    if (discover == null)
    //    {
    //        return null;
    //    }

    //    if (discover.dialogData == data)
    //    {
    //        return discover;
    //    }

    //    Node found = FindData(discover.left, data);
    //    if (found != null) return found;
    //    return FindData(discover.right, data);
    //}

    //BFS
    public Node FindData(Node discover, Dictionary<int, Prologue> data)
    {
        Node answer = null;
        Queue<Node> q = new Queue<Node>();
        List<Node> tmp = new List<Node>();
        q.Enqueue(discover);
        discover.isVisited = true;
        tmp.Add(discover);

        while(q.Count > 0)
        {
            Node t = q.Dequeue();
            if(t.dialogData == data)
            {
                answer = t;
                while (q.Count > 0)
                {
                    Node temp = q.Dequeue();
                    temp.isVisited = false;
                }
                break;
            }

            if (t.left != null && !t.left.isVisited)
            {
                t.left.isVisited = true;
                tmp.Add(t.left);
                q.Enqueue(t.left);
            }

            if (t.right != null && !t.right.isVisited)
            {
                t.right.isVisited = true;
                tmp.Add(t.right);
                q.Enqueue(t.right);
            }
        }

        foreach(var i in tmp)
        {
            i.isVisited = false;
        }

        return answer;
    }

    public Node FindData(Node discover, int data)
    {
        if (discover == null)
        {
            return null;
        }

        if (discover.num == data)
        {
            return discover;
        }

        Node found = FindData(discover.left, data);
        if (found != null) return found;

        if (found != null) return found;

        return FindData(discover.right, data);
    }
}