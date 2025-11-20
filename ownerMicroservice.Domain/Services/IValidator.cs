using ownerMicroservice.Domain.Services;

namespace ownerMicroservice.Domain.Services;

public interface IValidator<T>
{
    Result Validate(T entity);
}