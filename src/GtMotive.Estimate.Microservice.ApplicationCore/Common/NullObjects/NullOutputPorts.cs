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

public class NullErrorOutputPort : IErrorOutputPort
{
    public void NotFoundHandle(string message)
    {
    }

    public void BadRequestHandle(string message)
    {
    }

    public void GeneralErrorHandle(string message)
    {
    }
}