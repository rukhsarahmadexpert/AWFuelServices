using IT.Core.ViewModels;
using IT.Core.ViewModels.Common;
using IT.Repository;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Script.Serialization;

namespace IT.WebServices.Controllers
{
    public class CountryController : ApiController
    {
        UnitOfWork unitOfWork = new UnitOfWork();
        ServiceResponseModel userRepsonse = new ServiceResponseModel();

        readonly string contentType = "application/json";

        [HttpPost]
        public HttpResponseMessage GetAll()
        {
            List<CountryViewModel> countryViewModels = new List<CountryViewModel>();         
            try
            {
                var countryList = unitOfWork.GetRepositoryInstance<CountryViewModel>().ReadStoredProcedure("CountryAll"
                        ).ToList();
                userRepsonse.Success((new JavaScriptSerializer()).Serialize(countryList));

                return Request.CreateResponse(HttpStatusCode.Accepted, userRepsonse, contentType);
            }
            catch (Exception)
            {
                return Request.CreateResponse(HttpStatusCode.Ambiguous, userRepsonse, contentType);
            }
        }
        
        [HttpPost]
        public HttpResponseMessage StateAll()
        {
            
            try
            {
                var StateList = unitOfWork.GetRepositoryInstance<StateViewModel>().ReadStoredProcedure("StateAll"
                       ).ToList();
                userRepsonse.Success((new JavaScriptSerializer()).Serialize(StateList));

                return Request.CreateResponse(HttpStatusCode.Accepted, userRepsonse, contentType);
            }
            catch (Exception)
            {
                return Request.CreateResponse(HttpStatusCode.Ambiguous, userRepsonse, contentType);
            }
        }

        [HttpPost]
        public HttpResponseMessage CityAll()
        {           
            try
            {
                var CityList = unitOfWork.GetRepositoryInstance<CityViewModel>().ReadStoredProcedure("CityAll"
                          ).ToList();
                userRepsonse.Success((new JavaScriptSerializer()).Serialize(CityList));

                return Request.CreateResponse(HttpStatusCode.Accepted, userRepsonse, contentType);
            }
            catch (Exception)
            {
                return Request.CreateResponse(HttpStatusCode.Ambiguous, userRepsonse, contentType);
            }
        }
        
        [HttpPost]
        public HttpResponseMessage All()
        {
            List<CountryViewModel> countryViewModels = new List<CountryViewModel>();
            List<CityViewModel> cityViewModels = new List<CityViewModel>();
            List<StateViewModel> stateViewModels = new List<StateViewModel>();
            try
            {
                var countryList = unitOfWork.GetRepositoryInstance<CountryViewModel>().ReadStoredProcedure("CountryAll"
                        ).ToList();

                var StateList = unitOfWork.GetRepositoryInstance<StateViewModel>().ReadStoredProcedure("StateAll"
                        ).ToList();

                var CityList = unitOfWork.GetRepositoryInstance<CityViewModel>().ReadStoredProcedure("CityAll"
                         ).ToList();

                if (countryList.Count > 0)
                {
                    foreach (var item in countryList)
                    {
                        CountryViewModel countryViewMod = new CountryViewModel();

                        if (StateList.Where(x => x.CountryId == item.Id).ToList().Count > 0)
                        {
                            countryViewMod.stateViewModels = StateList.Where(x => x.CountryId == item.Id).ToList();
                            countryViewMod.CountryName = item.CountryName;
                            countryViewMod.Id = item.Id;


                            foreach (var Sitem in countryViewMod.stateViewModels)
                            {
                                if (CityList.Where(x => x.StateId == Sitem.Id).ToList().Count > 0)
                                {
                                    StateViewModel stateViewModelse = new StateViewModel();
                                    Sitem.cityViewModels = CityList.Where(x => x.StateId == Sitem.Id).ToList();
                                    stateViewModelse.Id = Sitem.Id;
                                    stateViewModelse.States = Sitem.States;
                                }

                            }
                            countryViewModels.Add(countryViewMod);
                        }

                    }

                }

                userRepsonse.Success((new JavaScriptSerializer()).Serialize(countryViewModels));

                return Request.CreateResponse(HttpStatusCode.Accepted, userRepsonse, contentType);
            }
            catch (Exception)
            {
                return Request.CreateResponse(HttpStatusCode.Ambiguous, userRepsonse, contentType);
            }
        }

