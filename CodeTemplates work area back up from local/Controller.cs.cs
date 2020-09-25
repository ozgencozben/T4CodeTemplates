// Generated code by KoçSistem

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using ADF.Contracts.DataContracts;
using ADF.MVC.Controllers;
using Newtonsoft.Json;
using Business.Contracts.Common;
using Business.Contracts.Custom;
using Business.Contracts.DataContracts.Models;
using Business.Contracts.DataContracts.ViewModels;
using Business.Contracts.ServiceContracts;


namespace Controllers
{
    public class DnmTabloController  : BaseController
    {
        #region Variables
		protected Common.Common.FormData m_retValue;

		[Serializable]
        public class DataTableResult<T>
        {
            public int draw { get; set; }

            public int recordsTotal { get; set; }

            public int recordsFiltered { get; set; }

            public IList<T> data { get; set; }
        }
        #endregion

        #region Default Metods
        protected override void Initialize(RequestContext requestContext)
        {
            base.Initialize(requestContext);
        }

		[YetkiKontrol(SecurityKeys.DnmTablo_Goruntuleme)]
        [ViewBagDoldur(SecurityKeys.DnmTablo_Yetkileri)]
        [SayfaYonlendir]
        public ActionResult Index()
        {
            return View();
        }

		//Table Ajax Resource
		[HttpPost]
        [YetkiKontrol(SecurityKeys.DnmTablo_Listeleme)]
        public ActionResult GetAll(DataTableParameters dataTable, bool? active, string[] filterArray)
        {
            IList<DnmTabloVm> obj = new List<DnmTabloVm>();

            var pageIndex = dataTable.Start / dataTable.Length;
            var pageSize = dataTable.Length;

            var orderBy = dataTable.SortOrder;
            if (orderBy == "Id")
                orderBy = "CreatedDate DESC";

            var ascending = true;
            if (orderBy.Contains("DESC"))
            {
                ascending = false;
                orderBy = orderBy.Replace("DESC", string.Empty).Trim();
            }

            var serviceResult =
                ProxyHelper.ExecuteCall<IDnmTabloService, ServiceResult<PagedDataList>>(srv => srv.GetAllAjax(active, pageIndex, pageSize, orderBy, ascending, filterArray));

            var recordsTotal = serviceResult.Result.RecordsTotal;
            obj = serviceResult.Result.ResultSet as List<DnmTabloVm>;
            var recordsFiltered = serviceResult.Result.RecordsFiltered;

            DataTableResult<DnmTabloVm> result = new DataTableResult<DnmTabloVm>
            {
                draw = dataTable.Draw,
                data = obj,
                recordsTotal = recordsTotal,
                recordsFiltered = recordsFiltered
            };

            return Json(result);
        }

