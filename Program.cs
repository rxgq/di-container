class UserService : IUserService
{
    public void GetUsers()
    {
        Console.Write("Got users..");
    }
}

interface IUserService {
    void GetUsers();
}

class Program {
    static void Main() {
        var container = new DIContainer();

        container.Register<IUserService, UserService>();
        var userService = container.Resolve<IUserService>();

        userService.GetUsers();
    }
}

sealed class DIContainer {
    private readonly Dictionary<Type, Type> Registrations = [];

    public void Register<IInterface, TType>() where TType : class, IInterface {
        Registrations[typeof(IInterface)] = typeof(TType);
    }

    public T Resolve<T>() where T : class {
        var type = Registrations[typeof(T)];
        var service = Activator.CreateInstance(type);

        if (service is not T TService) 
            throw new Exception($"Unable to convert {type} to {typeof(T)}");

        return TService;
    }
}

