using ProductService.Models;
using System;

namespace ProductService.SystemIntegration
{
    public interface IProductUpdatedEventPublisher
    {
        void Publish(Product product);
    }


}