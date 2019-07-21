using Elma.Models;
using NHibernate;
using NHibernate.Criterion;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Elma.Controllers
{
    public class AccountController : Controller
    {
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            NHibernateHelper.getInstance();
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult Login(LoginDto model, string returnUrl)
        {
            if (ModelState.IsValid)
            {

                using (ISession session = NHibernateHelper.getInstance().OpenSession())
                {
                    if (ModelState.IsValid)
                    {
                        bool isValideUser;
                        try
                        {
                            User user = session
                                .Query<User>()
                                .Where(u => u.Login.Equals(model.UserName))
                                .Single();
                            isValideUser = getHash(model.Password).Equals(user.Password);
                        }
                        catch
                        {
                            isValideUser = false;
                        }
                        if (isValideUser)
                        {
                            FormsAuthentication.SetAuthCookie(model.UserName, model.RememberMe);
                            if (Url.IsLocalUrl(returnUrl))
                            {
                                return Redirect(returnUrl);
                            }
                            else
                            {
                                return RedirectToAction("Index", "Home");
                            }
                        }
                        else
                        {
                            ModelState.AddModelError("", "The user name or password provided is incorrect.");
                        }
                    }
                }
            }
            return View(model);
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "Account");
        }

        private string getHash(string toHash)
        {
            byte[] byteArray = Encoding.Default.GetBytes(toHash);
            MemoryStream stream = new MemoryStream(byteArray);

            var sha1 = new SHA1CryptoServiceProvider();
            var sha1data = sha1.ComputeHash(stream);
            var hashedPassword = Convert.ToBase64String(sha1data);

            return hashedPassword;
        }
    }
}