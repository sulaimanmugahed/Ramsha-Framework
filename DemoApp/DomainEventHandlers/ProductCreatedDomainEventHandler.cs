using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DemoApp.Entities;
using Ramsha.Domain;
using Ramsha.LocalMessaging.Abstractions;
using Ramsha.UnitOfWork.Abstractions;

namespace DemoApp.DomainEventHandlers;

public class ProductCreatedEventHandler(IRepository<Category, Guid> repository, IUnitOfWorkManager unitOfWorkManager) : LocalEventHandler<ProductCreatedEvent>
{
    public async override Task HandleAsync(ProductCreatedEvent message, CancellationToken cancellationToken = default)
    {
        var current = unitOfWorkManager.Current;
        var category = new Category(Guid.NewGuid(), "hhh");

        await repository.CreateAsync(category);
        throw new Exception("for test");
    }
}

