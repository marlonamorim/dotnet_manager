﻿namespace Data.Interfaces.DataConnector
{
    public interface IUnitOfWork
    {
        IDbConnector dbConnector { get; }

        void BeginTransaction();
        void CommitTransaction();
        void RollbackTransaction();
    }
}
