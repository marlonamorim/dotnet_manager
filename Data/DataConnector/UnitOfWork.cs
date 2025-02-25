﻿using Data.Interfaces.DataConnector;

namespace Data.DataConnector
{
    public class UnitOfWork : IUnitOfWork
    {

        public IDbConnector dbConnector { get; }

        public UnitOfWork(IDbConnector dbConnector)
        {
            this.dbConnector = dbConnector;
        }

        public void BeginTransaction()
        {
            dbConnector.BeginTransaction(System.Data.IsolationLevel.ReadUncommitted);
        }

        public void CommitTransaction()
        {
            if (dbConnector.dbConnection.State == System.Data.ConnectionState.Open)
            {
                dbConnector.dbTransaction.Commit();
            }
        }

        public void RollbackTransaction()
        {
            if (dbConnector.dbConnection.State == System.Data.ConnectionState.Open)
            {
                dbConnector.dbTransaction.Rollback();
            }
        }
    }
}