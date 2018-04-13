﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductService.Services
{
    public interface IRepository<T> where T : class
    {
        IEnumerable<T> GetAll();
        bool Add(T t);
        T GetById(Guid id);
        bool Remove(Guid id);
    }
}
