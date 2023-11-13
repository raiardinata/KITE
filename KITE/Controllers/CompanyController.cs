using System.Collections.Generic;

namespace KITE.Controllers
{
    public class CompanyController
    {
        DataClasses1DataContext dc = new DataClasses1DataContext();
        public IEnumerable<Models.Company> Company(string NIK)
        {
            object[] param = new[] { NIK };
            IEnumerable<Models.Company> Result = dc.ExecuteQuery<Models.Company>("Exec Select_Company  @NIK={0}", param);
            return Result;
        }
    }
}