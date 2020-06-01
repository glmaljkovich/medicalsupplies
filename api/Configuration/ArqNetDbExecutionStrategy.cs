
using System;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore;

public class ArqNetDbExecutionStrategy : ExecutionStrategy
    {
        public ArqNetDbExecutionStrategy(DbContext context) : base(context, ExecutionStrategy.DefaultMaxRetryCount, ExecutionStrategy.DefaultMaxDelay)
        {

        }

        public ArqNetDbExecutionStrategy(ExecutionStrategyDependencies dependencies) : base(dependencies, ExecutionStrategy.DefaultMaxRetryCount, ExecutionStrategy.DefaultMaxDelay)
        {

        }

        public ArqNetDbExecutionStrategy(DbContext context, int maxRetryCount, TimeSpan maxRetryDelay) :
            base(context, maxRetryCount, maxRetryDelay)
        {

        }

        protected override bool ShouldRetryOn(Exception exception)
        {
            return true;
        }
    }