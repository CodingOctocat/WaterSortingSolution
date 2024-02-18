namespace WaterSortingSolution;

internal class StepTreeNode
{
    public List<Bottle> BottleStates { get; } = [];

    public List<StepTreeNode> NextStepNodes { get; } = [];

    public StepTreeNode? Parent { get; private set; }

    public Bottle? Source { get; }

    public Bottle? Target { get; }

    public StepTreeNode()
    { }

    public StepTreeNode(Bottle source, Bottle target)
    {
        Source = source;
        Target = target;
    }

    public void AddNextStepNode(Bottle source, Bottle target)
    {
        var shortcut = BottleStates.Select(x => x.DeepClone()).ToList();
        var sourceCloned = shortcut.Find(x => x.Id == source.Id)!;
        var targetCloned = shortcut.Find(x => x.Id == target.Id)!;
        sourceCloned.PourOutTo(targetCloned);

        var nextStepNode = new StepTreeNode(source, target) {
            Parent = this
        };

        nextStepNode.BottleStates.AddRange(shortcut);

        NextStepNodes.Add(nextStepNode);
    }
}
