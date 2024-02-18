using System.Collections.Immutable;

namespace WaterSortingSolution;

internal class Bottle
{
    private readonly Stack<Water> _bottle;

    public int Capacity { get; } = 4;

    public int Count => _bottle.Count;

    public int FreeCapacity => Capacity - _bottle.Count;

    public bool HasFreeCapacity => FreeCapacity > 0;

    public string Id { get; }

    public bool IsCompleted => IsFull && _bottle.All(x => x == _bottle.Peek());

    public bool IsEmpty => _bottle.Count == 0;

    public bool IsFull => _bottle.Count == Capacity;

    public bool IsMonochrome => !IsEmpty && _bottle.All(x => x == _bottle.Peek());

    public ImmutableArray<Water> Waters => [.. _bottle];

    public Bottle(string id, int capacity = 4)
    {
        Id = id;
        Capacity = capacity;
        _bottle = new(capacity);
    }

    public Bottle(string id, params Water[] waters)
    {
        Id = id;
        _bottle = new(waters);
    }

    public Bottle(string id, IEnumerable<Water> waters)
    {
        Id = id;
        _bottle = new(waters.Reverse());
    }

    public Bottle(string id, int capacity = 4, params Water[] waters)
    {
        Id = id;
        Capacity = capacity;
        _bottle = new(waters.Reverse());
    }

    public Bottle(string id, int capacity, IEnumerable<Water> waters)
    {
        Id = id;
        Capacity = capacity;
        _bottle = new(waters.Reverse());
    }

    public bool CanPourOutTo(Bottle other)
    {
        if (IsEmpty)
        {
            return false;
        }

        if (other.IsFull)
        {
            return false;
        }

        bool result = false;

        while (other.HasFreeCapacity)
        {
            if (!other.IsEmpty)
            {
                if (!TopColorEquals(other))
                {
                    return false;
                }
            }

            return true;
        }

        return result;
    }

    public Bottle DeepClone()
    {
        var cloned = new Bottle(Id, Capacity, _bottle);

        return cloned;
    }

    public Water Peek()
    {
        return _bottle.Peek();
    }

    public Water PourOut()
    {
        return _bottle.Pop();
    }

    public bool PourOutTo(Bottle other)
    {
        try
        {
            Console.Write($"{this} --> {other}: ");

            if (IsEmpty)
            {
                Console.WriteLine($"没有颜色可以倒出。");

                return false;
            }

            if (other.IsFull)
            {
                Console.WriteLine($"瓶子已经满了。");

                return false;
            }

            bool result = false;

            while (other.HasFreeCapacity)
            {
                if (!other.IsEmpty)
                {
                    if (!TopColorEquals(other))
                    {
                        if (!result)
                        {
                            Console.Write("颜色相同才能倒入。");
                        }

                        Console.WriteLine();

                        return result;
                    }
                }

                var w = PourOut();
                Console.Write($"{w}~");

                other.PourIn(w);
                result = true;
            }

            Console.WriteLine();

            return result;
        }
        finally
        {
            Console.WriteLine($"{this}  &  {other}");
            Console.WriteLine(new string('-', 64));
        }
    }

    public bool TopColorEquals(Bottle other)
    {
        if (TryPeek(out var top))
        {
            if (other.TryPeek(out var otherTop))
            {
                return top == otherTop;
            }
        }

        return false;
    }

    public override string ToString()
    {
        string colors = String.Join(" | ", _bottle);
        colors = String.IsNullOrEmpty(colors) ? "空" : colors;

        return $"[{Id}={colors})";
    }

    public bool TryPeek(out Water result)
    {
        return _bottle.TryPeek(out result);
    }

    private void PourIn(Water water)
    {
        if (IsFull)
        {
            throw new OverflowException($"{this} 瓶子已经满了.");
        }

        _bottle.Push(water);
    }
}
