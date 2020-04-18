using IT.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using IT.Core.ViewModels;
using System.Data.SqlClient;

namespace IT.WebServices.MISC
{
    public class UpdateReason
    {
        UnitOfWork unitOfWork = new UnitOfWork();
        
        public UpdateReasonDescriptionViewModel Add(UpdateReasonDescriptionViewModel updateReasonModel)
        {
            var ReasonAdded = new UpdateReasonDescriptionViewModel();
            try
            {
                 ReasonAdded = unitOfWork.GetRepositoryInstance<UpdateReasonDescriptionViewModel>().ReadStoredProcedure("UpdateReasonDescriptionAdd @Id,@ReasonDescription,@Flag,@CreatedBy",
                 new SqlParameter("Id", System.Data.SqlDbType.Int) { Value = updateReasonModel.Id }
               , new SqlParameter("ReasonDescription", System.Data.SqlDbType.NVarChar) { Value = updateReasonModel.ReasonDescription ?? (object)DBNull.Value }
               , new SqlParameter("Flag", System.Data.SqlDbType.NVarChar) { Value = updateReasonModel.Flag ?? (object)DBNull.Value}
               , new SqlParameter("CreatedBy", System.Data.SqlDbType.Int) { Value = updateReasonModel.CreatedBy }
               
               ).FirstOrDefault();

                return ReasonAdded;
            }
            catch (Exception ex)
            {
                return ReasonAdded;
            }
        }
    }
}