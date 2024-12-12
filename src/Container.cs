namespace dicontainer;

public interface IContainer {
    public void Register<IInterface, TType>() where TType : class, IInterface;
    public T Resolve<T>() where T : class;
}

public sealed class DIContainer : IContainer {
    private readonly Dictionary<Type, Type> _registrations = [];

    public void Register<IInterface, TType>() where TType : class, IInterface {
        _registrations[typeof(IInterface)] = typeof(TType);
    }

    public T Resolve<T>() where T : class {
        return (T)Resolve(typeof(T));
    }

    private object Resolve(Type type) {
        if (type.IsAbstract || type.IsInterface) {
            if (!_registrations.ContainsKey(type))
                throw new Exception($"Type {type} has not been registered");

            type = _registrations[type];
        }

        return ResolveWithDependencies(type);
    }

    private object ResolveWithDependencies(Type type) {
        var constructor = type.GetConstructors().FirstOrDefault()
            ?? throw new Exception($"No public constructor for {type}");

        var parameters = constructor.GetParameters();
        var dependencies = parameters.Select(p => Resolve(p.ParameterType)).ToArray();

        return Activator.CreateInstance(type, dependencies)!;
    }
}