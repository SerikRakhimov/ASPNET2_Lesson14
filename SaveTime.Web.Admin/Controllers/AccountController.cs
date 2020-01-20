using AutoMapper;
using Ninject;
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
    public class AccountController : BaseController
    {
        // GET: Account
        private readonly IRepository<Account> _repository;
        IMapper _mapper;

        public AccountController()
        {
            _repository = kernel.Get<IRepository<Account>>();
            var config = new MapperConfiguration(
                c => {
                    c.CreateMap<Account, AccountViewModel>();
                    c.CreateMap<AccountViewModel, Account>();
                });
            _mapper = config.CreateMapper();
        }

        public ActionResult Index()
        {
            IEnumerable<Account> accounts = _repository.GetAll();

            IList<AccountViewModel> viewAccounts = new List<AccountViewModel>();
            foreach (var acc in accounts)
            {
                AccountViewModel avm = _mapper.Map<AccountViewModel>(acc);
                viewAccounts.Add(avm);
            }
            return View(viewAccounts);
        }
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(AccountViewModel avm)
        {
            if (ModelState.IsValid)
            {
                Account account = _mapper.Map<Account>(avm);
                _repository.Create(account);
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
            Account account = _repository.GetById(id);
            if (account == null)
            {
                return HttpNotFound();
            }
            AccountViewModel avm = _mapper.Map<AccountViewModel>(account);

            return View(avm);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(AccountViewModel avm)
        {
            if (ModelState.IsValid)
            {
                Account account = _mapper.Map< Account> (avm);
                _repository.Edit(account);
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

            Account account = _repository.GetById(id);
            if (account == null)
            {
                return HttpNotFound();
            }
            AccountViewModel avm = _mapper.Map<AccountViewModel>(account);

            return View(avm);

        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            // "GetByIdInclude(id, "Employees")"
            // нужно чтобы при чтобы при каскадном удалении 
            // записей из Account 
            // связанные Accounts.Id = Employees.AccountId
            // данные из Employee не удалялись,
            // Employees.AccountId присваиваося null
            Account account = _repository.GetByIdInclude(id, "Employees");
            _repository.Remove(account);
            return RedirectToAction("Index");
        }

    }
}