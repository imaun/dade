using System;

namespace imun.Dade.Core
{
    public interface IDadeContext
    {
        void Commit();
        void Rollback();
        void Execute(string sql, object param = null);
        int ExecuteScalar32(string sql, object param = null);
        long ExecuteScalar64(string sql, object param = null);
    }

    public class DadeContext: IDadeContext
    {
        private readonly IUnitOfWorkFactory _unitOfWorkFactory;

        private IUnitOfWork _unitOfWork;

        private IUnitOfWork UnitOfWork => _unitOfWork ?? (_unitOfWork = _unitOfWorkFactory.Create());

        public DadeContext(IUnitOfWorkFactory unitOfWorkFactory)
        {
            _unitOfWorkFactory = unitOfWorkFactory;
        }

        public void Execute(string sql, object param = null) {
            UnitOfWork.Execute(sql, param);
        }

        public int ExecuteScalar32(string sql, object param = null) {
            return UnitOfWork.ExecuteScalar32(sql, param);
        }

        public long ExecuteScalar64(string sql, object param = null) {
            return UnitOfWork.ExecuteScalar64(sql, param);
        }


        public void Commit()
        {
            try
            {
                UnitOfWork.Commit();
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public void Rollback()
        {
            try
            {
                UnitOfWork.Rollback();
            }
            catch (Exception e)
            {
                throw;
            }
        }
    }
}
