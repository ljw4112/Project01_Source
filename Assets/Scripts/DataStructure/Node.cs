using System.Collections.Generic;
public class Node
{
    public int num;
    public bool isVisited;
    public Node left;
    public Node right;
    public Dictionary<int, Prologue> dialogData;
    public Node(Dictionary<int, Prologue> dialog, int num, Node left = null, Node right = null)
    {
        this.num = num;
        this.isVisited = false;
        this.left = left;
        this.right = right;
        this.dialogData = dialog;
    }
}