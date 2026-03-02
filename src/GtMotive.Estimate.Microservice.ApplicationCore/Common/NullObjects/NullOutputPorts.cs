using GtMotive.Estimate.Microservice.ApplicationCore.UseCases;

namespace GtMotive.Estimate.Microservice.ApplicationCore.Common.NullObjects;

public class NullOutputPortStandard<T> : IOutputPortStandard<T>
    where T : IUseCaseOutput
{
    public void StandardHandle(T response)
    {
        // No-op
    }
}

public class NullOutputPortNotFound : IOutputPortNotFound
{
    public void NotFoundHandle(string message)
    {
        // No-op
    }
}