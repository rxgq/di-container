namespace dicontainer;

class Program {
    static void Main() {
        var container = new DIContainer();

        container.Register<IServiceA, ServiceA>();
        container.Register<IServiceB, ServiceB>();
        container.Register<IServiceC, ServiceC>();

        var serviceA = container.Resolve<IServiceA>();
        serviceA.PerformTask();

        var serviceB = container.Resolve<IServiceB>();
        serviceB.PerformAnotherTask();

        var serviceC = container.Resolve<IServiceC>();
        serviceC.PerformComplexTask();
    }
}

public interface IContainer {
    void Register<IInterface, TType>() where TType : class, IInterface;
    T Resolve<T>() where T : class;
}

public sealed class DIContainer : IContainer {
    private readonly Dictionary<Type, Type> _registrations = new();

    public void Register<IInterface, TType>() where TType : class, IInterface {
        _registrations[typeof(IInterface)] = typeof(TType);
    }

    public T Resolve<T>() where T : class {
        return (T)Resolve(typeof(T));
    }

    public object Resolve(Type type) {
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

public interface IServiceA {
    void PerformTask();
}

public class ServiceA : IServiceA {
    public void PerformTask() {
        Console.WriteLine("ServiceA: Performing Task");
    }
}

public interface IServiceB {
    void PerformAnotherTask();
}

public class ServiceB : IServiceB {
    private readonly IServiceA _serviceA;

    public ServiceB(IServiceA serviceA) {
        _serviceA = serviceA;
    }

    public void PerformAnotherTask() {
        Console.WriteLine("ServiceB: Performing Another Task with the help of ServiceA");
        _serviceA.PerformTask();
    }
}

public interface IServiceC {
    void PerformComplexTask();
}

public class ServiceC : IServiceC {
    private readonly IServiceA _serviceA;
    private readonly IServiceB _serviceB;

    public ServiceC(IServiceA serviceA, IServiceB serviceB) {
        _serviceA = serviceA;
        _serviceB = serviceB;
    }

    public void PerformComplexTask() {
        Console.WriteLine("ServiceC: Performing Complex Task involving multiple dependencies");
        _serviceA.PerformTask();
        _serviceB.PerformAnotherTask();
    }
}