        [HttpPost]
        public HttpResponseMessage Add([FromBody] List<CountryViewModel> countryViewModel)
        {
            try
            {
                int Res = 0;
                foreach (CountryViewModel countryViewModels in countryViewModel)
                {
                    Res = unitOfWork.GetRepositoryInstance<SingleIntegerValueResult>().WriteStoredProcedure("CountryAdd @Name",
                     new SqlParameter("Name", System.Data.SqlDbType.VarChar) { Value = countryViewModels.CountryName ?? (object)DBNull.Value }
                  );
                }
                userRepsonse.Success((new JavaScriptSerializer()).Serialize(Res));
                return Request.CreateResponse(HttpStatusCode.Accepted, userRepsonse, contentType);
            }
            catch (Exception ex)
            {
                userRepsonse.Success((new JavaScriptSerializer()).Serialize(ex));
                return Request.CreateResponse(HttpStatusCode.BadRequest, userRepsonse, contentType);
            }
        }

        [HttpPost]
        public HttpResponseMessage Update([FromBody] CountryViewModel countryViewModel)
        {
            try
            {
                var Res = new countryViewModels();
                Res = unitOfWork.GetRepositoryInstance<countryViewModels>().ReadStoredProcedure("CountryUpdate @Id,@Name",
                 new SqlParameter("Id", System.Data.SqlDbType.Int) { Value = countryViewModel.Id },
                 new SqlParameter("Name", System.Data.SqlDbType.VarChar) { Value = countryViewModel.CountryName ?? (object)DBNull.Value }
                 ).FirstOrDefault();                  
                  
                userRepsonse.Success((new JavaScriptSerializer()).Serialize(Res));
                return Request.CreateResponse(HttpStatusCode.Accepted, userRepsonse, contentType);
            }
            catch (Exception ex)
            {
                userRepsonse.Success((new JavaScriptSerializer()).Serialize(ex));
                return Request.CreateResponse(HttpStatusCode.BadRequest, userRepsonse, contentType);
            }
        }

        [HttpPost]
        public HttpResponseMessage AddUpdateState([FromBody] StateViewModel stateViewModel)
        {
            try
            {
                var Res = new StateViewModel();
                  Res = unitOfWork.GetRepositoryInstance<StateViewModel>().ReadStoredProcedure("AddState @Id,@CountryId,@Name",
                     new SqlParameter("Id", System.Data.SqlDbType.Int) { Value = stateViewModel.Id },
                     new SqlParameter("CountryId", System.Data.SqlDbType.Int) { Value = stateViewModel.CountryId },
                     new SqlParameter("Name", System.Data.SqlDbType.VarChar) { Value = stateViewModel.States ?? (object)DBNull.Value }
                  ).FirstOrDefault();
                
                userRepsonse.Success((new JavaScriptSerializer()).Serialize(Res));
                return Request.CreateResponse(HttpStatusCode.Accepted, userRepsonse, contentType);
            }
            catch (Exception ex)
            {
                userRepsonse.Success((new JavaScriptSerializer()).Serialize(ex));
                return Request.CreateResponse(HttpStatusCode.BadRequest, userRepsonse, contentType);
            }
        }

        [HttpPost]
        public HttpResponseMessage AddUpdateCity([FromBody] CityViewModel cityViewModel)
        {
            try
            {
                var Res = new CityViewModel();
                Res = unitOfWork.GetRepositoryInstance<CityViewModel>().ReadStoredProcedure("AddUpdateCity @Id,@StateId,@CityName",
                   new SqlParameter("Id", System.Data.SqlDbType.Int) { Value = cityViewModel.Id },
                   new SqlParameter("StateId", System.Data.SqlDbType.Int) { Value = cityViewModel.StateId },
                   new SqlParameter("CityName", System.Data.SqlDbType.VarChar) { Value = cityViewModel.CityName ?? (object)DBNull.Value }
                ).FirstOrDefault();

                userRepsonse.Success((new JavaScriptSerializer()).Serialize(Res));
                return Request.CreateResponse(HttpStatusCode.Accepted, userRepsonse, contentType);
            }
            catch (Exception ex)
            {
                userRepsonse.Success((new JavaScriptSerializer()).Serialize(ex));
                return Request.CreateResponse(HttpStatusCode.BadRequest, userRepsonse, contentType);
            }
        }

    }
}
