using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using Ninject;
using SaveTime.DaraAccess;
using SaveTime.DataModel.Organization;
using SaveTime.Web.Admin.Models;
using SaveTime.Web.Admin.Repo;

namespace SaveTime.Web.Admin.Controllers
{
    public class EmployeeController : BaseController
    {
        private readonly IRepository<Employee> _repository;
        IMapper _mapper;

        public EmployeeController()
        {
            _repository = kernel.Get<IRepository<Employee>>();
            var config = new MapperConfiguration(
                c => {
                    c.CreateMap<EmployeeEditModel, Employee>();
                    c.CreateMap<Employee, EmployeeEditModel>();
                });
            _mapper = config.CreateMapper();

        }

        public ActionResult Index()
        {
            IEnumerable<Employee> employees = _repository.GetAll();
            IList<EmployeeViewModel> employeeViewModels = new List<EmployeeViewModel>() { };
            foreach (var employee in employees)
            {
                var svm = new EmployeeViewModel();

                svm.Id = employee.Id;
                svm.Name = employee.Name;

                if (employee.AccountId != null)
                {
                    svm.AccountPhone = employee.Account.Phone;
                    svm.AccountEmail = employee.Account.Email;
                }
                else
                {
                    svm.AccountPhone = "";
                    svm.AccountEmail = "";
                }

                if (employee.BranchId != null)
                {
                    if (employee.Branch.CompanyId != null)
                    {
                        svm.CompanyName = employee.Branch.Company.Name;
                    }
                    else
                    {
                        svm.CompanyName = "";
                    }
                    svm.BranchAdress = employee.Branch.Adress;
                }
                else
                {
                    svm.CompanyName = "";
                    svm.BranchAdress = "";
                }

                employeeViewModels.Add(svm);

            }
            return View(employeeViewModels);
        }

        // GET: Employees/Details/5
        //public ActionResult Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Employee employee = db.Employees.Find(id);
        //    if (employee == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(employee);
        //}

        public ActionResult Create()
        {
            EmployeeEditModel eem = new EmployeeEditModel();

            IRepository<Account> _repoAccount;
            _repoAccount = kernel.Get<IRepository<Account>>();
            eem.Accounts = _repoAccount.GetAll().ToList();
            if (eem.Accounts.Count == 0)
            {
                return Content("<h3>В таблице Accounts нет записей!</h3><h3>Нужно внести данные в таблицу Accounts!</h3>");
            }

            IRepository<Branch> _repoBranch;
            _repoBranch = kernel.Get<IRepository<Branch>>();
            eem.Branches = _repoBranch.GetAll().ToList();
            if (eem.Branches.Count == 0)
            {
                return Content("<h3>В таблице Branches нет записей!</h3><h3>Нужно внести данные в таблицу Branches!</h3>");
            }

            return View(eem);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(EmployeeEditModel eem)
        {
            if (ModelState.IsValid)
            {
                Employee employee = _mapper.Map<Employee>(eem);
                _repository.Create(employee);
                return RedirectToAction("Index");
            }

            return View();
        }

        public ActionResult Edit(int? id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Employee employee = _repository.GetById(id);
            if (employee == null)
            {
                return HttpNotFound();
            }

            EmployeeEditModel eem = _mapper.Map<EmployeeEditModel>(employee);

            IRepository<Account> _repoAccount;
            _repoAccount = kernel.Get<IRepository<Account>>();
            eem.Accounts = _repoAccount.GetAll().ToList();
            if (eem.Accounts.Count == 0)
            {
                return Content("<h3>В таблице Accounts нет записей!</h3><h3>Нужно внести данные в таблицу Accounts!</h3>");
            }

            IRepository<Branch> _repoBranch;
            _repoBranch = kernel.Get<IRepository<Branch>>();
            eem.Branches = _repoBranch.GetAll().ToList();
            if (eem.Branches.Count == 0)
            {
                return Content("<h3>В таблице Branches нет записей!</h3><h3>Нужно внести данные в таблицу Branches!</h3>");
            }

            return View(eem);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(EmployeeEditModel eem)
        {
            if (ModelState.IsValid)
            {
                Employee employee = _mapper.Map<Employee>(eem);
                _repository.Edit(employee);
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

            Employee employee = _repository.GetById(id);
            if (employee == null)
            {
                return HttpNotFound();
            }

            EmployeeEditModel eem = _mapper.Map<EmployeeEditModel>(employee);

            IRepository<Account> _repoAccount;
            _repoAccount = kernel.Get<IRepository<Account>>();
            eem.Accounts = _repoAccount.GetAll().ToList();

            IRepository<Branch> _repoBranch;
            _repoBranch = kernel.Get<IRepository<Branch>>();
            eem.Branches = _repoBranch.GetAll().ToList();

            return View(eem);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Employee employee = _repository.GetById(id);
            _repository.Remove(employee);
            return RedirectToAction("Index");
        }

        //protected override void Dispose(bool disposing)
        //{
        //    if (disposing)
        //    {
        //        db.Dispose();
        //    }
        //    base.Dispose(disposing);
        //}
    }
}
