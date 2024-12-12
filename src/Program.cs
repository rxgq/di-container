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