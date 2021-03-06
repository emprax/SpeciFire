﻿using System;
using SpeciFire.Rules;

namespace SpeciFire.DependencyInjection.Rules
{
    internal interface IRuleFactory<TInput, TContext> where TContext : class
    {
        Rule<TInput, TContext> Create(IServiceProvider provider);
    }
}
