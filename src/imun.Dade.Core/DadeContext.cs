using System;

namespace imun.Dade.Core
{
    public interface IDadeContext
    {
        void Commit();
        void Rollback();
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
            finally
            {
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
            finally
            {

            }
        }
    }
}
