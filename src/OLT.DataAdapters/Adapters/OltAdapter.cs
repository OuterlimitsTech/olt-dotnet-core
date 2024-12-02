using System.Collections.Concurrent;

namespace OLT.Core;


public abstract class OltAdapter<TObj1, TObj2> : IOltAdapter<TObj1, TObj2>
{
    public virtual string Name => OltAdapterExtensions.BuildAdapterName<TObj1, TObj2>();

    public abstract void Map(TObj1 source, TObj2 destination);
    public abstract void Map(TObj2 source, TObj1 destination);

    public virtual IEnumerable<TObj2> Map(IEnumerable<TObj1> sourceItems)
    {
        ConcurrentBag<TObj2> result = new ConcurrentBag<TObj2>();
        Parallel.ForEach(sourceItems, obj1 =>
        {
            var instance = Activator.CreateInstance(typeof(TObj2));
            if (instance == null) throw new OltAdapterException($"Unable to create instance of {typeof(TObj2).Name}");
            var obj2 = (TObj2)instance;
            Map(obj1, obj2);
            result.Add(obj2);
        });
        return result.ToList();
    }

    public virtual IEnumerable<TObj1> Map(IEnumerable<TObj2> sourceItems)
    {
        ConcurrentBag<TObj1> result = new ConcurrentBag<TObj1>();
        Parallel.ForEach(sourceItems, obj2 =>
        {
            var instance = Activator.CreateInstance(typeof(TObj1));
            if (instance == null) throw new OltAdapterException($"Unable to create instance of {typeof(TObj1).Name}");
            var obj1 = (TObj1)instance;
            Map(obj2, obj1);
            result.Add(obj1);
        });
        return result.ToList();
    }        
}