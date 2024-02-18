using System.Diagnostics;

namespace WaterSortingSolution;

internal class Solver
{
    public static long Steps = 1;

    private StepTreeNode? _finalStepNode;

    public List<Bottle> Bottles { get; }

    public StepTreeNode Root { get; }

    public Solver(params Bottle[] bottles)
    {
        Bottles = new(bottles);
        Root = new();
        Root.BottleStates.AddRange(Bottles);
    }

    public Solver(IEnumerable<Bottle> bottles)
    {
        Bottles = new(bottles);
        Root = new();
        Root.BottleStates.AddRange(Bottles);
    }

    public static List<Bottle> FindNextSteps(Bottle current, IEnumerable<Bottle> others)
    {
        if (current.IsEmpty)
        {
            return [];
        }

        var steps = new List<Bottle>();

        foreach (var other in others)
        {
            if (current.IsMonochrome && other.IsEmpty)
            {
                continue;
            }

            if (current.IsFull && other.Count == 2
                && current.Peek() == other.Peek()
                && current.Waters.Take(3).All(x => x == current.Peek()))
            {
                continue;
            }

            if (current.IsFull && other.Count == 3
                && current.Peek() == other.Peek()
                && current.Waters.Take(2).All(x => x == current.Peek()))
            {
                continue;
            }

            if (current.CanPourOutTo(other))
            {
                steps.Add(other);
            }
        }

        return steps;
    }

    public static bool IsLevelCompleted(StepTreeNode node)
    {
        return node.BottleStates.All(x => x.IsEmpty || x.IsCompleted);
    }

    public void DFT(StepTreeNode node)
    {
        if (IsLevelCompleted(node))
        {
            Console.WriteLine("过关了！");
            _finalStepNode = node;

            return;
        }

        Console.WriteLine($">>> #{Steps++}");

        foreach (var source in node.BottleStates)
        {
            var nextSteps = FindNextSteps(source, node.BottleStates.Except([source]));

            Console.WriteLine($"{source}: 找到 {nextSteps.Count} 种倒法。");

            foreach (var target in nextSteps)
            {
                node.AddNextStepNode(source, target);
            }

            Console.WriteLine(new string('=', 80));
        }

        foreach (var nextStep in node.NextStepNodes)
        {
            if (_finalStepNode is not null)
            {
                // 已经过关，直接返回，不再递归
                return;
            }

            DFT(nextStep);
        }
    }

    public void DFTNonRecursive(StepTreeNode rootNode)
    {
        var stack = new Stack<StepTreeNode>();
        stack.Push(rootNode);

        while (stack.Count > 0)
        {
            var currentNode = stack.Pop();

            if (IsLevelCompleted(currentNode))
            {
                Console.WriteLine("过关了！");
                _finalStepNode = currentNode;

                return;
            }

            Console.WriteLine($">>> #{Steps++}");

            foreach (var source in currentNode.BottleStates)
            {
                var nextSteps = FindNextSteps(source, currentNode.BottleStates.Except([source]));

                Console.WriteLine($"{source}: 找到 {nextSteps.Count} 种倒法。");

                foreach (var target in nextSteps)
                {
                    currentNode.AddNextStepNode(source, target);
                }

                Console.WriteLine(new string('=', 80));
            }

            foreach (var nextStep in currentNode.NextStepNodes.AsEnumerable().Reverse())
            {
                stack.Push(nextStep);
            }
        }
    }

    public bool IsLevelCompleted()
    {
        return Bottles.All(x => x.IsEmpty || x.IsCompleted);
    }

    public void Solve()
    {
        var stopwatch = Stopwatch.StartNew();

        //DFT(Root);
        DFTNonRecursive(Root);

        stopwatch.Stop();

        Console.WriteLine();

        if (_finalStepNode is null)
        {
            Console.WriteLine($"！！！居然没能过关 /(ㄒoㄒ)/~~ ({stopwatch.Elapsed.TotalSeconds:F3}s)");

            return;
        }

        Console.WriteLine($"过关了 ヾ(•ω•`)o ({stopwatch.Elapsed.TotalSeconds:F3}s)");
        Console.WriteLine("步骤：");

        var steps = GetSteps(_finalStepNode);

        int i = 0;

        foreach (var step in steps)
        {
            Console.WriteLine($"第 {i++} 步: ({step.Item1?.Id} -> {step.Item2?.Id}) {step.Item1} -> {step.Item2}");
        }

        Console.WriteLine("结束！");
    }

    private static Queue<(Bottle?, Bottle?)> GetSteps(StepTreeNode final)
    {
        var stack = new Stack<StepTreeNode>();
        stack.Push(final);
        var curr = final;

        while (curr.Parent is not null)
        {
            stack.Push(curr.Parent);
            curr = curr.Parent;
        }

        var steps = stack.Select(x => (x.Source, x.Target));

        var queue = new Queue<(Bottle?, Bottle?)>(steps);

        return queue;
    }
}
