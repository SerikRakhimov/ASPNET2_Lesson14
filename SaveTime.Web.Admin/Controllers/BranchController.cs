using AutoMapper;
using Ninject;
using SaveTime.DaraAccess;
using SaveTime.DataModel.Organization;
using SaveTime.Web.Admin.Models;
using SaveTime.Web.Admin.Repo;
using SaveTime.Web.Admin.Repo.Impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace SaveTime.Web.Admin.Controllers
{
    public class BranchController : BaseController
    {
        private readonly IRepository<Branch> _repository;
        IMapper _mapper;

        public BranchController()
        {
            _repository = kernel.Get<IRepository<Branch>>();
            var config = new MapperConfiguration(
                    c => {
                        c.CreateMap<BranchEditModel, Branch>();
                        c.CreateMap<Branch, BranchViewModel>();
                        c.CreateMap<Branch, BranchEditModel>();
                    });
            _mapper = config.CreateMapper();
        }

        public ActionResult Index()
        {
            IEnumerable<Branch> branchs = _repository.GetAll();
            IList<BranchViewModel> viewBranchs = new List<BranchViewModel>();
            foreach (var brn in branchs)
            {
                BranchViewModel avm = _mapper.Map<BranchViewModel>(brn);
                if (brn.CompanyId != null)
                {
                    avm.CompanyName = brn.Company.Name;
                }
                else
                {
                    avm.CompanyName = "";
                }
                viewBranchs.Add(avm);
            }
            return View(viewBranchs);
        }
        public ActionResult Create()
        {
            BranchEditModel bem = new BranchEditModel();

            return View(bem);

        }
        [HttpPost]
        public ActionResult Create(BranchEditModel bem)
        {
            if (ModelState.IsValid)
            {
                Branch branch = _mapper.Map<Branch>(bem);
                _repository.Create(branch);
                return Content("Данные добавлены");
            }

          return View();

        }

        public ActionResult Edit(int? id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Branch branch = _repository.GetById(id);
            if (branch == null)
            {
                return HttpNotFound();
            }
            BranchEditModel bem = _mapper.Map<BranchEditModel>(branch);

            IRepository<Company> _repoCompany;
            _repoCompany = kernel.Get<IRepository<Company>>();
            bem.Companies = _repoCompany.GetAll().ToList();

            return View(bem);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(BranchEditModel bem)
        {
            if (ModelState.IsValid)
            {
                Branch branch = _mapper.Map<Branch>(bem);
                _repository.Edit(branch);
                return RedirectToAction("Index");
            }
            return View();
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Branch branch = _repository.GetById(id);
            if (branch == null)
            {
                return HttpNotFound();
            }
            BranchEditModel bem = _mapper.Map<BranchEditModel>(branch);

            IRepository<Company> _repoCompany;
            _repoCompany = kernel.Get<IRepository<Company>>();
            bem.Companies = _repoCompany.GetAll().ToList();

            return View(bem);
   
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            // GetByIdInclude(id, "Employees")
            // нужно чтобы при каскадном удалении 
            // записей из Branch 
            // связанные Branches.Id = Employees.BranchId
            // данные из Employee не удалялись,
            // Employees.BranchId присваиваося null
            Branch branch = _repository.GetByIdInclude(id, "Employees");
            _repository.Remove(branch);
            return RedirectToAction("Index");
        }

    }
}