namespace dicontainer;

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