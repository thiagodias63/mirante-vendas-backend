using Vendas.Domain.Repositories;

namespace Vendas.Tests.TestDoubles;

internal sealed class FakeUnitOfWork : IUnitOfWork
{
    public int ChamadasSaveChanges { get; private set; }

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        ChamadasSaveChanges++;
        return Task.FromResult(1);
    }
}
