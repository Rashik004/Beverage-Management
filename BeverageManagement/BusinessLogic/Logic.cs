using BeverageManagement.Models;
using BeverageManagement.Models.EntityModel;
using DevMvcComponent.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BeverageManagement.BusinessLogic
{
    public class Logic
    {
        private BeverageManagementEntities db;
        public Logic(BeverageManagementEntities db)
        {
            this.db = db;
        }


        #region Get Final Selected Employee For Current Cycle
        /// <summary>
        /// Returns all the employees selected for payment in this cycle, if a new Cycle is created, boolIsCycleOver is true, otherwise false.
        /// </summary>
        /// <param name="peopleInCycle"></param>
        /// <param name="currentCycle"></param>
        /// <returns></returns>
        public List<Employee> GetFinalSelectedEmployeesForCycle(int peopleInCycle, int currentCycle, out bool boolIsCycleOver)
        {
            var selectedEmployeesForPayment = GetEmployeesInCurrentCycle(peopleInCycle, currentCycle);
            int peopleFoundInCycle = selectedEmployeesForPayment.Count;
            boolIsCycleOver = selectedEmployeesForPayment.Count() < peopleInCycle;
            if (boolIsCycleOver)
            {
                var peopleNeedToCompleteCycle = peopleInCycle - peopleFoundInCycle;
                var newlySelectedEmployeesForPayment = GetEmployeesInCurrentCycle(peopleNeedToCompleteCycle, currentCycle + 1);
                selectedEmployeesForPayment = selectedEmployeesForPayment.Concat(newlySelectedEmployeesForPayment).ToList();
            }
            return selectedEmployeesForPayment;
        } 
        #endregion


        #region Get Employees To Be Selected in Current Cycle
        /// <summary>
        /// tries to get next batch of employees who's current running cycle is less than the actual running cycle
        /// </summary>
        /// <param name="expectedNumberOfEmployee"></param>
        /// <param name="currentRunningCycle"></param>
        /// <returns></returns>
        private List<Employee> GetEmployeesInCurrentCycle(int expectedNumberOfEmployee, int currentRunningCycle)
        {
            var employees = db.Employees;
            var pageInfo = GetPageInfo(expectedNumberOfEmployee, 1);
           //var currentDateTime=(DateTime.Now-employees.FirstOrDefault().JoiningDate).Days;
            var currentDateTime = DateTime.Now;
            var oneMonthback = currentDateTime.AddMonths(-1);
            var pagedEmployees = employees
                        .Where(e => e.Cycle < currentRunningCycle && e.IsWorking == true && !(e.JoiningDate>=oneMonthback && e.JoiningDate<=currentDateTime))
                        .OrderBy(n => n.JoiningDate)
                        .GetPageData(pageInfo).ToList();
            return pagedEmployees;
        } 
        #endregion

        
        
        #region Pagination
        public PaginationInfo GetPageInfo(int perCyclePerson, int pageNumber)
        {
            var pageInfo = new PaginationInfo
            {
                ItemsInPage = perCyclePerson,
                PageNumber = pageNumber,
            };
            return pageInfo;
        } 
        #endregion

    }
}