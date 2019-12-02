using System.Collections.Generic;
using UnityEngine;

// 状态转移类，记录了从一个节点到另一个节点进行状态转移船上的人员情况
public class move
{
    public int priest_num;
    public int devil_num;
    public move(int pnum, int dnum)
    {
        this.priest_num = pnum;
        this.devil_num = dnum;
    }
}


// 状态图节点
public class stateNode
{
    public int priest_num;  // 右岸牧师数量
    public int devil_num;   // 右岸魔鬼数量
    public bool boat;       // 船是否停靠在右岸
    public List<stateNode> nextStates; // 相邻节点

    public stateNode start; // 在搜索时用到，用于记录最短路径对应的起始步

    public stateNode(int pnum, int dnum, bool boat)
    {
        this.priest_num = pnum;
        this.devil_num = dnum;
        this.boat = boat;
        nextStates = new List<stateNode>();
    }

    // 加入相邻节点
    public bool addNextState(stateNode nextState)
    {
        foreach (stateNode node in nextStates)
        {
            if (nextState == node)
            {
                return false;
            }
        }

        nextStates.Add(nextState);
        return true;
    }

    // 获得当前可选择的走法
    public move[] getAvailableMove()
    {
        move[] res = new move[nextStates.Count];
        for(int i = 0; i < nextStates.Count; i++)
        {
            res[i] = new move(Mathf.Abs(nextStates[i].priest_num - this.priest_num), Mathf.Abs(nextStates[i].devil_num - this.devil_num));
        }
        return res;
    }

    public void move(move mv)
    {
        if (boat)
        {
            this.priest_num -= mv.priest_num;
            this.devil_num -= mv.devil_num;
        }
        else
        {
            this.priest_num += mv.priest_num;
            this.devil_num += mv.devil_num;
        }
        this.boat = !this.boat;
    }

    // 船行驶向左岸，相当于右岸少人了
    public static stateNode operator - (stateNode from, move mv)
    {
        stateNode to = new stateNode(from.priest_num - mv.priest_num, from.devil_num - mv.devil_num, false);
        return to;
    }

    // 船行驶回右岸，相当于右岸多人了
    public static stateNode operator +(stateNode from, move mv)
    {
        stateNode to = new stateNode(from.priest_num + mv.priest_num, from.devil_num + mv.devil_num, true);
        return to;
    }
    public static bool operator ==(stateNode state1, stateNode state2)
    {
        return state1.priest_num == state2.priest_num && state1.devil_num == state2.devil_num && state1.boat == state2.boat;
    }

    public static bool operator !=(stateNode state1, stateNode state2)
    {
        return state1.priest_num != state2.priest_num || state1.devil_num != state2.devil_num || state1.boat != state2.boat;
    }
}

// 状态图类
public class stateGraph
{
    public List<stateNode> nodes;
    public move[] prosibleMove =
    {
        new move(0, 1),
        new move(1, 0),
        new move(1, 1),
        new move(2, 0),
        new move(0, 2),
    };
    public stateNode startState;
    public stateNode endState;

    public stateGraph()
    {
        nodes = new List<stateNode>();
        startState = new stateNode(3, 3, true);
        endState = new stateNode(0, 0, false);
        generateGraph();
    }

    // 图生成函数
    private void generateGraph()
    {
        // 首先加入初始状态节点
        nodes.Add(startState);

        for(int i = 0; i < nodes.Count; i++)
        {
            stateNode currentState = nodes[i];
            foreach(move mv in prosibleMove)
            {
                stateNode nextState;
                if (currentState.boat)
                {
                    nextState = currentState - mv;
                }
                else
                {
                    nextState = currentState + mv;
                }

                if (isLegalState(nextState))
                {
                    nextState = addNewStateToGraph(nextState);
                    currentState.addNextState(nextState);
                    nextState.addNextState(currentState);
                }

            }
        }
    }

    // 将节点加入图中
    private stateNode addNewStateToGraph(stateNode newState)
    {
        // 先判断列表里当前有没有
        foreach (stateNode state in nodes)
        {
            if (state == newState)
            {
                return state;
            }
        }
    
        nodes.Add(newState);
        return newState;
    }

    private stateNode getGraphNode(stateNode newState)
    {
        foreach (stateNode state in nodes)
        {
            if (state == newState)
            {
                return state;
            }
        }
        return null;
    }

    // 验证状态是否合法
    private bool isLegalState(stateNode state)
    {
        return (state.priest_num >= state.devil_num || state.priest_num == 0)
            && ((3 - state.priest_num) >= (3 - state.devil_num) || (3 - state.priest_num) == 0)
            && (state.priest_num <= 3 && state.devil_num <= 3)
            && ((3 - state.priest_num) <= 3 && (3 - state.devil_num) <= 3);
    }

    public move getNextMove(stateNode currentState)
    {
        // 找到图中真正的节点
        currentState = getGraphNode(currentState);
        stateNode next = getNextState(currentState);
        return new move(Mathf.Abs(currentState.priest_num - next.priest_num), Mathf.Abs(currentState.devil_num - next.devil_num));
    }

    // 找到最短路径中当前状态的下一步
    private stateNode getNextState(stateNode from)
    { 
        List<stateNode> alreadSearchState = new List<stateNode>();
        alreadSearchState.Add(from);

        foreach (stateNode state in from.nextStates)
        {
            // 判断是不是终点
            if(state == endState)
            {
                return state; 
            }
            else
            {
                state.start = state;
                alreadSearchState.Add(state);
            }
        }

        for(int i = 1; i < alreadSearchState.Count; i++)
        {
            foreach(stateNode state in alreadSearchState[i].nextStates)
            {
                // 判断是不是终点
                if (state == endState)
                {
                    state.start = alreadSearchState[i].start;
                    return state.start;
                }
                else
                {
                    bool flag = false;
                    // 检查是否已经搜索过
                    foreach (stateNode searchedState in alreadSearchState)
                    {
                        if (state == searchedState)
                        {
                            flag = true;
                            break;
                        }
                    }
                    if (!flag)
                    {
                        state.start = alreadSearchState[i].start;
                        alreadSearchState.Add(state);
                    }
                }
            }
        }
        return null;
    }
}