		//Create Modal
        [HttpPost]
        [ValidateAntiForgeryToken]
        [YetkiKontrol(SecurityKeys.DnmTablo_Ekleme)]
        public JsonResult Save(DnmTabloVm model)
        {
            this.m_retValue = new Common.Common.FormData();

            try
            {
                if (ModelState.IsValid)
                {
					//if (!MukerrerMi(model))
                    //{
						var serviceResult = ProxyHelper.ExecuteCall<IDnmTabloService, ServiceResult<bool>>(srv => srv.Save(model));
						m_retValue.IsSuccess = true;
						return Json(m_retValue, JsonRequestBehavior.AllowGet);
					//}
					//else
                    //{
                    //  m_retValue.IsSuccess = false;
                    //  m_retValue.Message = Res.Res.MukerrerKayit;
                    //  return Json(m_retValue, JsonRequestBehavior.AllowGet);
                    //}
                }
                else
                {
                    m_retValue.IsSuccess = false;
                    m_retValue.Message = Res.Res.InvalidModel;
                    return Json(m_retValue, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                m_retValue.IsSuccess = false;
                m_retValue.Message = Res.Res.HataOlustu + ex.Message;
                return Json(m_retValue, JsonRequestBehavior.AllowGet);
            }
        }	

		//Edit Modal
        [HttpPost]
        [ValidateAntiForgeryToken]
        [YetkiKontrol(SecurityKeys.DnmTablo_Guncelleme)]
        public JsonResult GetVm(long id)
        {
            this.m_retValue = new Common.Common.FormData();

            try
            {
                var serviceResult = ProxyHelper.ExecuteCall<IDnmTabloService, ServiceResult<DnmTabloVm>>(srv => srv.GetVm(id));
                m_retValue.Data = serviceResult.Result;
                m_retValue.IsSuccess = true;
                var result = JsonConvert.SerializeObject(m_retValue, Formatting.None, new JsonSerializerSettings() { ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore });
            }
            catch (Exception ex)
            {
                m_retValue.IsSuccess = false;
                m_retValue.Message = Res.Res.HataOlustu + ex.Message;
            }
            return Json(m_retValue, JsonRequestBehavior.AllowGet);
        }

		//Getting Data By Writing Native SQLs
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult GetNative()
        {
            this.m_retValue = new Common.Common.FormData();

            try
            {
                var serviceResult = ProxyHelper.ExecuteCall<IDnmTabloService, ServiceResult<IList<DnmTabloVm>>>(srv => srv.GetNative(true, ""));
                m_retValue.Data = serviceResult.Result;
                m_retValue.IsSuccess = true;
                var result = JsonConvert.SerializeObject(m_retValue, Formatting.None, new JsonSerializerSettings() { ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore });
            }
            catch (Exception ex)
            {
                m_retValue.IsSuccess = false;
                m_retValue.Message = Res.Res.HataOlustu + ex.Message;
            }
            return Json(m_retValue, JsonRequestBehavior.AllowGet);
        }

		// Edit Modal Submit
        [ValidateAntiForgeryToken]
        [YetkiKontrol(SecurityKeys.DnmTablo_Guncelleme)]
        public JsonResult Update(DnmTabloVm model)
        {
            this.m_retValue = new Common.Common.FormData();

            try
            {
                if (ModelState.IsValid)
                {
                    var serviceResult = ProxyHelper.ExecuteCall<IDnmTabloService, ServiceResult<bool>>(srv => srv.Update(model));
                    m_retValue.IsSuccess = true;
                    return Json(m_retValue, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    m_retValue.IsSuccess = false;
                    m_retValue.Message = Res.Res.InvalidModel;
                    return Json(m_retValue, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                m_retValue.IsSuccess = false;
                m_retValue.Message = Res.Res.HataOlustu + ex.Message;
                return Json(m_retValue, JsonRequestBehavior.AllowGet);
            }
        }

		// Make Resource Passive
        [ValidateAntiForgeryToken]
        [YetkiKontrol(SecurityKeys.DnmTablo_Silme)]
        public JsonResult Passive(long id)
        {
            this.m_retValue = new Common.Common.FormData();

            try
            {
                var serviceResult = ProxyHelper.ExecuteCall<IDnmTabloService, ServiceResult<bool>>(srv => srv.Passive(id));
                if (!serviceResult.Result)
                {
                    m_retValue.IsSuccess = false;
                    m_retValue.Message = serviceResult.Message;
                }
                else
                    m_retValue.IsSuccess = true;
            }
            catch (Exception ex)
            {
                m_retValue.IsSuccess = false;
                m_retValue.Message = Res.Res.HataOlustu + ex.Message;
            }
            return Json(m_retValue, JsonRequestBehavior.AllowGet);
        }

		// Delete Record Completely 
        [ValidateAntiForgeryToken]
        [YetkiKontrol(SecurityKeys.DnmTablo_Silme)]
        public JsonResult Delete(long id)
        {
            this.m_retValue = new Common.Common.FormData();

            try
            {
                var serviceResult = ProxyHelper.ExecuteCall<IDnmTabloService, ServiceResult<bool>>(srv => srv.Delete(id));
                if (!serviceResult.Result)
                {
                    m_retValue.IsSuccess = false;
                    m_retValue.Message = serviceResult.Message;
                }
                else
                    m_retValue.IsSuccess = true;
            }
            catch (Exception ex)
            {
                m_retValue.IsSuccess = false;
                m_retValue.Message = Res.Res.HataOlustu + ex.Message;
            }

            return Json(m_retValue, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Other Metods
		private bool MukerrerMi(DnmTabloVm model)
        {
            var retval = true;
            try
            {
                var serviceResult = ProxyHelper.ExecuteCall<IDnmTabloService, ServiceResult<bool>>(srv => srv.IsDuplicateRecord(model));
                retval = serviceResult.Result;
                return retval;
            }
            catch (Exception)
            {
                return retval;
            }
        }
        #endregion
    }
}





// Generated code by KoçSistem move to ServiceContracts

using System.Collections.Generic;
using ADF.Contracts.DataContracts;
using ADF.Contracts.FaultContracts;
using Business.Contracts.DataContracts.ViewModels;
using System.ServiceModel;
using Business.Contracts.Custom;
using Business.Contracts.DataContracts.Models;

namespace Business.Contracts.ServiceContracts
{

[ServiceContract(Namespace = "http://localhost/DnmTabloService/")]
    public interface IDnmTabloService
    {
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        ServiceResult<DnmTablo> Get(long id);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        ServiceResult<PagedDataList> GetAllAjax(bool? active, int pageIndex, int pageSize, string orderBy, bool ascending, string[] filterArray);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        ServiceResult<IList<DnmTabloVm>> GetAll(bool? active);
       
        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        ServiceResult<bool> Save(DnmTabloVm model);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        ServiceResult<DnmTabloVm> GetVm(long id);

		[OperationContract]
        [FaultContract(typeof(ServiceException))]
        ServiceResult<IList<DnmTabloVm>> GetNative(bool active, string condition);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        ServiceResult<bool> Update(DnmTabloVm model);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        ServiceResult<bool> Passive(long id);

        [OperationContract]
        [FaultContract(typeof(ServiceException))]
        ServiceResult<bool> Delete(long id);

		[OperationContract]
        [FaultContract(typeof(ServiceException))]
        ServiceResult<bool> IsDuplicateRecord(DnmTabloVm model);
    }
}





// Generated code by KoçSistem move to BusinessImplemenations

using System;
using System.Collections.Generic;
using ADF.Common.ServiceComponents.Infrastructure;
using ADF.Contracts.DataContracts;
using Business.Contracts.DataContracts.Models;
using Business.Contracts.DataContracts.ViewModels;
using Business.Contracts.ServiceContracts;
using System.Linq;
using System.Web;
using ADF.Contracts.FaultContracts;
using AutoMapper.QueryableExtensions;
using Business.Contracts.Custom;
using System.Linq.Dynamic;

namespace Business.Services.BusinessImplementations
{

 public class DnmTabloService : BusinessBase, IDnmTabloService
    {
        public ServiceResult<DnmTablo> Get(long id)
        {
            try
            {
                return new ServiceResult<DnmTablo>(UnitOfWork.Repository<DnmTablo>().FirstOrDefault(x => x.Id == id));
            }
            catch (Exception ex)
            {
                return new ServiceResult<DnmTablo>(null, ex.Message, MessageResultState.FAIL, FaultCodes.Unset);
            }
        }

        public ServiceResult<PagedDataList> GetAllAjax(bool? active, int pageIndex, int pageSize, string orderBy, bool ascending, string[] filterArray)
        {
            var retval = new PagedDataList();
            var list = new List<DnmTabloVm>();

            orderBy += ascending ? " asc" : " desc";

            var filterUsed = false;
            
			//TODO filtreler sırasıyla tanımlanıcak ve doldurulacak.
			//var FilAdi = string.Empty;
			//
            //if (filterArray != null)
            //{
            //    if (filterArray[0] != "")
            //    {
            //        FilAdi = filterArray[0];
            //        filterUsed = true;
            //    }
			//
            //}

            try
            {
                var skip = pageIndex * pageSize;

                list =
                    UnitOfWork.Repository<DnmTablo>()
                        .ProjectTo<DnmTabloVm>()
                        .Where(x => (active.HasValue) ? x.AktifMi == active : true)
                         //.Where(x => (FilAdi != string.Empty) ? x.Adi.Contains(FilAdi) : true)
                        .OrderBy(orderBy)
                        .Skip(skip)
                        .ToList();

                retval.RecordsTotal = list.Count();
                retval.ResultSet = list.Take(pageSize).ToList();

				if (!active.HasValue)
					retval.RecordsFiltered = filterUsed ? list.Count() : UnitOfWork.Repository<DnmTablo>().ProjectTo<DnmTabloVm>().Count();
				else
					retval.RecordsFiltered = filterUsed ? list.Count() : UnitOfWork.Repository<DnmTablo>().ProjectTo<DnmTabloVm>().Count(x => x.AktifMi == active.Value);

                return new ServiceResult<PagedDataList>(retval, "SUCCESS", MessageResultState.SUCCESS, FaultCodes.Unset);

            }
            catch (Exception ex)
            {
                return new ServiceResult<PagedDataList>(null, ex.Message, MessageResultState.FAIL, FaultCodes.Unset);
            }
        }

        public ServiceResult<IList<DnmTabloVm>> GetAll(bool? active)
        {
            try
            {
                if (!active.HasValue)
                    return new ServiceResult<IList<DnmTabloVm>>(UnitOfWork.Repository<DnmTablo>().ProjectTo<DnmTabloVm>().ToList());
                else
                    return new ServiceResult<IList<DnmTabloVm>>(UnitOfWork.Repository<DnmTablo>().ProjectTo<DnmTabloVm>().Where(x => x.AktifMi == active).ToList());

            }
            catch (Exception ex)
            {
                return new ServiceResult<IList<DnmTabloVm>>(null, ex.Message, MessageResultState.FAIL, FaultCodes.Unset);
            }
        }

        public ServiceResult<bool> Save(DnmTabloVm model)
        {
            try
            {
                var obj = AutoMapper.Mapper.Map<DnmTabloVm, DnmTablo>(model);
                obj.CreatedDate = DateTime.Now;
                obj.CreatedBy = Convert.ToString(HttpContext.Current.Session[Contracts.Common.Constants.USER_ID]);
                obj.AktifMi = true;
                UnitOfWork.Repository<DnmTablo>().Insert(obj);
                UnitOfWork.SaveChanges();
                return new ServiceResult<bool>(true, "Success", MessageResultState.SUCCESS, FaultCodes.Unset);
            }
            catch (Exception ex)
            {
                return new ServiceResult<bool>(false, ex.Message, MessageResultState.FAIL, FaultCodes.Unset);
            }
        }

        public ServiceResult<DnmTabloVm> GetVm(long id)
        {
            try
            {
                return new ServiceResult<DnmTabloVm>(UnitOfWork.Repository<DnmTablo>().ProjectTo<DnmTabloVm>().FirstOrDefault(x => x.Id == id));
            }
            catch (Exception ex)
            {
                return new ServiceResult<DnmTabloVm>(null, ex.Message, MessageResultState.FAIL, FaultCodes.Unset);
            }
        }

		public ServiceResult<IList<DnmTabloVm>> GetNative(bool active, string condition)
        {
            try
            {
                var whereConditions = "\"1\" = \"1\"";

                if (!string.IsNullOrEmpty(condition) && condition != "")
                    whereConditions = condition;

                return new ServiceResult<IList<DnmTabloVm>>(UnitOfWork.Repository<DnmTablo>().ProjectTo<DnmTabloVm>().Where(x => x.AktifMi == active).Where(whereConditions).ToList());
            }
            catch (Exception ex)
            {
                return new ServiceResult<IList<DnmTabloVm>>(null, ex.Message, MessageResultState.FAIL, FaultCodes.Unset);
            }
        }

        public ServiceResult<bool> Update(DnmTabloVm model)
        {
            try
            {
                var obj = UnitOfWork.Repository<DnmTablo>().FirstOrDefault(x => x.Id == model.Id);
                obj.AktifMi = model.AktifMi;
				//TODO veri bağlama işlemi yap.
                //obj.Adi = model.Adi;
                //obj.AdiEn = model.AdiEn;
                //obj.AdiRu = model.AdiRu;
                obj.UpdatedDate = DateTime.Now;
                obj.UpdatedBy = Convert.ToString(HttpContext.Current.Session[Contracts.Common.Constants.USER_ID]);
                UnitOfWork.RepositoryAsync<DnmTablo>().Update(obj);
                UnitOfWork.SaveChanges();
                return new ServiceResult<bool>(true, "Success", MessageResultState.SUCCESS, FaultCodes.Unset);
            }
            catch (Exception ex)
            {
                return new ServiceResult<bool>(false, ex.Message, MessageResultState.FAIL, FaultCodes.Unset);
            }
        }

        public ServiceResult<bool> Passive(long id)
        {
            try
            {
                var obj = UnitOfWork.Repository<DnmTablo>().FirstOrDefault(x => x.Id == id);
                obj.AktifMi = false;
				obj.UpdatedBy = Convert.ToString(HttpContext.Current.Session[Contracts.Common.Constants.USER_ID]);
                obj.UpdatedDate = DateTime.Now;
                UnitOfWork.RepositoryAsync<DnmTablo>().Update(obj);
                UnitOfWork.SaveChanges();
                return new ServiceResult<bool>(true, "Success", MessageResultState.SUCCESS, FaultCodes.Unset);
            }
            catch (Exception ex)
            {
                return new ServiceResult<bool>(false, ex.Message, MessageResultState.FAIL, FaultCodes.Unset);
            }
        }

        public ServiceResult<bool> Delete(long id)
        {
            try
            {
                UnitOfWork.RepositoryAsync<DnmTablo>().Delete(id);
                UnitOfWork.SaveChanges();
                return new ServiceResult<bool>(true, "Success", MessageResultState.SUCCESS, FaultCodes.Unset);
            }
            catch (Exception ex)
            {
                return new ServiceResult<bool>(false, ex.Message, MessageResultState.FAIL, FaultCodes.Unset);
            }
        }

		public ServiceResult<bool> IsDuplicateRecord(DnmTabloVm model)
        {
            try
            {
				int check = 0;

                //check =
                //    UnitOfWork.Repository<DnmTablo>()
                //        .Where(x => x.AktifMi == true)
                //        .Count(x => x.Adi == model.Adi);

				//// Mükerrerlik kontrolü için bakılacak başka alanlar varsa burada kontrol edilecekler.
				//// check işlemi farklı alanlar için çoğaltılabilinir.

				//if (check == 0)
                //{
                //    check = UnitOfWork.Repository<Kullanici>()
                //      .Where(x => x.AktifMi == true)
                //      .Count(x => x.GlobalId == model.GlobalId);
                //}

                return check > 0 ? new ServiceResult<bool>(true, "Success", MessageResultState.SUCCESS, FaultCodes.Unset) : new ServiceResult<bool>(false, "Success", MessageResultState.SUCCESS, FaultCodes.Unset);
            }
            catch (Exception ex)
            {
                return new ServiceResult<bool>(false, ex.Message, MessageResultState.FAIL, FaultCodes.Unset);
            }
        }
    }
}

#region DnmTablo Yetkileri
[Display(Name = "DnmTablo_Yetkileri")]
public const string DnmTablo_Yetkileri = "DnmTablo.*";

[Display(Name = "DnmTablo_Listeleme")]
public const string DnmTablo_Listeleme = "DnmTablo.Listeleme";

[Display(Name = "DnmTablo_Goruntuleme")]
public const string DnmTablo_Goruntuleme = "DnmTablo.Goruntuleme";

[Display(Name = "DnmTablo_Ekleme")]
public const string DnmTablo_Ekleme = "DnmTablo.Ekleme";

[Display(Name = "DnmTablo_Guncelleme")]
public const string DnmTablo_Guncelleme = "DnmTablo.Guncelleme";

[Display(Name = "DnmTablo_Silme")]
public const string DnmTablo_Silme = "DnmTablo.Silme";
#endregion

--DnmTablo Yetkileri
INSERT [dbo].[YetkiAnahtaris] VALUES (N'DnmTablo_Yetkileri', N'DnmTablo.*', 1, NULL, NULL, NULL, NULL, N'DnmTablo_YetkileriEn', N'DnmTablo_YetkileriRu')
GO
INSERT [dbo].[YetkiAnahtaris] VALUES (N'DnmTablo_Listeleme', N'DnmTablo.Listeleme', 1, NULL, NULL, NULL, NULL, N'DnmTablo_ListelemeEn', N'DnmTablo_ListelemeRu')
GO
INSERT [dbo].[YetkiAnahtaris] VALUES (N'DnmTablo_Goruntuleme', N'DnmTablo.Goruntuleme', 1, NULL, NULL, NULL, NULL, N'DnmTablo_GoruntulemeEn', N'DnmTablo_GoruntulemeRu')
GO
INSERT [dbo].[YetkiAnahtaris] VALUES (N'DnmTablo_Ekleme', N'DnmTablo.Ekleme', 1, NULL, NULL, NULL, NULL, N'DnmTablo_EklemeEn', N'DnmTablo_EklemeRu')
GO
INSERT [dbo].[YetkiAnahtaris] VALUES (N'DnmTablo_Guncelleme', N'DnmTablo.Guncelleme', 1, NULL, NULL, NULL, NULL, N'DnmTablo_GuncellemeEn', N'DnmTablo_GuncellemeRu')
GO
INSERT [dbo].[YetkiAnahtaris] VALUES (N'DnmTablo_Silme', N'DnmTablo.Silme', 1, NULL, NULL, NULL, NULL, N'DnmTablo_SilmeEn', N'DnmTablo_SilmeRu')
GO
