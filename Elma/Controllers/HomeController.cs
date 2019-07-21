using Elma.Models;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.Transform;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Elma.Controllers
{
    //Убрать пирамиду из дублирований
    [Authorize]
    public class HomeController : Controller
    {
        const char fileIdPrefix = '_';
        const string filePath = "~/App_Data/Files/";

        public ActionResult Index(string sortOrder, string searchString)
        {
            if (String.IsNullOrEmpty(sortOrder))
            {
                sortOrder = "name_ask";
                ViewBag.NameSortParm = "name_ask";
                ViewBag.DateSortParm = "date_ask";
            }

            IList<Document> documents = new List<Document>();
            using (ISession session = NHibernateHelper.getInstance().OpenSession())
            {
                try
                {
                    User user = session
                        .Query<User>()
                        .Where(u => u.Login.Equals(User.Identity.Name))
                        .Single();
                    if (sortOrder == "name_ask")
                    {
                        ViewBag.NameSortParm = "name_desk";
                        documents = session
                            .Query<Document>()
                            .Where(d => d.User.Id.Equals(user.Id))
                            .Where(d => String.IsNullOrEmpty(searchString) ? true : d.Name.Contains(searchString))
                            .OrderBy(doc => doc.Name)
                            .ToList();
                    }
                    if (sortOrder == "name_desk")
                    {
                        ViewBag.NameSortParm = "name_ask";
                        documents = session
                            .Query<Document>()
                            .Where(d => d.User.Id.Equals(user.Id))
                            .Where(d => String.IsNullOrEmpty(searchString) ? true : d.Name.Contains(searchString))
                            .OrderByDescending(doc => doc.Name)
                            .ToList();
                    }
                    if (sortOrder == "date_ask")
                    {
                        ViewBag.DateSortParm = "date_desk";
                        documents = session
                            .Query<Document>()
                            .Where(d => d.User.Id.Equals(user.Id))
                            .Where(d => String.IsNullOrEmpty(searchString) ? true : d.Name.Contains(searchString))
                            .OrderBy(doc => doc.DateTime)
                            .ToList();
                    }
                    if (sortOrder == "date_desk")
                    {
                        ViewBag.DateSortParm = "date_ask";
                        documents = session
                            .Query<Document>()
                            .Where(d => d.User.Id.Equals(user.Id))
                            .Where(d => String.IsNullOrEmpty(searchString) ? true : d.Name.Contains(searchString))
                            .OrderByDescending(doc => doc.DateTime)
                            .ToList();
                    }
                }
                catch
                {
                }
            }
            return View(documents);
        }

        [Authorize]
        [HttpPost]
        public ActionResult Create(HttpPostedFileBase upload)
        {
            if (upload != null)
            {
                using (ISession session = NHibernateHelper.getInstance().OpenSession())
                {
                    var user = session
                        .Query<User>()
                        .Where(u => u.Login.Equals(User.Identity.Name))
                        .Single();

                    decimal returningId = (decimal)session.CreateSQLQuery("exec PostDocument :pName, :pDateTim, :pUserId, :pDataType")
                        .SetParameter("pName", upload.FileName)
                        .SetParameter("pDateTim", DateTime.Now)
                        .SetParameter("pUserId", user.Id)
                        .SetParameter("pDataType", upload.ContentType)
                        .UniqueResult();
                    string fileName = setIdPrifixToFileName(decimal.ToInt32(returningId), upload.FileName);
                    upload.SaveAs(Server.MapPath(filePath + fileName));
                }
            }
            else
            {
                ModelState.AddModelError("noFileSelected", "No file selected");
            }
            return View();
        }

        [Authorize]
        public ActionResult Create()
        {
            return PartialView();
        }

        //Отстой
        [Authorize]
        public FileResult Download(object id)
        {
            try
            {
                Int32.TryParse(id.ToString(), out int documentId);

                Document document;
                using (ISession session = NHibernateHelper.getInstance().OpenSession())
                {
                    document = session
                        .Query<Document>()
                        .Where(u => u.Id == documentId)
                        .Single();
                }

                string file_path = Server.MapPath(filePath + setIdPrifixToFileName(document.Id, document.Name));
                string file_type = document.DataType;
                string file_name = document.Name;

                return File(file_path, file_type, file_name);
            }
            catch
            {
                return null;
            }

        }

        private Tuple<int, string> splitIdPrifixWithFileName(string idPrifixWithFileName)
        {
            int index = idPrifixWithFileName.IndexOf(fileIdPrefix);
            Int32.TryParse(idPrifixWithFileName.Substring(0, index), out int id);
            string fileName = idPrifixWithFileName.Substring(index);
            return Tuple.Create(id, fileName);
        }

        private string setIdPrifixToFileName(int id, string fileName)
        {
            return String.Concat(id.ToString(), fileIdPrefix.ToString(), fileName);
        }
    }
}